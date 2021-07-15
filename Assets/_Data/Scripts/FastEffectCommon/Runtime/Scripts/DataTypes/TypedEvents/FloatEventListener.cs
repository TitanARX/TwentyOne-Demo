using System;
using UnityEngine;
using UnityEngine.Events;
using Com.FastEffect.DataTypes;
namespace Com.FastEffect.Events
{
    

    public class FloatEventListener : AbstractEventListener<float>
    {
        [Tooltip("Event to register with.")]
        public FloatValue m_Value;

        [Tooltip("Response to invoke when Event is raised.")]
        public FloatUEvent m_OnValueUpdate;

        protected override AbstractValue<float> ValueEvent { get => m_Value; }

        protected override UnityEvent<float> Response => m_OnValueUpdate;
    }

    [Serializable]
    public class FloatUEvent : UnityEvent<float>
    {

    }
}