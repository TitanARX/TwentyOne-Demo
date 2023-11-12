using Com.FastEffect.DataTypes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Com.FastEffect.Events
{
    

    public class StringSetEventListener : AbstractSetEventListener<string>
    {
        
        public StringSet m_observedSet;
        public StringSetUEvent m_OnSetAdd;
        public StringSetUEvent m_OnSetRefresh;
        public IntUEvent m_onItemChanged;

        protected override RuntimeSet<string> ObservedSet => m_observedSet;

        protected override UnityEvent<List<string>> OnSetAdd => m_OnSetAdd;

        protected override UnityEvent<List<string>> OnSetRefresh => m_OnSetRefresh;

        protected override UnityEvent<int> OnSetItemChanged => m_onItemChanged;

        [Serializable]
        public class StringSetUEvent : UnityEvent<List<string>>{}
        [Serializable]
        public class IntUEvent : UnityEvent<int>{}
    }
    
}