using UnityEngine;
using UnityEngine.Events;

namespace Com.FastEffect.Events
{
    public class OnEnableDisableEvents : MonoBehaviour
    {
        
        public UnityEvent m_OnEnable = new UnityEvent();
        public UnityEvent m_OnDisable = new UnityEvent();
        private void OnEnable()
        {
            m_OnEnable.Invoke();
        }
        private void OnDisable()
        {
            m_OnDisable.Invoke();
        }
    }
}