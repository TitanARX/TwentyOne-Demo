
using UnityEngine;

public enum SolveState { Idle, SolvingRows, SolvingColumns, SolvingRightDiagonal, SolvingLeftDiagonal }

interface ISolve
{
    public SolverModel SolverModel { get; set; }

    public SolverView SolverView { get; set;}

    SolveState SolverState { get; set; }

    void UpdateGrid();

    int SolveColumn(int[,] array, int columnIndex);

    int SolveRow(int rowIndex);

    int SolveRightDiagonal(int[,] arr);

    int SolveLeftDiagonal(int[,] array);

}
