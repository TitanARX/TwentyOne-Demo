using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Com.FastEffect.DataTypes;
namespace Com.FastEffect.Events
{

    public class IntEventListenerSwitch : MonoBehaviour, IOneArgEventListener<int>
    {
        [Tooltip("Event to register with.")]
        public IntValue m_Value;

        [Tooltip("Responses to invoke per int Value")]
        public List<UnityEvent> m_OnValueUpdate = new List<UnityEvent>();
        //TODO: argument overload modes: clamp, wrap, more?

        public void OnEnable()
        {
            if (m_Value != null)
            {
                m_Value.Subscribe(this);
            }
        }

        public void OnDisable()
        {
            if (m_Value != null)
            {
                m_Value.Unsubscribe(this);
            }
        }

        public void OnEventRaised(int arg)
        {
            if (m_OnValueUpdate.Count > 0)
            {
                if (arg >= 0 && arg < m_OnValueUpdate.Count)
                {
                    m_OnValueUpdate[arg].Invoke();
                }
                else
                {
                    //Event Argument out of bounds!
                    Debug.LogWarning("Index [" + arg + "] is not valid! Accepted values are 0 - " + (m_OnValueUpdate.Count - 1) + "!");
                }
            }
            else
            {
                Debug.LogWarning("OnEventRaised called but [List<UnityEvent> m_OnValueUpdate] is empty or null!");
            }
        }
    }
}