namespace Com.FastEffect.Events
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Com.FastEffect.DataTypes;

    public class BoolEventListener : AbstractEventListener<bool>
    {
        [Tooltip("Event to register with.")]
        public BoolValue m_Value;

        [Tooltip("Response to invoke when Event is raised.")]
        public BoolUEvent m_OnValueUpdate;
        [Tooltip("Called if true, after OnValueUpdate")]
        [SerializeField]
        private UnityEvent m_OnTrueOnly = new UnityEvent();
        [Tooltip("Called if false, after OnValueUpdate")]
        [SerializeField]
        private UnityEvent m_OnFalseOnly = new UnityEvent();
        protected override AbstractValue<bool> ValueEvent => m_Value;

        protected override UnityEvent<bool> Response => m_OnValueUpdate;

        public override void OnEventRaised(bool arg)
        {
            base.OnEventRaised(arg);
            if(arg)
                m_OnTrueOnly.Invoke();
            else
                m_OnFalseOnly.Invoke();

        }
    }

    [Serializable]
    public class BoolUEvent : UnityEvent<bool>
    {

    }
}