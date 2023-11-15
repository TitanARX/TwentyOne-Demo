using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SolverBase : MonoBehaviour, ISolve
{
    [SerializeField]
    [Header("Solver External References")]
    private SolverModel _solvermodel;

    [SerializeField]
    [Header("Solver Output References")]
    private SolverView _solverView;

    [SerializeField]
    [Header("Current State of Solver")]
    private SolveState _solveState = SolveState.Idle;

    public SolverModel SolverModel { get => _solvermodel; set => _solvermodel = value; }

    public SolverView SolverView { get => _solverView; set => _solverView = value; }


    public SolveState SolverState { get => _solveState; set => _solveState = value; }

    public virtual int SolveColumn(int[,] array, int columnIndex)
    {
        _solveState = SolveState.SolvingColumns;

        // Validate input
        if (array == null || array.GetLength(1) < columnIndex)
        {
            Debug.Log("Invalid input, please provide clear instructions.");
        }

        // Initialize sum
        int sum = 0;

        // Iterate over rows and add the value of the specified column to the sum
        for (int i = 0; i < array.GetLength(0); i++)
        {
            sum += array[i, columnIndex];
        }

        // Return the sum
        return sum;
    }

    public virtual int SolveRow(int rowIndex)
    {
        Transform[,] array = MatrixGrid.grid;

        _solveState = SolveState.SolvingRows;

        int sum = 0;

        for (int i = 0; i < array.GetLength(1); i++)
        {
            sum += array[rowIndex, i] != null ?  array[rowIndex, i].parent.GetComponent<BlockObject>().PointValue : sum;
        }

        return sum;
    }

    public virtual int SolveLeftDiagonal(int[,] array)
    {
        _solveState = SolveState.SolvingLeftDiagonal;

        // Check if the array is valid
        if (array == null || array.GetLength(0) != array.GetLength(1))
        {
            return -1;
        }

        // Initialize the sum
        int sum = 0;

        // Iterate through the array and add the diagonal elements
        for (int i = 0; i < array.GetLength(0); i++)
        {
            sum += array[i, i];
        }

        return sum;
    }

    public virtual int SolveRightDiagonal(int[,] arr)
    {
        _solveState = SolveState.SolvingRightDiagonal;

        int sum = 0;
        int row = arr.GetLength(0) - 1;
        int col = 0;

        while (row >= 0 && col < arr.GetLength(1))
        {
            sum += arr[row, col];
            row--;
            col++;
        }

        return sum;
    }

    public virtual void UpdateGrid()
    {
        //SolverModel.Grid.UpdateAvailableGridPositions();
    }
}
