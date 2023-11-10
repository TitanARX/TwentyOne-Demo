using UnityEngine;
using System.Collections;

public class BlockSpawner : MonoBehaviour
{
    public GameObject BlockPrefab;

    private SaveSystem GetSaveSystem;

    public IntVariable scoreVar;

    public GameObject menu;

    public Solver _solver;

    private void Awake()
    {
        GetSaveSystem = new SaveSystem();
    }

    public void SubscribeToCheck(object sender, BlockSettleArgs args)
    {
        //Debug.Log("Block Responding has value of " + args.Value);

        //_solver.CheckGridColumns();

        //Debug.Log(_solver.CountRowsInColumn());

       

        SpawnBlock((int)args.Pos.y);

        //_solver.CheckGridTotal(MatrixGrid.grid);


        /*
        //This should check and return true : false and the we check based on this
        bool targetValueReached = MatrixGrid.CheckAllDirectionTargetReach((int)args.Pos.x, (int)args.Pos.y);

        if(targetValueReached)
        {
            Debug.Log("Target Value Reached");

            SubscribeToCheck(this, new BlockSettleArgs(0, Vector2.zero));

            //Should React to Value reached by destroying the cubes here then use recursion to check again.
        }
        else
        {
            SpawnBlock((int)args.Pos.y);
        }
        */
    }

    public void SpawnBlock(int prevY)
    {
        if (IsValidGridPosition() && HasNotReachedTop(prevY))
        {
            //Spawn Block
            GameObject block = Instantiate(BlockPrefab, transform.position, Quaternion.identity);

            if(block.transform.TryGetComponent(out BlockObject blockObject))
            {
                blockObject.OnSettle += SubscribeToCheck;
            }

            //Assign Spawned Block to Matrix Grid for solving check
            MatrixGrid.currentBlock = block.GetComponent<BlockObject>();
        }
        else
        {
            //Game Over - Top Reached
            menu.SetActive(true);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// This is where you would add any code you want to run when the block stack reaches the highest level. ///
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Debug.Log("END OF GAME REACHED! Block spawn position obstructed: Spawner disabled."); // Placeholder message to indicate the end.

            int tempScore = scoreVar.Value;

            GetSaveSystem.SaveData(tempScore);

        

            // Disable block spawning and movement
            MatrixGrid.currentBlock = null;
            enabled = false;
        }
    }

    bool IsValidGridPosition()
    {
        Vector2 spawnerPosition = transform.position;

        if (!MatrixGrid.IsInsideBorder(spawnerPosition))
            return false;

        return true;
    }

    bool HasNotReachedTop(int prevY)
    {
        if (prevY >= transform.position.y - 1)
            return false;
        else
            return true;
    }

    public void CallInstantiateCouritine(int Value)
    {
        StartCoroutine(InstantiateNewBlock(Value));
    }

    IEnumerator InstantiateNewBlock(int Value)
    {
        yield return new WaitForSeconds(2);

        SpawnBlock(Value);

    }
}
