using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public int numberOfObstacles = 5;
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
            Vector3 position = gridManager.GetRandomPosition(true);
            position.y = 0.5f; // 장애물의 높이를 조정합니다.
            if (position != Vector3.zero)
            {
                GameObject obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);
                obstacle.transform.SetParent(transform);
                currentObstacles.Add(obstacle);
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

    
}