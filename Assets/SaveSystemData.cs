using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveSystemData : MonoBehaviour
{
    private static SaveSystemData _instance;
    public static SaveSystemData Instance { get { return _instance; } }

    private SaveSystem GetSaveSystem;
    [Header("Hi Score UI Element")]
    public TextMeshProUGUI savedHiScoreHeader;
    [Header("Hi Score UI Element")]
    public IntVariable savedHiScore;

    private void Awake()
    {
        GetSaveSystem = new SaveSystem();
    }

    public void SetHiScore(TextMeshProUGUI displayScoreUI)
    {
        GetSaveSystem.FetchData(displayScoreUI);
    }

    public void SaveHiScore(int scoreToSave)
    {
        GetSaveSystem.SaveData(scoreToSave);
    }

}
