using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Shoot Settings")]
    public bool canShoot = false; // 탄환을 발사할 수 있는지 저장한다.
    public EnemyAttackPattern attackPattern = EnemyAttackPattern.Single; // 적 공격 패턴을 저장한다.
    public GameObject enemyBulletPrefab; // 적 탄환 프리팹을 저장한다.
    public Transform firePoint; // 탄환 발사 위치를 저장한다.
    public float shootInterval = 1.5f; // 탄환 발사 간격을 저장한다.

    [Header("Bullet Settings")]
    public int bulletDamage = 10; // 탄환 피해량을 저장한다.
    public float bulletSpeed = 5f; // 탄환 속도를 저장한다.
    public float bulletLifeTime = 4f; // 탄환 생존 시간을 저장한다.

    [Header("Burst Settings")]
    public int burstCount = 3; // 연사 탄환 수를 저장한다.
    public float burstInterval = 0.12f; // 연사 탄환 사이 간격을 저장한다.

    [Header("Spread Settings")]
    public int spreadBulletCount = 5; // 산탄 탄환 수를 저장한다.
    public float spreadAngle = 12f; // 산탄 탄환 사이 각도를 저장한다.

    [Header("Circle Settings")]
    public int circleBulletCount = 12; // 원형 탄막 탄환 수를 저장한다.

    private Transform playerTransform; // 플레이어 위치를 저장한다.
    private float shootTimer = 0f; // 발사 대기 시간을 저장한다.
    private bool isBursting = false; // 연사 중인지 저장한다.

    private void Start() // 시작 시 플레이어를 찾는다.
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); // Player 태그를 가진 오브젝트를 찾는다.

        if (playerObject != null) // 플레이어 오브젝트가 있는지 확인한다.
        {
            playerTransform = playerObject.transform; // 플레이어 Transform을 저장한다.
        }

        shootTimer = Random.Range(0f, shootInterval); // 적마다 첫 발사 타이밍을 다르게 만든다.
    }

    private void Update() // 매 프레임 발사 타이머를 처리한다.
    {
        if (canShoot == false) return; // 발사할 수 없는 적이면 실행하지 않는다.
        if (enemyBulletPrefab == null) return; // 탄환 프리팹이 없으면 실행하지 않는다.
        if (playerTransform == null) return; // 플레이어가 없으면 실행하지 않는다.
        if (isBursting) return; // 연사 중이면 새 공격을 시작하지 않는다.

        shootTimer -= Time.deltaTime; // 발사 대기 시간을 감소시킨다.

        if (shootTimer <= 0f) // 발사 시간이 되었는지 확인한다.
        {
            ShootByPattern(); // 현재 공격 패턴에 따라 발사한다.
            shootTimer = shootInterval; // 발사 대기 시간을 초기화한다.
        }
    }

    private void ShootByPattern() // 공격 패턴에 따라 발사 방식을 선택한다.
    {
        switch (attackPattern) // 공격 패턴을 확인한다.
        {
            case EnemyAttackPattern.Single: // 단일 탄환 패턴인지 확인한다.
                ShootSingle(); // 단일 탄환을 발사한다.
                break; // switch문을 종료한다.

            case EnemyAttackPattern.Burst: // 연사 패턴인지 확인한다.
                StartCoroutine(ShootBurstRoutine()); // 연사 코루틴을 실행한다.
                break; // switch문을 종료한다.

            case EnemyAttackPattern.Spread: // 산탄 패턴인지 확인한다.
                ShootSpread(); // 산탄을 발사한다.
                break; // switch문을 종료한다.

            case EnemyAttackPattern.Circle: // 원형 탄막 패턴인지 확인한다.
                ShootCircle(); // 원형 탄막을 발사한다.
                break; // switch문을 종료한다.
        }
    }

    private void ShootSingle() // 플레이어 방향으로 탄환 1발을 발사한다.
    {
        Vector2 direction = GetDirectionToPlayer(); // 플레이어 방향을 가져온다.

        CreateBullet(direction); // 탄환을 생성한다.
    }

    private IEnumerator ShootBurstRoutine() // 짧은 간격으로 여러 발을 연속 발사한다.
    {
        isBursting = true; // 연사 상태로 변경한다.

        for (int i = 0; i < burstCount; i++) // 연사 탄환 수만큼 반복한다.
        {
            if (playerTransform == null) break; // 플레이어가 없으면 연사를 중단한다.

            Vector2 direction = GetDirectionToPlayer(); // 매 발마다 플레이어 방향을 다시 계산한다.
            CreateBullet(direction); // 탄환을 생성한다.

            yield return new WaitForSeconds(burstInterval); // 다음 탄환까지 잠시 기다린다.
        }

        isBursting = false; // 연사 상태를 해제한다.
    }

    private void ShootSpread() // 플레이어 방향을 기준으로 부채꼴 탄환을 발사한다.
    {
        Vector2 baseDirection = GetDirectionToPlayer(); // 플레이어 방향을 기준 방향으로 가져온다.
        float centerIndex = (spreadBulletCount - 1) / 2f; // 가운데 탄환 기준 인덱스를 계산한다.

        for (int i = 0; i < spreadBulletCount; i++) // 산탄 탄환 수만큼 반복한다.
        {
            float angleOffset = (i - centerIndex) * spreadAngle; // 각 탄환의 회전 각도를 계산한다.
            Vector2 direction = RotateVector(baseDirection, angleOffset); // 기준 방향을 회전한다.

            CreateBullet(direction); // 탄환을 생성한다.
        }
    }

    private void ShootCircle() // 적 주변 모든 방향으로 탄환을 발사한다.
    {
        if (circleBulletCount <= 0) return; // 탄환 수가 0 이하이면 실행하지 않는다.

        float angleStep = 360f / circleBulletCount; // 탄환 사이 각도를 계산한다.

        for (int i = 0; i < circleBulletCount; i++) // 원형 탄환 수만큼 반복한다.
        {
            float angle = angleStep * i; // 현재 탄환의 각도를 계산한다.
            Vector2 direction = AngleToDirection(angle); // 각도를 방향 벡터로 변환한다.

            CreateBullet(direction); // 탄환을 생성한다.
        }
    }

    private void CreateBullet(Vector2 direction) // 탄환을 생성한다.
    {
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position; // 발사 위치를 결정한다.
        GameObject bulletObject = Instantiate(enemyBulletPrefab, spawnPosition, Quaternion.identity); // 탄환 프리팹을 생성한다.
        EnemyBullet enemyBullet = bulletObject.GetComponent<EnemyBullet>(); // EnemyBullet 컴포넌트를 가져온다.

        if (enemyBullet == null) // EnemyBullet 컴포넌트가 없는지 확인한다.
        {
            Debug.LogError("Enemy Bullet Prefab has no EnemyBullet component."); // 오류 로그를 출력한다.
            return; // 탄환 설정을 중단한다.
        }

        enemyBullet.Initialize(direction, bulletSpeed, bulletDamage, bulletLifeTime); // 탄환 정보를 설정한다.
    }

    private Vector2 GetDirectionToPlayer() // 플레이어 방향을 계산한다.
    {
        Vector2 direction = playerTransform.position - transform.position; // 적에서 플레이어까지의 방향을 계산한다.

        if (direction.sqrMagnitude <= 0.001f) // 방향 값이 너무 작은지 확인한다.
        {
            return Vector2.down; // 기본 방향을 아래로 반환한다.
        }

        return direction.normalized; // 정규화된 방향을 반환한다.
    }

    private Vector2 RotateVector(Vector2 vector, float angle) // 벡터를 지정 각도만큼 회전한다.
    {
        float radian = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환한다.
        float cos = Mathf.Cos(radian); // 코사인 값을 계산한다.
        float sin = Mathf.Sin(radian); // 사인 값을 계산한다.

        float x = vector.x * cos - vector.y * sin; // 회전된 x값을 계산한다.
        float y = vector.x * sin + vector.y * cos; // 회전된 y값을 계산한다.

        return new Vector2(x, y).normalized; // 회전된 방향을 반환한다.
    }

    private Vector2 AngleToDirection(float angle) // 각도를 방향 벡터로 변환한다.
    {
        float radian = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환한다.
        float x = Mathf.Cos(radian); // x 방향 값을 계산한다.
        float y = Mathf.Sin(radian); // y 방향 값을 계산한다.

        return new Vector2(x, y).normalized; // 방향 벡터를 반환한다.
    }
}