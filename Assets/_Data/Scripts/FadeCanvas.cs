using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityScript.Steps;

[RequireComponent(typeof(CanvasGroup))]
public class FadeCanvas : MonoBehaviour
{
    public bool fading = false;

    public FadeManager fadeManager;

    CanvasGroup canvas;

    
    public void Start()
    {
        canvas = GetComponent<CanvasGroup>();

        fadeManager = new FadeManager(fading, canvas);    
    }

    public void Fade(string values)
    {
        string[] val = values.Split(',');

        string targetRaw = val[0];

        string speedRaw = val[1];

        float target = float.Parse(targetRaw);

        float speed = float.Parse(speedRaw);

        fadeManager.OnFade(target, speed);
    }

    public void TransitionFade(string values)
    {
        string[] val = values.Split(',');

        string delayRaw = val[0];

        string speedRaw = val[1];

        float delay = float.Parse(delayRaw);

        float speed = float.Parse(speedRaw);

        fadeManager.TransitionFade(delay , speed);
    }



}
