using Com.FastEffect.DataTypes;
using UnityEngine;
using UnityEngine.Events;
namespace Com.FastEffect.Events
{

    public class IntInvoker : AbstractInvoker<int>
    {
        [System.Serializable]
        public class IntUEvent : UnityEvent<int>
        {

        }
        [SerializeField]
        private IntReference m_value = new IntReference();
        public override int Value { get => m_value.Value; set => m_value.Value = value; }

        [SerializeField]
        public IntUEvent m_FunctionsToInvoke;
        protected override UnityEvent<int> InvokedFunctions => m_FunctionsToInvoke;
    }
}