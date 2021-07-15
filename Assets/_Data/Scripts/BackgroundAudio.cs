using Com.FastEffect.DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BackgroundAudio
{
    public bool EnableBackgroundMusic { get; set; }

    public AudioClip BackgroundMusicClip { get; set;}

    private AudioSource AudioSource { get; set; }

    public AudioMixerGroup MixerGroup { get; set; }

    public Toggle Toggle { get; set;}

    public BackgroundAudio(bool enable, AudioClip music, AudioSource audioSource, AudioMixerGroup mixerGroup, Toggle toggle)
    {
        this.EnableBackgroundMusic = enable;
        this.BackgroundMusicClip = music;
        this.AudioSource = audioSource;
        this.MixerGroup = mixerGroup;
        this.Toggle = toggle;
    }

    public void PlayBackgroundAudio()
    {
        AudioSource.playOnAwake = true;

        AudioSource.loop = true;

        AudioSource.clip = BackgroundMusicClip;

        AudioSource.outputAudioMixerGroup = MixerGroup;

        if (!EnableBackgroundMusic)
        {
            return;
        }
        else
        {
            if ( AudioSource.clip != null && !AudioSource.isPlaying)
            {
                AudioSource.Play();

                //Debug.Log("Playing Background Audio " + BackgroundMusicClip.name);
            }
        }
    }

    public void Mute(bool b)
    {
        AudioSource.mute = b;
    }
}
