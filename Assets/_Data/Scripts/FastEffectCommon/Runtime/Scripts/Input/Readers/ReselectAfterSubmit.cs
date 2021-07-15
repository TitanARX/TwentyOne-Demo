using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.FastEffect.InputHandeling.readers
{

    [RequireComponent(typeof(Selectable))]
    public class ReselectAfterSubmit : MonoBehaviour, ISubmitHandler
    {
        private Selectable m_returnFocusAfterInvoke;
        public void OnSubmit(BaseEventData eventData)
        {
            if(eventData.selectedObject == m_returnFocusAfterInvoke)
            {
                m_returnFocusAfterInvoke.Select();
            }
        }

        void Awake()
        {
            m_returnFocusAfterInvoke = GetComponent<Selectable>();
        }

        void OnEnabled()
        {

        }
    }
}