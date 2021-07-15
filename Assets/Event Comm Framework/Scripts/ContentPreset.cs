using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Content Loader", menuName = "TitanARX/Custom Content Preset")]
public class ContentPreset : ScriptableObject
{
    public List<VideoClip> Content;

}
