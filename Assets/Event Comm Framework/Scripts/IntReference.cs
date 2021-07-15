using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntReference : MonoBehaviour
{
    public bool useConstant = false;
    public int ConstantValue;
    public IntVariable _value; 

    public int IntRef
    {
        get { return useConstant ? ConstantValue : _value.Value; } 
    }
}
