//using System.Diagnostics;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BlockObject : MonoBehaviour
{
    [Tooltip("The percentage chance that a destroyer wildcard will spawn"), Range(0, 100)]
    public int DestroyerWildcardChance = 10;
    [Space]
    [Tooltip("The minimum and maximum value for random point assignment")]
    public Vector2 RanGenRange = new Vector2(1, 9);
    [Space]
    [Tooltip("The point value assigned to this block")]
    public int Point;

    private float lastFall = 0f;

    private bool isNewBlockSpawn=false;

    [Header("DELETE THIS")]
    public bool isDestroyerWildcard;

    [Header("Movement Events")]
    public UnityEvent L_MovementEvent;
    public UnityEvent R_MovementEvent;
    public UnityEvent D_MovementEvent;

    [Header("Block Attributes")]
    public FloatVariable FallSpeed;
    public IntVariable WildcardChance;

    private void Start()
    {
        int willDestroyerSpawn = UnityEngine.Random.Range(0, 100);

        DestroyerWildcardChance = WildcardChance.Value;

        if (willDestroyerSpawn > DestroyerWildcardChance)
        {
             MatrixGrid.isWildCard=false;
            Point = Random.Range((int)RanGenRange.x, (int)RanGenRange.y);
        }
        else
        {
            MatrixGrid.isWildCard=true;
            Point =Random.Range(10,13);           //  < 10  - for single colum>    < 11 -  for single row >   < 12  -   for row and column >
        }
        
    }

    private void Update()
    {
        // Movement controls via GetKeyDowns
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveBlockLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveBlockRight();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= FallSpeed.value)
        {
            MoveBlockDown();
        }
    }

    #region Control movement
    public void MoveBlockLeft()
    {
        L_MovementEvent.Invoke();

        transform.position += new Vector3(-1, 0, 0);

        if (IsValidGridPosition())
            UpdateMatrixGrid();
        else
            transform.position += new Vector3(1, 0, 0);
    }

    public void MoveBlockRight()
    {
        R_MovementEvent.Invoke();

        transform.position += new Vector3(1, 0, 0);

        if (IsValidGridPosition())
            UpdateMatrixGrid();
        else
            transform.position += new Vector3(-1, 0, 0);
    }

    public void MoveBlockDown()
    {
        D_MovementEvent.Invoke();

        transform.position += new Vector3(0, -1, 0);

        if (IsValidGridPosition())
            UpdateMatrixGrid();
        else
        {
            transform.position += new Vector3(0, 1, 0);
            
            bool isReachTerget =  MatrixGrid.checkALLDirectionTargetReach((int)transform.position.x,(int)transform.position.y);

            UpdateMatrixGrid();
          
         if(isReachTerget&&!isNewBlockSpawn)
            {
                isNewBlockSpawn=true;
                 FindObjectOfType<BlockSpawner>().callInstantiateCouritine((int)transform.position.y);
                enabled = false;
            }
            else if(!isNewBlockSpawn)
            {
                isNewBlockSpawn=true;
                FindObjectOfType<BlockSpawner>().SpawnBlock((int)transform.position.y);
                enabled = false;
            }
         
           
        }

        lastFall = Time.time;
    }
    #endregion

    private bool IsValidGridPosition()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = MatrixGrid.RoundVector(child.position);

            if (!MatrixGrid.IsInsideBorder(v))
                return false;
            
            if (MatrixGrid.grid[(int)v.x, (int)v.y] != null && MatrixGrid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    private void UpdateMatrixGrid()
    {
        for (int y = 0; y < MatrixGrid.heightRows; ++y)
        {
            for (int x = 0; x < MatrixGrid.widthColumns; ++x)
            {
                if (MatrixGrid.grid[x, y] != null)
                {
                    if (MatrixGrid.grid[x, y].parent == transform)
                        MatrixGrid.grid[x, y] = null;
                }
            }
        }

        foreach (Transform child in transform)
        {
            Vector2 v = MatrixGrid.RoundVector(child.position);
            MatrixGrid.grid[(int)v.x, (int)v.y] = child;
        }
    }

  
}