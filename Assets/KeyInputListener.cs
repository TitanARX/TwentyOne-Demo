using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public enum KeyInputListenerState { Initializing, Listening, Invoked }

public class KeyInputListener : MonoBehaviour
{
    private UniversalControls _inputDefinitions;
    private InputAction _movementInput;

    private DualShockGamepad dualShock;

    public KeyInputListenerState state = KeyInputListenerState.Listening;

    public UnityEvent OnInputDetected;

    private void Awake()
    {
        _inputDefinitions = new UniversalControls();
    }

    private void Start()
    {
        state = KeyInputListenerState.Initializing;


        // Get the DualShockGamepad instance
        dualShock = InputSystem.GetDevice<DualShockGamepad>();

      
    }

    private void OnEnable()
    {
        _inputDefinitions.Player.Movement.performed += ProcessMovmentInput;
        _inputDefinitions.Player.Movement.Enable();

        _inputDefinitions.Player.Input.performed +=ProcessInput;
        _inputDefinitions.Player.Input.Enable();

    }

    private void OnDisable()
    {
        _inputDefinitions.Player.Movement.performed += ProcessMovmentInput;
        _inputDefinitions.Player.Movement.Disable();

        _inputDefinitions.Player.Input.performed -= ProcessInput;
        _inputDefinitions.Player.Input.Disable();
    }


    public void Init()
    {
        state = KeyInputListenerState.Listening;
    }

    private void ProcessInput(InputAction.CallbackContext context)
    {
        if (state == KeyInputListenerState.Invoked || state == KeyInputListenerState.Initializing)
            return;

        OnInputDetected?.Invoke();

        _movementInput.Disable();
        _inputDefinitions.Player.Input.Disable();


        // Check if the controller is connected
        if (dualShock != null)
        {
            Debug.LogWarning("DualShock controller found.");

            // Set the light bar color to blue
            dualShock.SetLightBarColor(Color.blue);
        }
        else
        {
            Debug.LogWarning("DualShock controller not found.");
        }


        state = KeyInputListenerState.Invoked;
    }


    private void ProcessMovmentInput(InputAction.CallbackContext context)
    {
        Debug.Log("Yoooo");

    }






}
