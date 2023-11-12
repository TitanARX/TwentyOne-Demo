using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum MenuVisiblity { Visible, Hidden}

public class Menu : MonoBehaviour
{
    [SerializeField]
    private UniversalControls inputActions;

    [SerializeField] 
    [Header("Pause Menu")]
    private GameObject pausePanel;

    public UnityEvent OnPause;
    public UnityEvent OnResume;

    void Awake()
    {
        Time.timeScale = 1;

        pausePanel.SetActive(false);

        inputActions = new UniversalControls();
    }

    private void OnEnable()
    {
        inputActions.UI.Pause.performed += SetMenuVisiblity;
        inputActions.UI.Pause.Enable();

    }

    private void OnDisable()
    {
        inputActions.UI.Pause.performed -= SetMenuVisiblity;
        inputActions.UI.Pause.Disable();
    }

    public void SetMenuVisiblity(InputAction.CallbackContext obj)
    {
        if (pausePanel.activeInHierarchy)
        {
            Resume();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);

        Time.timeScale = 0;

        OnPause?.Invoke();
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        
        Time.timeScale = 1;

        OnResume?.Invoke();
    }
}
