using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class FloatLimit
{
    public float Min;
    public float Max;

    public float Range
    {
        get
        {
            return Max - Min;
        }
    }
    public float Clamp(float value)
    {
        return Mathf.Clamp(value, Min, Max);
    }
    public FloatLimit(float min, float max)
    {
        this.Min = min;
        this.Max = max;
    }
    public static FloatLimit ZeroToInfinity = new FloatLimit(0,Mathf.Infinity);
    public static FloatLimit ZeroToNegativeInfinity = new FloatLimit(-Mathf.Infinity, 0);
}
