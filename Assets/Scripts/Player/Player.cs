using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float invincibilityDuration = 3f;
    private Vector3 moveDirection = Vector3.forward;
    private bool isMoving = true;
    private bool isInvincible = false;
    private Renderer playerRenderer;

    private void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer == null)
        {
            Debug.LogWarning("Renderer component not found on Player. Visual invincibility effect will not be shown.");
        }
    }

    private void Update()
    {
        if (isMoving && !GameManager.Instance.IsPaused())
        {
            // 자동 전진
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        // 입력 처리
        if (Input.GetKeyDown(KeyCode.W))
        {
            RotateTruck(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            RotateTruck(Vector3.back);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            RotateTruck(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            RotateTruck(Vector3.right);
        }
    }

    private void RotateTruck(Vector3 newDirection)
    {
        if (newDirection != moveDirection && newDirection != -moveDirection)
        {
            moveDirection = newDirection;
            Quaternion targetRotation = Quaternion.LookRotation(newDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
        }
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void ResumeMoving()
    {
        isMoving = true;
    }

    public void StartInvincibility()
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        if (playerRenderer != null)
        {
            StartCoroutine(BlinkEffect());
        }

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
        if (playerRenderer != null)
        {
            playerRenderer.enabled = true;
        }
    }

    private IEnumerator BlinkEffect()
    {
        while (isInvincible)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null!");
            return;
        }

        if (other.CompareTag("Target"))
        {
            GameManager.Instance.HandleTargetCollision();
            StartInvincibility();
        }
        else if (other.CompareTag("Obstacle") && !isInvincible)
        {
            GameManager.Instance.HandleObstacleCollision(other.gameObject);
            StartInvincibility();
        }
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    public void ResetPlayer()
    {
        transform.position = Vector3.zero; // 또는 시작 위치로 설정
        transform.rotation = Quaternion.identity;
        moveDirection = Vector3.forward;
        isMoving = true;
        isInvincible = false;
        if (playerRenderer != null)
        {
            playerRenderer.enabled = true;
        }
        StopAllCoroutines();
    }
}