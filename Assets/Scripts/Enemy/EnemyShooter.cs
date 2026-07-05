using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("References")]
    public Transform target; // 공격할 대상을 저장한다.
    public GameObject enemyBulletPrefab; // 적 탄환 프리팹을 저장한다.
    public Transform firePoint; // 탄환 발사 위치를 저장한다.

    [Header("Shoot Settings")]
    public float shootInterval = 1.5f; // 발사 간격을 저장한다.
    public float bulletSpeed = 5f; // 탄환 속도를 저장한다.
    public int bulletDamage = 35; // 탄환 피해량을 저장한다.
    public float bulletLifeTime = 5f; // 탄환 생존 시간을 저장한다.
    public float firstShootDelay = 1f; // 첫 발사까지의 대기 시간을 저장한다.

    private float nextShootTime; // 다음 발사 가능 시간을 저장한다.

    private void Start() // 시작 시 기본 참조를 설정한다.
    {
        if (target == null) // 대상이 연결되지 않았는지 확인한다.
        {
            PlayerStats playerStats = FindFirstObjectByType<PlayerStats>(); // 씬에서 PlayerStats를 찾는다.

            if (playerStats != null) // PlayerStats가 있는지 확인한다.
            {
                target = playerStats.transform; // 플레이어 Transform을 대상으로 설정한다.
            }
        }

        if (firePoint == null) // 발사 위치가 연결되지 않았는지 확인한다.
        {
            firePoint = transform; // 적 자신의 위치에서 발사하게 설정한다.
        }

        nextShootTime = Time.time + firstShootDelay; // 첫 발사 시간을 설정한다.
    }

    private void Update() // 매 프레임 발사 가능 여부를 확인한다.
    {
        if (target == null) return; // 대상이 없으면 실행하지 않는다.
        if (enemyBulletPrefab == null) return; // 탄환 프리팹이 없으면 실행하지 않는다.

        if (Time.time >= nextShootTime) // 현재 시간이 다음 발사 시간 이상인지 확인한다.
        {
            ShootAtTarget(); // 대상 방향으로 탄환을 발사한다.
            nextShootTime = Time.time + shootInterval; // 다음 발사 시간을 설정한다.
        }
    }

    private void ShootAtTarget() // 대상 방향으로 탄환을 발사한다.
    {
        Vector2 direction = target.position - firePoint.position; // 적에서 플레이어까지의 방향을 계산한다.

        if (direction.sqrMagnitude <= 0.001f) // 방향 값이 너무 작은지 확인한다.
        {
            direction = Vector2.down; // 기본 방향을 아래로 설정한다.
        }

        GameObject bulletObject = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity); // 적 탄환을 생성한다.
        EnemyBullet enemyBullet = bulletObject.GetComponent<EnemyBullet>(); // 생성된 탄환의 EnemyBullet 컴포넌트를 가져온다.

        if (enemyBullet == null) // EnemyBullet 컴포넌트가 없는지 확인한다.
        {
            Debug.LogError("EnemyBullet 프리팹에 EnemyBullet.cs가 없습니다."); // 오류 로그를 출력한다.
            return; // 발사 처리를 중단한다.
        }

        enemyBullet.Initialize(direction, bulletSpeed, bulletDamage, bulletLifeTime); // 탄환의 이동 방향과 수치를 설정한다.
    }
}