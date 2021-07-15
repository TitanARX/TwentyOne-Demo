using System;
using UnityEngine.Events;
using Com.FastEffect.DataTypes;

namespace Com.FastEffect.Events
{

    public class StringInvoker : AbstractInvoker<string>
    {
        [Serializable]
        public class StringUEvent : UnityEvent<string>
        {

        }
        public StringReference m_value = new StringReference("");
        public StringUEvent m_FunctionsToInvoke;

        // Using SetWithoutTrigger() to prevent potential unintentional infinate loops
        public override string Value { get => m_value; set => m_value.SetWithoutTrigger(value); }
        protected override UnityEvent<string> InvokedFunctions => m_FunctionsToInvoke;
    }
}