using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditions
{
    public class BaseCondition
    {
        protected Structs.Range Range;

        public virtual bool IsTrue(float value)
        {
            return false;
        }
    }

    public class RangeCondition : BaseCondition
    {
        public RangeCondition(float start, float end)
        {
            Range = new Structs.Range(start, end);
        }

        public override bool IsTrue(float value)
        {
            return Range.Contains(value);
        }
    }
}
