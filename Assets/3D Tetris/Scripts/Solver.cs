using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Solver : SolverBase
{
    public override void UpdateGrid()
    {
        base.UpdateGrid();
    }

    public override int SolveColumn(int[,] array, int columnIndex)
    {
        return base.SolveColumn(array, columnIndex);
    }

    public override int SolveRow(int rowIndex)
    {
        return base.SolveRow(rowIndex);
    }

    public override int SolveLeftDiagonal(int[,] array)
    {
        return base.SolveLeftDiagonal(array);
    }

    public override int SolveRightDiagonal(int[,] arr)
    {
        return base.SolveRightDiagonal(arr);
    }

    public List<int> HCount = new List<int>();
    private bool recheck;

    public event EventHandler RowTotalEquals21;
    public event EventHandler RowTotalsNotEqual21;

    private void OnEnable()
    {
        RowTotalEquals21 += ReCheck;
    }

    public List<Vector2> BlocksToDeleteCache = new List<Vector2>();


    public void CheckDiag()
    {
        Transform[,] grid = MatrixGrid.grid;
        // Traverse the grid diagonally
        for (int i = 0; i < 6; i++)
        {
            int sum = 0;
            int x = i;
            int y = 0;

            while (x >= 0 && y < 11)
            {
                if (grid[x, y])
                {
                    sum += grid[x, y].transform.parent.GetComponent<BlockObject>().PointValue;

                    // Break out if the diagonal sum equals 21
                    if (sum == 21)
                    {
                        Console.WriteLine("Indices whose sum equaled 21: ({0}, {1})", x, y);
                        break;
                    }

                    x--;
                    y++;
                }
            }

        }
    }

    public bool CountRowsInColumn()
    {
        Transform[,] grid = MatrixGrid.grid;

        int y = MatrixGrid.widthColumns;
        int x = MatrixGrid.heightRows;

        int rowCount = 0;
        int columnCount = 0;

        // Check if grid is valid
        if (grid == null || grid.GetLength(0) == 0 || grid.GetLength(1) == 0)
        {
            Debug.LogErrorFormat("Grd Size is Zero");   
        }

        else
        {
            //Iterate over rows
            for (int i = 0; i < x; i++)
            {
                rowCount++;

                if (grid[0, i])
                {
                    if (grid[0, i].parent.TryGetComponent(out BlockObject block))
                    {
                        Debug.Log("Detected " + block.PointValue);
                    }
                }
            }

            //Iterate over columns
            for (int j = 0; j < y; j++)
            {
                columnCount++;

                /*
                if (grid[row, col])
                {
                    if (grid[row, col].parent.TryGetComponent(out BlockObject block))
                    {
                        rowSum += block.Point;
                    }
                }
                */

            }

            // Iterate over the grid diagonally
            for (int row = 0; row < 6; row++)
            {
                int sum = 0;

                for (int col = 0; col < 6; col++)
                {
                    // Sum the diagonal elements
                    sum += grid[row + col, col] != null ? grid[row + col, col].parent.GetComponent<BlockObject>().PointValue : 0;

                    // Break out if the sum equals 21
                    if (sum == 21)
                    {
                        BlocksToDeleteCache.Add(new Vector2((row + col), col));

                        break;
                    }
                }
            }


            // Iterate over the left diagonal
            for (int i = 0; i < x; i++)
            {
                // Get the element at the current position
                if (grid[0, i])
                {
                    if (grid[0, i].parent.TryGetComponent(out BlockObject block))
                    {
                        Debug.Log("Detected " + block.PointValue);
                    }
                }
                // Do something with the element
                // ...
            }


            Debug.Log("The current Row is  " + rowCount + " and the current Column is " + columnCount);
        }

        return BlocksToDeleteCache.Count > 0;
    }


    public void CheckGridRows()
    {
        Transform[,] grid = MatrixGrid.grid;

        int rowLength = grid.GetLength(1);
        int colLength = grid.GetLength(0);

        int column = 0;

        for (int row = 0; row < rowLength; row++)
        {
            int rowSum = 0;

            for (int col = 0; col < colLength; col++)
            {
                column = col;

                if (grid[row, col])
                {
                    if (grid[row, col].parent.TryGetComponent(out BlockObject block))
                    {
                        rowSum += block.PointValue;
                    }
                }
            }

            if (rowSum == 21)
            {
                Debug.Log("Match found at column " + column + " / " + row);

                MatrixGrid.DeleteRow(column);

                UpdateGrid();

                recheck = true;

                OnRowTotalEquals21();

                break;
            }
        }

        RowTotalsNotEqual21?.Invoke(this, EventArgs.Empty);
    }


    public void CheckGridColumns()
    {
        Transform[,] grid = MatrixGrid.grid;

        int rowLength = grid.GetLength(0);
        int colLength = grid.GetLength(1);

        int column = 0;

        for (int row = 0; row < rowLength; row++)
        {
            int rowSum = 0;

            for (int col = 0; col < colLength; col++)
            {
                column = col;

                if (grid[row, col])
                {
                    if(grid[row, col].parent.TryGetComponent(out BlockObject block))
                    {
                        rowSum += block.PointValue;
                    }
                }
            }

            if (rowSum == 21)
            {
                Debug.Log("Match found at column " + column + " / " + row );

                MatrixGrid.DeleteColumn(row);
                
                UpdateGrid();
                
                recheck = true;

                OnRowTotalEquals21();

                break;
            }
        }

        RowTotalsNotEqual21?.Invoke(this, EventArgs.Empty); 
    }

    protected virtual void OnRowTotalEquals21()
    {
        Debug.Log("Flaggin for Recheck");

        if (!recheck)
            return;

        RowTotalEquals21?.Invoke(this, EventArgs.Empty);
    }


    public void ReCheck(object sender, EventArgs args)
    {
        Debug.Log("Rechecking");

        recheck = false;
    }



    /// <summary>
    /// Function to check if the rows and columns of a grid of ints total to 21
    /// </summary>
    /// <param name="grid">The grid of ints to check</param>
    /// <returns>True if the grid or rows total to 21, false otherwise</returns>
    public bool CheckGridTotal(int[,] grid)
    {
        // Check if the grid is null
        if (grid == null)
        {
            return false;
        }

        // Get the number of rows and columns in the grid
        int rows = grid.GetLength(0);
        int columns = grid.GetLength(1);

        // Check the rows
        for (int i = 0; i < rows; i++)
        {
            int rowTotal = 0;

            // Sum the values in the row
            for (int j = 0; j < columns; j++)
            {
                rowTotal += grid[i, j];
            }

            // Check if the row total is 21
            if (rowTotal == 21)
            {
                return true;
            }
        }

        // Check the columns
        for (int j = 0; j < columns; j++)
        {
            int columnTotal = 0;

            // Sum the values in the column
            for (int i = 0; i < rows; i++)
            {
                columnTotal += grid[i, j];
            }

            // Check if the column total is 21
            if (columnTotal == 21)
            {
                return true;
            }
        }

        // If the row and column totals are not 21, return false
        return false;
    }




    public bool DoubleCheck()
    {
        bool sumtotals = HCount.Count > 0;

        bool ready = false;

        if(sumtotals)
        {
            for (int i = 0; i < HCount.Count; i++)
            {
                if(HCount[i] == 21)
                {
                    Debug.Log("21 Found");
                    
                    ready =  true;
                }
                else
                {
                    ready = false;
                }
            }
        }





        return ready;
    }

    public int[] SumGridRows()
    {
        HCount.Clear();

        Transform[,] grid = MatrixGrid.grid;

        int[] sums = new int[grid.GetLength(0)];

        for (int row = 0; row < grid.GetLength(0); row++)
        {
            int sum = 0;

            for (int col = 0; col < grid.GetLength(1); col++)
            {
                if (grid[row, col] != null)
                {
                    sum += grid[row, col].parent.GetComponent<BlockObject>().PointValue;
                }
            }

            sums[row] = sum;

        }

        HCount.AddRange(sums);

        return sums;


    }

}
