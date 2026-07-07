using TMPro; // TMP 텍스트 사용
using UnityEngine; // Unity 기본 기능 사용
using UnityEngine.UI; // Unity UI 사용

public class EventRoomUIController : MonoBehaviour // 이벤트 방 UI 관리 클래스
{
    [Header("Panel Reference")] // 패널 참조 제목
    public GameObject eventRoomPanel; // 이벤트 방 패널
    public GameObject mapPanel; // 맵 패널

    [Header("Artwork Reference")] // 이미지 참조 제목
    public Image artworkImage; // 이벤트 이미지
    public Sprite defaultArtworkSprite; // 기본 이벤트 이미지

    [Header("Text References")] // 텍스트 참조 제목
    public TextMeshProUGUI titleText; // 이벤트 제목 텍스트
    public TextMeshProUGUI descriptionText; // 이벤트 설명 텍스트
    public TextMeshProUGUI choiceAText; // 선택지 A 텍스트
    public TextMeshProUGUI choiceBText; // 선택지 B 텍스트
    public TextMeshProUGUI choiceCText; // 선택지 C 텍스트

    [Header("Button References")] // 버튼 참조 제목
    public Button choiceAButton; // 선택지 A 버튼
    public Button choiceBButton; // 선택지 B 버튼
    public Button choiceCButton; // 선택지 C 버튼

    private void Awake() // 초기화 함수
    {
        Hide(); // 시작 시 숨김
    }

    private void Start() // 시작 함수
    {
        RegisterToGameFlowManager(); // 진행 관리자 등록
        ConnectButtons(); // 버튼 연결
    }

    private void OnDestroy() // 제거 함수
    {
        UnregisterFromGameFlowManager(); // 진행 관리자 해제
    }

    private void RegisterToGameFlowManager() // 진행 관리자 등록 함수
    {
        if (GameFlowManager.Instance == null) // 진행 관리자 확인
        {
            return; // 등록 중단
        }

        GameFlowManager.Instance.RegisterEventRoomUI(this); // 이벤트 방 UI 등록
    }

    private void UnregisterFromGameFlowManager() // 진행 관리자 해제 함수
    {
        if (GameFlowManager.Instance == null) // 진행 관리자 확인
        {
            return; // 해제 중단
        }

        GameFlowManager.Instance.UnregisterEventRoomUI(this); // 이벤트 방 UI 해제
    }

    private void ConnectButtons() // 버튼 연결 함수
    {
        if (choiceAButton != null) // A 버튼 확인
        {
            choiceAButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            choiceAButton.onClick.AddListener(OnClickChoiceA); // A 선택 연결
        }

        if (choiceBButton != null) // B 버튼 확인
        {
            choiceBButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            choiceBButton.onClick.AddListener(OnClickChoiceB); // B 선택 연결
        }

        if (choiceCButton != null) // C 버튼 확인
        {
            choiceCButton.onClick.RemoveAllListeners(); // 기존 연결 제거
            choiceCButton.onClick.AddListener(OnClickChoiceC); // C 선택 연결
        }
    }

    public void ShowSampleEvent() // 샘플 이벤트 표시 함수
    {
        SetSampleEventTexts(); // 샘플 텍스트 설정
        SetSampleEventArtwork(); // 샘플 이미지 설정
        Show(); // 이벤트 방 표시
    }

    private void SetSampleEventTexts() // 샘플 텍스트 설정 함수
    {
        if (titleText != null) // 제목 텍스트 확인
        {
            titleText.text = "Mysterious Shrine"; // 제목 설정
        }

        if (descriptionText != null) // 설명 텍스트 확인
        {
            descriptionText.text = "An old shrine stands quietly on the path.\nA faint energy can be felt from within.\nChoose how to approach it."; // 설명 설정
        }

        if (choiceAText != null) // A 텍스트 확인
        {
            choiceAText.text = "Offer a prayer  [Heal]"; // A 문구 설정
        }

        if (choiceBText != null) // B 텍스트 확인
        {
            choiceBText.text = "Take the blessing  [Gain Shield]"; // B 문구 설정
        }

        if (choiceCText != null) // C 텍스트 확인
        {
            choiceCText.text = "Touch the strange card  [Upgrade Card]"; // C 문구 설정
        }
    }

    private void SetSampleEventArtwork() // 샘플 이미지 설정 함수
    {
        if (artworkImage == null) // 이미지 컴포넌트 확인
        {
            return; // 이미지 설정 중단
        }

        if (defaultArtworkSprite == null) // 기본 이미지 확인
        {
            return; // 이미지 설정 중단
        }

        artworkImage.sprite = defaultArtworkSprite; // 기본 이미지 적용
    }

    public void Show() // 표시 함수
    {
        if (eventRoomPanel == null) // 이벤트 패널 확인
        {
            Debug.LogError("Event Room Panel is not assigned."); // 오류 로그
            return; // 표시 중단
        }

        if (mapPanel != null) // 맵 패널 확인
        {
            mapPanel.SetActive(false); // 맵 패널 숨김
        }

        eventRoomPanel.transform.SetAsLastSibling(); // 이벤트 패널 최상단 이동
        eventRoomPanel.SetActive(true); // 이벤트 패널 표시
    }

    public void Hide() // 숨김 함수
    {
        if (eventRoomPanel != null) // 이벤트 패널 확인
        {
            eventRoomPanel.SetActive(false); // 이벤트 패널 숨김
        }

        if (mapPanel != null) // 맵 패널 확인
        {
            mapPanel.SetActive(true); // 맵 패널 표시
        }
    }

    private void OnClickChoiceA() // A 선택 함수
    {
        if (GameFlowManager.Instance == null) // 진행 관리자 확인
        {
            Debug.LogError("GameFlowManager is missing."); // 오류 로그
            return; // 선택 중단
        }

        Hide(); // 이벤트 방 숨김
        GameFlowManager.Instance.SelectEventPrayerChoice(); // 기도 선택 전달
    }

    private void OnClickChoiceB() // B 선택 함수
    {
        if (GameFlowManager.Instance == null) // 진행 관리자 확인
        {
            Debug.LogError("GameFlowManager is missing."); // 오류 로그
            return; // 선택 중단
        }

        Hide(); // 이벤트 방 숨김
        GameFlowManager.Instance.SelectEventBlessingChoice(); // 축복 선택 전달
    }

    private void OnClickChoiceC() // C 선택 함수
    {
        if (GameFlowManager.Instance == null) // 진행 관리자 확인
        {
            Debug.LogError("GameFlowManager is missing."); // 오류 로그
            return; // 선택 중단
        }

        Hide(); // 이벤트 방 숨김
        GameFlowManager.Instance.SelectEventStrangeCardChoice(); // 카드 선택 전달
    }
}