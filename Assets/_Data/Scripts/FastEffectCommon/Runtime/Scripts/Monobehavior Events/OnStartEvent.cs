using UnityEngine;
using UnityEngine.Events;

namespace Com.FastEffect.Events
{
    public class OnStartEvent : MonoBehaviour
    {
        public UnityEvent m_OnStart = new UnityEvent();
        // Start is called before the first frame update
        void Start()
        {
            m_OnStart.Invoke();
        }
    }
}