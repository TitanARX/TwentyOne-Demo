using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugStuff : MonoBehaviour
{
    
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

}
