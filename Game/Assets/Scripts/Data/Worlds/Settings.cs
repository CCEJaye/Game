using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Conditions;
using static Modifiers;

public class Settings
{

    public class ParamSettings
    {

        private readonly BaseCondition[] Conditions;
        private readonly BaseModifier[] Modifiers;
        public float Occurence { get; protected set; }

        public ParamSettings(BaseCondition[] conditions, BaseModifier[] modifiers, float occurence)
        {
            Conditions = conditions;
            Modifiers = modifiers;
            Occurence = occurence;
        }

        public bool MeetsConditions(float value)
        {
            if (Conditions.Length == 0) return true;
            foreach (BaseCondition condition in Conditions)
            {
                if (!condition.IsTrue(value)) return false;
            }

            return true;
        }

        public float CalculateValue(float value)
        {
            if (Modifiers.Length == 0) return value;
            float newValue = 1f;
            foreach (BaseModifier modifier in Modifiers)
            {
                newValue *= modifier.GetModifiedValue(value);
            }
            return newValue;
        }
    }
}