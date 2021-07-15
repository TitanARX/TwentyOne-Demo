using UnityEngine;
using UnityEngine.Events;
using Com.FastEffect.DataTypes;
using System;

namespace Com.FastEffect.ObjectInteraction
{
    public class TriggerTagFilter : MonoBehaviour
    {
        [SerializeField][TagSelector]
        private string m_tagToCheck = TagSelector.NoTag;

        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, Vector3.one * 0.25f);
        }


        [Serializable]
        public class ColliderUEvent : UnityEvent<Collider>
        {

        }

        public ColliderUEvent m_OnTriggerEnter;
        public void OnTriggerEnter(Collider other)
        {
            if (m_tagToCheck == TagSelector.NoTag || other.CompareTag(m_tagToCheck))
            {
                m_OnTriggerEnter.Invoke(other);
            }
        }
        public ColliderUEvent m_OnTriggerExit;
        public void OnTriggerExit(Collider other)
        {
            if (m_tagToCheck == TagSelector.NoTag || other.CompareTag(m_tagToCheck))
            {
                m_OnTriggerExit.Invoke(other);
            }
        }
        public ColliderUEvent m_OnTriggerStay;
        public void OnTriggerStay(Collider other)
        {
            if (m_tagToCheck == TagSelector.NoTag || other.CompareTag(m_tagToCheck))
            {
                m_OnTriggerStay.Invoke(other);
            }
        }
    }
}