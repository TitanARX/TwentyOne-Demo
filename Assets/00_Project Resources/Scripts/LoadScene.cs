using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Events;

[System.Serializable]
public class FadeEventBase
{
    public UnityEvent fadeEvent;
}

public class LoadScene : MonoBehaviour
{
    public List<FadeEventBase> OnFadeEvent = new List<FadeEventBase>();

    private int eventID = 0;

    public void Open(string s)
    {
        SceneManager.LoadScene(s);
    }

    public void FadeSceneLoad(int i)
    {
        if (OnFadeEvent[i] == null)
        {
            return;
        }
        else
        {
            eventID = i;

            Sequence sequence = DOTween.Sequence();

            //set first event to universal behaviour
            OnFadeEvent[0].fadeEvent.Invoke();

            sequence.PrependInterval(1.75f);

            sequence.AppendCallback(OnFadeOut);
        }
    }

    public void OnFadeOut()
    {
        OnFadeEvent[eventID].fadeEvent.Invoke();
    }

}
