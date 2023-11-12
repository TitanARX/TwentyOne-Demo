using System;
using UnityEngine;
using UnityEngine.Events;
using Com.FastEffect.DataTypes;
namespace Com.FastEffect.Events
{

    public class Vector3EventListener : AbstractEventListener<Vector3>
    {
        [Serializable]
        public class Vector3UEvent : UnityEvent<Vector3>
        {

        }
        [Tooltip("Event to register with.")]
        public Vector3Value m_Value;

        [Tooltip("Response to invoke when Event is raised.")]
        public Vector3UEvent m_OnValueUpdate;

        protected override AbstractValue<Vector3> ValueEvent { get => m_Value; }

        protected override UnityEvent<Vector3> Response => m_OnValueUpdate;

    }
}