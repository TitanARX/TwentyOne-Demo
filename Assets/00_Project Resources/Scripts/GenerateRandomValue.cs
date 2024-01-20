using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateRandomValue : MonoBehaviour
{
    // Adjust this percentage as needed
    public IntVariable superCubePercentage;

    [SerializeField]
    private List<int> valueList = new List<int>();
    [SerializeField]
    private int maxConsecutive = 1; // Adjust this value as needed
    private int consecutiveCount = 0;
    private int superPowerPercentage => superCubePercentage.Value;

    public List<TextMeshProUGUI> UpcomingNumberUI = new List<TextMeshProUGUI>();


    private void Start()
    {
        // Initialize the list with 10 random values
        for (int i = 0; i < 10; i++)
        {
            valueList.Add(GenerateValue());
        }

        UpcomingNumberUI[0].text = string.Empty;
        UpcomingNumberUI[1].text = string.Empty;
        UpcomingNumberUI[2].text = string.Empty;
    }

        public int GetNextCubeValue()
    {
        // Get the first value from the list
        int nextValue = valueList[0];

        // Remove the first value from the list
        valueList.RemoveAt(0);

        // Add a new value to the end of the list
        valueList.Add(GenerateValue());

        if(valueList.Count > 0)
        {
            Debug.Log("Ready Value Count");

            UpcomingNumberUI[0].text = valueList[0].ToString();
            UpcomingNumberUI[1].text = valueList[1].ToString();
            UpcomingNumberUI[2].text = valueList[2].ToString();

        }
        else
        {
            Debug.Log("Not Ready Value Count");

        }


        return nextValue;
    }

    public bool IsSuperPowerValue(int value)
    {
        return value >= 10 && value <= 12;
    }

    private int GenerateValue()
    {
        int randomValue = UnityEngine.Random.Range(0, 100);

        // Check if the random value is within the super power percentage
        if (randomValue < superPowerPercentage)
        {
            return UnityEngine.Random.Range(10, 13); // Super power values (10, 11, 12)
        }
        else
        {
            return UnityEngine.Random.Range(1, 9); // Regular values (1 to 9)
        }
    }
}
