using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Com.FastEffect.InputHandeling
{

    public class OnSubmitInvoker : MonoBehaviour,ISubmitHandler
    {
        public UnityEvent m_OnSubmit;
        public void OnSubmit(BaseEventData eventData)
        {
            m_OnSubmit.Invoke();
        }
    }
}