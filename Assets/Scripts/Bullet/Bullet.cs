using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Basic Settings")]
    private Vector2 moveDirection = Vector2.up; // 탄환 이동 방향을 저장한다.
    private float moveSpeed = 10f; // 탄환 이동 속도를 저장한다.
    private int damage = 10; // 탄환 데미지를 저장한다.
    private float lifeTime = 3f; // 탄환 생존 시간을 저장한다.

    [Header("Effect Settings")]
    private BulletEffectType effectType = BulletEffectType.Normal; // 탄환 효과 타입을 저장한다.
    private int remainPierceCount = 0; // 남은 관통 횟수를 저장한다.
    private float explosionRadius = 0f; // 폭발 범위를 저장한다.
    private float homingRange = 0f; // 유도 범위를 저장한다.
    private float homingTurnSpeed = 0f; // 유도 회전 속도를 저장한다.

    public void Initialize(Vector2 direction, float speed, int newDamage, float newLifeTime) // 일반 탄환 정보를 초기화한다.
    {
        moveDirection = direction.normalized; // 이동 방향을 정규화해서 저장한다.
        moveSpeed = speed; // 이동 속도를 저장한다.
        damage = newDamage; // 데미지를 저장한다.
        lifeTime = newLifeTime; // 생존 시간을 저장한다.
        effectType = BulletEffectType.Normal; // 일반 탄환으로 설정한다.
    }

    public void InitializeSpecial(Vector2 direction, float speed, int newDamage, float newLifeTime, BulletEffectType newEffectType, int newPierceCount, float newExplosionRadius, float newHomingRange, float newHomingTurnSpeed) // 특수 탄환 정보를 초기화한다.
    {
        moveDirection = direction.normalized; // 이동 방향을 정규화해서 저장한다.
        moveSpeed = speed; // 이동 속도를 저장한다.
        damage = newDamage; // 데미지를 저장한다.
        lifeTime = newLifeTime; // 생존 시간을 저장한다.
        effectType = newEffectType; // 탄환 효과 타입을 저장한다.
        remainPierceCount = newPierceCount; // 관통 횟수를 저장한다.
        explosionRadius = newExplosionRadius; // 폭발 범위를 저장한다.
        homingRange = newHomingRange; // 유도 범위를 저장한다.
        homingTurnSpeed = newHomingTurnSpeed; // 유도 회전 속도를 저장한다.
    }

    private void Update() // 매 프레임 탄환을 이동시킨다.
    {
        if (effectType == BulletEffectType.Homing) // 유도 탄환인지 확인한다.
        {
            UpdateHomingDirection(); // 가까운 적 방향으로 이동 방향을 보정한다.
        }

        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime); // 탄환을 이동시킨다.

        lifeTime -= Time.deltaTime; // 생존 시간을 감소시킨다.

        if (lifeTime <= 0f) // 생존 시간이 끝났는지 확인한다.
        {
            Destroy(gameObject); // 탄환을 제거한다.
        }
    }

    private void UpdateHomingDirection() // 가까운 적을 찾아 탄환 방향을 보정한다.
    {
        EnemyHealth targetEnemy = FindNearestEnemy(); // 가장 가까운 적을 찾는다.

        if (targetEnemy == null) return; // 적이 없으면 실행하지 않는다.

        Vector2 targetDirection = ((Vector2)targetEnemy.transform.position - (Vector2)transform.position).normalized; // 적 방향을 계산한다.
        moveDirection = Vector2.Lerp(moveDirection, targetDirection, homingTurnSpeed * Time.deltaTime).normalized; // 기존 방향에서 적 방향으로 부드럽게 회전한다.
    }

    private EnemyHealth FindNearestEnemy() // 유도 범위 안에서 가장 가까운 적을 찾는다.
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, homingRange); // 범위 안의 Collider를 모두 찾는다.

        EnemyHealth nearestEnemy = null; // 가장 가까운 적을 저장한다.
        float nearestDistance = float.MaxValue; // 가장 가까운 거리를 저장한다.

        for (int i = 0; i < colliders.Length; i++) // 찾은 Collider 수만큼 반복한다.
        {
            EnemyHealth enemyHealth = colliders[i].GetComponent<EnemyHealth>(); // EnemyHealth 컴포넌트를 가져온다.

            if (enemyHealth == null) continue; // 적이 아니면 건너뛴다.

            float distance = Vector2.Distance(transform.position, enemyHealth.transform.position); // 적과의 거리를 계산한다.

            if (distance < nearestDistance) // 더 가까운 적인지 확인한다.
            {
                nearestDistance = distance; // 가장 가까운 거리를 갱신한다.
                nearestEnemy = enemyHealth; // 가장 가까운 적을 갱신한다.
            }
        }

        return nearestEnemy; // 가장 가까운 적을 반환한다.
    }

    private void OnTriggerEnter2D(Collider2D collision) // 다른 Collider와 충돌했을 때 실행한다.
    {
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>(); // 충돌 대상의 EnemyHealth를 가져온다.

        if (enemyHealth == null) return; // 적이 아니면 실행하지 않는다.

        enemyHealth.TakeDamage(damage); // 적에게 피해를 준다.

        if (effectType == BulletEffectType.Bomb) // 폭발 탄환인지 확인한다.
        {
            Explode(); // 폭발 피해를 처리한다.
            Destroy(gameObject); // 탄환을 제거한다.
            return; // 충돌 처리를 종료한다.
        }

        if (effectType == BulletEffectType.Pierce) // 관통 탄환인지 확인한다.
        {
            remainPierceCount--; // 남은 관통 횟수를 감소시킨다.

            if (remainPierceCount <= 0) // 관통 횟수를 모두 사용했는지 확인한다.
            {
                Destroy(gameObject); // 탄환을 제거한다.
            }

            return; // 충돌 처리를 종료한다.
        }

        Destroy(gameObject); // 일반 탄환과 유도 탄환은 적중 시 제거한다.
    }

    private void Explode() // 폭발 범위 안의 적에게 추가 피해를 준다.
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius); // 폭발 범위 안의 Collider를 모두 찾는다.

        for (int i = 0; i < colliders.Length; i++) // 찾은 Collider 수만큼 반복한다.
        {
            EnemyHealth enemyHealth = colliders[i].GetComponent<EnemyHealth>(); // EnemyHealth 컴포넌트를 가져온다.

            if (enemyHealth == null) continue; // 적이 아니면 건너뛴다.

            enemyHealth.TakeDamage(damage); // 폭발 추가 피해를 준다.
        }
    }
}