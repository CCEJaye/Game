using System.Collections;
using System.Collections.Generic;
using static Types;
using UnityEngine;

public class Resources
{

    public static class ResourceCollection
    {
        public static Wealth Wealth { get; } = new Wealth();
    }

    public class ResourceBase
    {
        public string Name { get; protected set; }
        public ResourceTypes Category { get; protected set; }
        public ScopeTypes Scope { get; protected set; }
        public int Colour { get; protected set; }
        public bool IsUnique { get; protected set; }
        public float Occurence { get; protected set; }
        public string Description { get; protected set; }
    }

    public class Wealth : ResourceBase
    {
        public Wealth()
        {
            Name = "Wealth";
            Category = ResourceTypes.Core;
            Scope = ScopeTypes.Global;
            IsUnique = false;
            Occurence = 1f;
        }
    }

}
