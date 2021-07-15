using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomMultiEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public List<CustomEvent>CustomEvents = new List<CustomEvent>();

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent Response;

    private void OnEnable()
    {
        foreach (CustomEvent customEvent in CustomEvents)
        {
            //customEvent.Subscribe(this);
        }

        
    }

    private void OnDisable()
    {
        //Event.Unsubscribe(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
