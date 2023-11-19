using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.InputSystem;
using DG.Tweening;

public class BlockSettleArgs : EventArgs
{
    public int Value;
    public Vector2 Pos;
    public BlockObject block;


    public BlockSettleArgs(int value, Vector2 pos, BlockObject block)
    {
        this.Value = value;
        this.Pos = pos;
        this.block = block;
    }
}

public enum BlockState { Dropping, Settled }

public class BlockObject : MonoBehaviour
{
    //Event Fired to Signal Block has Landed
    public EventHandler<BlockSettleArgs> OnSettle = (sender, e) => { };

    //Generates Point Value
    private GenerateRandomValue _pointGenerator => FindObjectOfType<GenerateRandomValue>();

    public GameManager _manager => FindObjectOfType<GameManager>();

    public MeshRenderer _renderer => GetComponentInChildren<MeshRenderer>();

    //Controls for Cube
    private UniversalControls _inputActions;

    public InputAction QuickDrop;

    private float lastFall = 0f;

    private float originalFallSpeed;

    private bool _isQuickDropping = false;


    private float delayTimer = 0f;
    private bool hasDelayPassed = false;

    [Header("Ghost Cube")]
    public bool isGhostCubeDestroyed = false;
    public GameObject ghostCubePrefab;
    private GameObject ghostCubeInstance;

    [Header("Current Block State")]
    public BlockState state = BlockState.Dropping;

    [Header("Cube Relative Events")]
    [SerializeField] private int _pointValue;
    [SerializeField] private bool _isSuperBlock;

    [Header("Block Attributes")]
    public FloatVariable FallSpeed;

    [Header("Quick Hold Parameters")]
    private bool _canHold = true;
    private bool _isHolding = false;
    private float _holdTimer = 1.75f; // Hold timer duration in seconds
    private float _currentHoldTime = 0f;


    [Header("Cube Relative Events")]
    public UnityEvent L_MovementEvent;
    public UnityEvent R_MovementEvent;
    public UnityEvent D_MovementEvent;

    public int PointValue { get => _pointValue; set => _pointValue = value; }
    public bool IsSuperBlock { get => _isSuperBlock; set => _isSuperBlock = value; }

    private void Awake()
    {


        // Subscribe Controls
        _inputActions = new UniversalControls();
        _inputActions.Player.Movement.Enable();
        _inputActions.Player.Movement.performed += ProcessPlayerInput;

        // Quick Drop
        _inputActions.Player.QuickDrop.Enable();
        _inputActions.Player.QuickDrop.started += QuickDrop_started;
        _inputActions.Player.QuickDrop.canceled += QuickDrop_canceled;

        // Quick Hold
        _inputActions.Player.QuickHold.Enable();
        _inputActions.Player.QuickHold.started += StartHold;
        _inputActions.Player.QuickHold.canceled += StopHold;


    }


    private void OnEnable()
    {


        //Set Point Value
        PointValue = _pointGenerator.GenerateCubeValue();
        
        //Set State to Dropping on Inital Birth
        state = BlockState.Dropping;

        //Check if this is a SuperBlock Based on the Point Value
        IsSuperBlock = _pointGenerator.IsSuperPowerValue(PointValue);
        
        //Tell the Matrix Grid that this is a SuperBlock
        MatrixGrid.isSuperBlock = IsSuperBlock;

        originalFallSpeed = FallSpeed.value;

        //Debug.Log("Initial Fall Speed: " + originalFallSpeed);

    }

    private void OnDisable()
    {
        _inputActions.Player.QuickDrop.started -= QuickDrop_started;
        _inputActions.Player.QuickDrop.canceled -= QuickDrop_canceled;
        _inputActions.Player.Movement.performed -= ProcessPlayerInput;
        _inputActions.Player.QuickHold.started -= StartHold;
        _inputActions.Player.QuickHold.canceled -= StopHold;
    }

