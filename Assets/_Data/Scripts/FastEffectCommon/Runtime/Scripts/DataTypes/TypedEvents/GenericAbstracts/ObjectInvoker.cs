using UnityEngine.Events;
namespace Com.FastEffect.Events
{
    public class ObjectInvoker : AbstractInvoker<object>
    {
        [System.Serializable]
        public class ObjectUEvent : UnityEvent<object>
        {

        }
        public object m_value = null;
        public ObjectUEvent m_FunctionsToInvoke;

        public override object Value { get => m_value; set => m_value = value; }

        protected override UnityEvent<object> InvokedFunctions => m_FunctionsToInvoke;

    }
}