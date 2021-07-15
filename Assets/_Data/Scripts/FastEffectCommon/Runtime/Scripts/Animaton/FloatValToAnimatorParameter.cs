using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.FastEffect.InputHandeling
{
    using Com.FastEffect.DataTypes;

    // This monitors a floatvalue and updates a designated animator parameter from inside the animator controller itself
    public class FloatValToAnimatorParameter : StateMachineBehaviour
    {
        [SerializeField]
        private FloatReference m_watchedValue = new FloatReference();
        [SerializeField]
        private string m_parameterName = "None";

        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(m_parameterName != "None")
            {
                animator.SetFloat(m_parameterName, m_watchedValue);
            }
        }

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called before OnStateMove is called on any state inside this state machine
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateIK is called before OnStateIK is called on any state inside this state machine
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMachineEnter is called when entering a state machine via its Entry Node
        //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        //{
        //    
        //}

        // OnStateMachineExit is called when exiting a state machine via its Exit Node
        //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        //{
        //    
        //}
    }
}