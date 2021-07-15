using System;
using Com.FastEffect.DataTypes;
using UnityEngine;
using UnityEngine.Events;
namespace Com.FastEffect.Events
{
    public class StringEventListener : AbstractEventListener<string>
    {
        [Serializable]
        public class StringUEvent : UnityEvent<string>
        {

        }
        [Tooltip("Event to register with.")]
        public StringValue m_Value;

        [Tooltip("Response to invoke when Event is raised.")]
        public StringUEvent m_OnValueUpdate;

        protected override AbstractValue<string> ValueEvent { get => m_Value; }

        protected override UnityEvent<string> Response => m_OnValueUpdate;

    }
}