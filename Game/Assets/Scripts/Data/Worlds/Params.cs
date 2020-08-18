using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using static Settings;
using static Terrains;

public class Params
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

    public enum Categories
    {
        Generation, Event, Constant
    }

    public class BaseParam
    {
        public Categories Category { get; protected set; }
        public ParamSettings Settings { get; protected set; }

        public virtual bool MeetsConditions(float value)
        {
            return Settings == null || Settings.MeetsConditions(value);
        }

        public virtual float CalculateValue(float value)
        {
            return Settings == null ? value : Settings.ModifyValue(value);
        }

        public Categories GetCategory()
        {
            return Category;
        }
    }

    public class ElevationParam : BaseParam
    {
        private float[] Elevations;
        private float[] Ranges;
        private float SeaLevel;
        private float DeepSeaLevel;

        public ElevationParam(float[] elevations, float[] ranges, float seaLevel, float deepSeaLevel, ParamSettings settings)
        {
            Category = Categories.Generation;
            Settings = settings;
            Elevations = elevations;
            Ranges = ranges;
            SeaLevel = seaLevel;
            DeepSeaLevel = deepSeaLevel;
        }

        public float GetElevation(float value)
        {
            float modValue = Settings.Calculate(value, 0f, true);
            for (int i = 0; i < Ranges.Length; i++)
            {
                if (modValue <= Ranges[i])
                {
                    return Elevations[i];
                }
            }
            throw new Exception();
        }

        public float GetSeaLevel() { return SeaLevel; }

        public float GetDeepSeaLevel() { return DeepSeaLevel; }
    }

    public class TerrainParam : BaseParam
    {
        private TerrainBase[] Terrains;
        private float[] Ranges;

        public TerrainParam(TerrainBase[] terrains, float[] ranges, ParamSettings settings)
        {
            Category = Categories.Generation;
            Settings = settings;
            Terrains = terrains;
            Ranges = ranges;
        }

        public TerrainBase GetTerrain(float value)
        {
            float modValue = Settings.Calculate(value, 0f, true);
            for (int i = 0; i < Ranges.Length; i++)
            {
                if (modValue <= Ranges[i])
                {
                    return Terrains[i];
                }
            }

            throw new Exception();
        }
    }

    public class OutcropParam : BaseParam
    {
        // ADD FEATURES

        private int Outcrop;

        public OutcropParam(int outcrop, ParamSettings settings)
        {
            Category = Categories.Generation;
            Settings = settings;
            Outcrop = outcrop;
        }

        public int GetOutcrop()
        {
            return Outcrop;
        }
    }

    public class RiverParam : BaseParam
    {
        // ADD TERRAINS

        private int River;

        public RiverParam(int river, ParamSettings settings)
        {
            Category = Categories.Generation;
            Settings = settings;
            River = river;
        }

        public int GetRiver()
        {
            return River;
        }
    }

    public class FeatureParam : BaseParam
    {
        public enum Features
        {
            // ADD FEATURES
        }

        private Features Feature;

        public FeatureParam(Features feature, ParamSettings settings)
        {
            Category = Categories.Generation;
            Settings = settings;
            Feature = feature;
        }

        public Features GetFeature()
        {
            return Feature;
        }
    }

    public class ResourceParam : BaseParam
    {
        // ADD RESOURCES

        private int Resource;

        public ResourceParam(int resource, ParamSettings settings)
        {
            Category = Categories.Generation;
            Settings = settings;
            Resource = resource;
        }

        public int GetResource()
        {
            return Resource;
        }
    }

    public class PrecipitationParam : BaseParam
    {
        public static readonly int None = 0;
        public static readonly int Rain = 1;
        public static readonly int Snow = 2;
        public static readonly int Hail = 3;
        public static readonly int Acid = 4;
        public static readonly int Molten = 5;
        public static readonly int Mineral = 6;

        private int Precipitation;

        public PrecipitationParam(int precipitation, ParamSettings settings)
        {
            Category = Categories.Event;
            Settings = settings;
            Precipitation = precipitation;
        }

        public int GetPrecipitation()
        {
            return Precipitation;
        }
    }

    public class StormParam : BaseParam
    {
        public static readonly int None = 0;
        public static readonly int Lightning = 1;
        public static readonly int Tornado = 2;
        public static readonly int Cyclone = 3;
        public static readonly int Sand = 4;
        public static readonly int Blizzard = 5;
        public static readonly int Fire = 6;

        private int Storm;

        public StormParam(int storm, ParamSettings settings)
        {
            Category = Categories.Event;
            Settings = settings;
            Storm = storm;
        }

        public int GetStorm()
        {
            return Storm;
        }
    }

    public class EruptionParam : BaseParam
    {
        public static readonly int None = 0;
        public static readonly int Steam = 1;
        public static readonly int Pyro = 2;
        public static readonly int Cryo = 3;
        public static readonly int Ash = 4;

        private int Eruption;

        public EruptionParam(int eruption, ParamSettings settings)
        {
            Category = Categories.Event;
            Settings = settings;
            Eruption = eruption;
        }

        public int GetEruption()
        {
            return Eruption;
        }
    }

    public class TemperatureParam : BaseParam
    {
        public static readonly float Freezing = 0f;
        public static readonly float Cold = 0.1f;
        public static readonly float Cool = 0.25f;
        public static readonly float Temperate = 0.4f;
        public static readonly float Warm = 0.6f;
        public static readonly float Hot = 0.75f;
        public static readonly float Melting = 0.9f;

        private float Temperature;

        public TemperatureParam(float temperature)
        {
            Category = Categories.Constant;
            Temperature = temperature;
        }

        public float GetTemperature()
        {
            return Temperature;
        }
    }

    public class HumidityParam : BaseParam
    {
        public static readonly float Arid = 0f;
        public static readonly float Dry = 0.2f;
        public static readonly float Humid = 0.4f;
        public static readonly float Wet = 0.6f;
        public static readonly float Immersed = 0.8f;

        private float Humidity;

        public HumidityParam(float humidity)
        {
            Category = Categories.Constant;
            Humidity = humidity;
        }

        public float GetHumidity()
        {
            return Humidity;
        }
    }

    public class WindParam : BaseParam
    {
        public static readonly float Still = 0f;
        public static readonly float Breeze = 0.2f;
        public static readonly float Gale = 0.4f;
        public static readonly float Storm = 0.6f;
        public static readonly float Tempest = 0.8f;

        private float Wind;

        public WindParam(float wind)
        {
            Category = Categories.Constant;
            Wind = wind;
        }

        public float GetWind()
        {
            return Wind;
        }
    }

    public class LightParam : BaseParam
    {
        public static readonly float Bright = 0f;
        public static readonly float Clear = 0.2f;
        public static readonly float Fair = 0.4f;
        public static readonly float Dim = 0.6f;
        public static readonly float Dark = 0.8f;

        private float Light;

        public LightParam(float light)
        {
            Category = Categories.Constant;
            Light = light;
        }

        public float GetLight()
        {
            return Light;
        }
    }

    public class AtmosphereParam : BaseParam
    {
        public static readonly float None = 0f;
        public static readonly float Thin = 0.2f;
        public static readonly float Breathable = 0.4f;
        public static readonly float Acidic = 0.6f;
        public static readonly float Burning = 0.8f;

        private float Atmosphere;

        public AtmosphereParam(float atmosphere)
        {
            Category = Categories.Constant;
            Atmosphere = atmosphere;
        }

        public float GetAtmosphere()
        {
            return Atmosphere;
        }
    }

    public class ToneParam : BaseParam
    {
        public static readonly int Red = 0;
        public static readonly int Orange = 1;
        public static readonly int Yellow = 2;
        public static readonly int Lime = 3;
        public static readonly int Green = 4;
        public static readonly int Cyan = 5;
        public static readonly int Blue = 6;
        public static readonly int Purple = 7;
        public static readonly int Brown = 8;
        public static readonly int Black = 9;
        public static readonly int Grey = 10;
        public static readonly int White = 11;

        private int Primary;
        private int Secondary;

        public ToneParam(int primary, int secondary)
        {
            Category = Categories.Constant;
            Primary = primary;
            Secondary = secondary;
        }

        public float GetPrimary()
        {
            return Primary;
        }

        public float GetSecondary()
        {
            return Secondary;
        }
    }
}

