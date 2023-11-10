using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPointTotal : MonoBehaviour
{
    int BlockObjectPointValue;

    public Renderer rend;

    public List<Material> TextureBank = new List<Material>();

    public bool isWildcard;

    private void Start()
    {
        isWildcard = GetComponent<BlockObject>().isDestroyerWildcard;

        BlockObjectPointValue = GetComponent<BlockObject>().Point;

        //rend.material = TextureBank[BlockObjectPointValue - 1];

        rend = rend.GetComponent<Renderer>();

        if (rend != null)
        {
            if (!isWildcard)
            {
                rend.sharedMaterial = TextureBank[BlockObjectPointValue - 1];
            }
            else
            {
                rend.sharedMaterial = TextureBank[9];
            }
        }

        //Debug.Log("Chaning Texture to: "  + BlockObjectPointValue);
    }

}
