using UnityEngine;
using Com.FastEffect.DataTypes;

namespace Com.FastEffect.InputHandeling 
{ 
    public class CharaContrTank : MonoBehaviour
    {
        // Movement Input
        [SerializeField]
        private Vector2Reference m_V2ValueSource = new Vector2Reference();
        // Movement Modifier
        [SerializeField]
        [Min(0f)] private float m_maxSpeed = 10f;
        // Movement Output
        [SerializeField]
        private FloatReference m_moveSpeedOutput = new FloatReference();

        [SerializeField]
        private bool m_invertRotation = false;
        // rotation speed modifier
        [SerializeField]
        private float m_rotationModifer = 1f;

        // Gravity Modifier
        [SerializeField]
        [Min(0f)] private float m_gravityModifier = 1f;
        // Vertical Movement Output
        [SerializeField]
        private FloatReference m_VerticalSpeedOutput = new FloatReference();

        // Required Components
        private CharacterController m_charControl;

        // Start is called before the first frame update
        void Start()
        {
            // Getting the Required Components
            m_charControl = GetComponent<CharacterController>();
        }

        // FixedUpdate is called once per physics frame
        void FixedUpdate()
        {
            float spin = m_invertRotation ? -m_V2ValueSource.Value.x : m_V2ValueSource.Value.x;
            // spin first
            transform.Rotate(Vector3.up, spin * m_rotationModifer /* Time.fixedDeltaTime*/);

            // declaring desired direction with a default value
            Vector3 desiredDirection = Vector3.zero;
            // only bother calculating desired direction if a direction is desired
            if (Mathf.Abs(m_V2ValueSource.Value.y) > 0)
            {
                // since it's tank controls, we only need to know what's forward reletive to the character.
                Vector3 cforward = transform.forward;
                cforward.y = 0f;
                cforward.Normalize();

                // setting deired direction based on input
                desiredDirection = cforward * m_V2ValueSource.Value.y;
            }
            if (m_charControl != null && m_charControl.enabled)
            {
                // calculate gravity
                Vector3 grav = Vector3.zero;
                if (!m_charControl.isGrounded)
                {
                    grav = Physics.gravity * m_gravityModifier;
                }

                // move the character controller
                m_charControl.Move((desiredDirection * m_maxSpeed + grav) * Time.fixedDeltaTime);

                // collecting the character controller's velocity and outputting it for other components
                Vector3 xzSpeed = m_charControl.velocity;
                m_VerticalSpeedOutput.Value = xzSpeed.y;
                xzSpeed.y = 0;
                m_moveSpeedOutput.Value = xzSpeed.magnitude;
            }
            else
            {
                Vector3 moveSpeed = desiredDirection * m_maxSpeed * Time.fixedDeltaTime;
                transform.Translate(moveSpeed,Space.World);
                m_VerticalSpeedOutput.Value = moveSpeed.y;
                moveSpeed.y = 0;
                m_moveSpeedOutput.Value = moveSpeed.magnitude;
            }

        }
    }
}