using UnityEngine;

namespace Com.FastEffect.ObjectInteraction{

    [RequireComponent(typeof(Collider))]
    public class ChildObjectTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_parentObject = null;
        public void SetTargetObject(GameObject targ = null)
        {
            if (targ != null)
            {
                m_parentObject = targ;
            }
        }

        private void Start()
        {
            if(m_parentObject == null)
            {
                if(transform.parent != null)
                {
                    m_parentObject = transform.parent.gameObject;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_parentObject != null)
            {
                m_parentObject.SendMessage("OnTriggerEnter", other);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (m_parentObject != null)
            {
                m_parentObject.SendMessage("OnTriggerExit", other);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (m_parentObject != null)
            {
                m_parentObject.SendMessage("OnTriggerStay", other);
            }
        }
    }
}