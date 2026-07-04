using UnityEngine; // 유니티 기능

public class PlayerStats : MonoBehaviour // 플레이어 수치 관리
{
    [Header("Health")] // 체력 영역
    [SerializeField] private int maxHealth = 100; // 최대 체력
    [SerializeField] private int currentHealth = 100; // 현재 체력

    [Header("Mana")] // 마나 영역
    [SerializeField] private float maxMana = 5f; // 최대 마나
    [SerializeField] private float currentMana = 5f; // 현재 마나
    [SerializeField] private float manaRegenPerSecond = 1f; // 초당 마나 회복량

    [Header("Shield")] // 보호막 영역
    [SerializeField] private int shield = 0; // 현재 보호막

    public int MaxHealth => maxHealth; // 최대 체력 읽기
    public int CurrentHealth => currentHealth; // 현재 체력 읽기
    public float MaxMana => maxMana; // 최대 마나 읽기
    public float CurrentMana => currentMana; // 현재 마나 읽기
    public int Shield => shield; // 보호막 읽기

    private void Update() // 매 프레임 갱신
    {
        RegenerateMana(); // 마나 회복
    }

    private void RegenerateMana() // 마나 자동 회복
    {
        currentMana += manaRegenPerSecond * Time.deltaTime; // 마나 증가

        currentMana = Mathf.Clamp(currentMana, 0f, maxMana); // 마나 범위 제한
    }

    public bool UseMana(float amount) // 마나 사용
    {
        if (currentMana < amount) // 마나 부족 확인
        {
            return false; // 사용 실패
        }

        currentMana -= amount; // 마나 감소

        return true; // 사용 성공
    }

    public void TakeDamage(int damage) // 피해 처리
    {
        if (shield > 0) // 보호막 확인
        {
            shield--; // 보호막 감소

            return; // 체력 피해 차단
        }

        currentHealth -= damage; // 체력 감소

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력 범위 제한

        if (currentHealth <= 0) // 사망 확인
        {
            Die(); // 사망 처리
        }
    }

    public void AddShield(int amount) // 보호막 추가
    {
        shield += amount; // 보호막 증가
    }

    private void Die() // 사망 처리
    {
        Debug.Log("플레이어 사망"); // 사망 로그
    }
}