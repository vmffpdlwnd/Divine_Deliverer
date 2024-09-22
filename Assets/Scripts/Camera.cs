using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform target; // Ʈ���� Transform
    [SerializeField] private float distance = 10f; // ī�޶�� Ʈ�� ������ �Ÿ�
    [SerializeField] private float height = 5f; // ī�޶��� ����
    [SerializeField] private float angle = 45f; // ī�޶��� ���� (0���� ����, 90���� ������ ž�ٿ� ��)
    [SerializeField] private float smoothSpeed = 0.125f; // ī�޶� �������� �ε巯��

    private Vector3 fixedRotation;

    private void Start()
    {
        // ���� �� ī�޶��� ������ ȸ�� ����
        fixedRotation = Quaternion.Euler(angle, 0, 0).eulerAngles;

        // �ʱ� ī�޶� ��ġ �� ȸ�� ����
        UpdateCameraPosition();
        transform.rotation = Quaternion.Euler(fixedRotation);
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is not set!");
            return;
        }

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        // ī�޶� ��ġ ���
        float angleRad = angle * Mathf.Deg2Rad;
        Vector3 targetPosition = target.position;
        Vector3 cameraPosition = targetPosition - Vector3.forward * distance * Mathf.Cos(angleRad);
        cameraPosition.y = targetPosition.y + height + distance * Mathf.Sin(angleRad);

        // �ε巯�� �̵�
        transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothSpeed);

        // ī�޶� ȸ���� ������ ������ ����
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}