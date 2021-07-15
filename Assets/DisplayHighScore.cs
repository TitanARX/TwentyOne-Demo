using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHighScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI GetText;

    private SaveSystem GetSaveSystem;

    // Start is called before the first frame update
    void Start()
    {
        GetSaveSystem = new SaveSystem();
    }

    public void LoadSavedScore()
    {
        GetSaveSystem.FetchData(GetText);
    }
}
