using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f; // 탄환 이동 속도를 저장한다.
    public int damage = 10; // 탄환 피해량을 저장한다.
    public float lifeTime = 3f; // 탄환 생존 시간을 저장한다.

    private Vector2 moveDirection = Vector2.right; // 탄환 이동 방향을 저장한다.
    private Rigidbody2D rb; // 탄환 Rigidbody2D를 저장한다.

    private void Awake() // 탄환이 생성될 때 Rigidbody2D를 가져온다.
    {
        rb = GetComponent<Rigidbody2D>(); // 현재 오브젝트의 Rigidbody2D를 가져온다.
    }

    private void Start() // 탄환 생성 후 자동 삭제 시간을 설정한다.
    {
        Destroy(gameObject, lifeTime); // lifeTime 시간이 지나면 탄환을 삭제한다.
    }

    private void FixedUpdate() // 물리 업데이트마다 탄환을 이동시킨다.
    {
        if (rb == null) return; // Rigidbody2D가 없으면 실행하지 않는다.

        rb.linearVelocity = moveDirection * speed; // 지정된 방향으로 탄환을 이동시킨다.
    }

    public void Initialize(Vector2 direction, float newSpeed, int newDamage, float newLifeTime) // 탄환 정보를 외부에서 설정한다.
    {
        moveDirection = direction.normalized; // 이동 방향을 정규화해서 저장한다.
        speed = newSpeed; // 탄환 속도를 설정한다.
        damage = newDamage; // 탄환 피해량을 설정한다.
        lifeTime = newLifeTime; // 탄환 생존 시간을 설정한다.
    }

    private void OnTriggerEnter2D(Collider2D other) // 다른 Collider와 닿았을 때 실행한다.
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>(); // 닿은 대상에서 EnemyHealth를 찾는다.

        if (enemyHealth == null) return; // EnemyHealth가 없으면 적이 아니므로 실행하지 않는다.

        enemyHealth.TakeDamage(damage); // 적에게 피해를 준다.
        Destroy(gameObject); // 적에게 닿은 탄환을 삭제한다.
    }
}