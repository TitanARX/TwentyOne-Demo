using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Com.FastEffect.Events
{
    public class StringListInvoker : MonoBehaviour
    {
        [Serializable]
        public class StringSetUEvent : UnityEvent<IList<string>>
        {

        }
        [Serializable]
        public class StringUEvent : UnityEvent<string>
        {

        }
        [SerializeField]
        private List<string> m_list = new List<string>();
        public List<string> ListValue
        {
            get => m_list;
            set
            {
                m_list = value;
                //m_WholeListEvent.Invoke(value);
            }
        }

        public StringSetUEvent m_WholeListEvent;
        public void SendList()
        {
            m_WholeListEvent.Invoke(m_list);
        }

        public StringUEvent m_AsSingleString;
        [SerializeField]
        private string m_seperator = "\n";
        /// <summary>
        /// Converts stored list to a single string and invokes the event
        /// </summary>
        public void InvokeListToString()
        {
            string formatted = string.Join(m_seperator, m_list);
            m_AsSingleString.Invoke(formatted);
        }
        /// <summary>
        /// Converts supplied list to a single string and invokes the event
        /// </summary>
        /// <param name="list">List-like object to join and output</param>
        public void InvokeListToString(IList<string> list)
        {
            string formatted = string.Join(m_seperator, list);
            m_AsSingleString.Invoke(formatted);
        }
        /// <summary>
        /// Iterates through the stored list from lowest index to highest
        /// </summary>
        public void InvokeListAcending()
        {
            for (int i = 0; i < m_list.Count; i++)
            {
                m_AsSingleString.Invoke(m_list[i]);
            }
        }
        /// <summary>
        /// Iterates through the supplied list from lowest index to highest
        /// </summary>
        /// <param name="list">List-like object to iterate through</param>
        public void InvokeListAcending(IList<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                m_AsSingleString.Invoke(list[i]);
            }
        }
        /// <summary>
        /// Iterates through the stored list from highest index to lowest
        /// </summary>
        public void InvokeListDecending()
        {
            for (int i = m_list.Count - 1; i >= 0; i--)
            {
                m_AsSingleString.Invoke(m_list[i]);
            }
        }
        /// <summary>
        /// Iterates through the supplied list from highest index to lowest
        /// </summary>
        /// <param name="list">List-like object to iterate through</param>
        public void InvokeListDecending(IList<string> list)
        {
            for (int i = list.Count-1; i >=0; i--)
            {
                m_AsSingleString.Invoke(list[i]);
            }
        }
    }
}