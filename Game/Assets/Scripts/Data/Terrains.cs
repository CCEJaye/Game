using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Types;

public class Terrains
{

    public static class TerrainCollection
    {
        public static MoltenRock MoltenRock { get; } = new MoltenRock();
        public static Ice Ice { get; } = new Ice();
        public static Crystal Crystal { get; } = new Crystal();
        public static Lava Lava { get; } = new Lava();
        public static Swamp Swamp { get; } = new Swamp();
        public static Water Water { get; } = new Water();
        public static Chemical Chemical { get; } = new Chemical();
        public static LavaFloe LavaFloe { get; } = new LavaFloe();
        public static Marsh Marsh { get; } = new Marsh();
        public static IceFloe IceFloe { get; } = new IceFloe();
        public static Sand Sand { get; } = new Sand();
        public static Slush Slush { get; } = new Slush();
        public static Snow Snow { get; } = new Snow();
        public static Gravel Gravel { get; } = new Gravel();
        public static Rock Rock { get; } = new Rock();
        public static Stone Stone { get; } = new Stone();
        public static DryDirt DryDirt { get; } = new DryDirt();
        public static Mud Mud { get; } = new Mud();
        public static Clay Clay { get; } = new Clay();
        public static Soil Soil { get; } = new Soil();
        public static Ash Ash { get; } = new Ash();
        public static Moss Moss { get; } = new Moss();
        public static Mulch Mulch { get; } = new Mulch();
        public static Grass Grass { get; } = new Grass();
    }

    public class TerrainBase
    {
        public TerrainTypes Name { get; protected set; }
        public int TerrainID { get; protected set; }
        public bool IsLiquid { get; protected set; }
        public AreaTypes AllowedAreas { get; protected set; }
        public UnitTypes AllowedUnits { get; protected set; }
        public UnitTypes DeniedUnits { get; protected set; }

        public int BaseResearch { get; protected set; }
        public int BaseWealth { get; protected set; }
        public int BaseProductivity { get; protected set; }
        public int BasePower { get; protected set; }

        public float LandMovementModifier { get; protected set; }
        public float SeaMovementModifier { get; protected set; }
        //public float OutwardVisibilityModifier { get; protected set; }
        //public float InwardVisibilityModifier { get; protected set; }
        public float DamageConstant { get; protected set; }
        //public float LOSModifier { get; protected set; }
        public float DissipationModifier { get; protected set; }
        public float EvasionModifier { get; protected set; }
    }

    public class MoltenRock : TerrainBase
    {
        public MoltenRock()
        {
            Name = TerrainTypes.MoltenRock;
            TerrainID = 0;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.4f;
            SeaMovementModifier = 0f;
            DamageConstant = 1f;
            DissipationModifier = 0.6f;
            EvasionModifier = 0.9f;
        }
    }

    public class Ice : TerrainBase
    {
        public Ice()
        {
            Name = TerrainTypes.Ice;
            TerrainID = 1;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.7f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 0.9f;
        }
    }

    public class Crystal : TerrainBase
    {
        public Crystal()
        {
            Name = TerrainTypes.Crystal;
            TerrainID = 2;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 1.1f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1.1f;
        }
    }

    public class Lava : TerrainBase
    {
        public Lava()
        {
            Name = TerrainTypes.Lava;
            TerrainID = 3;
            IsLiquid = true;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0f;
            SeaMovementModifier = 0.6f;
            DamageConstant = 1f;
            DissipationModifier = 0.5f;
            EvasionModifier = 0.8f;
        }
    }

    public class Swamp : TerrainBase
    {
        public Swamp()
        {
            Name = TerrainTypes.Swamp;
            TerrainID = 4;
            IsLiquid = true;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0f;
            SeaMovementModifier = 0.8f;
            DamageConstant = 0f;
            DissipationModifier = 1.4f;
            EvasionModifier = 0.9f;
        }
    }

    public class Water : TerrainBase
    {
        public Water()
        {
            Name = TerrainTypes.Water;
            TerrainID = 5;
            IsLiquid = true;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0f;
            SeaMovementModifier = 1f;
            DamageConstant = 0f;
            DissipationModifier = 1.5f;
            EvasionModifier = 1f;
        }
    }

