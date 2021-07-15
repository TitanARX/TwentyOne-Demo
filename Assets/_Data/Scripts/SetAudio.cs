using Com.FastEffect.DataTypes;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Audio;

[RequireComponent(typeof(Toggle))]
public class SetAudio : MonoBehaviour
{
    private Toggle audioToggle;
    public BoolValue muted;
    public AudioMixerGroup muteGroup;
    public void Awake()
    {
        audioToggle = GetComponent<Toggle>();
    }

    void Start() {
        audioToggle.SetIsOnWithoutNotify(!muted.Value);
        ChangeToggle(muted.Value);
    }

    public void ChangeToggle(bool value)
    {
        string volumeKey = $"{muteGroup.name} Volume";

        Debug.Log("Toggle Fired");

        if (value) {
            // unmute bgm
            muteGroup.audioMixer.SetFloat(volumeKey, -31f);
        } else {
            // mute bgm
            muteGroup.audioMixer.SetFloat(volumeKey, -120f);
        }
    }

}
