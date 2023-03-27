using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public IntVariable SceneToLoad;

    public List<UnityEvent> PreLoadingEvents = new List<UnityEvent>();

    public List<UnityEvent> PostLoadingEvents = new List<UnityEvent>();

    public LevelLoader(int scene)
    {
        this.SceneToLoad.Value = scene;
    }

    public void LoadLevel(int i)
    {
        StartCoroutine(AsyncLoad(i));
    }

    IEnumerator AsyncLoad(int i)
    {
        yield return new WaitForSeconds(1.5f);

        PreLoadingEvents[0].Invoke();

        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(i);

        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            yield return null;
        }

        PostLoadingEvents[0].Invoke();

    }

}
