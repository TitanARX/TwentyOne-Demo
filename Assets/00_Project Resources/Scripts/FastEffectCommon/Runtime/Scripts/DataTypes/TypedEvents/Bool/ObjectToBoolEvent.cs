namespace Com.FastEffect.Events
{ 
    using UnityEngine.Events;
    public class ObjectToBoolEvent : ObjectToAbstractEvent<bool>
    {
        [System.Serializable]
        public class BoolUEvent:UnityEvent<bool>{}
        public BoolUEvent m_output;
        protected override UnityEvent<bool> OutputEvent => m_output;
    }
}