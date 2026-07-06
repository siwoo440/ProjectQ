using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Move Type")]
    public EnemyMoveType moveType = EnemyMoveType.Chase; // 적 이동 방식을 저장한다.

    [Header("Basic Move")]
    public float moveSpeed = 2f; // 기본 이동 속도를 저장한다.
    public float stopDistance = 0.5f; // 플레이어에게 이 거리 이하로 가까워지면 멈춘다.
    public float retreatDistance = 2f; // 거리 유지형 적이 이 거리보다 가까우면 뒤로 물러난다.

    [Header("Charge Move")]
    public float chargeSpeed = 7f; // 돌진 속도를 저장한다.
    public float chargeCooldown = 2.5f; // 돌진 재사용 시간을 저장한다.
    public float chargeReadyTime = 0.4f; // 돌진 전 준비 시간을 저장한다.
    public float chargeDuration = 0.35f; // 돌진 지속 시간을 저장한다.

    private Transform playerTransform; // 플레이어 위치를 저장한다.
    private Rigidbody2D rb; // 적 Rigidbody2D를 저장한다.

    private float chargeTimer = 0f; // 돌진 대기 시간을 저장한다.
    private float readyTimer = 0f; // 돌진 준비 시간을 저장한다.
    private float chargingTimer = 0f; // 돌진 중 남은 시간을 저장한다.

    private bool isPreparingCharge = false; // 돌진 준비 중인지 저장한다.
    private bool isCharging = false; // 돌진 중인지 저장한다.
    private Vector2 chargeDirection; // 돌진 방향을 저장한다.

    private void Awake() // 필요한 컴포넌트를 가져온다.
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D를 가져온다.
    }

    private void Start() // 시작 시 플레이어를 찾는다.
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); // Player 태그를 가진 오브젝트를 찾는다.

        if (playerObject != null) // 플레이어 오브젝트가 있는지 확인한다.
        {
            playerTransform = playerObject.transform; // 플레이어 Transform을 저장한다.
        }
    }

    private void FixedUpdate() // 물리 이동을 처리한다.
    {
        if (playerTransform == null) // 플레이어가 없는지 확인한다.
        {
            StopMove(); // 이동을 멈춘다.
            return; // 이동 처리를 중단한다.
        }

        switch (moveType) // 적 이동 타입을 확인한다.
        {
            case EnemyMoveType.Chase: // 추적형 적인지 확인한다.
                MoveChase(); // 추적 이동을 실행한다.
                break; // switch문을 종료한다.

            case EnemyMoveType.KeepDistance: // 거리 유지형 적인지 확인한다.
                MoveKeepDistance(); // 거리 유지 이동을 실행한다.
                break; // switch문을 종료한다.

            case EnemyMoveType.Charge: // 돌진형 적인지 확인한다.
                MoveCharge(); // 돌진 이동을 실행한다.
                break; // switch문을 종료한다.
        }
    }

    private void MoveChase() // 플레이어를 계속 추적한다.
    {
        Vector2 direction = GetDirectionToPlayer(); // 플레이어 방향을 가져온다.
        float distance = GetDistanceToPlayer(); // 플레이어와의 거리를 가져온다.

        if (distance <= stopDistance) // 너무 가까운지 확인한다.
        {
            StopMove(); // 이동을 멈춘다.
            return; // 추적을 중단한다.
        }

        Move(direction, moveSpeed); // 플레이어 방향으로 이동한다.
    }

    private void MoveKeepDistance() // 플레이어와 거리를 유지한다.
    {
        Vector2 direction = GetDirectionToPlayer(); // 플레이어 방향을 가져온다.
        float distance = GetDistanceToPlayer(); // 플레이어와의 거리를 가져온다.

        if (distance > stopDistance) // 너무 멀리 떨어져 있는지 확인한다.
        {
            Move(direction, moveSpeed); // 플레이어 쪽으로 접근한다.
        }
        else if (distance < retreatDistance) // 너무 가까운지 확인한다.
        {
            Move(-direction, moveSpeed); // 플레이어 반대 방향으로 물러난다.
        }
        else // 적당한 거리에 있는 경우다.
        {
            StopMove(); // 이동을 멈춘다.
        }
    }

    private void MoveCharge() // 플레이어를 향해 돌진한다.
    {
        if (isCharging) // 현재 돌진 중인지 확인한다.
        {
            chargingTimer -= Time.fixedDeltaTime; // 돌진 시간을 감소시킨다.

            Move(chargeDirection, chargeSpeed); // 저장된 방향으로 빠르게 이동한다.

            if (chargingTimer <= 0f) // 돌진 시간이 끝났는지 확인한다.
            {
                isCharging = false; // 돌진 상태를 해제한다.
                chargeTimer = chargeCooldown; // 다음 돌진 대기 시간을 설정한다.
                StopMove(); // 이동을 멈춘다.
            }

            return; // 돌진 중에는 다른 이동을 하지 않는다.
        }

        if (isPreparingCharge) // 현재 돌진 준비 중인지 확인한다.
        {
            readyTimer -= Time.fixedDeltaTime; // 준비 시간을 감소시킨다.
            StopMove(); // 준비 중에는 멈춘다.

            if (readyTimer <= 0f) // 준비 시간이 끝났는지 확인한다.
            {
                isPreparingCharge = false; // 준비 상태를 해제한다.
                isCharging = true; // 돌진 상태로 변경한다.
                chargingTimer = chargeDuration; // 돌진 지속 시간을 설정한다.
                chargeDirection = GetDirectionToPlayer(); // 돌진 방향을 현재 플레이어 방향으로 저장한다.
            }

            return; // 준비 중에는 다른 이동을 하지 않는다.
        }

        chargeTimer -= Time.fixedDeltaTime; // 돌진 대기 시간을 감소시킨다.

        if (chargeTimer <= 0f) // 돌진할 시간이 되었는지 확인한다.
        {
            isPreparingCharge = true; // 돌진 준비 상태로 변경한다.
            readyTimer = chargeReadyTime; // 준비 시간을 설정한다.
            StopMove(); // 준비 시작 시 멈춘다.
            return; // 이동을 중단한다.
        }

        Vector2 direction = GetDirectionToPlayer(); // 플레이어 방향을 가져온다.
        Move(direction, moveSpeed); // 돌진 대기 중에는 천천히 접근한다.
    }

    private Vector2 GetDirectionToPlayer() // 플레이어 방향을 계산한다.
    {
        Vector2 direction = playerTransform.position - transform.position; // 적에서 플레이어까지의 방향을 계산한다.

        if (direction.sqrMagnitude <= 0.001f) // 방향 값이 너무 작은지 확인한다.
        {
            return Vector2.zero; // 방향 없음으로 반환한다.
        }

        return direction.normalized; // 정규화된 방향을 반환한다.
    }

    private float GetDistanceToPlayer() // 플레이어와의 거리를 계산한다.
    {
        return Vector2.Distance(transform.position, playerTransform.position); // 현재 위치와 플레이어 위치 사이의 거리를 반환한다.
    }

    private void Move(Vector2 direction, float speed) // 지정한 방향과 속도로 이동한다.
    {
        if (rb != null) // Rigidbody2D가 있는지 확인한다.
        {
            rb.linearVelocity = direction * speed; // Rigidbody2D 속도를 설정한다.
        }
        else // Rigidbody2D가 없는 경우다.
        {
            transform.position += (Vector3)(direction * speed * Time.fixedDeltaTime); // Transform으로 직접 이동한다.
        }
    }

    private void StopMove() // 이동을 멈춘다.
    {
        if (rb != null) // Rigidbody2D가 있는지 확인한다.
        {
            rb.linearVelocity = Vector2.zero; // Rigidbody2D 속도를 0으로 만든다.
        }
    }
}