using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Com.FastEffect.Events
{

    public class StringObjDictionarySwitchInvoker : MonoBehaviour
    {
        [Serializable]
        public class ObjectUEvent : UnityEvent<object>
        {

        }
        [Serializable]
        public class IntUEvent : UnityEvent<int>
        {

        }
        [Serializable]
        public class StringUEvent : UnityEvent<string>
        {

        }
        [SerializeField]
        private List<string> m_keys = new List<string>();
        private int m_heldKey = -1;
        public string LatestKey 
        {
            get
            {
                if (m_heldKey > -1 && m_heldKey < m_keys.Count)
                {
                    return m_keys[m_heldKey];
                }
                return null;
            }
            set 
            {
                m_heldKey = m_keys.IndexOf(value);
            }
        }
        private object m_heldData = null;
        public object LatestData
        {
            get => m_heldData;
            set => m_heldData = value;
        }
        [SerializeField]
        private List<ObjectUEvent> m_ObjectEvents = new List<ObjectUEvent>();
        [SerializeField]
        private List<IntUEvent> m_IntEvents = new List<IntUEvent>();
        [SerializeField]
        private List<FloatUEvent> m_FloatEvents = new List<FloatUEvent>();
        [SerializeField]
        private List<StringUEvent> m_StringEvents = new List<StringUEvent>();
        [SerializeField]
        private List<BoolUEvent> m_BoolEvents = new List<BoolUEvent>();

        public void Invoke(string key, object data)
        {
            if (!string.IsNullOrEmpty(key) && m_keys.Contains(key))
            {
                int index = m_keys.IndexOf(key);
                switch (data)
                {
                    case int i:
                        if (index > -1 && index < m_IntEvents.Count)
                        {
                            m_IntEvents[index].Invoke(i);
                            m_heldKey = index;
                            m_heldData = data;
                        }
                        return;
                    case float f:
                        if (index > -1 && index < m_FloatEvents.Count)
                        {
                            m_FloatEvents[index].Invoke(f);
                            m_heldKey = index;
                            m_heldData = data;
                        }
                        return;
                    case string s:
                        if (index > -1 && index < m_StringEvents.Count)
                        {
                            m_StringEvents[index].Invoke(s);
                            m_heldKey = index;
                            m_heldData = data;
                        }
                        return;
                    case bool b:
                        if (index > -1 && index < m_BoolEvents.Count)
                        {
                            m_BoolEvents[index].Invoke(b);
                            m_heldKey = index;
                            m_heldData = data;
                        }
                        return;
                    default:
                        if (index > -1 && index < m_ObjectEvents.Count)
                        {
                            m_ObjectEvents[index].Invoke(data);
                            m_heldKey = index;
                            m_heldData = data;
                        }
                        return;
                }
            }
        }
        
        public void Invoke(object data)
        {
            Invoke(LatestKey, data);
        }

        public void Invoke(string key)
        {
            Invoke(key, LatestData);
        }
        public void InvokeInt(string key, int i)
        {
            Invoke(key, i);
        }
        public void InvokeInt(int i)
        {
            Invoke(LatestKey, i);
        }
        public void InvokeFloat(string key, float f)
        {
            Invoke(key, f);
        }
        public void InvokeFloat(float f)
        {
            Invoke(LatestKey, f);
        }
        public void InvokeString(string key, string s)
        {
            Invoke(key, s);
        }
        public void InvokeString(string s)
        {
            Invoke(LatestKey, s);
        }
        public void InvokeBool(string key, bool b)
        {
            Invoke(key, b);
        }
        public void InvokeBool(bool b)
        {
            Invoke(LatestKey, b);
        }
    }
}