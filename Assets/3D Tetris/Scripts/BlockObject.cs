//using System.Diagnostics;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class BlockSettleArgs : EventArgs 
{
    public int Value;
    public Vector2 Pos;

    public BlockSettleArgs(int value, Vector2 pos)
    {
        this.Value = value;
        this.Pos = pos;
    }
}

public enum BlockState { Dropping, Settled }

public class BlockObject : MonoBehaviour
{
    public BlockState state = BlockState.Dropping;

    [Tooltip("The percentage chance that a destroyer wildcard will spawn"), Range(0, 100)]
    public int DestroyerWildcardChance = 10;
    [Space]
    [Tooltip("The minimum and maximum value for random point assignment")]
    public Vector2 RanGenRange = new Vector2(1, 9);
    [Space]
    [Tooltip("The point value assigned to this block")]
    public int Point;

    private float lastFall = 0f;

    private bool nextSpawnedBlock=false;

    [Header("DELETE THIS")]
    public bool isDestroyerWildcard;

    [Header("Movement Events")]
    public UnityEvent L_MovementEvent;
    public UnityEvent R_MovementEvent;
    public UnityEvent D_MovementEvent;

    [Header("Block Attributes")]
    public FloatVariable FallSpeed;
    public IntVariable WildcardChance;

    public Solver solver => GameObject.FindObjectOfType<Solver>();

    public EventHandler<BlockSettleArgs> OnSettle = (sender, e) => { };


    private void OnEnable()
    {
        state = BlockState.Dropping;

        int willDestroyerSpawn = UnityEngine.Random.Range(0, 100);

        DestroyerWildcardChance = WildcardChance.Value;

        if (willDestroyerSpawn > DestroyerWildcardChance)
        {
             MatrixGrid.isWildCard=false;
                Point = UnityEngine.Random.Range((int)RanGenRange.x, (int)RanGenRange.y);
        }
        else
        {
            MatrixGrid.isWildCard=true;
                Point = UnityEngine.Random.Range(10,13);           //  < 10  - for single colum>    < 11 -  for single row >   < 12  -   for row and column >
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

        if (GridPositionAvailable())

            UpdateAvailableGridPositions();
        else
            transform.position += new Vector3(1, 0, 0);
    }

    public void MoveBlockRight()
    {
        R_MovementEvent.Invoke();

        transform.position += new Vector3(1, 0, 0);

        if (GridPositionAvailable())

            UpdateAvailableGridPositions();
        else
            transform.position += new Vector3(-1, 0, 0);
    }

    public int solves = 0;

    public void MoveBlockDown()
    {
        //External Events
        D_MovementEvent.Invoke();

        //Move Block Down
        transform.position += new Vector3(0, -1, 0);

        //Check if blocks Current Position is Empty
        if (GridPositionAvailable())
        {
            //Update Grid Matrix as Block Moves
            UpdateAvailableGridPositions();
        }
        else
        {            
            //Bump Cube Up 1 Space
            transform.position += new Vector3(0, 1, 0);

            //Gather Last point Before Destroying Block
            //solver.ReferencePosition = transform.position;

            //Check if the target value (21) has been met

            //Debug.Log("Checking > Vertical > Horizontal > Right Diagonal > Left Diagonal Values");

            //bool targetValueReached = MatrixGrid.CheckAllDirectionTargetReach((int)transform.position.x, (int)transform.position.y);

            //Update Grid Positions after Block Settles

            UpdateAvailableGridPositions();

            nextSpawnedBlock = true;

            OnSettle?.Invoke(this, new BlockSettleArgs(Point, transform.position));

            enabled = false;

            #region Deprecate Soon


            /*
            if (targetValueReached && !nextSpawnedBlock)
            {
                solver.SumGridRows();

                if (!solver.DoubleCheck())
                {
                    Debug.Log("No Chaining Detected");

                    //FindObjectOfType<BlockSpawner>().CallInstantiateCouritine((int)transform.position.y);
                }
                else
                {
                    Debug.Log("Chaining Detected");
                }

                nextSpawnedBlock = true;
                //Wait for 2 seconds after match then spawn new cube

                //Disable Block
                enabled = false;

            }
            else if (!nextSpawnedBlock)
            {
                Debug.Log(solver.SumGridRows());

                nextSpawnedBlock = true;

                //Spawn Block - Value not met
                FindObjectOfType<BlockSpawner>().SpawnBlock((int)transform.position.y);

                //Disable Block
                enabled = false;
            }
            */

            #endregion

        }

        lastFall = Time.time;
    }
    #endregion

    private bool GridPositionAvailable()
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

    public void UpdateAvailableGridPositions()
    {

        for (int y = 0; y < MatrixGrid.heightRows; ++y)
        {
            for (int x = 0; x < MatrixGrid.widthColumns; ++x)
            {
                if (MatrixGrid.grid[x, y] != null)
                {
                    if (MatrixGrid.grid[x, y].parent == transform)
                    {
                        MatrixGrid.grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform child in transform)
        {
            Vector2 roundedPosition = MatrixGrid.RoundVector(child.position);
            
            MatrixGrid.grid[(int)roundedPosition.x, (int)roundedPosition.y] = child;
        }
    }

  
}