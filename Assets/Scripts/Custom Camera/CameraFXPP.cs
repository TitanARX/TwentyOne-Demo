using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RetroLookPro;
using UnityEngine.Rendering.PostProcessing;


public class CameraFXPP : MonoBehaviour
{
    public PostProcessVolume processProfile;

    RLProNoise noise;
    RLProGlitch1 glitch;

    private void Start()
    {
        processProfile.profile.TryGetSettings(out noise);
        processProfile.profile.TryGetSettings(out glitch);


        noise.active = false;
        glitch.active = false;

    }

    public void OnToggleEffects()
    {
        StartCoroutine(ToggleEffects());
    }

    public IEnumerator ToggleEffects()
    {
        Debug.Log("Starting Of");

        noise.active = true;
        glitch.active = true;

        yield return new WaitForSeconds(3f);

        noise.active = false;
        glitch.active = false;

    }
}
