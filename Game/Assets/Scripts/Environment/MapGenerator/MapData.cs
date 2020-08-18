using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HexNoise;
using static Objects;
using static Terrains;
using static Types;
using static Worlds;
using static MetaData;

public class MapData
{
    public int Seed;
    public WorldParameters WorldParams;
    private HexNoise NoiseValues;
    public Dictionary<Vector2, CellProperties> Cells;

    public MapData(int seed, WorldParameters worldParams)
    {
        Seed = seed;
        WorldParams = worldParams;
        NoiseValues = new HexNoise(seed, worldParams.Octaves, 
            worldParams.Persistance, worldParams.Lacunarity, worldParams.Scale, worldParams.IsMeridianFading, worldParams.IsParallelFading);
        Cells = new Dictionary<Vector2, CellProperties>();
        foreach (KeyValuePair<Vector2Int, ChunkMeta> chunk in WorldMeta)
        {
            foreach (KeyValuePair<Vector2Int, HexMeta> hex in chunk.Value.HexMeta)
            {
                StoreRawValue(hex.Value.RealPos);
            }
        }
        Normalise();
        CalculateProperties();
    }

    private NoiseValues StoreRawValue(Vector2 realPos)
    {
        return NoiseValues.StoreRawValues(realPos);
    }

    private void Normalise()
    {
        NoiseValues.NormaliseAll();
    }

    private void CalculateProperties()
    {
        foreach (Vector2 hexRealPos in NoiseValues.ValueList.Keys)
        {
            CellProperties cell = new CellProperties();
            cell.RealPos = hexRealPos;
            cell.NoiseValues = NoiseValues.ValueList[hexRealPos];
            cell.Elevation = WorldParams.Elevation.GetElevation(cell.NoiseValues.All);
            cell.Terrain = WorldParams.Terrain.GetTerrain(cell.NoiseValues.All);
            Cells.Add(hexRealPos, cell);
        }
    }

    public float GetElevation(Vector2 realPos)
    {
        return Cells[realPos].Elevation;
    }

    public TerrainBase GetTerrain(Vector2 realPos)
    {
        return Cells[realPos].Terrain;
    }

    public float GetVertexElevation(HexMeta hex, VertexMeta vert)
    {
        Vector2 baseSample = hex.RealPos;
        Vector2 sampleA = GetHexNeighbourForVertex(hex, vert.HexNeighbourA);
        Vector2 sampleB = GetHexNeighbourForVertex(hex, vert.HexNeighbourB);
        float averagedValue = GetElevation(baseSample);
        int notNullSamples = 1;
        if (sampleA != Generation.NullVector)
        {
            averagedValue += GetElevation(sampleA);
            notNullSamples++;
        }
        if (sampleB != Generation.NullVector)
        {
            averagedValue += GetElevation(sampleB);
            notNullSamples++;
        }
        return averagedValue /= notNullSamples;
    }

    public class CellProperties
    {
        public Vector2 RealPos;
        internal NoiseValues NoiseValues;

        public float Elevation;
        public TerrainBase Terrain;
        public OutcropTypes Outcrop;
        public FeatureTypes Feature;
        public ResourceTypes Resource;
        //
    }
}
/*
public int Octaves { get; protected set; }
public float Persistance { get; protected set; }
public float Lacunarity { get; protected set; }
public float Scale { get; protected set; }
public ElevationParam Elevation { get; protected set; }
public TerrainParam Terrain { get; protected set; }
public OutcropParam Outcrop { get; protected set; }
public RiverParam River { get; protected set; }
public FeatureParam Feature { get; protected set; }
public ResourceParam Resource { get; protected set; }
public PrecipitationParam Precipitation { get; protected set; }
public StormParam Storm { get; protected set; }
public EruptionParam Eruption { get; protected set; }
public TemperatureParam Temperature { get; protected set; }
public HumidityParam Humidity { get; protected set; }
public WindParam Wind { get; protected set; }
public LightParam Light { get; protected set; }
public AtmosphereParam Atmosphere { get; protected set; }
public ToneParam Tone { get; protected set; }*/