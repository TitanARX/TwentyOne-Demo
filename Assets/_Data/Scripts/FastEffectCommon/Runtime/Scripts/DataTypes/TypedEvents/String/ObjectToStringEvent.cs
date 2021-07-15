using System;
using UnityEngine.Events;
namespace Com.FastEffect.Events
{ 
    public class ObjectToStringEvent : ObjectToAbstractEvent<string>
    {
        [Serializable]
        public class StringUEvent : UnityEvent<string>
        {

        }
        public StringUEvent m_output;
        protected override UnityEvent<string> OutputEvent => m_output;
    }
}