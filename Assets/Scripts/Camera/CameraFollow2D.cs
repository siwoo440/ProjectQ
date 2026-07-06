using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // 카메라가 따라갈 대상을 저장한다.

    [Header("Follow Settings")]
    public float smoothTime = 0.12f; // 카메라가 부드럽게 따라가는 시간을 저장한다.
    public Vector3 offset = new Vector3(0f, 0f, -10f); // 카메라와 플레이어 사이의 기본 거리를 저장한다.

    [Header("Limit Settings")]
    public bool useCameraLimit = false; // 카메라 이동 제한을 사용할지 저장한다.
    public Vector2 minPosition = new Vector2(-20f, -20f); // 카메라 최소 위치를 저장한다.
    public Vector2 maxPosition = new Vector2(20f, 20f); // 카메라 최대 위치를 저장한다.

    private Vector3 currentVelocity; // SmoothDamp에서 사용할 현재 속도를 저장한다.

    private void LateUpdate() // 플레이어 이동이 끝난 뒤 카메라를 따라가게 한다.
    {
        if (target == null) return; // 따라갈 대상이 없으면 실행하지 않는다.

        Vector3 targetPosition = target.position + offset; // 플레이어 위치를 기준으로 카메라 목표 위치를 계산한다.

        if (useCameraLimit) // 카메라 제한을 사용하는지 확인한다.
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x); // x 위치를 제한한다.
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y); // y 위치를 제한한다.
        }

        targetPosition.z = offset.z; // 카메라 z 위치를 고정한다.

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime); // 카메라를 부드럽게 이동시킨다.
    }
}