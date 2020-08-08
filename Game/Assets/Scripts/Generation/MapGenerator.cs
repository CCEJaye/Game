using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Data;
using static PositionalMetaData;

public class MapGenerator : MonoBehaviour
{
    public GameObject MeshFilter;

    public void Generate()
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;
        // smaller scale = zoom out
        System.Random rand = new System.Random();
        HexNoise noise = new HexNoise(rand.Next(), 4, 0.3f, 3f, 100f);
        Dictionary<Vector2Int, ChunkMeta> worldMeta = GetWorldMeta;
        foreach (KeyValuePair<Vector2Int, ChunkMeta> chunk in worldMeta)
        {
            foreach (KeyValuePair<Vector2Int, HexMeta> hex in chunk.Value.HexMeta)
            {
                /*GameObject obj = Instantiate(Hex, new Vector3(
                    hex.Value.RealPos.x, 0, hex.Value.RealPos.y), Quaternion.identity);
                EditorObjects.Add(obj);*/
                noise.StoreRawValues(hex.Value.RealPos);

                foreach (KeyValuePair<Vector2Int, VertexMeta> vert in hex.Value.VertexMeta)
                {
                    if (hex.Value.ParentChunk.RelPos == new Vector2Int(0, 0))
                    {
                        if (vert.Value.RealPos.x < minX) minX = vert.Value.RealPos.x;
                        if (vert.Value.RealPos.x > maxX) maxX = vert.Value.RealPos.x;
                        if (vert.Value.RealPos.y < minY) minY = vert.Value.RealPos.y;
                        if (vert.Value.RealPos.y > maxY) maxY = vert.Value.RealPos.y;
                    }
                }
            }
        }
        noise.NormaliseAll();

        List<Vector3> allVertices = new List<Vector3>();
        List<int> allTriangles = new List<int>();
        List<Vector2> allUVs = new List<Vector2>();
        int vertexCount = 0;
        int triangleCount = 0;

        for (int dX = 0; dX < 3; dX++)
        {
            for (int dY = 0; dY < 2; dY++)
            {
                ChunkMeta currentChunk = worldMeta[new Vector2Int(dX, dY)];
                List<Vector3> chunkVertices = new List<Vector3>();
                List<int> chunkTriangles = new List<int>();
                foreach (KeyValuePair<Vector2Int, HexMeta> h in currentChunk.HexMeta)
                {
                    HexMeta currentHex = h.Value;
                    List<Vector3> hexVertices = new List<Vector3>();
                    List<int> hexTriangles = new List<int>();
                    foreach (Vector3Int triangle in currentHex.Triangles)
                    {
                        hexTriangles.Add(triangle.x + vertexCount);
                        hexTriangles.Add(triangle.y + vertexCount);
                        hexTriangles.Add(triangle.z + vertexCount);
                        triangleCount += 3;
                    }
                    foreach (KeyValuePair<Vector2Int, VertexMeta> v in currentHex.VertexMeta)
                    {
                        VertexMeta currentVertex = v.Value;

                        Vector2 baseSample = currentHex.RealPos;
                        Vector2 sampleA = currentHex.GetNeighbourRealPos(currentVertex.HexNeighbourA);
                        Vector2 sampleB = currentHex.GetNeighbourRealPos(currentVertex.HexNeighbourB);
                        float averagedValue = GetSteppedValue(noise.ValueList[baseSample].All, 0.1f);
                        int notNullSamples = 1;
                        if (sampleA != NullVector)
                        {
                            averagedValue += GetSteppedValue(noise.ValueList[sampleA].All, 0.1f);
                            notNullSamples++;
                        }
                        if (sampleB != NullVector)
                        {
                            averagedValue += GetSteppedValue(noise.ValueList[sampleB].All, 0.1f);
                            notNullSamples++;
                        }
                        averagedValue /= notNullSamples;

                        /*if (currentVertex.IsFlat)
                        {
                            averagedValue = GetSteppedValue(averagedValue, 0.1f);
                        }*/

                        hexVertices.Add(new Vector3(currentVertex.RealPos.x, averagedValue * 80f, currentVertex.RealPos.y));
                        allUVs.Add(new Vector2(currentVertex.RealPos.x / (maxX - minX), currentVertex.RealPos.y / (maxY - minY)));
                        vertexCount++;
                    }
                    chunkVertices.AddRange(hexVertices);
                    chunkTriangles.AddRange(hexTriangles);
                }
                //GameObject chunkMesh = GameObject.CreatePrimitive(PrimitiveType.Plane);
                //chunkMesh.transform.
                //Mesh newMesh = new Mesh();
                //newMesh.vertices = chunkVertices.ToArray();
                //newMesh.triangles = chunkTriangles.ToArray();
                //newMesh.uv = allUVs.ToArray();
                allVertices.AddRange(chunkVertices);
                allTriangles.AddRange(chunkTriangles);
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = allVertices.ToArray();
        mesh.triangles = allTriangles.ToArray();
        mesh.uv = allUVs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter = GameObject.CreatePrimitive(PrimitiveType.Plane);
        DestroyImmediate(MeshFilter.GetComponent<MeshCollider>());
        MeshFilter.GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    public float GetSteppedValue(float value, float interval)
    {
        float step = 0f;
        while (step < value)
        {
            step += interval;
        }
        return step;
    }
}
