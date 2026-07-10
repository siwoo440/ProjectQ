using UnityEngine; // Unity 기본 기능 사용

public class RoomPortal : MonoBehaviour // 다음층 이동 포탈 클래스
{
    [Header("Interaction Settings")] // 상호작용 설정 제목
    public KeyCode interactKey = KeyCode.F; // 포탈 상호작용 키

    [Header("Reference")] // 참조 제목
    public RoomSceneController roomSceneController; // 방 씬 관리자

    public GameObject visualRoot; // 포탈 시각 오브젝트

    private bool isPlayerInside = false; // 플레이어가 포탈 범위 안에 있는지 여부

    private bool hasUsedPortal = false; // 포탈 사용 여부

    private void Start() // 시작 함수
    {
        if (roomSceneController == null) // 방 씬 관리자 연결 확인
        {
            roomSceneController = FindFirstObjectByType<RoomSceneController>(); // 방 씬 관리자 자동 검색
        }
    }

    public void Initialize(RoomSceneController sceneController) // 포탈 초기화 함수
    {
        roomSceneController = sceneController; // 방 씬 관리자 저장

        hasUsedPortal = false; // 포탈 사용 여부 초기화

        if (visualRoot != null) // 시각 오브젝트 확인
        {
            visualRoot.SetActive(true); // 포탈 표시
        }
    }

    private void Update() // 매 프레임 입력 확인 함수
    {
        if (!isPlayerInside) // 플레이어가 범위 안에 없는지 확인
        {
            return; // 입력 처리 중단
        }

        if (hasUsedPortal) // 이미 포탈을 사용했는지 확인
        {
            return; // 중복 사용 방지
        }

        if (Input.GetKeyDown(interactKey)) // 상호작용 키 입력 확인
        {
            UsePortal(); // 포탈 사용
        }
    }

    private void UsePortal() // 포탈 사용 함수
    {
        hasUsedPortal = true; // 사용 처리

        Debug.Log("Next Floor Portal Used"); // 포탈 사용 로그

        if (roomSceneController != null) // 방 씬 관리자 확인
        {
            roomSceneController.MoveToNextFloorFromPortal(); // 다음층 이동 요청
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // 포탈 범위 진입 감지 함수
    {
        PlayerStats playerStats = other.GetComponentInParent<PlayerStats>(); // 플레이어 스탯 검색

        if (playerStats == null) // 플레이어가 아닌지 확인
        {
            return; // 처리 중단
        }

        isPlayerInside = true; // 플레이어 범위 진입 처리

        Debug.Log("Press F to move to next floor."); // 임시 안내 로그
    }

    private void OnTriggerExit2D(Collider2D other) // 포탈 범위 이탈 감지 함수
    {
        PlayerStats playerStats = other.GetComponentInParent<PlayerStats>(); // 플레이어 스탯 검색

        if (playerStats == null) // 플레이어가 아닌지 확인
        {
            return; // 처리 중단
        }

        isPlayerInside = false; // 플레이어 범위 이탈 처리
    }
}