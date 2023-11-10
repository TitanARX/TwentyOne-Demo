using Com.FastEffect.DataTypes;
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
        }
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
