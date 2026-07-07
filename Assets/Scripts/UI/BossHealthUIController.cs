using TMPro; // TMP 텍스트 사용
using UnityEngine; // Unity 기본 기능 사용
using UnityEngine.UI; // UI Slider 사용

public class BossHealthUIController : MonoBehaviour // 보스 체력 UI 관리 클래스
{
    [Header("Panel Reference")] // 패널 참조 제목
    public GameObject bossHealthPanel; // 보스 체력 패널

    [Header("UI References")] // UI 참조 제목
    public Slider bossHealthSlider; // 보스 체력 슬라이더
    public TextMeshProUGUI bossNameText; // 보스 이름 텍스트
    public TextMeshProUGUI bossHealthText; // 보스 체력 수치 텍스트

    private EnemyHealth targetBossHealth; // 추적할 보스 체력

    private void Awake() // 초기화 함수
    {
        Hide(); // 시작 시 보스 체력 UI 숨김
    }

    private void Update() // 매 프레임 갱신 함수
    {
        if (targetBossHealth == null) // 추적 대상 확인
        {
            Hide(); // 대상이 없으면 UI 숨김
            return; // 갱신 중단
        }

        UpdateHealthBar(); // 체력바 갱신
    }

    public void Show(EnemyHealth bossHealth, string bossName) // 보스 체력 UI 표시 함수
    {
        if (bossHealth == null) // 보스 체력 확인
        {
            Debug.LogError("Boss Health is null."); // 오류 로그
            return; // 표시 중단
        }

        targetBossHealth = bossHealth; // 보스 체력 저장

        if (bossNameText != null) // 이름 텍스트 확인
        {
            bossNameText.text = bossName; // 보스 이름 표시
        }

        if (bossHealthPanel != null) // 패널 확인
        {
            bossHealthPanel.SetActive(true); // 패널 표시
        }

        UpdateHealthBar(); // 체력바 즉시 갱신
    }

    public void Hide() // 보스 체력 UI 숨김 함수
    {
        targetBossHealth = null; // 추적 대상 비우기

        if (bossHealthPanel != null) // 패널 확인
        {
            bossHealthPanel.SetActive(false); // 패널 숨김
        }
    }

    private void UpdateHealthBar() // 체력바 갱신 함수
    {
        if (targetBossHealth == null) // 보스 체력 확인
        {
            return; // 갱신 중단
        }

        int maxHealth = Mathf.Max(1, targetBossHealth.maxHealth); // 최대 체력 보정
        int currentHealth = Mathf.Clamp(targetBossHealth.currentHealth, 0, maxHealth); // 현재 체력 보정
        float healthRate = (float)currentHealth / maxHealth; // 체력 비율 계산

        if (bossHealthSlider != null) // 슬라이더 확인
        {
            bossHealthSlider.value = healthRate; // 슬라이더 값 적용
        }

        if (bossHealthText != null) // 체력 텍스트 확인
        {
            bossHealthText.text = currentHealth + " / " + maxHealth; // 체력 수치 표시
        }
    }
}