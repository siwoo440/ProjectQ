using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 50; // 적 최대 체력을 저장한다.
    public int currentHealth; // 적 현재 체력을 저장한다.

    private void Awake() // 적이 생성될 때 체력을 초기화한다.
    {
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 설정한다.
    }

    public void TakeDamage(int damage) // 적이 피해를 받는다.
    {
        currentHealth -= damage; // 현재 체력을 피해량만큼 감소시킨다.
        currentHealth = Mathf.Max(currentHealth, 0); // 체력이 0 아래로 내려가지 않게 한다.

        Debug.Log(gameObject.name + " damaged : " + damage); // 피해량을 로그로 출력한다.
        Debug.Log(gameObject.name + " HP : " + currentHealth); // 현재 체력을 로그로 출력한다.

        if (currentHealth <= 0) // 체력이 0 이하인지 확인한다.
        {
            Die(); // 사망 함수를 실행한다.
        }
    }

    private void Die() // 적 사망을 처리한다.
    {
        Debug.Log(gameObject.name + " Dead"); // 적 사망 로그를 출력한다.
        Destroy(gameObject); // 적 오브젝트를 삭제한다.
    }
}