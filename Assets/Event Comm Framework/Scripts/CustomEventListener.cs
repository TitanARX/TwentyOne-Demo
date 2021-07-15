using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public CustomEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.Subscribe(this);
    }

    private void OnDisable() 
    {
        Event.Unsubscribe(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
