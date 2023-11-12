using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSystem : ISave
{
    public void FetchData(TextMeshProUGUI i)
    {
        if (PlayerPrefs.HasKey("HISCORE"))
        {
            int tempI = PlayerPrefs.GetInt("HISCORE");

            i.text = tempI.ToString();
        }
        else
        {
            i.text = 0.ToString();

            Debug.Log("No Key Exist");
        }
    }

    public void SaveData(int i)
    {
        Debug.LogErrorFormat("Saving Value of: " + i);

        if (PlayerPrefs.HasKey("HISCORE"))
        {
            int tempI = PlayerPrefs.GetInt("HISCORE");

           if(i > tempI)
            {
                PlayerPrefs.SetInt("HISCORE", i);
                PlayerPrefs.Save();

                Debug.Log("SAVING COMPLETE");
            }
           else
            {
                Debug.Log("Current Score is Not Greater than Hi Score");
            }
        }
        else
        {
            PlayerPrefs.SetInt("HISCORE", i);
            PlayerPrefs.Save();

            Debug.Log("SAVING COMPLETE");
        }

       
    }
}
