using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ButtonNavigator : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    private int currentIndex = 0;

    void Start()
    {
        // Set the first button as the initially selected one
        SelectButton(currentIndex);
    }

    void Update()
    {
        // Get D-pad input
        Vector2 dpadInput = new Vector2(Input.GetAxis("DPadHorizontal"), Input.GetAxis("DPadVertical"));

        // Check if there is any D-pad input
        if (dpadInput.magnitude > 0.5f)
        {
            // Determine the direction of the D-pad input
            float angle = Vector2.SignedAngle(Vector2.up, dpadInput);
            if (angle < 45f && angle > -45f) // Up
            {
                ChangeSelectedButton(-1);
            }
            else if (angle > 45f && angle < 135f) // Right
            {
                // Additional logic for right if needed
            }
            else if (angle < -45f && angle > -135f) // Left
            {
                // Additional logic for left if needed
            }
            else // Down
            {
                ChangeSelectedButton(1);
            }
        }
    }

    void ChangeSelectedButton(int direction)
    {
        // Deselect the current button
        buttons[currentIndex].OnDeselect(null);

        // Update the current index
        currentIndex = (currentIndex + direction + buttons.Count) % buttons.Count;

        // Select the new button
        SelectButton(currentIndex);
    }

    void SelectButton(int index)
    {
        buttons[index].OnSelect(null);
    }
}
