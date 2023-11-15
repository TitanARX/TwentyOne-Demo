using UnityEngine;
using System.Collections;
using System;

public enum SpawnerState { Initializing, Ready, Started, Paused, Stopped}

public class SpawnEventArgs : EventArgs{ }

public class BlockSpawner : MonoBehaviour
{
    public delegate void SpawnBlockHandler(object sender, EventArgs args);
    public static event SpawnBlockHandler SpawnBlockSignal;

    public SpawnerState _state = SpawnerState.Initializing;

    public GameObject BlockPrefab;

    private SaveSystem GetSaveSystem;

    public IntVariable scoreVar;

    public GameObject menu;


    private void Awake()
    {
        _state = SpawnerState.Initializing;

        menu.SetActive(false);

        GetSaveSystem = new SaveSystem();

        _state = SpawnerState.Ready;
    }

    private void OnEnable()
    {
        SpawnBlockSignal += HandleSpawnBlockSignal;
    }

    private void OnDisable()
    {
        SpawnBlockSignal -= HandleSpawnBlockSignal;
    }

    public void HandleSpawnBlockSignal(object sender, EventArgs args)
    {
        _state = SpawnerState.Ready;

        // Implement your spawning logic here
        SpawnBlock(0);
    }

    bool IsValidGridPosition()
    {
        Vector2 spawnerPosition = transform.position;

        if (!MatrixGrid.IsInsideBorder(spawnerPosition))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool HasNotReachedTop(int prevY)
    {
        if (prevY >= transform.position.y - 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SpawnBlock(int prevY)
    {
        if (_state == SpawnerState.Initializing || _state == SpawnerState.Stopped || _state == SpawnerState.Paused)
        {
            return;
        }
        else
        {
            // Is the position of the cube in a valid place and is the top reached
            if (IsValidGridPosition() && HasNotReachedTop(prevY))
            {
                _state = SpawnerState.Started;

                // Spawn Block
                GameObject block = Instantiate(BlockPrefab, transform.position, Quaternion.identity);

                BlockObject blockObj = block.GetComponent<BlockObject>();

                if (!blockObj)
                    return;

                //blockObj.OnSpawnBlock += OnSpawnSignal;

                // Subscribe The Current Blocks Settled Event to Spawn the Next Cube After Checks are made 
                blockObj.OnSettle += MatrixGrid.Instance.HandleBlockSettled;

                //blockObj.OnSettle += matrixGrid.OnBlockSettle;

                MatrixGrid.currentBlock = blockObj;

                // Comment this line to allow the spawner to stay paused after spawning
               _state = SpawnerState.Paused;
            }
            else
            {
                // Check if any point total is reached in the grid
                bool anyPointTotalReached = MatrixGrid.CheckAllDirectionTargetReach((int)MatrixGrid.currentBlock.transform.position.x, (int)MatrixGrid.currentBlock.transform.position.y);

                if (anyPointTotalReached)
                {
                    // Continue spawning new blocks
                    SpawnBlock(prevY);
                }
                else
                {
                    // Set spawner state to ready
                    _state = SpawnerState.Ready;
                }
            }
        }
    }





}
