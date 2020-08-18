using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Params;
using static Settings;
using static Terrains;

public class Worlds
{

    // CATEGORY     PARAMETER        CON MOD OCC MAP         CONSTANT EFFECT
    // Generation   Elevation        Y   Y   N   Noise       Y
    // Generation   Terrain          Y   Y   N   Noise       Y
    // Generation   Outcrop          Y   Y   Y   Outcrop     Y
    // Generation   River            Y   Y   Y   River       Y
    // Generation   Features         Y   Y   Y   Random      Y
    // Generation   Resources        Y   Y   Y   Random      Y
    // Event        Precipitation    N   N   Y   Determined  N
    // Event        Storms           N   N   Y   Determined  N
    // Event        Eruptions        N   N   Y   Determined  N
    // Constant     Temperature      N   N   N   Determined  Y
    // Constant     Humidity         N   N   N   Determined  Y
    // Constant     Wind             N   N   N   Determined  Y
    // Constant     Light            N   N   N   Determined  Y
    // Constant     Atmosphere       N   N   N   Determined  Y
    // Constant     Tone             N   N   N   Determined  Y

    // TERRAINS HAVE BOOLS FOR WHETHER OR NOT THEY AFFECT OUTCROPS, RIVERS, ETC.
    // AND ALSO RANGES FOR HUMIDITY AND TEMPERATURE ETC. CONDITIONS

    public static class WorldCollection
    {
        public static Continent Continent { get; } = new Continent();
    }

    public class WorldParameters
    {
        public int Octaves { get; protected set; }
        public float Persistance { get; protected set; }
        public float Lacunarity { get; protected set; }
        public float Scale { get; protected set; }
        public bool IsMeridianFading { get; protected set; }
        public bool IsParallelFading { get; protected set; }
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
        public ToneParam Tone { get; protected set; }
    }

    public class Continent : WorldParameters
    {
        public Continent()
        {
            Octaves = 4;
            Persistance = 0.36f;
            Lacunarity = 3.2f;
            Scale = 40f;
            IsMeridianFading = true;
            IsParallelFading = true;
            Elevation = new ElevationParam(
                new float[] { 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f }, 
                new float[] { 0.55f, 0.6f, 0.65f, 0.675f, 0.7f, 0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1f },
                0.15f, 0.05f, new ParamSettings(null, null, 0f));
            Terrain = new TerrainParam(
                new TerrainBase[] {
                    TerrainCollection.Water,
                    TerrainCollection.Sand,
                    TerrainCollection.Mulch,
                    TerrainCollection.Grass,
                    TerrainCollection.Gravel,
                    TerrainCollection.Stone },
                new float[] { 0.6f, 0.65f, 0.7f, 0.8f, 0.9f, 1f },
                new ParamSettings(null, null, 0f));
        }
    }
}
