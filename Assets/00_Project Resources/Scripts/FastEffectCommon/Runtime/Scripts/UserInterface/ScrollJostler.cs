namespace Com.FastEffect.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ScrollJostler : MonoBehaviour
    {
        public LayoutElement m_target;
        private bool m_state = false;
        void Start()
        {
            if(m_target != null)
            {
                m_target.ignoreLayout = true;
            }
        }
        public void Jostle(bool enabled = false)
        {
            if (m_target != null)
            {
                m_target.gameObject.SetActive(enabled);
                m_state = enabled;
            }
        }

        void LateUpdate()
        {
            Jostle(!m_state);
        }
    }
}