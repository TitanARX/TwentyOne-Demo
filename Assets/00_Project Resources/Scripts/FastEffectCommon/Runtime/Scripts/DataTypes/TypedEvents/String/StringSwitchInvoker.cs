using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Com.FastEffect.DataTypes;

namespace Com.FastEffect.Events
{

    public class StringSwitchInvoker : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField]
        private StringReference m_value = new StringReference();
        [SerializeField]
        private  List<StringCaseEvent> m_stringCases = new List<StringCaseEvent>();
        private Dictionary<string,StringUEvent> m_eventDictionary = new Dictionary<string, StringUEvent>();
        public void OnBeforeSerialize()
        {
            m_stringCases.Clear();
            foreach(KeyValuePair<string,StringUEvent> kvp in m_eventDictionary)
            {
                m_stringCases.Add(new StringCaseEvent(kvp.Key,kvp.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            m_eventDictionary.Clear();
            foreach(StringCaseEvent scase in m_stringCases)
            {
                if(m_eventDictionary.ContainsKey(scase.Name))
                {
                    m_eventDictionary.Add(scase.Name+m_eventDictionary.Count, scase.StrEvent);
                }
                else
                {
                    m_eventDictionary.Add(scase.Name, scase.StrEvent);
                }
            }
        }
        [SerializeField]
        private StringUEvent m_onAnyNonEmptyValue = new StringUEvent();

        public void InvokeCase()
        {
            InvokeCase(m_value);
        }
        public void InvokeCase(string value)
        {
            if(m_eventDictionary.ContainsKey(value))
            {
                m_eventDictionary[value].Invoke(value);
            }
            if(!string.IsNullOrWhiteSpace(value))
            {
                m_onAnyNonEmptyValue.Invoke(value);
            }
        }

        [Serializable]
        private class StringUEvent:UnityEvent<string>{}
        [Serializable]
        private struct StringCaseEvent
        {
            public string Name;
            public StringUEvent StrEvent;

            public StringCaseEvent(string name, StringUEvent strEvent)
            {
                Name = name;
                StrEvent = strEvent;
            }
        }
    }
}