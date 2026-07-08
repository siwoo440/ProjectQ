using UnityEngine;
using UnityEngine.UI; // UI 버튼 사용
using UnityEngine.SceneManagement; // 씬 이동 사용

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel; // 게임 오버 패널 오브젝트를 저장한다.

    [Header("Target References")]
    public BattleProgressManager battleProgressManager; // 전투 진행 관리자를 저장한다.

    [Header("Button References")] // 버튼 참조 제목
    public Button retryButton; // 다시 시작 버튼
    public Button mainMenuButton; // 메인 메뉴 버튼
  
    [Header("Scene Names")] // 씬 이름 제목
    public string mainMenuSceneName = "MainMenuScene"; // 메인 메뉴 씬 이름

    private void Start() // 시작 함수
    {
        if (retryButton != null) // 다시 시작 버튼 확인
        {
            retryButton.onClick.RemoveAllListeners(); // 기존 클릭 연결 제거
            retryButton.onClick.AddListener(OnClickRetry); // 다시 시작 클릭 연결
        }

        if (mainMenuButton != null) // 메인 메뉴 버튼 확인
        {
            mainMenuButton.onClick.RemoveAllListeners(); // 기존 클릭 연결 제거
            mainMenuButton.onClick.AddListener(OnClickMainMenu); // 메인 메뉴 클릭 연결
        }
    }

    public void Show() // 게임 오버 UI를 표시한다.
    {
        if (gameOverPanel != null) // 게임 오버 패널이 연결되어 있는지 확인한다.
        {
            gameOverPanel.SetActive(true); // 게임 오버 패널을 켠다.
        }

        Debug.Log("Game Over UI Opened"); // 게임 오버 UI 표시 로그를 출력한다.
    }

    public void Hide() // 게임 오버 UI를 숨긴다.
    {
        if (gameOverPanel != null) // 게임 오버 패널이 연결되어 있는지 확인한다.
        {
            gameOverPanel.SetActive(false); // 게임 오버 패널을 끈다.
        }
    }

    private void OnClickRetry() // 다시 시작 버튼 함수
    {
        Time.timeScale = 1f; // 시간 복구

        if (GameFlowManager.Instance != null) // 진행 관리자 확인
        {
            GameFlowManager.Instance.StartNewRun(); // 새 런 시작
            return; // 함수 종료
        }

        SceneManager.LoadScene("MapScene"); // 맵 씬 이동
    }

    private void OnClickMainMenu() // 메인 메뉴 버튼 함수
    {
        Time.timeScale = 1f; // 시간 복구
        SceneManager.LoadScene(mainMenuSceneName); // 메인 메뉴 씬 이동
    }
}