using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[SerializeField]
public class ContentPresetReference : MonoBehaviour
{
    public ContentPreset preset;

    public List<VideoClip> Videos
    {
       get
        {
         return preset.Content;
        }
        set
        {
            preset.Content = value;
        }
    }

}
