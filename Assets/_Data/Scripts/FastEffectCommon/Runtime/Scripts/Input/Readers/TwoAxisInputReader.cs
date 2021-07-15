using Com.FastEffect.DataTypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.FastEffect.InputHandeling
{
    
    public class TwoAxisInputReader : MonoBehaviour
    {
        public Vector2 m_movementSpeed = Vector2.one;
        [SerializeField]
        private string m_xAxis = "Mouse X", m_yAxis = "Mouse Y";
        [SerializeField]
        private Vector2Reference m_outputValues = new Vector2Reference();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!IsUIElementActive())
            {
                Vector2 inputData = new Vector2(Input.GetAxis(m_xAxis) * m_movementSpeed.x, Input.GetAxis(m_yAxis) * m_movementSpeed.y);
                m_outputValues.Value = inputData;
            }
            else
            {
                m_outputValues.Value = Vector2.zero;
            }
        }

        protected static bool IsUIElementActive()
        {
            if(EventSystem.current.currentSelectedGameObject != null)
            {
                if (EventSystem.current.currentSelectedGameObject.TryGetComponent<Selectable>(out _))
                {
                    return true;
                }
            }
            return false;
        }
    }
}