using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof (AudioSource))]
public class BootScreen : MonoBehaviour
{

    AudioSource aSource => GetComponent<AudioSource>();
    public AudioClip _biosBeep;
    public TextMeshProUGUI bootTextPrefab; // Reference to the TextMeshProUGUI prefab
    public Transform bootTextContainer; // Parent object for organizing the boot text objects
    public float underscoreFlashTime = 0.5f; // Time in seconds for the underscore flash
    public List<string> bootMessages = new List<string>();
    private int currentMessageIndex = 0;

    void Start()
    {
        StartCoroutine(BootSequence());
    }

    IEnumerator BootSequence()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(1.75f);

        while (currentMessageIndex < bootMessages.Count)
        {
            // Instantiate a new bootTextPrefab for each message
            TextMeshProUGUI newBootText = Instantiate(bootTextPrefab, bootTextContainer);

            // Set the initial text to an empty string
            newBootText.text = "";

            // Flash underscore for 2 seconds using DOTween
            yield return FlashUnderscoreCoroutine(0.75f, newBootText);


            if (currentMessageIndex == bootMessages.Count - 1)
            {

                aSource.PlayOneShot(_biosBeep);
            }


            // Display the current message
            newBootText.text = bootMessages[currentMessageIndex];

            // Wait for 2 seconds before moving to the next message
            yield return new WaitForSeconds(0.3f);


            // Move to the next message
            currentMessageIndex++;
        }

        yield return new WaitForSeconds(1.75f);


        SceneManager.UnloadSceneAsync("Boot Screen");

        asyncLoad.allowSceneActivation = true;
    }

    IEnumerator FlashUnderscoreCoroutine(float duration, TextMeshProUGUI textObject)
    {
        char underscoreChar = '_';
        string originalText = textObject.text; // Save the original text

        // Use DOTween to create the blinking underscore animation
        var tweener = DOTween.To(() => textObject.text, x => textObject.text = x, originalText + " " + underscoreChar, underscoreFlashTime)
            .SetLoops((int)(duration / (2 * underscoreFlashTime)), LoopType.Yoyo);

        // Wait for the total duration of the animation
        yield return new WaitForSeconds(duration);

        // Stop the tweener to prevent potential issues
        tweener.Kill();
    }

}
