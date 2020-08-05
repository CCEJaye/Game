using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Types
{

    public enum ScopeTypes
    {
        Local, Global
    }

    public enum TerrainTypes
    {
        Snow, Marsh, Sand, Water,
        Rock, Soil, Gravel, Grass,
        Lava, Chemical, Space, Ice,
        Savanna, Mud, Crystal, Gas,
        Dust, Swamp, Coral, Clay,
        Stone, Ash, Silt, Shale, Moss
    }

    public enum AreaTypes
    {
        Land, Sea, Air, Space
    }

    public enum UnitTypes
    {
        LandSmall, LandMedium, LandLarge,
        SeaSmall, SeaMedium, SeaLarge,
        AirSmall, AirMedium, AirLarge,
        SpaceSmall, SpaceMedium, SpaceLarge
    }

    public enum ResourceTypes
    {
        Core,
        Alkali,
        Alkaline,
        Lanthanoid,
        Actinide,
        Refractory,
        Precious,
        PostTransition,
        Metalloid,
        Halogen,
        Noble,
        Other

    }

    public enum FeatureTypes
    {
        Geyser
    }

    public enum OutcropTypes
    {
        Trees
    }
}
