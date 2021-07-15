using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class CustomAudioContent : ScriptableObject
{
    public List<AudioClip> AudioClips = new List<AudioClip>();

    public AudioMixerGroup mixerGroup;

}
