using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Params;
using static Settings;

public class Worlds
{

    // CATEGORY     PARAMETER        CON MOD OCC MAP         CONSTANT EFFECT
    // Generation   Elevation        Y   Y   N   Elevation   Y
    // Generation   Terrain          Y   Y   N   Elevation   Y
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
        public static Frozen Frozen { get; } = new Frozen();
        public static Continent Continent { get; } = new Continent();
    }

    public class WorldParameters
    {
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
        public ToneParam Tone { get; protected set; }
    }

    public class Frozen : WorldParameters
    {
        public Frozen()
        {
            Elevation = new ElevationParam(0, new ParamSettings(null, null, 0f));
            Terrain = new TerrainParam(0, new ParamSettings(null, null, 0f));
        }
    }

    public class Continent : WorldParameters
    {
        public Continent()
        {
            Octaves = 4;
            Persistance = 0.4f;
            Lacunarity = 3f;
            Scale = 12f;
            
        }
    }
}
