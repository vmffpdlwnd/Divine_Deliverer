using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstaclePrefabs;
    [SerializeField] private int numberOfObstacles = 5;
    [SerializeField] private int minObstacleDistance = 2;
    private List<GameObject> currentObstacles = new List<GameObject>();
    private GridManager gridManager;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridManager component not found on the same GameObject!");
        }
    }

    public void GenerateObstacles()
    {
        ClearCurrentObstacles();
        for (int i = 0; i < numberOfObstacles; i++)
        {
            Vector3 position = gridManager.GetRandomPosition(false, minObstacleDistance);
            if (position != Vector3.zero)
            {
                GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
                GameObject obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);

                // 장애물의 높이를 고려하여 y 위치 조정
                Collider obstacleCollider = obstacle.GetComponent<Collider>();
                if (obstacleCollider != null)
                {
                    float yOffset = obstacleCollider.bounds.extents.y;
                    obstacle.transform.position = new Vector3(position.x, yOffset, position.z);
                }

                obstacle.transform.SetParent(transform);
                currentObstacles.Add(obstacle);
            }
            else
            {
                Debug.LogWarning("Could not find a suitable position for obstacle. Stopping obstacle generation.");
                break;
            }
        }
    }

    private void ClearCurrentObstacles()
    {
        foreach (GameObject obstacle in currentObstacles)
        {
            Destroy(obstacle);
        }
        currentObstacles.Clear();
    }

    public void RemoveObstacle(GameObject obstacle)
    {
        if (currentObstacles.Contains(obstacle))
        {
            currentObstacles.Remove(obstacle);
            Destroy(obstacle);
        }
    }

    public void SetObstacleDistance(int distance)
    {
        minObstacleDistance = Mathf.Max(0, distance);
    }
}