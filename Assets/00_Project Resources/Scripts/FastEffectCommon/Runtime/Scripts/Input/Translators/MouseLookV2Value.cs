using UnityEngine;
using Com.FastEffect.DataTypes;

namespace Com.FastEffect.InputHandeling
{

    public class MouseLookV2Value : MonoBehaviour
    {
        // source values
        [SerializeField]
        private Vector2Reference m_V2ValueSource = new Vector2Reference();
        
        // axis invert options
        [SerializeField]
        private bool m_invertY = true;
        [SerializeField]
        private bool m_invertX = false;

        // rotation speed modifier
        [SerializeField]
        private Vector2 m_rotationModifer = Vector2.one;

        // splitting the horizontal spin and vertical tilt objects from each other, typically we want the vertically tilting object parented to the horizontal spinning object
        [SerializeField]
        private bool m_enableTilting = false;
        [SerializeField]
        public Transform m_tiltTarget;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // set tilt/spin based on axis inversion settings
            float tilt = m_invertY ? -m_V2ValueSource.Value.y : m_V2ValueSource.Value.y;
            float spin = m_invertX ? -m_V2ValueSource.Value.x : m_V2ValueSource.Value.x;

            // spin first
            transform.Rotate(Vector3.up, spin * m_rotationModifer.x /* Time.deltaTime*/);

            // if we want to tilt, and have set up an object to tilt
            if (m_enableTilting && m_tiltTarget != null)
            {
                m_tiltTarget.Rotate(Vector3.right, tilt * m_rotationModifer.y /* Time.deltaTime*/);
            }

        }
    }
}