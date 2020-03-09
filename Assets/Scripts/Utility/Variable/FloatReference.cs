using System;
using UnityEngine;

[Serializable]
public class FloatReference : BaseVariableReference<float, FloatVariable>
{
    public FloatReference(float value) : base(value)
    {
    }
}