    private void OnDestroy()
    {
        _inputActions.Player.QuickDrop.started -= QuickDrop_started;
        _inputActions.Player.QuickDrop.canceled -= QuickDrop_canceled;
        _inputActions.Player.Movement.performed -= ProcessPlayerInput;
        _inputActions.Player.QuickHold.started -= StartHold;
        _inputActions.Player.QuickHold.canceled -= StopHold;


        if (ghostCubeInstance != null)
            Destroy(ghostCubeInstance);
    }

    private void Update()
    {
        // Disable Movement When Block's Position Settles
        if (state == BlockState.Settled)
            return;

        delayTimer += Time.deltaTime;

        if (!hasDelayPassed && delayTimer >= 0.5f)
        {
            hasDelayPassed = true;
            lastFall = Time.time;
        }

        if (!hasDelayPassed)
            return;

        if (_isHolding && _canHold)
        {
            _currentHoldTime += Time.deltaTime;

            if (_currentHoldTime >= _holdTimer)
            {
                _isHolding = false;
                _canHold = false;
                // TODO: Implement any visual feedback for releasing hold after the timer (if needed)
                FallSpeed.value = originalFallSpeed / 2f; // Set a higher fall speed after releasing hold
            }
        }
        else
        {
            if (_isQuickDropping)
            {
                // Adjust the movement during quick drop
                // For example, decrease the time interval between falls

                var quickDropSpeed = !_canHold ? FallSpeed.value : 0.1f; 

                if (Time.time - lastFall >= quickDropSpeed)
                {
                    MoveBlockDown();
                    lastFall = Time.time;
                }
            }
            else
            {
                // Standard movement logic
                if (Time.time - lastFall >= FallSpeed.value)
                {
                    MoveBlockDown();
                    lastFall = Time.time;
                }
            }
        }

        if (!ghostCubeInstance)
            return;

        if (state == BlockState.Dropping)
        {
            // Find the position for the ghost cube
            Vector3 ghostPosition = FindGhostPosition();

            // Check if ghostCubeInstance is null and instantiate it if needed
            // Update the position of the ghost cube
            ghostCubeInstance.transform.position = ghostPosition;

            // Show the ghost cube
            ghostCubeInstance.SetActive(true);

            // Check the distance between the ghost cube and the current cube
            float distance = Vector3.Distance(ghostCubeInstance.transform.position, transform.position);

            // If the distance is less than 1 meter, destroy the ghost cube
            if (distance < 2f)
            {
                DestroyGhostCube();
            }
        }
        else
        {
            // Hide the ghost cube when the block has settled
            ghostCubeInstance.SetActive(false);

            DestroyGhostCube();
        }
    }

    private void DestroyGhostCube()
    {
        // Destroy the ghost cube instance
        if (ghostCubeInstance != null)
        {
            Destroy(ghostCubeInstance);
            ghostCubeInstance = null;
            isGhostCubeDestroyed = true;
        }
    }


    private void QuickDrop_canceled(InputAction.CallbackContext obj)
    {
        _isQuickDropping = false;

        FallSpeed.value = originalFallSpeed;
    }

    private void QuickDrop_started(InputAction.CallbackContext obj)
    {
        _isQuickDropping = true;

        FallSpeed.value = 1.5f;
    }

    private void StartHold(InputAction.CallbackContext context)
    {
        if (!_isHolding)
        {
            _isHolding = true;
            _currentHoldTime = 0f;

            Debug.Log(" Holding");

            // TODO: Implement any visual feedback for holding (if needed)
        }
    }

    private void StopHold(InputAction.CallbackContext context)
    {
        if (_isHolding)
        {
            _isHolding = false;

            Debug.Log("Stop Holding");
            // TODO: Implement any visual feedback for releasing hold (if needed)
        }
    }

    private void ProcessPlayerInput(InputAction.CallbackContext context)
    {
        if (state == BlockState.Settled)
            return;

        Vector2 inputVector = context.ReadValue<Vector2>();

        if (inputVector.x > 0.5f)
        {
            MoveBlockRight();
        }
        else if (inputVector.x < -0.5f)
        {
            MoveBlockLeft();
        }
        else if (inputVector.y < -0.5f)
        {
            MoveBlockDown();
        }
    }

