using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 90f;

    private Vector3 moveDirection = Vector3.forward;
    private bool isMoving = true;

    private void Update()
    {
        if (isMoving)
        {
            // �ڵ� ����
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        // �Է� ó��
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
}
