using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GridManager gridManager;
    private ObstacleManager obstacleManager;
    private TargetManager targetManager;

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

        gridManager = GetComponent<GridManager>();
        obstacleManager = GetComponent<ObstacleManager>();
        targetManager = GetComponent<TargetManager>();

        if (gridManager == null || obstacleManager == null || targetManager == null)
        {
            Debug.LogError("One or more required components are missing!");
        }
    }

    private void Start()
    {
        GenerateObstaclesAndTarget();
    }

    public void GenerateObstaclesAndTarget()
    {
        gridManager.ResetGrid();
        obstacleManager.GenerateObstacles();
        targetManager.SpawnTarget();
    }

    public void HandleObstacleCollision(GameObject obstacle)
    {
        obstacleManager.RemoveObstacle(obstacle);
        // 타겟 위치 변경 로직 제거
    }
}