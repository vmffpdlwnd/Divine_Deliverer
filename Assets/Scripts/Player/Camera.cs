using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform target; // 트럭의 Transform
    [SerializeField] private float distance = 10f; // 카메라와 트럭 사이의 거리
    [SerializeField] private float height = 5f; // 카메라의 높이
    [SerializeField] private float angle = 45f; // 카메라의 각도 (0도는 수평, 90도는 완전한 탑다운 뷰)
    [SerializeField] private float smoothSpeed = 0.125f; // 카메라 움직임의 부드러움

    private Vector3 fixedRotation;

    private void Start()
    {
        // 시작 시 카메라의 고정된 회전 설정
        fixedRotation = Quaternion.Euler(angle, 0, 0).eulerAngles;

        // 초기 카메라 위치 및 회전 설정
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
        // 카메라 위치 계산
        float angleRad = angle * Mathf.Deg2Rad;
        Vector3 targetPosition = target.position;
        Vector3 cameraPosition = targetPosition - Vector3.forward * distance * Mathf.Cos(angleRad);
        cameraPosition.y = targetPosition.y + height + distance * Mathf.Sin(angleRad);

        // 부드러운 이동
        transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothSpeed);

        // 카메라 회전을 고정된 각도로 유지
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}