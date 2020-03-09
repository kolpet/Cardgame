using System;
using UnityEngine;

[Serializable]
public class Vector2Reference : BaseVariableReference<Vector2, Vector2Variable>
{
    public Vector2Reference(Vector2 value) : base(value)
    {
    }
}