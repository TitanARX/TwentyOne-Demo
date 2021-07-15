using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Custom Event Object", menuName = "TitanARX / Custom Event Preset")]
public class CustomEvent : ScriptableObject
{
    // The list of listeners that this event will notify if it is raised
    private readonly List<CustomEventListener> listeners = new List<CustomEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1 ; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void Subscribe(CustomEventListener t)
    {
        if (!listeners.Contains(t))
        {
            listeners.Add(t);
        }
    }

    public void Unsubscribe(CustomEventListener t)
    {
        if (listeners.Contains(t))
        {
            listeners.Add(t);
        }
    }

    
}
