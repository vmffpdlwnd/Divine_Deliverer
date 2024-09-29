using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public Slider timerSlider;
    public Button pauseButton;

    [Header("Timer Sprites")]
    public Sprite normalTimerSprite;
    public Sprite lowTimerSprite;

    [Header("Game Settings")]
    public float gameDuration = 60f;
    public float lowTimeThreshold = 10f;
    public int scorePerTarget = 100;
    public int penaltyPerObstacle = 50;

    private int currentScore = 0;
    private float remainingTime;
    private Image sliderFillImage;

    public UnityEvent onGameOver;

    private void Start()
    {
        InitializeUI();
        InitializeGame();
    }

    private void InitializeUI()
    {
        if (scoreText != null) scoreText.text = "����: 0";
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
            TextMeshProUGUI buttonText = pauseButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null) buttonText.text = "�Ͻ�����";
        }
        if (timerSlider != null)
        {
            sliderFillImage = timerSlider.fillRect.GetComponent<Image>();
            if (sliderFillImage != null && normalTimerSprite != null)
            {
                sliderFillImage.sprite = normalTimerSprite;
            }
        }
    }

    private void InitializeGame()
    {
        remainingTime = gameDuration;
        if (timerSlider != null)
        {
            timerSlider.maxValue = gameDuration;
            timerSlider.value = gameDuration;
        }
        UpdateScore(0);
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimer();

            if (remainingTime <= 0)
            {
                GameOver();
            }
        }
    }

    private void UpdateTimer()
    {
        if (timerSlider != null)
        {
            timerSlider.value = remainingTime;
        }

        if (sliderFillImage != null)
        {
            if (remainingTime <= lowTimeThreshold && lowTimerSprite != null)
            {
                sliderFillImage.sprite = lowTimerSprite;
            }
            else if (normalTimerSprite != null)
            {
                sliderFillImage.sprite = normalTimerSprite;
            }
        }
    }

    public void UpdateScore(int points)
    {
        currentScore += points;
        if (scoreText != null)
        {
            scoreText.text = $"����: {currentScore}";
        }
    }

    private void GameOver()
    {
        Debug.Log($"���� ����! ���� ����: {currentScore}");
        onGameOver.Invoke();
    }

    public void AddTime(float seconds)
    {
        remainingTime = Mathf.Min(remainingTime + seconds, gameDuration);
        UpdateTimer();
    }

    private void PauseGame()
    {
        Debug.Log("���� �Ͻ� ����");
        // ���� �Ͻ� ���� ���� ����
    }

    public void OnTargetCollision()
    {
        UpdateScore(scorePerTarget);
        Debug.Log($"Ÿ�� ȹ��! ���� ����: {currentScore}");
    }

    public void OnObstacleCollision()
    {
        UpdateScore(-penaltyPerObstacle);
        Debug.Log($"��ֹ� �浹! ���� ����: {currentScore}");
    }
}