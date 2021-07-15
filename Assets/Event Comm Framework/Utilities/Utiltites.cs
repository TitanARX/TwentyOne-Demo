using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Titan
{
    public static class Utiltites
    {
        public static float Map(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static float currentTime;
        public static float LerpTime = 1f;

        public static float SmoothLerp(float value, float targetValue, float speed)
        {
          

            currentTime = 0;

            if (targetValue >= 0)
            {
                currentTime += Time.deltaTime * speed;

                if (currentTime > LerpTime)
                {
                    currentTime = LerpTime;
                }

                float t = currentTime / LerpTime;

                value = Mathf.Lerp(value, targetValue, t);
            }
            else
            {
                currentTime += Time.deltaTime;

                if (currentTime > LerpTime)
                {
                    currentTime = LerpTime;
                }

                float t = currentTime / LerpTime;

                value = Mathf.InverseLerp(value, targetValue, t);
            }

            return value;
        }
    }



}
