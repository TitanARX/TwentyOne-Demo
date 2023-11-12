using System;
using UnityEngine;
using UnityEngine.Events;
using Com.FastEffect.DataTypes;
namespace Com.FastEffect.Events
{

    public class IntEventListener : AbstractEventListener<int>
    {
        [Serializable]
        public class IntUEvent : UnityEvent<int>
        {

        }
        [Tooltip("Event to register with.")]
        public IntValue m_Value;

        [Tooltip("Response to invoke when Event is raised.")]
        public IntUEvent m_OnValueUpdate;

        protected override AbstractValue<int> ValueEvent { get => m_Value; }

        protected override UnityEvent<int> Response => m_OnValueUpdate;
    }
}