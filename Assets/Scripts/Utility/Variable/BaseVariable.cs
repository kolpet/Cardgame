using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseVariable<T> : ScriptableObject, IVariable<T>
{
    [SerializeField] private T value;

    public T Value { get => value; set => this.value = value; }
}
