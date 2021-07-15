using System.Collections.Generic;
using UnityEngine;
using Com.FastEffect.DataTypes;

namespace Com.FastEffect.InputHandeling
{

    public class ButtonReader : MonoBehaviour
    {
        [SerializeField]
        private List<InputBoolStatus> m_ButtonAxisList = new List<InputBoolStatus>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            foreach (InputBoolStatus btn in m_ButtonAxisList)
            {
                switch (btn.m_EventType)
                {
                    case InputBoolStatus.ButtonEvents.HeldDown:
                        btn.m_StatusOutput.Value = UnityEngine.Input.GetButton(btn.m_ButtonName);
                        break;
                    case InputBoolStatus.ButtonEvents.OnPress:
                        btn.m_StatusOutput.Value = UnityEngine.Input.GetButtonDown(btn.m_ButtonName);
                        break;
                    case InputBoolStatus.ButtonEvents.OnRelease:
                        btn.m_StatusOutput.Value = UnityEngine.Input.GetButtonUp(btn.m_ButtonName);
                        break;
                    default: // in the case of ButtonEvents.None, do nothing
                        break;
                }
            }

        }
    }
}