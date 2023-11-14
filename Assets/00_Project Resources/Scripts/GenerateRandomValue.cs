using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomValue : MonoBehaviour
{
    [SerializeField]
    private int lastValue = -1;
    [SerializeField]
    private int consecutiveCount = 0;
    [SerializeField]
    private int maxConsecutive = 1; // Adjust this value as needed
    [SerializeField]
    private int superPowerPercentage = 20; // Adjust this percentage as needed

    public int GenerateCubeValue()
    {
        int newValue;

        // Check if consecutive count is greater than the allowed limit
        if (consecutiveCount >= maxConsecutive)
        {
            // Generate a new value that is different from the last one
            do
            {
                newValue = GenerateValue();
            } while (newValue == lastValue);

            consecutiveCount = 0;
        }
        else
        {
            // Generate a new value
            newValue = GenerateValue();

            // Check if the new value is the same as the last one
            if (newValue == lastValue)
            {
                consecutiveCount++;
            }
            else
            {
                consecutiveCount = 0;
            }
        }

        lastValue = newValue;

        return newValue;
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
            return UnityEngine.Random.Range(0, 9); // Regular values (0 to 9)
        }
    }
}
