using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Types
{

    public enum ScopeTypes
    {
        None, Local, Global
    }

    public enum TerrainTypes
    {
        None, MoltenRock, Ice, Crystal, Lava, Swamp, 
        Water, Chemical, LavaFloe, Marsh, IceFloe, Sand, 
        Slush, Snow, Gravel, Rock, Stone, DryDirt, Mud, 
        Clay, Soil, Ash, Moss, Mulch, Grass
    }

    public enum AreaTypes
    {
        None, Land, Sea, Air, Space
    }

    public enum UnitTypes
    {
        None, LandSmall, LandMedium, LandLarge,
        SeaSmall, SeaMedium, SeaLarge,
        AirSmall, AirMedium, AirLarge,
        SpaceSmall, SpaceMedium, SpaceLarge
    }

    public enum ResourceTypes
    {
        None,
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
        None,
        Geyser
    }

    public enum OutcropTypes
    {
        None,
        Trees
    }
}
