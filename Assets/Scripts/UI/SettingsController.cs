using TMPro; // TextMeshPro 텍스트 사용
using UnityEngine; // Unity 기본 기능 사용
using UnityEngine.SceneManagement; // 씬 이동 기능 사용
using UnityEngine.UI; // UI Slider와 Button 사용

public class SettingsController : MonoBehaviour // 설정 화면 관리 클래스
{
    [Header("Slider References")] // 슬라이더 참조 제목
    public Slider bgmVolumeSlider; // BGM 볼륨 슬라이더

    public Slider sfxVolumeSlider; // SFX 볼륨 슬라이더

    [Header("Text References")] // 텍스트 참조 제목
    public TextMeshProUGUI bgmValueText; // BGM 볼륨 값 텍스트

    public TextMeshProUGUI sfxValueText; // SFX 볼륨 값 텍스트

    [Header("Button References")] // 버튼 참조 제목
    public Button backButton; // 뒤로가기 버튼

    [Header("Scene Names")] // 씬 이름 제목
    public string mainMenuSceneName = "MainMenuScene"; // 메인 메뉴 씬 이름

    private const string BgmVolumeKey = "BGM_VOLUME"; // BGM 볼륨 저장 키

    private const string SfxVolumeKey = "SFX_VOLUME"; // SFX 볼륨 저장 키

    private const float DefaultVolume = 0.8f; // 기본 볼륨 값

    private void Start() // 시작 함수
    {
        LoadSavedValues(); // 저장된 설정값 불러오기

        ConnectUIEvents(); // UI 이벤트 연결

        UpdateValueTexts(); // 값 텍스트 갱신
    }

    private void LoadSavedValues() // 저장된 설정값 불러오기 함수
    {
        float bgmVolume = PlayerPrefs.GetFloat(BgmVolumeKey, DefaultVolume); // 저장된 BGM 볼륨 불러오기

        float sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, DefaultVolume); // 저장된 SFX 볼륨 불러오기

        if (bgmVolumeSlider != null) // BGM 슬라이더 연결 확인
        {
            bgmVolumeSlider.value = bgmVolume; // BGM 슬라이더 값 적용
        }

        if (sfxVolumeSlider != null) // SFX 슬라이더 연결 확인
        {
            sfxVolumeSlider.value = sfxVolume; // SFX 슬라이더 값 적용
        }
    }

    private void ConnectUIEvents() // UI 이벤트 연결 함수
    {
        if (bgmVolumeSlider != null) // BGM 슬라이더 연결 확인
        {
            bgmVolumeSlider.onValueChanged.RemoveAllListeners(); // 기존 이벤트 제거
            bgmVolumeSlider.onValueChanged.AddListener(OnChangeBgmVolume); // BGM 값 변경 이벤트 연결
        }

        if (sfxVolumeSlider != null) // SFX 슬라이더 연결 확인
        {
            sfxVolumeSlider.onValueChanged.RemoveAllListeners(); // 기존 이벤트 제거
            sfxVolumeSlider.onValueChanged.AddListener(OnChangeSfxVolume); // SFX 값 변경 이벤트 연결
        }

        if (backButton != null) // 뒤로가기 버튼 연결 확인
        {
            backButton.onClick.RemoveAllListeners(); // 기존 클릭 이벤트 제거
            backButton.onClick.AddListener(OnClickBack); // 뒤로가기 클릭 이벤트 연결
        }
    }

    private void OnChangeBgmVolume(float value) // BGM 볼륨 변경 함수
    {
        PlayerPrefs.SetFloat(BgmVolumeKey, value); // BGM 볼륨 저장

        PlayerPrefs.Save(); // PlayerPrefs 즉시 저장

        UpdateValueTexts(); // 값 텍스트 갱신
    }

    private void OnChangeSfxVolume(float value) // SFX 볼륨 변경 함수
    {
        PlayerPrefs.SetFloat(SfxVolumeKey, value); // SFX 볼륨 저장

        PlayerPrefs.Save(); // PlayerPrefs 즉시 저장

        UpdateValueTexts(); // 값 텍스트 갱신
    }

    private void UpdateValueTexts() // 값 텍스트 갱신 함수
    {
        if (bgmValueText != null && bgmVolumeSlider != null) // BGM 텍스트와 슬라이더 확인
        {
            int bgmPercent = Mathf.RoundToInt(bgmVolumeSlider.value * 100f); // BGM 퍼센트 계산
            bgmValueText.text = bgmPercent + "%"; // BGM 퍼센트 표시
        }

        if (sfxValueText != null && sfxVolumeSlider != null) // SFX 텍스트와 슬라이더 확인
        {
            int sfxPercent = Mathf.RoundToInt(sfxVolumeSlider.value * 100f); // SFX 퍼센트 계산
            sfxValueText.text = sfxPercent + "%"; // SFX 퍼센트 표시
        }
    }

    private void OnClickBack() // 뒤로가기 버튼 함수
    {
        Time.timeScale = 1f; // 시간 흐름 복구

        SceneManager.LoadScene(mainMenuSceneName); // 메인 메뉴 씬으로 이동
    }
}