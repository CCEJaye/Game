using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Modifiers
{
    public class BaseModifier
    {
        protected Structs.Range Range;

        public virtual float GetModifiedValue(float value)
        {
            return value;
        }
    }

    public class CurveModifier : BaseModifier
    {
        // https://www.desmos.com/calculator/jmyf3dx45t

        public enum ModifyMode
        {
            // Modifier overrides operations
            // + & - and * & / cancel out
            // Everything else will combine
            Modifier, Invert, Addition, Subtraction, Multiplication, Division
        }

        // Tension -1 : 1
        // Target >0
        private readonly float Tension;
        private readonly float Target;
        private readonly ModifyMode Mode;

        public CurveModifier(float tension, float target, ModifyMode mode)
        {
            Tension = tension;
            Target = target;
            Mode = mode;
        }

        public override float GetModifiedValue(float value)
        {
            float newValue = value;
            float modifier = Target * Mathf.Pow(value, Mathf.Pow(10, 2 * Tension));
            if (Mode.HasFlag(ModifyMode.Invert)) modifier = Target - modifier;
            if (Mode.HasFlag(ModifyMode.Modifier)) return modifier;
            if (Mode.HasFlag(ModifyMode.Division)) newValue = Maths.SafeDivide(value, modifier, 1f);
            if (Mode.HasFlag(ModifyMode.Multiplication)) newValue *= modifier;
            if (Mode.HasFlag(ModifyMode.Subtraction)) newValue -= modifier;
            if (Mode.HasFlag(ModifyMode.Addition)) newValue += modifier;
            return newValue;
        }
    }
}
