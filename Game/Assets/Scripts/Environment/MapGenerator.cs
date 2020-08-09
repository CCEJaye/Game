using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Objects;
using static MetaData;
using static Data;

public class MapGenerator : MonoBehaviour
{
    public enum WorldType
    {
        None, Continent
    }

    public Material AtlasMaterial;
    public TextureAtlas TextureAtlas;
    public WorldType World;

    private List<GameObject> Chunks = new List<GameObject>();

    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;
        System.Random rand = new System.Random();
        HexNoise noise = new HexNoise(rand.Next(), 4, 0.3f, 3f, 200f);

        Dictionary<Vector2Int, ChunkMeta> worldMeta = WorldMeta;
        foreach (KeyValuePair<Vector2Int, ChunkMeta> chunk in worldMeta)
        {
            foreach (KeyValuePair<Vector2Int, HexMeta> hex in chunk.Value.HexMeta)
            {
                noise.StoreRawValues(hex.Value.RealPos);

                foreach (KeyValuePair<Vector2Int, VertexMeta> vert in hex.Value.VertexMeta)
                {
                    if (vert.Value.RealPos.x < minX) minX = vert.Value.RealPos.x;
                    if (vert.Value.RealPos.x > maxX) maxX = vert.Value.RealPos.x;
                    if (vert.Value.RealPos.y < minY) minY = vert.Value.RealPos.y;
                    if (vert.Value.RealPos.y > maxY) maxY = vert.Value.RealPos.y;
                }
            }
        }
        noise.NormaliseAll();

        List<Vector3> allVertices = new List<Vector3>();
        List<int> allTriangles = new List<int>();
        List<Vector2> allUVs = new List<Vector2>();

        foreach (KeyValuePair<Vector2Int, ChunkMeta> c in WorldMeta)
        {
            ChunkMeta currentChunk = c.Value;

            List<Vector3> chunkVertices = new List<Vector3>();
            List<int> chunkTriangles = new List<int>();
            List<Vector2> chunkUVs = new List<Vector2>();

            int vertexCount = 0;
            int triangleCount = 0;

            foreach (KeyValuePair<Vector2Int, HexMeta> h in currentChunk.HexMeta)
            {
                HexMeta currentHex = h.Value;

                int hexRand = rand.Next(0, 63);

                List<Vector3> hexVertices = new List<Vector3>();
                List<int> hexTriangles = new List<int>();
                List<Vector2> hexUVs = new List<Vector2>();

                foreach (int triangle in currentHex.Triangles)
                {
                    hexTriangles.Add(triangle + vertexCount);
                    triangleCount++;
                }
                foreach (KeyValuePair<Vector2Int, VertexMeta> v in currentHex.VertexMeta)
                {
                    VertexMeta currentVertex = v.Value;

                    Vector2 baseSample = currentHex.RealPos;
                    Vector2 sampleA = GetHexNeighbourForVertex(currentHex, currentVertex.HexNeighbourA);
                    Vector2 sampleB = GetHexNeighbourForVertex(currentHex, currentVertex.HexNeighbourB);
                    float averagedValue = GetSteppedValue(noise.ValueList[baseSample].All, 0.125f);
                    int notNullSamples = 1;
                    if (sampleA != NullVector)
                    {
                        averagedValue += GetSteppedValue(noise.ValueList[sampleA].All, 0.125f);
                        notNullSamples++;
                    }
                    if (sampleB != NullVector)
                    {
                        averagedValue += GetSteppedValue(noise.ValueList[sampleB].All, 0.125f);
                        notNullSamples++;
                    }
                    averagedValue /= notNullSamples;

                    hexVertices.Add(new Vector3(currentVertex.RealPos.x, averagedValue * 80f, currentVertex.RealPos.y));
                    Vector2 realUV = new Vector2(currentVertex.RealPos.x / (maxX - minX), currentVertex.RealPos.y / (maxY - minY));
                    hexUVs.Add(TextureAtlas.GetUVForVertex(realUV, hexRand));
                    vertexCount++;
                }
                chunkVertices.AddRange(hexVertices);
                chunkTriangles.AddRange(hexTriangles);
                chunkUVs.AddRange(hexUVs);
            }
            allVertices.AddRange(chunkVertices);
            allTriangles.AddRange(chunkTriangles);
            allUVs.AddRange(chunkUVs);

            CreateChunk(chunkVertices.ToArray(), chunkTriangles.ToArray(), chunkUVs.ToArray());
        }
    }

    private void CreateChunk(Vector3[] vertices, int[] triangles, Vector2[] uvs)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        GameObject meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //DestroyImmediate(meshObject.GetComponent<MeshCollider>());
        meshObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        meshObject.GetComponent<MeshRenderer>().material = AtlasMaterial;
        meshObject.transform.localScale = Vector3.one / 10f;

        Chunks.Add(meshObject);
    }

    public void Clear()
    {
        foreach(GameObject chunk in Chunks)
        {
            DestroyImmediate(chunk);
        }
        Chunks.Clear();
    }

    private float GetSteppedValue(float value, float interval)
    {
        float step = 0f;
        while (step < value)
        {
            step += interval;
        }
        return step;
    }
}
