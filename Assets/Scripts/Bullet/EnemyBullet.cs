using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Enemy Bullet Settings")]
    public float speed = 5f; // 적 탄환 속도를 저장한다.
    public int damage = 35; // 적 탄환 피해량을 저장한다.
    public float lifeTime = 5f; // 적 탄환 생존 시간을 저장한다.

    private Vector2 moveDirection = Vector2.down; // 탄환 이동 방향을 저장한다.
    private Rigidbody2D rb; // Rigidbody2D를 저장한다.

    private void Awake() // 탄환이 생성될 때 Rigidbody2D를 가져온다.
    {
        rb = GetComponent<Rigidbody2D>(); // 현재 오브젝트의 Rigidbody2D를 가져온다.
    }

    private void Start() // 탄환이 생성된 뒤 자동 삭제 시간을 설정한다.
    {
        Destroy(gameObject, lifeTime); // lifeTime 시간이 지나면 탄환을 삭제한다.
    }

    private void FixedUpdate() // 물리 업데이트마다 탄환을 이동시킨다.
    {
        if (rb == null) return; // Rigidbody2D가 없으면 실행하지 않는다.

        rb.linearVelocity = moveDirection * speed; // 지정된 방향으로 탄환을 이동시킨다.
    }

    public void Initialize(Vector2 direction, float newSpeed, int newDamage, float newLifeTime) // 탄환 정보를 설정한다.
    {
        moveDirection = direction.normalized; // 이동 방향을 정규화해서 저장한다.
        speed = newSpeed; // 탄환 속도를 설정한다.
        damage = newDamage; // 탄환 피해량을 설정한다.
        lifeTime = newLifeTime; // 탄환 생존 시간을 설정한다.
    }

    private void OnTriggerEnter2D(Collider2D other) // 다른 Collider와 닿았을 때 실행한다.
    {
        PlayerStats playerStats = other.GetComponent<PlayerStats>(); // 닿은 대상에서 PlayerStats를 찾는다.

        if (playerStats == null) return; // PlayerStats가 없으면 플레이어가 아니므로 실행하지 않는다.

        PlayerDodge playerDodge = other.GetComponent<PlayerDodge>(); // 닿은 대상에서 PlayerDodge를 찾는다.

        if (playerDodge != null && playerDodge.IsInvincible) // 플레이어가 무적 상태인지 확인한다.
        {
            Debug.Log("회피 무적으로 탄환 회피"); // 회피 성공 로그를 출력한다.
            Destroy(gameObject); // 회피한 탄환을 삭제한다.
            return; // 피해를 주지 않고 종료한다.
        }

        playerStats.TakeDamage(damage); // 플레이어에게 피해를 준다.
        Destroy(gameObject); // 플레이어에게 닿은 탄환을 삭제한다.
    }
}