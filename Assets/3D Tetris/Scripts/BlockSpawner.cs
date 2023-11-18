using UnityEngine;
using System.Collections;
using System;

public enum SpawnerState { Initializing, Ready, Started, Paused, Stopped }

public class SpawnEventArgs : EventArgs { }

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
        SpawnBlockSignal += SpawnNextBlock;
    }

    private void OnDisable()
    {
        SpawnBlockSignal -= SpawnNextBlock;
    }

    public void StartSpawner()
    {
        _state = SpawnerState.Ready;

        SpawnBlock(0);
    }

    public void PauseSpawner()
    {
        _state = SpawnerState.Paused;
    }

    public void StopSpawner()
    {
        _state = SpawnerState.Stopped;
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
            if (SpawnerPositionValid() && HasNotReachedTop(prevY))
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

                Debug.Log("Invalid Pos");

                // Check if any point total is reached in the grid
                /*
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
                }*/

                _state = SpawnerState.Stopped;

                MatrixGrid.Instance.GameOver();
            }
        }
    }

    public void SpawnNextBlock(object sender, EventArgs args)
    {
        StartSpawner();
    }

    bool SpawnerPositionValid()
    {
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        return MatrixGrid.grid[x, y] == null;
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




}