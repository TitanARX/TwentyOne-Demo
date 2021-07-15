using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using Com.FastEffect.DataTypes;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    public IntValue currentPosition;

    public float scaleFactor;

    public List<RectTransform> BTN_Object = new List<RectTransform>();


    public void Update()
    {

    }

    public void SizeActiveBTN(int i)
    {
        if(!BTN_Object[i])
        {
            return;
        }
        else
        {
            currentPosition.Value = i;

            Vector3 v = Vector3.one * scaleFactor;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(BTN_Object[i].transform.DOPunchScale(v, .75f, 2, 1.0f));

            sequence.Play();
            
        }
    }

    public void SetBTNInteractivestate(bool b)
    {
        foreach (RectTransform _button in BTN_Object)
        {
            if(_button.TryGetComponent(out EventTrigger btn))
            {
                btn.enabled = b;
            }

        }
    }

    public void ResetBTNGroup(int i)
    {
        Vector3 v = Vector3.one;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(BTN_Object[i].transform.DOScale(v, .75f));

        sequence.Play();
    }
}
