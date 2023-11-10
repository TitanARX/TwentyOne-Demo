using UnityEngine;
using UnityEngine.Events;


public enum KeyInputListenerState { Initializing, Listening, Invoked}

public class KeyInputListener : MonoBehaviour
{
    public KeyInputListenerState state = KeyInputListenerState.Listening;

    public UnityEvent OnInputDeteccted;

    public void Init()
    {
        state = KeyInputListenerState.Listening;
    }

    private void Start()
    {
        state = KeyInputListenerState.Initializing;
    }

    void Update()
    {
        // Check for input from any key except escape or function keys
        if (Input.anyKeyDown && !Input.GetKey(KeyCode.Escape) && !IsFunctionKey() && state == KeyInputListenerState.Listening)
        {
            // Handle the input here
            Debug.Log("Key pressed (except for escape or function keys)!");

            OnInputDeteccted?.Invoke();

            state = KeyInputListenerState.Invoked;
        }
    }

    bool IsFunctionKey()
    {
        // Check if any function key (F1 to F12) is pressed
        for (KeyCode keyCode = KeyCode.F1; keyCode <= KeyCode.F12; keyCode++)
        {
            if (Input.GetKey(keyCode))
            {
                return true;
            }
        }
        return false;
    }


}
