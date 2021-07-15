using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringReference : MonoBehaviour
{
    public StringVariable _value;

    public string StringRef
    {
        get { return  _value.value; }
    }
}
