using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GridManager gridManager;
    private ObstacleManager obstacleManager;
    private TargetManager targetManager;
    private UIManager uiManager;
    private Player player;

    [Header("게임 설정")]
    [SerializeField]
    [Tooltip("타겟 충돌 시 시간 증가: 타겟과 충돌했을 때 증가하는 시간(초)")]
    private float timeAddedOnTargetCollision = 5f;

    [SerializeField]
    [Tooltip("장애물 충돌 시 시간 감소: 장애물과 충돌했을 때 감소하는 시간(초)")]
    private float timeRemovedOnObstacleCollision = 3f;

    [Header("난이도별 장애물 간격")]
    [SerializeField]
    [Tooltip("쉬움 난이도 장애물 간격: 쉬움 난이도에서의 장애물 간 최소 거리")]
    private int easyObstacleDistance = 3;

    [SerializeField]
    [Tooltip("보통 난이도 장애물 간격: 보통 난이도에서의 장애물 간 최소 거리")]
    private int mediumObstacleDistance = 2;

    [SerializeField]
    [Tooltip("어려움 난이도 장애물 간격: 어려움 난이도에서의 장애물 간 최소 거리")]
    private int hardObstacleDistance = 1;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        gridManager = GetComponent<GridManager>();
        obstacleManager = GetComponent<ObstacleManager>();
        targetManager = GetComponent<TargetManager>();
        uiManager = FindObjectOfType<UIManager>();
        player = FindObjectOfType<Player>();

        if (gridManager == null || obstacleManager == null || targetManager == null || uiManager == null || player == null)
        {
            Debug.LogError("One or more required components are missing!");
        }
    }

    private void Start()
    {
        SetDifficulty(Difficulty.Easy); // 기본 난이도를 쉬움으로 설정
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
        if (!player.IsInvincible())
        {
            obstacleManager.RemoveObstacle(obstacle);
            uiManager.AddTime(-timeRemovedOnObstacleCollision);
            Debug.Log($"장애물 충돌! 시간 감소: {timeRemovedOnObstacleCollision}초");
            player.StartInvincibility();
        }
    }

    public void HandleTargetCollision()
    {
        targetManager.RemoveCurrentTarget();
        uiManager.OnTargetCollision();
        uiManager.AddTime(timeAddedOnTargetCollision);
        player.StartInvincibility();
        GenerateObstaclesAndTarget();
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                obstacleManager.SetObstacleDistance(easyObstacleDistance);
                break;
            case Difficulty.Medium:
                obstacleManager.SetObstacleDistance(mediumObstacleDistance);
                break;
            case Difficulty.Hard:
                obstacleManager.SetObstacleDistance(hardObstacleDistance);
                break;
        }
    }

    public void SetPauseState(bool pause)
    {
        isPaused = pause;
        Time.timeScale = isPaused ? 0 : 1;

        if (player != null)
        {
            if (isPaused)
            {
                player.StopMoving();
            }
            else
            {
                player.ResumeMoving();
            }
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        // 여기에 게임 오버 시 수행할 작업을 추가하세요.
        // 예: 점수 저장, 게임 오버 UI 표시 등
    }

    public void RestartGame()
    {
        SetPauseState(false);

        if (uiManager != null)
        {
            uiManager.ResetUI();
        }

        if (player != null)
        {
            player.ResetPlayer();
        }

        GenerateObstaclesAndTarget();
    }
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}