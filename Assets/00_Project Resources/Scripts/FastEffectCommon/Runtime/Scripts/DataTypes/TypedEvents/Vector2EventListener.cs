using System;
using UnityEngine;
using UnityEngine.Events;
using Com.FastEffect.DataTypes;
namespace Com.FastEffect.Events
{

    public class Vector2EventListener : AbstractEventListener<Vector2>
    {
        [Serializable]
        public class Vector2UEvent : UnityEvent<Vector2>
        {

        }
        [Tooltip("Event to register with.")]
        public Vector2Value m_Value;

        [Tooltip("Response to invoke when Event is raised.")]
        public Vector2UEvent m_OnValueUpdate;

        protected override AbstractValue<Vector2> ValueEvent { get => m_Value; }

        protected override UnityEvent<Vector2> Response => m_OnValueUpdate;

    }
}