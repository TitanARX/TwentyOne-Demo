using UnityEngine;
using Com.FastEffect.DataTypes;

namespace Com.FastEffect.InputHandeling
{

    public class FaceDirection : MonoBehaviour
    {
        // Movement Input
        [SerializeField]
        private Vector2Reference m_V2ValueSource = new Vector2Reference();
        // Rotation Modifier
        [SerializeField]
        [Min(0f)] private float m_rotationSpeed = 10f;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // FixedUpdate is called once per physics frame
        void FixedUpdate()
        {
            // declaring angle with a default value
            float rotationAngle = 0f;
            // only bother calculating desired rotation angle if a direction is desired
            if (m_V2ValueSource.Value.magnitude > 0)
            {
                // getting camera-based front and right vectors
                Vector3 cforward = Camera.main.transform.forward;
                Vector3 cright = Camera.main.transform.right;
                // preventing camera's x-axis rotation from effecting intended rotation (this assumes the main camera is always upright!)
                cforward.y = 0f;
                cright.y = 0f;
                cforward.Normalize();
                cright.Normalize();

                // setting deired direction based on input
                Vector3 desiredDirection = cforward * m_V2ValueSource.Value.y +
                                          cright * m_V2ValueSource.Value.x;

                // calculate the angle desiredDirection is reletive to the character's forward direction
                rotationAngle = Vector3.SignedAngle(transform.forward, desiredDirection, Vector3.up);

            }

            // rotate the transform
            transform.Rotate(Vector3.up, rotationAngle * m_rotationSpeed * Time.fixedDeltaTime);

        }
    }
}