using Com.FastEffect.DataTypes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Com.FastEffect.Events
{
    public class IntSwitchInvoker : MonoBehaviour
    {
        [System.Serializable]
        public class IntUEvent : UnityEvent<int>
        {

        }
        [SerializeField]
        private IntReference m_value = new IntReference();
        public int Value { get => m_value.Value; set => m_value.Value = value; }

        [SerializeField]
        private List<IntUEvent> m_EventList = new List<IntUEvent>();

        public void InvokeValueUsingArg(int arg = 0)
        {
            if (m_EventList.Count > 0)
            {
                if (m_value >= 0 && m_value < m_EventList.Count - 1)
                {
                    m_EventList[m_value].Invoke(arg);
                }
            }
        }
        public void InvokeArgUsingValue(int arg = 0)
        {
            if (m_EventList.Count > 0)
            {
                if (arg >= 0 && arg < m_EventList.Count - 1)
                {
                    m_EventList[arg].Invoke(m_value);
                }
            }
        }
    }
}