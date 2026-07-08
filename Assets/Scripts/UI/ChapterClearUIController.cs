using TMPro; // TMP 텍스트 사용
using UnityEngine; // Unity 기본 기능 사용
using UnityEngine.SceneManagement; // 씬 이동 기능 사용
using UnityEngine.UI; // UI 버튼 사용

public class ChapterClearUIController : MonoBehaviour // 챕터 클리어 UI 관리 클래스
{
    [Header("Panel Reference")] // 패널 참조 제목
    public GameObject chapterClearPanel; // 챕터 클리어 패널

    [Header("Text References")] // 텍스트 참조 제목
    public TextMeshProUGUI titleText; // 제목 텍스트
    public TextMeshProUGUI descriptionText; // 설명 텍스트
    public TextMeshProUGUI resultText; // 결과 텍스트

    [Header("Button References")] // 버튼 참조 제목
    public Button retryRunButton; // 다시 시작 버튼
    public Button backToMapButton; // 맵 복귀 버튼
    public Button mainMenuButton; // 메인 메뉴 버튼

    [Header("Scene Names")] // 씬 이름 제목
    public string mapSceneName = "MapScene"; // 맵 씬 이름
    public string mainMenuSceneName = "MainMenuScene"; // 메인 메뉴 씬 이름

    private void Awake() // 초기화 함수
    {
        Hide(); // 시작 시 패널 숨김
    }

    private void Start() // 시작 함수
    {
        ConnectButtons(); // 버튼 연결
    }

    private void ConnectButtons() // 버튼 연결 함수
    {
        if (retryRunButton != null) // 다시 시작 버튼 확인
        {
            retryRunButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            retryRunButton.onClick.AddListener(OnClickRetryRun); // 다시 시작 함수 연결
        }

        if (backToMapButton != null) // 맵 복귀 버튼 확인
        {
            backToMapButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            backToMapButton.onClick.AddListener(OnClickBackToMap); // 맵 복귀 함수 연결
        }

        if (mainMenuButton != null) // 메인 메뉴 버튼 확인
        {
            mainMenuButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            mainMenuButton.onClick.AddListener(OnClickMainMenu); // 메인 메뉴 함수 연결
        }
    }

    public void Show(int clearedBattleNumber) // 챕터 클리어 표시 함수
    {
        if (titleText != null) // 제목 텍스트 확인
        {
            titleText.text = "CHAPTER CLEAR"; // 제목 설정
        }

        if (descriptionText != null) // 설명 텍스트 확인
        {
            descriptionText.text = "You defeated the boss and cleared this chapter."; // 설명 설정
        }

        if (resultText != null) // 결과 텍스트 확인
        {
            resultText.text = "Cleared Battle : " + clearedBattleNumber; // 결과 설정
        }

        if (chapterClearPanel != null) // 패널 확인
        {
            chapterClearPanel.SetActive(true); // 패널 표시
        }

        Time.timeScale = 0f; // 게임 시간 정지
    }

    public void Hide() // 챕터 클리어 숨김 함수
    {
        if (chapterClearPanel != null) // 패널 확인
        {
            chapterClearPanel.SetActive(false); // 패널 숨김
        }
    }

    private void OnClickRetryRun() // 다시 시작 버튼 함수
    {
        Time.timeScale = 1f; // 게임 시간 복구

        if (GameFlowManager.Instance != null) // 진행 관리자 확인
        {
            GameFlowManager.Instance.StartNewRun(); // 새 런 시작
            return; // 함수 종료
        }

        SceneManager.LoadScene(mapSceneName); // 맵 씬 이동
    }

    private void OnClickBackToMap() // 맵 복귀 버튼 함수
    {
        Time.timeScale = 1f; // 게임 시간 복구
        SceneManager.LoadScene(mapSceneName); // 맵 씬 이동
    }

    private void OnClickMainMenu() // 메인 메뉴 버튼 함수
    {
        Time.timeScale = 1f; // 게임 시간 복구

        if (IsSceneInBuildSettings(mainMenuSceneName)) // 메인 메뉴 씬 등록 확인
        {
            SceneManager.LoadScene(mainMenuSceneName); // 메인 메뉴 씬 이동
            return; // 함수 종료
        }

        Debug.LogWarning("MainMenuScene is not added to Build Settings."); // 경고 로그
        SceneManager.LoadScene(mapSceneName); // 임시로 맵 씬 이동
    }

    private bool IsSceneInBuildSettings(string sceneName) // 빌드 설정 씬 확인 함수
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings; // 빌드 설정 씬 수

        for (int i = 0; i < sceneCount; i++) // 씬 수만큼 반복
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); // 씬 경로 가져오기
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath); // 씬 이름 추출

            if (sceneNameFromPath == sceneName) // 씬 이름 비교
            {
                return true; // 등록됨 반환
            }
        }

        return false; // 등록 안 됨 반환
    }
}