using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System;

[System.Serializable]
public class GridSize
{
    public int rows = 11;
    public int columns = 6;
}

public class MatrixGrid : MonoBehaviour
{
    #region Singleton 
    // Private static instance variable
    private static MatrixGrid instance;

    // Public property to access the singleton instance
    public static MatrixGrid Instance
    {
        get
        {
            if (instance == null)
            {
                // Try to find an existing instance in the scene
                instance = FindObjectOfType<MatrixGrid>();

                // If no instance is found, create a new GameObject with the singleton script
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("MatrixGrid Singleton");
                    instance = singletonObject.AddComponent<MatrixGrid>();
                }
            }

            return instance;
        }
    }
    #endregion


    [SerializeField]
    [Header("Gameplay Board Size")]
    private GridSize _gridSize;

    public GridSize GridSizeReference { get => _gridSize; set => _gridSize = value; }


    public static bool isSuperBlock;
    public static Transform[,] grid;

    public static int heightRows = 7;
    public static int widthColumns = 6;

    [Space]
    [Range(1, 20)]
    public int Height = 7;
    [Range(1, 20)]
    public int Width = 6;

    [Space, Space]
    public int PointTotal = 20;
    public static int PTotal;

    public static BlockObject currentBlock;

    private static CameraFX SetCameraFX;

    public GameObject explosionPrefab;

    private static GameObject explosion;


    private static List<Vector2> horizontalBlockObjects;

    private static List<Vector2> rightDiagonalBlockObjects;
    private static List<Vector2> leftDiagonalBlockObjects;

    public static CustomAudioManager customAudio;

    public static Scoremanager scoremanager;

    public BlockSpawner spawner;

    private void Awake()
    {
        // Ensure there's only one instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the GameObject alive when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }


        heightRows = Height;
        widthColumns = Width;

        PTotal = PointTotal;
        
        grid = new Transform[Width, Height];

        SetCameraFX = new CameraFX(Camera.main);

        explosion = explosionPrefab;

        customAudio = FindObjectOfType<CustomAudioManager>();

        scoremanager = FindObjectOfType<Scoremanager>();
    }

    //Response From Cube Alerting Matrix it Has Settled
    public void HandleBlockSettled(object sender, BlockSettleArgs args)
    {
        args.block.OnSettle -= HandleBlockSettled;

        StartCoroutine(HandleBlockSettledCoroutine(args));
    }

    //Begin Checking
    private IEnumerator HandleBlockSettledCoroutine(BlockSettleArgs args)
    {
        // Perform necessary logic here
        bool reachTarget = CheckAllDirectionTargetReach((int)args.Pos.x, (int)args.Pos.y);

        // Wait until CheckAllDirectionTargetReach is completed
        while (reachTarget)
        {
            yield return null; // Wait for the next frame

            reachTarget = CheckAllDirectionTargetReach((int)args.Pos.x, (int)args.Pos.y);
        }

        // Signal back to the spawner
        spawner.HandleSpawnBlockSignal(this, EventArgs.Empty);
    }

    public static bool CheckAllDirectionTargetReach(int indexX, int indexY)
    {
        if (grid[indexX, indexY] == null)
            return false;


        // Get the parent and check if it's null
        Transform parentTransform = grid[indexX, indexY].parent;

        if (parentTransform == null)
        {
            return false;
        }

        // Get the BlockObject component and check if it's null
        BlockObject blockObject = parentTransform.GetComponent<BlockObject>();
        
        if (blockObject == null)
        {
            return false;
        }

        int currentBlocksPointValue = blockObject.PointValue;

        bool isBonusCube = currentBlocksPointValue > 9;

       // Debug.Log($"Checking block at ({indexX}, {indexY}) with point value: {currentBlocksPointValue}");


        // If the current block is a bonus cube, handle it separately
        if (isBonusCube)
        {
            if (currentBlocksPointValue == 10)
            {
                DeleteWholeColumns(indexX);
            }
            else if (currentBlocksPointValue == 11)
            {
                DeleteWholeRow(indexY);
            }
            else if (currentBlocksPointValue == 12)
            {
                DeleteWholeRow(indexY);
                DeleteWholeColumns(indexX);
            }

            return true;
        }

        // Regular cube checks
        bool verticalCheck = IsPointValueReachedVertical(indexX);
        bool horizontalCheck = IsPointValueReachedHorizontal(indexX, indexY);
        bool rightDiagonalCheck = IsPointValueReachedRightDiagonal(indexX, indexY);
        bool leftDiagonalCheck = IsPointValueReachedLeftDiagonal(indexX, indexY);

        if (verticalCheck || horizontalCheck || rightDiagonalCheck || leftDiagonalCheck)
        {
            // Here you can add code to destroy the blocks or handle other logic
            // For regular cubes, let's destroy the blocks that contributed to the match
            if (verticalCheck)
            {
                DeleteSelectedObject(GetVerticalBlockObjects(indexX));
            }
            else if (horizontalCheck)
            {
                DeleteSelectedObject(horizontalBlockObjects);
            }
            else if (rightDiagonalCheck)
            {
                DeleteSelectedObject(rightDiagonalBlockObjects);
            }
            else if (leftDiagonalCheck)
            {
                DeleteSelectedObject(leftDiagonalBlockObjects);
            }

            return true;
        }

        return false;
    }



    /// <summary>
    /// Returns the GameObject currently at Grid[gridPosition], if there is one. Otherwise returns Null.
    /// </summary>
    /// <param name="gridPosition">Like any array, valid values start at 0 and go up to Height and Width - 1</param>
    /// <returns>A GameObect on which GetComponent can be used, or returns Null if empty.</returns>
    public static GameObject ReturnObjectAtGridPosition(Vector2 gridPosition)
    {
        Vector2 v = RoundVector(gridPosition);

        if (!IsInsideBorder(v))
            return null;

        if (grid[(int)v.x, (int)v.y] != null)

            return grid[(int)v.x, (int)v.y].parent.gameObject;

        else
            
            return null;
    }

    //round out position
    public static Vector2 RoundVector(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    //is object within grid
    public static bool IsInsideBorder(Vector2 pos)
    {
        return (int)pos.x >= 0 && (int)pos.x < widthColumns && (int)pos.y >= 0;
    }

    public static void DeleteMatchedObject(List<Vector2> objectsData)
    {
        foreach (Vector2 objData in objectsData)
        {
            int x = (int)objData.x;
            int y = (int)objData.y;

            if (grid[x, y] != null)
            {
                Debug.Log($"Deleting object at ({x}, {y})");

                OnDestroyCube(objData);

                // Destroy Cube upon match score
                Destroy(grid[x, y].parent.gameObject);

                // Clear positions of destroyed cube
                grid[x, y] = null;

                Debug.Log("Object deleted successfully.");
            }
        }

        // After deleting all matched objects, decrease rows
        foreach (Vector2 objData in objectsData)
        {
            int x = (int)objData.x;
            int y = (int)objData.y;

            if (grid[x, y] == null)
            {
                Debug.Log("Decreasing rows...");

                // Decrease Rows after
                DecreaseRowsAbove(x, y + 1);

            }
            else
            {
                Debug.Log("Nothing Here");
                Debug.Log("Error");
            }
        }
    }





    public static void DeleteColumn(int x)
    {
        for (int y = 0; y < heightRows; ++y)
        {
            if (grid[x, y] != null)
            {

                OnDestroyCube(grid[x, y].parent.gameObject.transform.position);

                Destroy(grid[x, y].parent.gameObject);

                grid[x, y] = null;
                
            }
        }
    }

    public static void DeleteRow(int y)
    {
        for (int x = 0; x < widthColumns; ++x)
        {
            if (grid[x, y] != null)
            {
                OnDestroyCube(grid[x, y].parent.gameObject.transform.position);

                Destroy(grid[x, y].parent.gameObject);

                grid[x, y] = null;

                DecreaseRowsAbove(x,y+1);
                
            }
        }
    }

    public static void DecreaseRowsAbove(int x,int y)
    {
        for (int i = y; i < heightRows; i++)
        {
            if(grid[x,i]!=null)
            {
                grid[x, i - 1] = grid[x, i];

                grid[x, i] = null;

                grid[x, i - 1].position += new Vector3(0, -1, 0);
            }         
        }

        CheckAllDirectionTargetReach(x, y - 1);
    }

    public enum Direction { Horizontal, Vertical }


    public static bool IsPointValueReachedVertical(int index)
    {
        //Total Value in each Column
        int CurrentTotal = 0;

            for (int y = 0; y < heightRows; ++y)
            {
                if (grid[index, y] != null)
                {
                    CurrentTotal += grid[index, y].parent.GetComponent<BlockObject>().PointValue;
                }
            }

       // Debug.Log("Vertical Total: " + CurrentTotal);

        //If the tally of the row == 21
        if (CurrentTotal == PTotal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsPointValueReachedHorizontal(int indexX,int indexY)
    {
        int CurrentTotal = 0;
        horizontalBlockObjects= new List<Vector2>();
        horizontalBlockObjects.Clear();

            for (int x = indexX; x < widthColumns; ++x)
            {
                if (grid[x, indexY] == null)
                    break;

                    CurrentTotal += grid[x, indexY].parent.GetComponent<BlockObject>().PointValue;
                    Vector2 temp=new Vector2();
                    temp.x=x;
                    temp.y=indexY;
                    horizontalBlockObjects.Add(temp);
            }
      
            for (int x = indexX-1; x >=0; x--)
            {
                if (grid[x, indexY] == null)
                    break;
                    CurrentTotal += grid[x, indexY].parent.GetComponent<BlockObject>().PointValue;
                    Vector2 temp=new Vector2();
                    temp.x=x;
                    temp.y=indexY;
                    horizontalBlockObjects.Add(temp);
            }
        
         //Debug.Log(" IsPointValueReachedHorizontal: Cur: "+CurrentTotal+" req:"+(PTotal-CurrentTotal));

        if (CurrentTotal == PTotal)
        {
            //Audio Call
            DestroyBlockAudio("2,1,false");

            return true;
        }
        else
        {
            return false;
        }
    }



    public static bool IsPointValueReachedRightDiagonal(int indexX,int indexY)
    {
        
        int CurrentTotal = 0;
        int storeIndexY=indexY;

        rightDiagonalBlockObjects= new List<Vector2>();
        
        rightDiagonalBlockObjects.Clear();
            
        for (int x = indexX; x < widthColumns; ++x)
            {
                if(indexY<0||indexY>=heightRows)
                    break;
                if (grid[x, indexY] == null)
                    break;
                
                    CurrentTotal += grid[x, indexY].parent.GetComponent<BlockObject>().PointValue;
                    Vector2 temp=new Vector2();
                    temp.x=x;
                    temp.y=indexY;
                    rightDiagonalBlockObjects.Add(temp);
                    indexY--;
            }
                indexY=storeIndexY+1;
            for (int x = indexX-1; x >=0; x--)
            {
                if(indexY>=heightRows||indexY<0)
                    break;
                if (grid[x, indexY] == null)
                    break;
                
                    CurrentTotal += grid[x, indexY].parent.GetComponent<BlockObject>().PointValue;
                    Vector2 temp=new Vector2();
                    temp.x=x;
                    temp.y=indexY;
                    rightDiagonalBlockObjects.Add(temp);
                    indexY++;
            }
             //Debug.Log(" IsPointValueReachedRightDiagonal: Cur: "+CurrentTotal+" req:"+(PTotal-CurrentTotal));


        if (CurrentTotal == PTotal)
        {
            DestroyBlockAudio("2,1,false");

            return true;
        }
        else
        {
            return false;
        }
    }

     public static bool IsPointValueReachedLeftDiagonal(int indexX,int indexY)
    {
        
        int CurrentTotal = 0;
        int storeIndexY=indexY;
        leftDiagonalBlockObjects= new List<Vector2>();
        leftDiagonalBlockObjects.Clear();
            for (int x = indexX; x < widthColumns; x++)
            {
                if(indexY>=heightRows||indexY<0)
                    break;
                if (grid[x, indexY] == null)
                    break;
                
                    CurrentTotal += grid[x, indexY].parent.GetComponent<BlockObject>().PointValue;
                    Vector2 temp=new Vector2();
                    temp.x=x;
                    temp.y=indexY;
                    leftDiagonalBlockObjects.Add(temp);
                    indexY++;
            }

                indexY=storeIndexY-1;


            for (int x = indexX-1; x >=0; x--)
            {
                if(indexY<0||indexY>=heightRows)
                    break;
                if (grid[x, indexY] == null)
                    break;
                
                    CurrentTotal += grid[x, indexY].parent.GetComponent<BlockObject>().PointValue;
                    Vector2 temp=new Vector2();
                    temp.x=x;
                    temp.y=indexY;
                    leftDiagonalBlockObjects.Add(temp);
                    indexY--;
            }


             //Debug.Log(" IsPointValueReachedLeftDiagonal: Cur: "+CurrentTotal+" req:"+(PTotal-CurrentTotal));


        if (CurrentTotal == PTotal)
        {
            DestroyBlockAudio("2,1,false");

            return true;
        }
        else
        {
            return false;
        }
    }


   

    private static List<Vector2> GetVerticalBlockObjects(int indexX)
    {
        List<Vector2> verticalBlockObjects = new List<Vector2>();

        for (int y = 0; y < heightRows; ++y)
        {
            if (grid[indexX, y] == null)
                continue;

            verticalBlockObjects.Add(new Vector2(indexX, y));
        }

        Debug.Log(verticalBlockObjects.Count + " Objects  set to Be Destroyed...");

        return verticalBlockObjects;
    }






    public static void DeleteSelectedObject(List<Vector2> objects)
    {
        //Update Score
        if (!isSuperBlock)
        {
            Scoremanager scoremanager = FindObjectOfType<Scoremanager>();

            scoremanager.AddPoints();
        }
        
        //Shake Camera
        SetCameraFX.ShakeCamera(2.5f, 0.1f);

        //Delete Objects
        DeleteMatchedObject(objects);
                 
    }

    public static void DeleteWholeColumns(int x)
    {
        DestroyBlockAudio("2,0,false");

        if (!isSuperBlock)
        {
            scoremanager.AddPoints();
        }
            SetCameraFX.ShakeCamera(2.5f, 0.1f);
            
            DeleteColumn(x);
        
    }

    public static void DeleteWholeRow(int x)
    {
        DestroyBlockAudio("2,0,false");

        if (!isSuperBlock)
        {
            scoremanager.AddPoints();
        }
            SetCameraFX.ShakeCamera(2.5f, 0.1f);

            DeleteRow(x);
        
    }
   

    #region FX
    public static void OnDestroyCube(Vector2 pos)
    {
        GameObject exp = Instantiate(explosion, pos, Quaternion.identity);
    }

    public static void DestroyBlockAudio(string i)
    {
        if (customAudio)
        {
            customAudio.PlayAudioClip(i);

            Debug.Log("Found AudioContent");
        }
        else
        {
            Debug.Log("Can't find AudioContent");
        }
    }

    #endregion

    #region Controls

    public void MoveLeft()
    {
        if (currentBlock)
            currentBlock.MoveBlockLeft();
    }

    public void MoveRight()
    {
        if (currentBlock)
            currentBlock.MoveBlockRight();
    }

    public void MoveDown()
    {
        if (currentBlock)
            currentBlock.MoveBlockDown();
    }

    #endregion


   

   
}



