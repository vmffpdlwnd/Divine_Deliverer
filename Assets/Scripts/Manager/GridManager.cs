using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public float playAreaWidth = 50f;
    public float playAreaLength = 50f;
    public int gridSize = 10;
    private bool[,] occupiedCells;

    private void Awake()
    {
        occupiedCells = new bool[gridSize, gridSize];
    }

    public Vector3 GetRandomPosition(bool isTarget = false, int minDistance = 0)
    {
        List<Vector2Int> availableCells = new List<Vector2Int>();
        int borderBuffer = isTarget ? 1 : 0;

        for (int gridX = borderBuffer; gridX < gridSize - borderBuffer; gridX++)
        {
            for (int gridZ = borderBuffer; gridZ < gridSize - borderBuffer; gridZ++)
            {
                if (!occupiedCells[gridX, gridZ] && IsCellFarEnough(gridX, gridZ, minDistance))
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

        return new Vector3(posX, 0f, posZ); // y 값을 0으로 설정
    }

    private bool IsCellFarEnough(int x, int z, int minDistance)
    {
        for (int dx = -minDistance; dx <= minDistance; dx++)
        {
            for (int dz = -minDistance; dz <= minDistance; dz++)
            {
                int nx = x + dx;
                int nz = z + dz;
                if (nx >= 0 && nx < gridSize && nz >= 0 && nz < gridSize && occupiedCells[nx, nz])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void ResetGrid()
    {
        occupiedCells = new bool[gridSize, gridSize];
    }

    // 기존의 OnDrawGizmos 메서드는 그대로 유지
}