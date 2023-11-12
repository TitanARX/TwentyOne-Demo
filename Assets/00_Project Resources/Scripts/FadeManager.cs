using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using JetBrains.Annotations;

public class FadeManager
{
    public bool activelyFading { get; set; }
    public CanvasGroup GetCanvasGroup { get; set; }

    public FadeManager(bool isFading, CanvasGroup canvasGroup)
    {
        this.activelyFading = isFading;
        this.GetCanvasGroup = canvasGroup;
    }

    public void OnFade(float fadeTargetAlpha, float fadeSpeed)
    {
        activelyFading = true;

        Tween fadeSequence = GetCanvasGroup.DOFade(fadeTargetAlpha, fadeSpeed);
        fadeSequence.OnComplete(ResetOnFadeBool);

        /*
        if (activelyFading)
        {
            return;
        }
        else
        {
            activelyFading = true;

            Tween fadeSequence = GetCanvasGroup.DOFade(fadeTargetAlpha, fadeSpeed);
            fadeSequence.OnComplete(ResetOnFadeBool);
        }
        */
    }

    public void TransitionFade(float delay, float fadeSpeed)
    {
        if (activelyFading)
        {
            return;
        }
        else
        {
            Debug.Log("Trans Fade");

            activelyFading = true;

            Sequence fadeSequence = DOTween.Sequence();

            fadeSequence.Append(GetCanvasGroup.DOFade(1, fadeSpeed))
            .AppendInterval(delay)
            .Append(GetCanvasGroup.DOFade(0, fadeSpeed));

            fadeSequence.OnComplete(ResetOnFadeBool);
        }
    }

    public void ResetOnFadeBool()
    {
        activelyFading = false;
    }
}
