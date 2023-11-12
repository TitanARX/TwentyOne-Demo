using UnityEngine;
using UnityEngine.Events;
namespace Com.FastEffect.Events
{

    public class DatalessEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public DatalessEvent m_eventObject;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent m_OnEventRaised;

        private void OnEnable()
        {
            if (m_eventObject != null)
            {
                m_eventObject.Subscribe(this);
            }
        }

        private void OnDisable()
        {
            if (m_eventObject != null)
            {
                m_eventObject.Unsubscribe(this);
            }
        }

        public void OnEventRaised()
        {
            m_OnEventRaised.Invoke();
        }
    }
}