    public class Chemical : TerrainBase
    {
        public Chemical()
        {
            Name = TerrainTypes.Chemical;
            TerrainID = 6;
            IsLiquid = true;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0f;
            SeaMovementModifier = 1f;
            DamageConstant = 1f;
            DissipationModifier = 1.6f;
            EvasionModifier = 1f;
        }
    }

    public class LavaFloe : TerrainBase
    {
        public LavaFloe()
        {
            Name = TerrainTypes.LavaFloe;
            TerrainID = 7;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.3f;
            SeaMovementModifier = 0.4f;
            DamageConstant = 0f;
            DissipationModifier = 0.7f;
            EvasionModifier = 0.7f;
        }
    }

    public class Marsh : TerrainBase
    {
        public Marsh()
        {
            Name = TerrainTypes.Marsh;
            TerrainID = 8;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.3f;
            SeaMovementModifier = 0.5f;
            DamageConstant = 0f;
            DissipationModifier = 1.3f;
            EvasionModifier = 0.8f;
        }
    }

    public class IceFloe : TerrainBase
    {
        public IceFloe()
        {
            Name = TerrainTypes.IceFloe;
            TerrainID = 9;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.4f;
            SeaMovementModifier = 0.6f;
            DamageConstant = 0f;
            DissipationModifier = 1.2f;
            EvasionModifier = 0.9f;
        }
    }

    public class Sand : TerrainBase
    {
        public Sand()
        {
            Name = TerrainTypes.Sand;
            TerrainID = 10;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.9f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1f;
        }
    }

    public class Slush : TerrainBase
    {
        public Slush()
        {
            Name = TerrainTypes.Slush;
            TerrainID = 11;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.8f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1.1f;
            EvasionModifier = 1f;
        }
    }

    public class Snow : TerrainBase
    {
        public Snow()
        {
            Name = TerrainTypes.Snow;
            TerrainID = 12;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.7f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1.1f;
            EvasionModifier = 0.9f;
        }
    }

    public class Gravel : TerrainBase
    {
        public Gravel()
        {
            Name = TerrainTypes.Gravel;
            TerrainID = 13;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.8f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1f;
        }
    }

    public class Rock : TerrainBase
    {
        public Rock()
        {
            Name = TerrainTypes.Rock;
            TerrainID = 14;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 1f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1f;
        }
    }

    public class Stone : TerrainBase
    {
        public Stone()
        {
            Name = TerrainTypes.Stone;
            TerrainID = 15;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 1f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1f;
        }
    }

    public class DryDirt : TerrainBase
    {
        public DryDirt()
        {
            Name = TerrainTypes.DryDirt;
            TerrainID = 16;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 1f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1f;
        }
    }

    public class Mud : TerrainBase
    {
        public Mud()
        {
            Name = TerrainTypes.Mud;
            TerrainID = 17;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.5f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 0.8f;
        }
    }

    public class Clay : TerrainBase
    {
        public Clay()
        {
            Name = TerrainTypes.Clay;
            TerrainID = 18;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.6f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1f;
        }
    }

    public class Soil : TerrainBase
    {
        public Soil()
        {
            Name = TerrainTypes.Soil;
            TerrainID = 19;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 1f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1f;
        }
    }

    public class Ash : TerrainBase
    {
        public Ash()
        {
            Name = TerrainTypes.Ash;
            TerrainID = 20;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 0.9f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1f;
        }
    }

    public class Moss : TerrainBase
    {
        public Moss()
        {
            Name = TerrainTypes.Moss;
            TerrainID = 21;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 1f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1f;
        }
    }

    public class Mulch : TerrainBase
    {
        public Mulch()
        {
            Name = TerrainTypes.Mulch;
            TerrainID = 22;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 1f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1f;
        }
    }

    public class Grass : TerrainBase
    {
        public Grass()
        {
            Name = TerrainTypes.Grass;
            TerrainID = 23;
            IsLiquid = false;
            BaseResearch = 1;
            BaseWealth = 1;
            BaseProductivity = 1;
            BasePower = 1;
            LandMovementModifier = 1f;
            SeaMovementModifier = 0f;
            DamageConstant = 0f;
            DissipationModifier = 1f;
            EvasionModifier = 1f;
        }
    }
}
