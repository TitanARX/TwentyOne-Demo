using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;
using JetBrains.Annotations;
using Com.FastEffect.DataTypes;

public class Scoremanager : MonoBehaviour
{
    private static Scoremanager _instance;

    public static Scoremanager Instance { get { return _instance; } }

    [Header("Scriptable Object Data Containers")]
    public IntVariable _scoreVar;
    public IntVariable _targetVar;
    public IntValue _linesVar;
    public FloatVariable _time;

    [Header("Game Info UI Elements")]
    [SerializeField]
    private TextMeshProUGUI _scoreHeader = null;
    [SerializeField]
    private TextMeshProUGUI _hiScoreHeader = null;
    [SerializeField]
    private TextMeshProUGUI _linesSolvedHeader = null;
    [SerializeField]
    private TextMeshProUGUI _targetScoreHeader = null;
    [SerializeField]
    private TextMeshProUGUI _timeHeader = null;
    
    public bool _timerActive;

    public void Update()
    {
        Timer();
    }

    public void InitScoreManager()
    {
        SetTimerState(false);

        ResetValues(0);     
    }

    public void SetTimerState(bool state)
    {
        _timerActive = state;
    }

    public void SetTimerValue(float value)
    {
        _time.value = value;

        _timeHeader.text = _time.value.ToString();

        _timerActive = true;
    }

    public void Timer()
    {
        if (_timerActive == false)
        {
            return;
        }
        else
        {
            _time.value = _time.value + Time.deltaTime;

            TimeSpan timeSpan = TimeSpan.FromSeconds(_time.value);

            _timeHeader.text = timeSpan.Minutes.ToString() + " : " + timeSpan.Seconds.ToString();
        }
    }

    public void AddPoints()
    {
        SetScoreValue(21);

        SetLinesCleared(1);
    }

    public void SetScoreValue(int value = 0)
    {
        _scoreVar.Value += value;
        _scoreHeader.text = _scoreVar.Value.ToString();
    }

    public void SetLinesCleared(int amount = 0)
    {
        _linesVar.Value += amount;
        _linesSolvedHeader.text = _linesVar.Value.ToString();
    }

    public void SetTargetValue(int target)
    {
        _targetVar.Value += target;

        _targetScoreHeader.text = _targetVar.Value.ToString();
    }

    public void ResetValues(int resetZero)
    {
        _scoreVar.Value = resetZero;
        _targetVar.Value = resetZero;
        _linesVar.Value = resetZero;
        _time.value = (float)resetZero;
    }

}
