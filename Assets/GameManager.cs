using Com.FastEffect.DataTypes;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public List<LevelInfo> GetLevelAttribute = new List<LevelInfo>();

    public List<Material> GetMaterials = new List<Material>();

    public IntVariable GetCurrentLevel;

    public IntVariable _targetVar;

    public IntValue _linesToClear;

    public IntValue _linesVar;

    [Header("Block Attributes")]
    public FloatVariable FallSpeed;
    public IntVariable WildcardChance;

    [Header("Level Progress Events")]
    public UnityEvent ProgressEvent;

    public void SetCurrentLevel()
    {
        GetCurrentLevel.Value = 0;
    }

    public Vector2 _pulseOrigin = Vector2.zero;

    public float pulseRange = 2f;

    public void UpdateLevelAtttributes()
    {
        string shaderValueID = "_HologramTint";

        var id = Shader.PropertyToID(shaderValueID);

        foreach (Material mat in GetMaterials)
        {
            mat.SetColor(id, GetLevelAttribute[GetCurrentLevel.Value].levelColor);

            _linesToClear.Value = GetLevelAttribute[GetCurrentLevel.Value].linesToClear;

            FallSpeed.value = GetLevelAttribute[GetCurrentLevel.Value].levelSpeed;

            WildcardChance.Value = GetLevelAttribute[GetCurrentLevel.Value].bombProbPercentage;

            if (GetCurrentLevel.Value > 0)
            {
                ProgressEvent.Invoke();
            }

            PulseNearbyBlocks(_pulseOrigin);
        }
    }



    public void PulseNearbyBlocks(Vector2 origin)
    {
        // Find all objects with the BlockObject script in the scene
        BlockObject[] allBlockObjects = FindObjectsOfType<BlockObject>();

        foreach (BlockObject blockObject in allBlockObjects)
        {
            // Check the distance between the two objects before pulsing
            float distance = Vector3.Distance(origin, blockObject.transform.position);

            if (distance <= pulseRange)
            {
                // Tween the material value to 0 over 0.3f seconds and back to 1
               blockObject._renderer.material.DOFloat(0.0f, "_HologramFade", 0.3f)
                    .SetEase(Ease.InOutQuad) // You can choose a different easing function if desired
                        .OnComplete(() =>
                        {
                            blockObject._renderer.material.DOFloat(1, "_HologramFade", 0.3f).SetEase(Ease.InOutQuad);

                        });
            }
        }
    }


    public bool ShouldProgressLevel()
    {
        return _linesVar.Value == GetLevelAttribute[GetCurrentLevel.Value].linesToClear;
    }

    public Color GetLevelColor()
    {
        return GetLevelAttribute[GetCurrentLevel.Value].levelColor;
    }

    public void UpdateLinesToClear ()
    {
      

        if(_linesVar.Value != GetLevelAttribute[GetCurrentLevel.Value].linesToClear)
        {
            return;
        }
        else
        {
            GetCurrentLevel.Value += 1;

            UpdateLevelAtttributes();
        }
    }

}
