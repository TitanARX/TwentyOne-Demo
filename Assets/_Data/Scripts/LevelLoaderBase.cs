using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelLoaderBase
{
    public string LevelToLoad { get; set; }
    public bool isLoading { get; set; }

    public LevelLoaderBase(string level, bool isLoading)
    {
        this.LevelToLoad = level;
        this.isLoading = isLoading;
    }

    public void LoadLevel()
    {
        if (!isLoading)
        {
            Debug.LogFormat("Now Loading: {0}", LevelToLoad);

            SceneManager.LoadScene(LevelToLoad);

            isLoading = true;
        }
    }

    
}
