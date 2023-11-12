using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEndEditHandler : MonoBehaviour
{
    [Serializable]
    public class SubmitStringEvent : UnityEvent<string> { }
    [SerializeField]
    private SubmitStringEvent m_OnSubmit = new SubmitStringEvent();
    public void OnSubmit(string str)
    {
        if (Input.GetButton("Submit"))
        {
            m_OnSubmit.Invoke(str);
        }
    }
}
