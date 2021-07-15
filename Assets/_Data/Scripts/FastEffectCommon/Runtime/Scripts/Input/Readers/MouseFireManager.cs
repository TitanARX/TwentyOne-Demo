using UnityEngine;
using Com.FastEffect.DataTypes;

namespace Com.FastEffect.InputHandeling
{
    
    /// <summary>
    /// Earlier version of the ButtonReader class
    /// Still useful for reading the On Down event for exactly two buttons
    /// </summary>
    public class MouseFireManager : MonoBehaviour
    {
        [SerializeField]
        private string m_primaryFire = "Fire1";
        [SerializeField]
        private BoolReference m_fireOneOutput = new BoolReference();
        [SerializeField]
        private string m_altFire = "Fire2";
        [SerializeField]
        private BoolReference m_fireTwoOutput = new BoolReference();
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // get button down status
            bool fireOneDown = UnityEngine.Input.GetButtonDown(m_primaryFire);
            bool fireTwoDown = UnityEngine.Input.GetButtonDown(m_altFire);

            // ONLY update the output values when GetButtonDown returns true; set the values to false wher the boolvalues are actually being checked to be true
            if (fireOneDown)
            {
                m_fireOneOutput.Value = true;
            }
            if (fireTwoDown)
            {
                m_fireTwoOutput.Value = true;
            }
        }
    }
}