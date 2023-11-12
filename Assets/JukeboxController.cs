using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using System;

public class JukeboxController : MonoBehaviour
{
    private UniversalControls inputActions;

    public List<AudioClip> audioClips;
    private AudioSource audioSource;

    [SerializeField]
    private int currentTrackIndex = 0;

    private void Awake()
    {
        inputActions = new UniversalControls();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        currentTrackIndex =  15;

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found. Please attach an AudioSource component to the GameObject.");
        }

        if (audioClips.Count == 0)
        {
            Debug.LogError("No audio clips provided. Please assign audio clips to the 'audioClips' list in the inspector.");
        }

        Play();
    }

    private void OnEnable()
    {
        inputActions.Jukebox.FWD.performed += SkipForward;
        inputActions.Jukebox.FWD.Enable();

        inputActions.Jukebox.RWD.performed += SkipBackward;
        inputActions.Jukebox.RWD.Enable();
    }

    private void OnDisable()
    {
        inputActions.Jukebox.FWD.performed -= SkipForward;
        inputActions.Jukebox.FWD.Disable();

        inputActions.Jukebox.RWD.performed -= SkipBackward;
        inputActions.Jukebox.RWD.Disable();
    }

    void Play()
    {
        if (audioClips.Count > 0)
        {
            audioSource.clip = audioClips[currentTrackIndex];

            // Fade in over 0.75 seconds
            audioSource.DOFade(1f, 1.75f)
                .OnStart(() =>
                {
                // This is called when the fade-in starts
                audioSource.Play();
                })
                .OnComplete(() =>
                {
                // This is called when the fade-in is complete
            });
        }
        else
        {
            Debug.LogError("No audio clips available to play.");
        }
    }


    public void Stop()
    {
        // Fade out over 0.75 seconds
        audioSource.DOFade(0f, 0.75f).OnComplete(() =>
        {
            audioSource.Stop();
        });
    }

    void Pause()
    {
        if (audioSource.isPlaying)
        {
            // Fade out over 0.75 seconds
            audioSource.DOFade(0f, 0.75f).OnComplete(() =>
            {
                audioSource.Pause();
            });
        }
        else
        {
            // Fade in over 0.75 seconds
            audioSource.DOFade(1f, 0.75f);
            audioSource.UnPause();
        }
    }

    void SkipForward(InputAction.CallbackContext obj)
    {
        audioSource.volume = 0;

        currentTrackIndex = (currentTrackIndex + 1) % audioClips.Count;
        Play();
    }

    void SkipBackward(InputAction.CallbackContext obj)
    {
        audioSource.volume = 0;

        currentTrackIndex = (currentTrackIndex - 1 + audioClips.Count) % audioClips.Count;
        Play();
    }
}
