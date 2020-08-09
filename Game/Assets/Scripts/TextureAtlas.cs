using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAtlas : MonoBehaviour
{
    public Texture2D AtlasTexture;

    public const int AtlasSize = 2048;
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

    public Vector2 GetUVForVertex(Vector2 realPos, int textureId)
    {
        if (TextureVectors.Count == 0) GenerateVectors();
        return GetUVForVertex(realPos, TextureVectors[textureId]);
    }

    public Vector2 GetUVForVertex(Vector2 realPos, Vector2 textureVector)
    {
        if (realPos.y > -1f && realPos.y < 0f)
        {
            bool test = true;
        }
        if (TextureVectors.Count == 0) GenerateVectors();
        Vector4 textureBounds = GetTextureBounds(textureVector);
        Vector2 uv = new Vector2(
            Mathf.Repeat(realPos.x, FloatOffset) + textureBounds.x,
            Mathf.Repeat(realPos.y, FloatOffset) + textureBounds.y);

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
