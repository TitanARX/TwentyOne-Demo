﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPointTotal : MonoBehaviour
{
    int BlockObjectPointValue;

    public Renderer rend;

    public List<Material> TextureBank = new List<Material>();

    private void Start()
    {
        BlockObjectPointValue = GetComponent<BlockObject>().PointValue;

        rend = rend.GetComponent<Renderer>();

        if (rend == null)
            return;

        SetBlockTexture(BlockObjectPointValue);
    }

    public void SetBlockTexture(int pointvalue)
    {
        switch (pointvalue)
        {
            case 1:
                Debug.Log("Point value is 1");
                rend.sharedMaterial = TextureBank[0];
                // Perform actions for point value 1
                break;

            case 2:
                Debug.Log("Point value is 2");
                rend.sharedMaterial = TextureBank[1];
                // Perform actions for point value 2
                break;

            case 3:
                Debug.Log("Point value is 3");
                rend.sharedMaterial = TextureBank[2];
                // Perform actions for point value 3
                break;

            case 4:
                Debug.Log("Point value is 4");
                rend.sharedMaterial = TextureBank[3];
                // Perform actions for point value 4
                break;

            case 5:
                Debug.Log("Point value is 5");
                rend.sharedMaterial = TextureBank[4];
                // Perform actions for point value 5
                break;

            case 6:
                Debug.Log("Point value is 6");
                rend.sharedMaterial = TextureBank[5];
                // Perform actions for point value 6
                break;

            case 7:
                Debug.Log("Point value is 7");
                rend.sharedMaterial = TextureBank[6];
                // Perform actions for point value 7
                break;

            case 8:
                Debug.Log("Point value is 8");
                rend.sharedMaterial = TextureBank[7];
                // Perform actions for point value 8
                break;

            case 9:
                Debug.Log("Point value is 9");
                rend.sharedMaterial = TextureBank[8];
                // Perform actions for point value 9
                break;

            case 10:
                Debug.Log("Point value is 10");
                rend.sharedMaterial = TextureBank[9];
                // Perform actions for point value 10
                break;

            case 11:
                Debug.Log("Point value is 11");
                rend.sharedMaterial = TextureBank[10];
                // Perform actions for point value 11
                break;

            case 12:
                Debug.Log("Point value is 12");
                rend.sharedMaterial = TextureBank[11];
                // Perform actions for point value 12
                break;

            default:
                Debug.Log("Unhandled point value");
                // Perform actions for unhandled values
                break;
        }
    }


}


