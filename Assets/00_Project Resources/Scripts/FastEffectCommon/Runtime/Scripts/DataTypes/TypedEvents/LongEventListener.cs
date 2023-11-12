using System;
using UnityEngine;

using UnityEngine.Events;
using Com.FastEffect.DataTypes;
namespace Com.FastEffect.Events
{
    

    public class LongEventListener : AbstractEventListener<long>
    {
        [Serializable]
        public class LongUEvent : UnityEvent<long>
        {

        }
        [Tooltip("Event to register with.")]
        public LongValue m_Value;

        [Tooltip("Response to invoke when Event is raised.")]
        public LongUEvent m_OnValueUpdate;

        protected override AbstractValue<long> ValueEvent { get => m_Value; }

        protected override UnityEvent<long> Response => m_OnValueUpdate;
    }
}