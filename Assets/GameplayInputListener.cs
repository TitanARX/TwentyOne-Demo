using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputChangeEventArgs : EventArgs
{
    public InputAction Action;

    public  InputChangeEventArgs(InputAction action)
    {
        this.Action = action;
    }
}

public class GameplayInputListener : MonoBehaviour
{
    public event EventHandler<InputChangeEventArgs> OnChangeInput = (e, semder) => {}; 

    public UniversalControls inputActions;

    private void Awake()
    {
        inputActions = new UniversalControls();
    }

    private void Start()
    {
        InputChangeEventHandler(inputActions.Player.Input);
    }

    public void InputChangeEventHandler(InputAction action)
    {

    }
}
