using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public float playAreaWidth = 50f;  // 플레이 영역 크기를 증가
    public float playAreaLength = 50f; // 플레이 영역 크기를 증가
    public int gridSize = 10;
    private bool[,] occupiedCells;

    private void Awake()
    {
        occupiedCells = new bool[gridSize, gridSize];
    }

    public Vector3 GetRandomPosition(bool isTarget = false)
    {
        List<Vector2Int> availableCells = new List<Vector2Int>();

        int borderBuffer = isTarget ? 1 : 0; // 타겟일 경우 가장자리에서 1칸 안쪽으로

        for (int gridX = borderBuffer; gridX < gridSize - borderBuffer; gridX++)
        {
            for (int gridZ = borderBuffer; gridZ < gridSize - borderBuffer; gridZ++)
            {
                if (!occupiedCells[gridX, gridZ] && !HasNeighbor(gridX, gridZ))
                {
                    availableCells.Add(new Vector2Int(gridX, gridZ));
                }
            }
        }

        if (availableCells.Count == 0)
        {
            return Vector3.zero;
        }

        Vector2Int selectedCell = availableCells[Random.Range(0, availableCells.Count)];
        occupiedCells[selectedCell.x, selectedCell.y] = true;

        float cellWidth = playAreaWidth / gridSize;
        float cellLength = playAreaLength / gridSize;

        float posX = -playAreaWidth / 2f + cellWidth * (selectedCell.x + 0.5f);
        float posZ = -playAreaLength / 2f + cellLength * (selectedCell.y + 0.5f);

        return new Vector3(posX, 0f, posZ);
    }

    private bool HasNeighbor(int x, int z)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dz = -1; dz <= 1; dz++)
            {
                int nx = x + dx;
                int nz = z + dz;
                if (nx >= 0 && nx < gridSize && nz >= 0 && nz < gridSize && occupiedCells[nx, nz])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void ResetGrid()
    {
        occupiedCells = new bool[gridSize, gridSize];
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            float cellWidth = playAreaWidth / gridSize;
            float cellLength = playAreaLength / gridSize;

            for (int gizmoX = 0; gizmoX < gridSize; gizmoX++)
            {
                for (int gizmoZ = 0; gizmoZ < gridSize; gizmoZ++)
                {
                    Vector3 center = new Vector3(
                        -playAreaWidth / 2f + cellWidth * (gizmoX + 0.5f),
                        0.5f,
                        -playAreaLength / 2f + cellLength * (gizmoZ + 0.5f)
                    );
                    Gizmos.DrawWireCube(center, new Vector3(cellWidth, 0.1f, cellLength));
                }
            }
        }
    }
}