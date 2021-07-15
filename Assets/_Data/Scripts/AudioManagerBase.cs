using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Com.FastEffect.DataTypes;


public abstract class AudioManagerBase : MonoBehaviour
{
    public BoolValue muted;

    public List<CustomAudioContent> audioContainer = new List<CustomAudioContent>();

    [Space]
    [Header("Audio Mixer Params")]
    [SerializeField]
    private string _mixerName = "Mixer";
    [SerializeField]
    private string _mixerGroupName = "BGM Adjust Volume"; 
    [SerializeField] 
    private float _mixerFadeInTime = 1.75f;

    int location = 0;

    bool IsAudioContainerValid => !audioContainer[location];

    public void PlayAudioClip(string clipIndex)
    {
            string[] convertedClipIndex = clipIndex.Split(',');

            string firstIndexParam = convertedClipIndex[0];
            location = int.Parse(firstIndexParam);

            string secondIndexParam = convertedClipIndex[1];
            int clip = int.Parse(secondIndexParam);

            string thirdIndexParam = convertedClipIndex[2];
            bool setLoop = bool.Parse(thirdIndexParam);

            if (IsAudioContainerValid)
            {
                Debug.LogErrorFormat("Missing Audio Clip @ audioContainer " + " [ " + location + " ] ");

                return;
            }
            else
            {
                AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();

                audioSource.outputAudioMixerGroup = audioContainer[location].mixerGroup;

                audioSource.loop = setLoop;

                audioSource.clip = audioContainer[location].AudioClips[clip];

                if (!setLoop)
                {
                    audioSource.PlayOneShot(audioSource.clip);
                }
                else
                {
                    audioSource.Play();
                }

                Sequence cleanupSequence = DOTween.Sequence();

                cleanupSequence.PrependInterval(audioSource.clip.length);

                cleanupSequence.OnComplete(() => RemoveUnusedAudioSources(audioSource, cleanupSequence));
            }
    }

    public void RemoveUnusedAudioSources(AudioSource audioSourceToRemove, Sequence sequenceToRemove)
    {
        if (audioSourceToRemove.loop)
        {
            return;
        }
        else
        { 
            Destroy(audioSourceToRemove);

            sequenceToRemove.Kill();
        }
    }

    public void MuteAudio(bool muteAudio, int audioSourceToMute = 0)
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        foreach (AudioSource source in audioSources)
        {
            source.mute = muteAudio;
        }
    }

    public void FadeAudio(float targetVolume)
    {

        Debug.Log("Working");

        if (!muted.Value)
        {
            Debug.LogWarning("Mixer muted");

            return;
        }
        else
        {
            AudioMixer mixer = Resources.Load(_mixerName) as AudioMixer;

            if (!mixer)
            {
                Debug.LogWarning("Mixer not Found");

                return;
            }
            else
            {
                Debug.Log("Mixer Found");

                StartCoroutine(FadeMixerGroup.StartFade(mixer, _mixerGroupName, _mixerFadeInTime, targetVolume));
            }
        }
    }

    
}
