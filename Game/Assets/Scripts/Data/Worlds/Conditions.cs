using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditions
{
    public class BaseCondition
    {
        protected float TopOfRange;

        public virtual bool IsTrue(float value)
        {
            return false;
        }
    }

    public class RangeCondition : BaseCondition
    {
        public RangeCondition(float topOfRange)
        {
            TopOfRange = topOfRange;
        }

        public override bool IsTrue(float value)
        {
            return value <= TopOfRange;
        }
    }
}
