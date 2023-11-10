using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SolverModel
{
    [Header("Grid Spawner")]
    public MatrixGrid Grid;

    [Header("Block Spawner")]
    public BlockSpawner Spawner;

}
