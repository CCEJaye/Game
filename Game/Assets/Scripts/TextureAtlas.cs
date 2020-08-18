using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Objects;

public class TextureAtlas : MonoBehaviour
{
    public Texture2D AtlasTexture;
    public float Scale = 10f;

    public const int AtlasSize = 4096;
    public const int Size = 8;
    public const int TextureSize = AtlasSize / Size;
    public const float FloatOffset = 1f / Size;
    public static readonly Vector2 VectorOffset = new Vector2(FloatOffset, FloatOffset);

    private List<Vector2> TextureVectors = new List<Vector2>();

    public void Start()
    {
        GenerateVectors();
    }

    public Vector4 GetTextureBounds(Vector2 textureVector)
    {
        Vector2 xy = textureVector * VectorOffset;
        return new Vector4(xy.x, xy.y, xy.x + FloatOffset, xy.y + FloatOffset);
    }

    public Vector2 CalculateVertexAtlasUV(Vector2 vertexUV, int textureId)
    {
        if (TextureVectors.Count == 0) GenerateVectors();
        return CalculateVertexAtlasUV(vertexUV, TextureVectors[textureId]);
    }

    public Vector2 CalculateVertexAtlasUV(Vector2 vertexUV, Vector2 textureVector)
    {
        if (TextureVectors.Count == 0) GenerateVectors();
        Vector4 textureBounds = GetTextureBounds(textureVector);
        float ratioModifier = Mathf.Sqrt(3f) / 2;
        Vector2 uv = new Vector2(textureBounds.x + vertexUV.x * FloatOffset * ratioModifier, 
            textureBounds.y + vertexUV.y * FloatOffset * ratioModifier);
        return uv;
    }

    private void GenerateVectors()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                TextureVectors.Add(new Vector2(x, y));
            }
        }
    }
}
