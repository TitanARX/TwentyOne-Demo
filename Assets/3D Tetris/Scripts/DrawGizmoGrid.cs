#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// draws a useful reference grid in the editor in Unity. 
// 09/01/15 - Hayden Scott-Baron

public class DrawGizmoGrid : MonoBehaviour
{
	public MatrixGrid matrixGrid;

	// universal grid scale
	private float gridScale = 1f;

	// extents of the grid
	private int minX;
	private int minY;

	// nudges the whole grid rel
	private Vector3 gridOffset = Vector3.zero;

	// choose a colour for the gizmos
	private int gizmoMajorLines = 6;
	public Color gizmoLineColor = new Color(0.4f, 0.4f, 0.3f, 1f);


	// draw the grid
	void OnDrawGizmos()
	{
		if (!matrixGrid)
			return;

		minX = 0;
		minY = 0;

		gizmoMajorLines = matrixGrid.Width;

		// orient to the gameobject, so you can rotate the grid independently if desired
		Gizmos.matrix = transform.localToWorldMatrix;

		// set colours
		Color dimColor = new Color(gizmoLineColor.r, gizmoLineColor.g, gizmoLineColor.b, 0.25f * gizmoLineColor.a);
		Color brightColor = Color.Lerp(Color.white, gizmoLineColor, 0.75f);

		// draw the horizontal lines
		for (int x = minX; x < matrixGrid.Width + 1; x++)
		{
			// find major lines
			Gizmos.color = (x % gizmoMajorLines == 0 ? gizmoLineColor : dimColor);
			if (x == 0)
				Gizmos.color = brightColor;

			Vector3 pos1 = new Vector3(x, minY, 0) * gridScale;
			Vector3 pos2 = new Vector3(x, matrixGrid.Height, 0) * gridScale;

			Gizmos.DrawLine((gridOffset + pos1), (gridOffset + pos2));
		}

		// draw the vertical lines
		for (int y = minY; y < matrixGrid.Height + 1; y++)
		{
			// find major lines
			Gizmos.color = (y % gizmoMajorLines == 0 ? gizmoLineColor : dimColor);
			if (y == 0)
				Gizmos.color = brightColor;

			Vector3 pos1 = new Vector3(minX, y, 0) * gridScale;
			Vector3 pos2 = new Vector3(matrixGrid.Width, y, 0) * gridScale;

			Gizmos.DrawLine((gridOffset + pos1), (gridOffset + pos2));
		}
	}
}
#endif