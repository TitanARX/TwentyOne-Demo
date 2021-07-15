using UnityEngine;
using System.Collections;

public class BlockSpawner : MonoBehaviour
{
    public GameObject BlockPrefab;

    private SaveSystem GetSaveSystem;

    public IntVariable scoreVar;

    public GameObject menu;

    private void Awake()
    {
        GetSaveSystem = new SaveSystem();
    }

    public void SpawnBlock(int prevY)
    {
        if (IsValidGridPosition() && HasNotReachedTop(prevY))
        {
            GameObject block = Instantiate(BlockPrefab, transform.position, Quaternion.identity);
            MatrixGrid.currentBlock = block.GetComponent<BlockObject>();
        }
        else
        {
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
        Vector2 v = transform.position;

        if (!MatrixGrid.IsInsideBorder(v))
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

    public void callInstantiateCouritine(int Value)
    {
        StartCoroutine(InstantiateNewBlock(Value));
    }

    IEnumerator InstantiateNewBlock(int Value)
    {
        yield return new WaitForSeconds(2);
        SpawnBlock(Value);

    }
}
