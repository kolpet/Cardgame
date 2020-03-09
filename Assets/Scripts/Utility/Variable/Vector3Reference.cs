using System;
using UnityEngine;

[Serializable]
public class Vector3Reference : BaseVariableReference<Vector3, Vector3Variable>
{
    public Vector3Reference(Vector3 value) : base(value)
    {
    }
}