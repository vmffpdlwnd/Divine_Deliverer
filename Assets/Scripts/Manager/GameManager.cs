using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GridManager gridManager;
    private ObstacleManager obstacleManager;
    private TargetManager targetManager;
    private UIManager uiManager;
    private Player player;

    [Header("���� ����")]
    [SerializeField]
    [Tooltip("Ÿ�� �浹 �� �ð� ����: Ÿ�ٰ� �浹���� �� �����ϴ� �ð�(��)")]
    private float timeAddedOnTargetCollision = 5f;

    [SerializeField]
    [Tooltip("��ֹ� �浹 �� �ð� ����: ��ֹ��� �浹���� �� �����ϴ� �ð�(��)")]
    private float timeRemovedOnObstacleCollision = 3f;

    [Header("���̵��� ��ֹ� ����")]
    [SerializeField]
    [Tooltip("���� ���̵� ��ֹ� ����: ���� ���̵������� ��ֹ� �� �ּ� �Ÿ�")]
    private int easyObstacleDistance = 3;

    [SerializeField]
    [Tooltip("���� ���̵� ��ֹ� ����: ���� ���̵������� ��ֹ� �� �ּ� �Ÿ�")]
    private int mediumObstacleDistance = 2;

    [SerializeField]
    [Tooltip("����� ���̵� ��ֹ� ����: ����� ���̵������� ��ֹ� �� �ּ� �Ÿ�")]
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
        SetDifficulty(Difficulty.Easy); // �⺻ ���̵��� �������� ����
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
            Debug.Log($"��ֹ� �浹! �ð� ����: {timeRemovedOnObstacleCollision}��");
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
        // ���⿡ ���� ���� �� ������ �۾��� �߰��ϼ���.
        // ��: ���� ����, ���� ���� UI ǥ�� ��
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