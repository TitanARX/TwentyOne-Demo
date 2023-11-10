using UnityEngine;

public class TetrisCube : MonoBehaviour
{
    private const float gridCellSize = 1f;
    private const int gridWidth = 7;
    private const int gridHeight = 7;
    private const float cubeDropSpeed = 1f;
    private const float quickDropSpeed = 0.25f;
    private const float spawnDelay = 0.35f;

    private Vector2Int currentPosition;
    private float timeSinceLastMove;
    private bool isCubeFalling;
    private bool isQuickDropActive;
    private bool isSpawningNextCube;
    private float spawnTimer;

    private void Start()
    {
        // Spawn the gameCube at the upper center of the grid
        currentPosition = new Vector2Int(gridWidth / 2, gridHeight - 1);
        SpawnCube();
    }

    private void Update()
    {
        if (isCubeFalling)
        {
            // Move the cube down automatically or by quick drop
            timeSinceLastMove += Time.deltaTime;
            float currentDropSpeed = isQuickDropActive ? quickDropSpeed : cubeDropSpeed;
            if (timeSinceLastMove >= 1f / currentDropSpeed)
            {
                timeSinceLastMove = 0f;
                MoveCubeDown();
            }
        }

        // Move the cube left or right based on arrow key input
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCubeHorizontal(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCubeHorizontal(Vector2Int.right);
        }

        // Enable quick drop when holding the down arrow
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isQuickDropActive = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isQuickDropActive = false;
        }

        // Spawn the next cube after a delay
        if (isSpawningNextCube)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnDelay)
            {
                spawnTimer = 0f;
                isSpawningNextCube = false;
                SpawnCube();
            }
        }
    }

    private void SpawnCube()
    {
        GameObject gameCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameCube.name = "gameCube";
        gameCube.transform.position = CalculateWorldPosition(currentPosition);
        isCubeFalling = true;
    }

    private void MoveCubeDown()
    {
        Vector2Int nextPosition = currentPosition + Vector2Int.down;

        if (IsPositionValidAndEmpty(nextPosition))
        {
            transform.position = CalculateWorldPosition(nextPosition);
            currentPosition = nextPosition;
        }
        else
        {
            // Settle the cube
            isCubeFalling = false;
            isSpawningNextCube = true;
        }
    }

    private void MoveCubeHorizontal(Vector2Int direction)
    {
        Vector2Int nextPosition = currentPosition + direction;

        if (IsPositionValidAndEmpty(nextPosition))
        {
            transform.position = CalculateWorldPosition(nextPosition);
            currentPosition = nextPosition;
        }
    }

    private Vector3 CalculateWorldPosition(Vector2Int gridPosition)
    {
        float x = (gridPosition.x - (gridWidth - 1) * 0.5f) * gridCellSize;
        float y = (gridPosition.y - (gridHeight - 1) * 0.5f) * gridCellSize;
        return new Vector3(x, y, 0f);
    }

    private bool IsPositionValidAndEmpty(Vector2Int gridPosition)
    {
        // Check if the grid position is within bounds
        if (gridPosition.x < 0 || gridPosition.x >= gridWidth ||
            gridPosition.y < 0 || gridPosition.y >= gridHeight)
        {
            return false;
        }

        // Check if the grid position is occupied by another cube
        Collider[] colliders = Physics.OverlapBox(CalculateWorldPosition(gridPosition), new Vector3(gridCellSize * 0.45f, gridCellSize * 0.45f, gridCellSize * 0.45f));
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject.CompareTag("Cube"))
            {
                // Occupy the current unoccupied position
                OccupyPosition();
                return false;
            }
        }

        return true;
    }

    private void OccupyPosition()
    {
        // Occupy the current unoccupied position in the grid
        GameObject gridCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gridCube.name = "gridCube";
        gridCube.tag = "Cube";
        gridCube.transform.position = CalculateWorldPosition(currentPosition);
    }
}
