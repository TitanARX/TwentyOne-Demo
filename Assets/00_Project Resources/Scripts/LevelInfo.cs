using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TitanARXObject", menuName = "TitanARX/LevelInfoScriptableObject", order = 1)]
public class LevelInfo : ScriptableObject
{
    public Color levelColor = Color.white;
    public int linesToClear = 7;
    public float levelSpeed = 1f;
    public int bombProbPercentage = 50;
}
