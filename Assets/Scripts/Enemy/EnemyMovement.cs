using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    public Transform target; // 따라갈 대상을 저장한다.

    [Header("Movement Settings")]
    public float moveSpeed = 1.5f; // 적 이동 속도를 저장한다.
    public float stopDistance = 3f; // 플레이어와 이 거리보다 가까우면 멈춘다.

    private Rigidbody2D rb; // 적 Rigidbody2D를 저장한다.

    private void Awake() // 시작 전에 Rigidbody2D를 가져온다.
    {
        rb = GetComponent<Rigidbody2D>(); // 현재 오브젝트의 Rigidbody2D를 가져온다.
    }

    private void Start() // 시작 시 타겟을 자동으로 찾는다.
    {
        if (target == null) // 대상이 연결되지 않았는지 확인한다.
        {
            PlayerStats playerStats = FindFirstObjectByType<PlayerStats>(); // 씬에서 PlayerStats를 찾는다.

            if (playerStats != null) // PlayerStats가 있는지 확인한다.
            {
                target = playerStats.transform; // 플레이어 Transform을 대상으로 설정한다.
            }
        }
    }

    private void FixedUpdate() // 물리 업데이트마다 적 이동을 처리한다.
    {
        if (target == null) return; // 대상이 없으면 실행하지 않는다.
        if (rb == null) return; // Rigidbody2D가 없으면 실행하지 않는다.

        MoveToTarget(); // 대상 방향으로 이동한다.
    }

    private void MoveToTarget() // 플레이어를 향해 이동한다.
    {
        Vector2 direction = target.position - transform.position; // 적에서 플레이어까지의 방향을 계산한다.
        float distance = direction.magnitude; // 플레이어와의 거리를 계산한다.

        if (distance <= stopDistance) // 정지 거리 안에 들어왔는지 확인한다.
        {
            rb.linearVelocity = Vector2.zero; // 이동을 멈춘다.
            return; // 이동 처리를 종료한다.
        }

        rb.linearVelocity = direction.normalized * moveSpeed; // 플레이어 방향으로 이동한다.
    }
}