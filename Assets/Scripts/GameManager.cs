using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Obstacle Generation")]
    public GameObject obstaclePrefab;
    public int numberOfObstacles = 5;
    public float playAreaWidth = 5f;
    public float playAreaLength = 5f;
    public int gridSize = 10;

    [Header("Target Spawning")]
    public GameObject targetPrefab;

    private bool[,] occupiedCells;
    private List<GameObject> currentObstacles = new List<GameObject>();
    private GameObject currentTarget;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        occupiedCells = new bool[gridSize, gridSize];
        GenerateObstaclesAndTarget();
    }

    public void GenerateObstaclesAndTarget()
    {
        ClearCurrentObstaclesAndTarget();
        occupiedCells = new bool[gridSize, gridSize];

        for (int i = 0; i < numberOfObstacles; i++)
        {
            Vector3 position = GetRandomPosition();
            if (position != Vector3.zero)
            {
                GameObject obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);
                obstacle.transform.SetParent(transform);
                currentObstacles.Add(obstacle);
            }
        }

        SpawnTarget();
    }

    private void ClearCurrentObstaclesAndTarget()
    {
        foreach (GameObject obstacle in currentObstacles)
        {
            Destroy(obstacle);
        }
        currentObstacles.Clear();

        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
        }
    }

    private Vector3 GetRandomPosition()
    {
        List<Vector2Int> availableCells = new List<Vector2Int>();

        for (int gridX = 0; gridX < gridSize; gridX++)
        {
            for (int gridZ = 0; gridZ < gridSize; gridZ++)
            {
                if (!occupiedCells[gridX, gridZ])
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

        return new Vector3(posX, 0.5f, posZ);
    }

    private void SpawnTarget()
    {
        Vector3 position = GetRandomPosition();
        if (position != Vector3.zero)
        {
            currentTarget = Instantiate(targetPrefab, position, Quaternion.identity);
        }
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