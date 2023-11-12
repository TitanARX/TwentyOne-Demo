using UnityEngine;

using Com.FastEffect.DataTypes;

namespace Com.FastEffect.InputHandeling
{ 
    [RequireComponent(typeof(CharacterController))]
    public class CharaContrXZ : MonoBehaviour
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
            // declaring desired direction with a default value
            Vector3 desiredDirection = Vector3.zero;
            // only bother calculating desired direction if a direction is desired
            if (m_V2ValueSource.Value.magnitude > 0)
            {
                // getting camera-based front and right vectors
                Vector3 cforward = Camera.main.transform.forward;
                Vector3 cright = Camera.main.transform.right;
                // preventing camera's x-axis rotation from effecting intended player movement (this assumes the main camera is always upright!)
                cforward.y = 0f;
                cright.y = 0f;
                cforward.Normalize();
                cright.Normalize();
                
                // setting deired direction based on input
                desiredDirection = cforward * m_V2ValueSource.Value.y + 
                                          cright * m_V2ValueSource.Value.x;
            }

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
    }
}