    public void MoveBlockLeft()
    {
        //FX Event
        L_MovementEvent.Invoke();
        transform.position += new Vector3(-1, 0, 0);

        if (IsGridPositionEmpty())
            UpdateAvailableGridPositions();
        else
            transform.position += new Vector3(1, 0, 0);
    }

    public void MoveBlockRight()
    {
        //FX Event
        R_MovementEvent.Invoke();
        transform.position += new Vector3(1, 0, 0);

        if (IsGridPositionEmpty())
            UpdateAvailableGridPositions();
        else
            transform.position += new Vector3(-1, 0, 0);
    }

    public void MoveBlockDown()
    {
        //FX Event
        D_MovementEvent.Invoke();
        transform.position += new Vector3(0, -1, 0);

        if (IsGridPositionEmpty())
        {
            UpdateAvailableGridPositions();
        }
        else
        {
            // Move the BlockObject up by one unit in the y-axis.
            transform.position += new Vector3(0, 1, 0);

            // Update the available grid positions based on the new position of the BlockObject.
            UpdateAvailableGridPositions();


            _manager._pulseOrigin = transform.position;


            // Set the state of the BlockObject to Settled, indicating that it has reached its final position.
            state = BlockState.Settled;

            // Invoke the OnSettle event, notifying any listeners that the block has settled.
            // Pass the BlockSettleArgs with the block's point value and final position.
            OnSettle?.Invoke(this, new BlockSettleArgs(PointValue, transform.position, this));



        }

        lastFall = Time.time;


        if (ghostCubeInstance == null)
        {
            if (isGhostCubeDestroyed)
                return;

            Vector3 ghostPosition = FindGhostPosition();

            ghostCubeInstance = Instantiate(ghostCubePrefab, ghostPosition, Quaternion.identity);
        }


    }

    private Vector3 FindGhostPosition()
    {
        Vector3 currentPosition = transform.position;
        Vector3 ghostPosition = new Vector3(currentPosition.x, 0, 0); // Default to one unit below

        RaycastHit hit;

        if (Physics.Raycast(currentPosition, Vector3.down, out hit))
        {
            Debug.Log(hit.transform.name);

            // Check if there's a block below the current position
            if (hit.transform.CompareTag("Block"))
            {
                Debug.Log("hit Cube");

                ghostPosition = new Vector3(currentPosition.x, hit.point.y + 0.5f, 0); // Position above the hit point
            }
            else
            {
                ghostPosition = new Vector3(currentPosition.x, 0, 0);
            }
         
        }

        return ghostPosition;
    }


    private bool IsGridPositionEmpty()
    {
        if (MatrixGrid.grid == null)
            return false;

        foreach (Transform child in transform)
        {
            //Cache and Round the Position of Each Bolock in the Grid
            Vector2 v = MatrixGrid.RoundVector(child.position);

            //Check if that Position is within the Grid and If not Return
            if (!MatrixGrid.IsInsideBorder(v))
                return false;

            //Return if the Position is Occupied
            if (MatrixGrid.grid[(int)v.x, (int)v.y] != null && MatrixGrid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }

        //Position Available
        return true;
    }

    public void UpdateAvailableGridPositions()
    {
        // Loop through each grid position
        for (int y = 0; y < MatrixGrid.heightRows; ++y)
        {
            for (int x = 0; x < MatrixGrid.widthColumns; ++x)
            {
                // Check if there is a block at the current grid position
                if (MatrixGrid.grid[x, y] != null)
                {
                    // Check if the parent of the block is the current BlockObject
                    if (MatrixGrid.grid[x, y].parent == transform)
                    {
                        // If yes, clear the grid position (the block has moved)
                        MatrixGrid.grid[x, y] = null;
                    }
                }
            }
        }

        // Loop through each child block of the BlockObject
        foreach (Transform child in transform)
        {
            // Round the position of the child block to the nearest integer
            Vector2 roundedPosition = MatrixGrid.RoundVector(child.position);

            // Update the grid position with the child block
            MatrixGrid.grid[(int)roundedPosition.x, (int)roundedPosition.y] = child;
        }
    }

  
       


       



    
}
