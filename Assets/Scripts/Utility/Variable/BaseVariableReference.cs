using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseVariableReference<C, R> : IVariable<C>
    where R : BaseVariable<C>
{
    [SerializeField] private R reference;

    [SerializeField] private C constant;

    [SerializeField] private bool useConstant = true;

    public bool UseConstant { get => useConstant; set => useConstant = value; }

    public C Value
    {
        get => useConstant ? constant : (reference != null ? reference.Value : constant);
        set
        {
            if (useConstant)
            {
                constant = value;
            }
            else
            {
                reference.Value = value;
            }
        }
    }

    public R Reference { get => reference; set => reference = value; }
    public C Constant { get => constant; set => constant = value; }

    public BaseVariableReference() { }

    public BaseVariableReference(C value)
    {
        useConstant = true;
        constant = value;
    }
}