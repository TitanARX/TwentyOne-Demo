using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatReference : MonoBehaviour
{
    public bool useConstant = false;
    public float ConstantValue;
    public IntVariable _value;

    public float IntRef
    {
        get { return useConstant ? ConstantValue : _value.Value; }
    }
}
