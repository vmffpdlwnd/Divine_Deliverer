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
    public TextMeshProUGUI pauseButtonText;

    [Header("Timer Sprites")]
    public Sprite normalTimerSprite;
    public Sprite lowTimerSprite;

    [Header("Game Settings")]
    public float gameDuration = 60f;
    public float lowTimeThreshold = 10f;
    public int scorePerTarget = 100;

    private int currentScore = 0;
    private float remainingTime;
    private Image sliderFillImage;
    private bool isPaused = false;

    public UnityEvent onGameOver;

    private void Start()
    {
        InitializeUI();
        InitializeGame();
    }

    private void InitializeUI()
    {
        if (scoreText != null) scoreText.text = "점수: 0";
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }
        if (pauseButtonText != null)
        {
            pauseButtonText.text = "일시정지";
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

    public void InitializeGame()
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
        if (!isPaused && remainingTime > 0)
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
        currentScore = Mathf.Max(0, currentScore + points);
        if (scoreText != null)
        {
            scoreText.text = $"점수: {currentScore}";
        }
    }

    private void GameOver()
    {
        Debug.Log($"게임 종료! 최종 점수: {currentScore}");
        onGameOver.Invoke();
    }

    public void AddTime(float seconds)
    {
        remainingTime = Mathf.Clamp(remainingTime + seconds, 0f, gameDuration);
        UpdateTimer();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        GameManager.Instance.SetPauseState(isPaused);

        if (pauseButtonText != null)
        {
            pauseButtonText.text = isPaused ? "계속하기" : "일시정지";
        }
    }

    public void OnTargetCollision()
    {
        UpdateScore(scorePerTarget);
        Debug.Log($"타겟 획득! 현재 점수: {currentScore}");
    }

    public void ResetUI()
    {
        currentScore = 0;
        remainingTime = gameDuration;

        if (scoreText != null)
        {
            scoreText.text = "점수: 0";
        }

        if (timerSlider != null)
        {
            timerSlider.value = gameDuration;
        }

        if (pauseButtonText != null)
        {
            pauseButtonText.text = "일시정지";
        }

        isPaused = false;
    }
}