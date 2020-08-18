using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Objects;
using static MetaData;
using static Generation;

public class MapGenerator : MonoBehaviour
{
    public enum WorldType
    {
        None, Continent
    }

    public Material AtlasMaterial;
    public TextureAtlas TextureAtlas;
    public WorldType World;

    public static float HeightMultiplier = 7.5f;

    private List<GameObject> Chunks = new List<GameObject>();

    public void Start()
    {
        //Generate();
    }

    public void Generate()
    {
        Clear();
        System.Random rand = new System.Random();
        MapData mapData = new MapData(rand.Next(), Worlds.WorldCollection.Continent);

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

                int textureId = mapData.GetTerrain(currentHex.RealPos).TerrainID;

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

                    hexVertices.Add(new Vector3(currentVertex.RealPos.x, 
                        mapData.GetVertexElevation(currentHex, currentVertex) * HeightMultiplier, 
                        currentVertex.RealPos.y));
                    hexUVs.Add(TextureAtlas.CalculateVertexAtlasUV(currentVertex.RelUV, textureId));
                    vertexCount++;
                }
                chunkVertices.AddRange(hexVertices);
                chunkTriangles.AddRange(hexTriangles);
                chunkUVs.AddRange(hexUVs);
            }

            CreateChunk(chunkVertices.ToArray(), chunkTriangles.ToArray(), chunkUVs.ToArray());
            CreateSea(chunkVertices.ToArray(), chunkTriangles.ToArray(), chunkUVs.ToArray(), mapData);
        }
    }

    private void CreateChunk(Vector3[] vertices, int[] triangles, Vector2[] uvs)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        GameObject meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        DestroyImmediate(meshObject.GetComponent<MeshCollider>());
        meshObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        meshObject.GetComponent<MeshRenderer>().material = AtlasMaterial;
        meshObject.transform.parent = transform;

        //meshObject.transform.localScale = Vector3.one / 10f;

        Chunks.Add(meshObject);
    }

    private void CreateSea(Vector3[] vertices, int[] triangles, Vector2[] uvs, MapData mapData)
    {
        Vector3[] newVertices = new Vector3[vertices.Length];
        for (int i = 0; i< vertices.Length; i++)
        {
            newVertices[i] = new Vector3(vertices[i].x, mapData.WorldParams.Elevation.GetSeaLevel() * HeightMultiplier, vertices[i].z);
        }
        Vector2[] newUVs = new Vector2[uvs.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            newUVs[i] = new Vector2(newVertices[i].x, newVertices[i].z);
        }
        Mesh mesh = new Mesh();
        mesh.vertices = newVertices;
        mesh.triangles = triangles;
        mesh.uv = newUVs;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        GameObject meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        DestroyImmediate(meshObject.GetComponent<MeshCollider>());
        meshObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        meshObject.GetComponent<MeshRenderer>().material = AtlasMaterial;
        meshObject.transform.parent = transform;

        //meshObject.transform.localScale = Vector3.one / 10f;

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

    /*private float GetSteppedValue(float value, float interval)
    {
        float step = 0f;
        while (step < value)
        {
            step += interval;
        }
        return step;
    }*/
}
