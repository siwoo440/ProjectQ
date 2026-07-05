using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100; // 최대 체력을 저장한다.
    public int currentHealth; // 현재 체력을 저장한다.

    [Header("Mana")]
    public float maxMana = 5f; // 최대 마나를 저장한다.
    public float currentMana; // 현재 마나를 저장한다.
    public float manaRegenPerSecond = 1.5f; // 초당 마나 회복량을 저장한다.

    [Header("Shield")]
    public int shield = 1; // 현재 보호막 수치를 저장한다.

    private void Awake() // 게임 시작 전에 기본 체력과 마나를 초기화한다.
    {
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 설정한다.
        currentMana = maxMana; // 현재 마나를 최대 마나로 설정한다.
    }

    private void Update() // 매 프레임 마나 회복을 처리한다.
    {
        RegenerateMana(); // 마나 회복 함수를 실행한다.
    }

    private void RegenerateMana() // 시간이 지날수록 마나를 회복한다.
    {
        if (currentMana >= maxMana) return; // 현재 마나가 최대 마나 이상이면 회복하지 않는다.

        currentMana += manaRegenPerSecond * Time.deltaTime; // 시간에 따라 마나를 회복한다.
        currentMana = Mathf.Min(currentMana, maxMana); // 마나가 최대치를 넘지 않도록 제한한다.
    }

    public bool UseMana(float amount) // 마나를 사용할 수 있는지 확인하고 사용한다.
    {
        if (currentMana < amount) // 현재 마나가 필요한 마나보다 적은지 확인한다.
        {
            Debug.Log("MP 부족"); // 마나 부족 로그를 출력한다.
            return false; // 마나 사용 실패를 반환한다.
        }

        currentMana -= amount; // 필요한 만큼 마나를 감소시킨다.
        Debug.Log("MP 사용 : " + amount); // 사용한 마나를 로그로 출력한다.
        return true; // 마나 사용 성공을 반환한다.
    }

    public void TakeDamage(int damage) // 플레이어가 피해를 받는다.
    {
        if (shield > 0) // 보호막이 있는지 확인한다.
        {
            shield -= 1; // 보호막을 1 감소시킨다.
            Debug.Log("보호막으로 피해 방어"); // 보호막 방어 로그를 출력한다.
            return; // 체력 피해를 받지 않고 종료한다.
        }

        currentHealth -= damage; // 현재 체력을 피해량만큼 감소시킨다.
        currentHealth = Mathf.Max(currentHealth, 0); // 체력이 0 아래로 내려가지 않게 한다.
        Debug.Log("Player HP : " + currentHealth); // 현재 체력을 로그로 출력한다.

        if (currentHealth <= 0) // 체력이 0 이하인지 확인한다.
        {
            Die(); // 사망 함수를 실행한다.
        }
    }

    public void AddShield(int amount) // 보호막을 추가한다.
    {
        shield += amount; // 보호막을 증가시킨다.
    }
    public void IncreaseMaxMana(float amount) // 최대 마나를 증가시킨다.
    {
        maxMana += amount; // 최대 마나를 증가시킨다.
        currentMana += amount; // 현재 마나도 같이 증가시킨다.
        currentMana = Mathf.Min(currentMana, maxMana); // 현재 마나가 최대 마나를 넘지 않게 한다.

        Debug.Log("최대 MP 증가 : " + maxMana); // 증가된 최대 마나를 로그로 출력한다.
    }
    private void Die() // 플레이어 사망을 처리한다.
    {
        Debug.Log("Player Dead"); // 사망 로그를 출력한다.
    }
}