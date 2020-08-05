using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;
using static Types;

public class Terrains
{

    public static class TerrainCollection
    {
        public static Grass Grass { get; } = new Grass();
    }

    public class TerrainBase
    {
        public TerrainTypes Name { get; protected set; }
        public int Colour { get; protected set; }
        public bool IsLiquid { get; protected set; }
        public AreaTypes AllowedAreas { get; protected set; }
        public UnitTypes AllowedUnits { get; protected set; }
        public ResourceTypes AllowedResources { get; protected set; }
        public ResourceTypes DeniedResources { get; protected set; }
        public FeatureTypes AllowedFeatures { get; protected set; }
        public FeatureTypes DeniedFeatures { get; protected set; }
        public OutcropTypes AllowedOutcrops { get; protected set; }
        public OutcropTypes DeniedOutcrops { get; protected set; }

        public int BaseResearch { get; protected set; }
        public int BaseWealth { get; protected set; }
        public int BaseProductivity { get; protected set; }
        public int BasePower { get; protected set; }

        public float MovementModifier { get; protected set; }
        public float OutwardVisibilityModifier { get; protected set; }
        public float InwardVisibilityModifier { get; protected set; }
        public float DamageModifier { get; protected set; }
        public float LOSModifier { get; protected set; }
        public float HeatModifier { get; protected set; }
        public float EvasionModifier { get; protected set; }
    }

    public class Grass : TerrainBase
    {
        public Grass()
        {
            Name = TerrainTypes.Grass;
        }
    }
}
