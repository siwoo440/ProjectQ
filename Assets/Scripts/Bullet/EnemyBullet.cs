using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public int damage = 10; // 탄환 피해량을 저장한다.
    public float moveSpeed = 5f; // 탄환 이동 속도를 저장한다.
    public float lifeTime = 4f; // 탄환 생존 시간을 저장한다.

    private Vector2 moveDirection = Vector2.down; // 탄환 이동 방향을 저장한다.

    public void Initialize(Vector2 direction, float speed, int newDamage, float newLifeTime) // 탄환 정보를 설정한다.
    {
        moveDirection = direction.normalized; // 이동 방향을 설정한다.
        moveSpeed = speed; // 이동 속도를 설정한다.
        damage = newDamage; // 피해량을 설정한다.
        lifeTime = newLifeTime; // 생존 시간을 설정한다.

        Destroy(gameObject, lifeTime); // 일정 시간 후 탄환을 삭제한다.
    }

    private void Start() // 초기화 없이 생성된 탄환의 삭제 시간을 설정한다.
    {
        Destroy(gameObject, lifeTime); // 생존 시간이 지나면 탄환을 삭제한다.
    }

    private void Update() // 매 프레임 탄환을 이동시킨다.
    {
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime); // 설정된 방향으로 탄환을 이동시킨다.
    }

    private void OnTriggerEnter2D(Collider2D other) // 다른 오브젝트와 닿았을 때 실행한다.
    {
        if (other.CompareTag("Player") == false) return; // Player가 아니면 실행하지 않는다.

        PlayerDodge playerDodge = other.GetComponent<PlayerDodge>(); // 플레이어 회피 컴포넌트를 가져온다.

        if (playerDodge != null && playerDodge.IsInvincible) // 플레이어가 무적 상태인지 확인한다.
        {
            Destroy(gameObject); // 무적 상태여도 탄환은 삭제한다.
            return; // 피해 처리를 중단한다.
        }

        PlayerStats playerStats = other.GetComponent<PlayerStats>(); // 플레이어 스탯 컴포넌트를 가져온다.

        if (playerStats != null) // 플레이어 스탯이 있는지 확인한다.
        {
            playerStats.TakeDamage(damage); // 플레이어에게 피해를 준다.
        }

        Destroy(gameObject); // 탄환을 삭제한다.
    }
}