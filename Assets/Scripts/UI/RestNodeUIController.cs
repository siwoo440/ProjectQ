using TMPro; // TMP 텍스트 사용
using UnityEngine; // Unity 기본 기능 사용
using UnityEngine.UI; // UI 버튼 사용

public class RestNodeUIController : MonoBehaviour // 휴식 노드 UI 관리 클래스
{
    [Header("Panel Reference")] // 패널 참조 제목
    public GameObject restPanel; // 휴식 UI 패널

    [Header("Text References")] // 텍스트 참조 제목
    public TextMeshProUGUI titleText; // 제목 텍스트
    public TextMeshProUGUI descriptionText; // 설명 텍스트
    public TextMeshProUGUI healText; // 회복 버튼 텍스트
    public TextMeshProUGUI shieldText; // 보호막 버튼 텍스트
    public TextMeshProUGUI upgradeCardText; // 카드 강화 버튼 텍스트

    [Header("Button References")] // 버튼 참조 제목
    public Button healButton; // 체력 회복 버튼
    public Button shieldButton; // 보호막 획득 버튼
    public Button upgradeCardButton; // 카드 강화 버튼

    private void Awake() // 오브젝트 초기화 함수
    {
        Hide(); // 시작 시 휴식 UI 숨김
    }

    private void Start() // 시작 처리 함수
    {
        RegisterToGameFlowManager(); // 진행 관리자에 등록
        ConnectButtons(); // 버튼 클릭 연결
        UpdateTexts(); // 텍스트 갱신
    }

    private void OnDestroy() // 오브젝트 제거 처리 함수
    {
        UnregisterFromGameFlowManager(); // 진행 관리자 등록 해제
    }

    private void RegisterToGameFlowManager() // 진행 관리자 등록 함수
    {
        if (GameFlowManager.Instance == null) // 진행 관리자가 없는지 확인
        {
            return; // 등록 중단
        }

        GameFlowManager.Instance.RegisterRestNodeUI(this); // 휴식 UI 등록
    }

    private void UnregisterFromGameFlowManager() // 진행 관리자 등록 해제 함수
    {
        if (GameFlowManager.Instance == null) // 진행 관리자가 없는지 확인
        {
            return; // 해제 중단
        }

        GameFlowManager.Instance.UnregisterRestNodeUI(this); // 휴식 UI 해제
    }

    private void ConnectButtons() // 버튼 연결 함수
    {
        if (healButton != null) // 회복 버튼 확인
        {
            healButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            healButton.onClick.AddListener(OnClickHeal); // 회복 클릭 연결
        }

        if (shieldButton != null) // 보호막 버튼 확인
        {
            shieldButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            shieldButton.onClick.AddListener(OnClickShield); // 보호막 클릭 연결
        }

        if (upgradeCardButton != null) // 카드 강화 버튼 확인
        {
            upgradeCardButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            upgradeCardButton.onClick.AddListener(OnClickUpgradeCard); // 카드 강화 클릭 연결
        }
    }

    private void UpdateTexts() // 텍스트 갱신 함수
    {
        if (titleText != null) // 제목 텍스트 확인
        {
            titleText.text = "Rest Area"; // 제목 문구
        }

        if (descriptionText != null) // 설명 텍스트 확인
        {
            descriptionText.text = "Choose one rest effect. The effect will be applied at the start of the next battle."; // 설명 문구
        }

        if (healText != null) // 회복 텍스트 확인
        {
            healText.text = "Heal"; // 회복 버튼 문구
        }

        if (shieldText != null) // 보호막 텍스트 확인
        {
            shieldText.text = "Gain Shield"; // 보호막 버튼 문구
        }

        if (upgradeCardText != null) // 카드 강화 텍스트 확인
        {
            upgradeCardText.text = "Upgrade Card"; // 카드 강화 버튼 문구
        }
    }

    public void Show() // 휴식 UI 표시 함수
    {
        if (restPanel == null) // 패널 연결 확인
        {
            Debug.LogError("Rest Panel is not assigned."); // 오류 로그
            return; // 표시 중단
        }

        restPanel.SetActive(true); // 패널 표시
    }

    public void Hide() // 휴식 UI 숨김 함수
    {
        if (restPanel == null) // 패널 연결 확인
        {
            return; // 숨김 중단
        }

        restPanel.SetActive(false); // 패널 숨김
    }

    private void OnClickHeal() // 체력 회복 선택 함수
    {
        if (GameFlowManager.Instance == null) // 진행 관리자 확인
        {
            Debug.LogError("GameFlowManager is missing."); // 오류 로그
            return; // 선택 중단
        }

        Hide(); // 휴식 UI 숨김
        GameFlowManager.Instance.SelectRestHealChoice(); // 체력 회복 선택 전달
    }

    private void OnClickShield() // 보호막 선택 함수
    {
        if (GameFlowManager.Instance == null) // 진행 관리자 확인
        {
            Debug.LogError("GameFlowManager is missing."); // 오류 로그
            return; // 선택 중단
        }

        Hide(); // 휴식 UI 숨김
        GameFlowManager.Instance.SelectRestShieldChoice(); // 보호막 선택 전달
    }

    private void OnClickUpgradeCard() // 카드 강화 선택 함수
    {
        if (GameFlowManager.Instance == null) // 진행 관리자 확인
        {
            Debug.LogError("GameFlowManager is missing."); // 오류 로그
            return; // 선택 중단
        }

        Hide(); // 휴식 UI 숨김
        GameFlowManager.Instance.SelectRestCardUpgradeChoice(); // 카드 강화 선택 전달
    }
}