using UnityEngine; // Unity 기본 기능 사용
using UnityEngine.SceneManagement; // 씬 이동 기능 사용
using UnityEngine.UI; // UI 버튼 사용

public class MainMenuController : MonoBehaviour // 메인 메뉴 관리 클래스
{
    [Header("Button References")] // 버튼 참조 제목
    public Button startGameButton; // 게임 시작 버튼
    public Button continueButton; // 이어하기 버튼
    public Button settingsButton; // 설정 버튼
    public Button quitButton; // 종료 버튼

    [Header("Scene Names")] // 씬 이름 제목
    public string mapSceneName = "MapScene"; // 맵 씬 이름

    private void Start() // 시작 함수
    {
        ConnectButtons(); // 버튼 연결
        PrepareLockedButtons(); // 미구현 버튼 처리
    }

    private void ConnectButtons() // 버튼 연결 함수
    {
        if (startGameButton != null) // 게임 시작 버튼 확인
        {
            startGameButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            startGameButton.onClick.AddListener(OnClickStartGame); // 시작 버튼 연결
        }

        if (continueButton != null) // 이어하기 버튼 확인
        {
            continueButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            continueButton.onClick.AddListener(OnClickContinue); // 이어하기 버튼 연결
        }

        if (settingsButton != null) // 설정 버튼 확인
        {
            settingsButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            settingsButton.onClick.AddListener(OnClickSettings); // 설정 버튼 연결
        }

        if (quitButton != null) // 종료 버튼 확인
        {
            quitButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            quitButton.onClick.AddListener(OnClickQuit); // 종료 버튼 연결
        }
    }

    private void PrepareLockedButtons() // 미구현 버튼 준비 함수
    {
        if (continueButton != null) // 이어하기 버튼 확인
        {
            continueButton.interactable = false; // 이어하기 비활성화
        }
    }

    private void OnClickStartGame() // 게임 시작 버튼 함수
    {
        Time.timeScale = 1f; // 시간 흐름 복구

        if (GameFlowManager.Instance != null) // 진행 관리자 확인
        {
            GameFlowManager.Instance.StartNewRun(); // 새 런 시작
            return; // 함수 종료
        }

        Debug.LogWarning("GameFlowManager is missing. Loading MapScene directly."); // 경고 로그
        SceneManager.LoadScene(mapSceneName); // 맵 씬 직접 이동
    }

    private void OnClickContinue() // 이어하기 버튼 함수
    {
        Debug.Log("Continue is not implemented yet."); // 이어하기 미구현 로그
    }

    private void OnClickSettings() // 설정 버튼 함수
    {
        Debug.Log("Settings is not implemented yet."); // 설정 미구현 로그
    }

    private void OnClickQuit() // 종료 버튼 함수
    {
        Debug.Log("Quit Game."); // 종료 로그

        Application.Quit(); // 빌드 환경 게임 종료
    }
}