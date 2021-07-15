using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFX : MonoBehaviour
{
    private readonly Camera MyCamera;

    public CameraFX(Camera camera)
    {
        this.MyCamera = camera;
    }

    public void ShakeCamera(float duration, float strength)
    {
        #region Explanation

        /*DOShakePosition(float duration, float / Vector3 strength, int vibrato, float randomness, bool snapping, bool fadeOut)

        Shakes a Transform's localPosition with the given values.

        duration - The duration of the tween.

        Strength - The shake strength. Using a Vector3 instead of a float lets you choose the strength for each axis.


        vibrato - Indicates how much will the shake vibrate.

        Randomness - Indicates how much the shake will be random(0 to 180 - values higher than 90 kind of suck, so beware).Setting it to 0 will shake along a single direction.

        Snapping - If TRUE the tween will smoothly snap all values to integers.
        
        fadeOut(default: true) - If TRUE the shake will automatically fadeOut smoothly within the tween's duration, otherwise it will not.*/
        
        #endregion

        Vector3 shakeVector = new Vector3(strength, strength, 0);

        MyCamera.transform.DOShakePosition(duration, shakeVector, 5, 10f, false, true);
    }
}
