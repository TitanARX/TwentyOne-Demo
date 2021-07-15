using UnityEngine;
using Com.FastEffect.DataTypes;

namespace Com.FastEffect.Events
{

    // This monitors a floatvalue and updates a designated animator controller parameter from the gameobject the animator is attached to
    [RequireComponent(typeof(Animator))]
    public class FloatValToAnimatorController : MonoBehaviour,IOneArgEventListener<float>
    {
        [SerializeField]
        private FloatValue m_watchedValue = null;
        [SerializeField]
        private string m_animatorParameterName = "None";

        private Animator m_anim;

        public void OnDisable()
        {
            if(m_watchedValue != null)
            {
                m_watchedValue.Unsubscribe(this);
            }
        }

        public void OnEnable()
        {
            if(m_watchedValue != null)
            {
                m_watchedValue.Subscribe(this);
            }
        }

        public void OnEventRaised(float arg)
        {
            m_anim.SetFloat(m_animatorParameterName,arg);
        }

        // Start is called before the first frame update
        void Start()
        {
            m_anim = GetComponent<Animator>();

        }

    }
}