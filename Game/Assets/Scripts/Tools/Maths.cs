using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Maths
{

    public static float SafeDivide(float num, float div, float def)
    {
        return (div == 0f) ? def : num / div;
    }
}
