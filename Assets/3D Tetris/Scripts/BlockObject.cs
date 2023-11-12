//using System.Diagnostics;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using UnityEngine.InputSystem;


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
    #region Private Vars and Backing Stores

    //Action For Block Settle State
    public EventHandler<BlockSettleArgs> OnSettle = (sender, e) => { };

    //Get Reference to the Solver
    private Solver solver => FindObjectOfType<Solver>();

    //Value Generator
    private GenerateRandomValue _pointGenerator => FindObjectOfType<GenerateRandomValue>();

    //Player Input
    private UniversalControls _inputActions;

    private float lastFall = 0f;

    //Refactor
    private bool spawnNextBlock = false;

    #endregion

    public BlockState state = BlockState.Dropping;

    [Header("Cube Relative Events")]
    [SerializeField]
    private int _pointValue;

    //Removing This - Deprecate Later
    [Header("Block Attributes")]
    public FloatVariable FallSpeed;  

    [Header("Cube Relative Events")]
    public UnityEvent L_MovementEvent;
    public UnityEvent R_MovementEvent;
    public UnityEvent D_MovementEvent;

    //Public Accessors
    public int PointValue { get => _pointValue; set => _pointValue = value; }

    private void Awake()
    {
        //Updated Input Structure
        _inputActions = new UniversalControls();
        _inputActions.Player.Movement.Enable();
        _inputActions.Player.Movement.performed += ProcessPlayerInput;     
    }


    private void OnEnable()
    {
        //Assign Point Value
        PointValue = _pointGenerator.GenerateCubeValue();

        //Set State
        state = BlockState.Dropping;

        //
        MatrixGrid.isSuperBlock = _pointGenerator.IsSuperPowerValue(PointValue);
    }

    private void ProcessPlayerInput(InputAction.CallbackContext context)
    {
        // Get the input vector from the context
        Vector2 inputVector = context.ReadValue<Vector2>();

        // Determine the direction based on the input vector
        if (inputVector.x > 0.5f) // Right
        {
            MoveBlockRight();
        }
        else if (inputVector.x < -0.5f) // Left
        {
            MoveBlockLeft();
        }
        else if (inputVector.y < -0.5f) // Down
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

            spawnNextBlock = true;

            OnSettle?.Invoke(this, new BlockSettleArgs(PointValue, transform.position));

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