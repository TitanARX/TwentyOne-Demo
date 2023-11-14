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
    [SerializeField]
    [Header("Gameplay Board Size")]
    [Tooltip("Determines the Size of the Object")]
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
    
    private MatrixGrid Instance;

    public static CustomAudioManager customAudio;

    public static Scoremanager scoremanager;


    private void Awake()
    {  
        heightRows = Height;
        widthColumns = Width;

        PTotal = PointTotal;
        
        grid = new Transform[Width, Height];

        SetCameraFX = new CameraFX(Camera.main);

        explosion = explosionPrefab;

        customAudio = FindObjectOfType<CustomAudioManager>();

        scoremanager = FindObjectOfType<Scoremanager>();
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
            int x=(int) objData.x;
            int y=(int) objData.y;

           if( grid[x,y]!=null)
           {
                OnDestroyCube(objData);

                //Destroy Cube upon match score
                Destroy(grid[x,y].parent.gameObject);
                
                //clear positions of destroyed cube
                grid[x,y]=null;

                //Decrease Rows after
                DecreaseRowsAbove(x, y + 1);           
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

                CheckAllDirectionTargetReach(x,i-1);
            }         
        }
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

        //Debug.Log(" IsPointValueReachedVertical: Cur: "+ CurrentTotal+" req:"+ ( PTotal-CurrentTotal));

        Debug.Log("Vertical Total: " + CurrentTotal);


        if (CurrentTotal == PTotal)
        {
            //Destroy Objects Audio Call
            DestroyBlockAudio("2,1,false");

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


    public static bool CheckAllDirectionTargetReach(int indexX, int indexY)
    {
        int currentBlocksPointValue = grid[indexX, indexY].parent.GetComponent<BlockObject>().PointValue;

        bool isBonusCube = currentBlocksPointValue > 9;

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

    private static List<Vector2> GetVerticalBlockObjects(int indexX)
    {
        List<Vector2> verticalBlockObjects = new List<Vector2>();

        for (int y = 0; y < heightRows; ++y)
        {
            if (grid[indexX, y] == null)
                continue;

            verticalBlockObjects.Add(new Vector2(indexX, y));
        }

        return verticalBlockObjects;
    }






    public static void DeleteSelectedObject(List<Vector2> objects)
    {
        if (!isSuperBlock)
        {
            Scoremanager scoremanager = FindObjectOfType<Scoremanager>();

            scoremanager.AddPoints();
        }
        
        SetCameraFX.ShakeCamera(2.5f, 0.1f);

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

    public void OnBlockSettle(object sender, BlockSettleArgs args)
    {
        UpdateGridAfterSettle(args);
    }

    private void UpdateGridAfterSettle(BlockSettleArgs args)
    {
        // Update grid positions based on the settled block
        // ...

        // Check for matches, delete rows/columns, etc.
        // ...

        // Example:
        int x = (int)args.Pos.x;
        int y = (int)args.Pos.y;
        DeleteRow(y);
        DeleteColumn(x);
    }

}



