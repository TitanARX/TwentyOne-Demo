using System.Collections.Generic;
using UnityEngine;
namespace Com.FastEffect.Events
{
    [CreateAssetMenu(menuName = "Fast Effect /Dataless Event")]
    public class DatalessEvent : ScriptableObject
    {
        // The list of listeners that this event will notify if it is raised
        private readonly List<DatalessEventListener> m_listeners = new List<DatalessEventListener>();

        public void Raise()
        {
            for (int i = m_listeners.Count - 1; i >= 0; i--)
            {
                m_listeners[i].OnEventRaised();
            }
        }

        public void Subscribe(DatalessEventListener newSubscriber)
        {
            if (!m_listeners.Contains(newSubscriber))
            {
                m_listeners.Add(newSubscriber);
            }
        }

        public void Unsubscribe(DatalessEventListener unsubscribingListener)
        {
            m_listeners.Remove(unsubscribingListener);
        }
    }
}