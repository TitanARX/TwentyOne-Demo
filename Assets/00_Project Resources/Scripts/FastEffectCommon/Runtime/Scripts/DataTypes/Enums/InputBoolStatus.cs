using System;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [Serializable]
    public class InputBoolStatus
    {
        // enum to represent Input.GetButton (held down), ButtonDown (on press), ButtonUp (on release), and unassigned (none)
        public enum ButtonEvents 
        {
            OnRelease = -1,
            None = 0,
            OnPress = 1,
            HeldDown = 2
        }

        /// <summary>
        /// Input Manager Axis button name
        /// </summary>
        public string m_ButtonName = "Unassigned";
        /// <summary>
        /// Value to update
        /// </summary>
        public BoolReference m_StatusOutput = new BoolReference();
        /// <summary>
        /// Button Event to watch
        /// </summary>
        public ButtonEvents m_EventType = ButtonEvents.None;

    }
}