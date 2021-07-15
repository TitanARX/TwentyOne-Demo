using UnityEngine;
using UnityEngine.Events;
using Com.FastEffect.DataTypes;
namespace Com.FastEffect.Events
{
    public class TransformValueEvent : AbstractEventListener<Transform>
    {
        [SerializeReference]
        private TransformValue m_watchedValue = null;
        [SerializeField]
        private TransformUEvent m_response = new TransformUEvent();

        protected override AbstractValue<Transform> ValueEvent => m_watchedValue;

        protected override UnityEvent<Transform> Response => m_response;
        [System.Serializable]
        private class TransformUEvent:UnityEvent<Transform>{}
    }
}