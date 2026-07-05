using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 50; // 적 최대 체력을 저장한다.
    public int currentHealth; // 적 현재 체력을 저장한다.

    private BattleManager battleManager; // 전투 관리자를 저장한다.
    private bool isDead = false; // 적이 이미 죽었는지 저장한다.

    private void Awake() // 적이 생성될 때 체력을 초기화한다.
    {
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 설정한다.
    }

    public void SetBattleManager(BattleManager manager) // BattleManager를 연결한다.
    {
        battleManager = manager; // 전달받은 BattleManager를 저장한다.
    }

    public void TakeDamage(int damage) // 적이 피해를 받는다.
    {
        if (isDead) return; // 이미 죽은 적이면 피해 처리를 하지 않는다.

        currentHealth -= damage; // 현재 체력을 피해량만큼 감소시킨다.
        currentHealth = Mathf.Max(currentHealth, 0); // 체력이 0 아래로 내려가지 않게 한다.

        Debug.Log(gameObject.name + " damaged : " + damage); // 피해량을 로그로 출력한다.
        Debug.Log(gameObject.name + " HP : " + currentHealth); // 현재 체력을 로그로 출력한다.

        if (currentHealth <= 0) // 체력이 0 이하인지 확인한다.
        {
            Die(); // 사망 처리를 실행한다.
        }
    }

    private void Die() // 적 사망을 처리한다.
    {
        if (isDead) return; // 이미 사망 처리된 적이면 다시 실행하지 않는다.

        isDead = true; // 사망 처리 상태로 변경한다.

        Debug.Log(gameObject.name + " Dead"); // 적 사망 로그를 출력한다.

        if (battleManager != null) // BattleManager가 연결되어 있는지 확인한다.
        {
            battleManager.OnEnemyDead(this); // BattleManager에게 적이 죽었다고 알린다.
        }

        Destroy(gameObject); // 적 오브젝트를 삭제한다.
    }
}