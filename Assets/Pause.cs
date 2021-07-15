using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    void Start()
    {
        Time.timeScale = 1;

        pausePanel.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(pausePanel.activeInHierarchy)
            {
                ContinueGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;

        //Disable scripts that still work while timescale is set to 0
    }

    public void ContinueGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        //enable the scripts again
    }
}
