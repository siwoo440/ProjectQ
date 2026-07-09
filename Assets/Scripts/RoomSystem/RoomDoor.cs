using UnityEngine; // Unity 기본 기능 사용

public class RoomDoor : MonoBehaviour // 방 문 상호작용 클래스
{
    [Header("Door Settings")] // 문 설정 제목
    public RoomDirection doorDirection; // 문 방향

    public KeyCode interactKey = KeyCode.F; // 상호작용 키

    [Header("Object References")] // 오브젝트 참조 제목
    public GameObject visualRoot; // 문 시각 오브젝트

    public GameObject lockedBarrier; // 전투 중 막는 베리어 오브젝트

    private RoomSceneController roomSceneController; // 방 씬 관리자

    private bool isPlayerInside; // 플레이어가 문 범위 안에 있는지 여부

    private bool hasTriggeredMove = false; // 문 이동 실행 여부

    public void Initialize(RoomSceneController controller, bool isConnected) // 문 초기화 함수
    {
        roomSceneController = controller; // 방 씬 관리자 저장

        gameObject.SetActive(isConnected); // 연결된 방이 있을 때만 문 활성화

        if (visualRoot != null) // 문 시각 오브젝트 확인
        {
            visualRoot.SetActive(isConnected); // 문 표시 여부 설정
        }

        if (lockedBarrier != null) // 베리어 오브젝트 확인
        {
            lockedBarrier.SetActive(false); // 24일차에는 베리어 비활성화
        }

        hasTriggeredMove = false; // 문 이동 실행 여부 초기화
    }

    private void Update() // 매 프레임 입력 확인 함수
    {
        if (!isPlayerInside) // 플레이어가 문 범위 안에 없는지 확인
        {
            return; // 입력 처리 중단
        }

        if (Input.GetKeyDown(interactKey)) // 상호작용 키 입력 확인
        {
            if (roomSceneController != null) // 방 씬 관리자 확인
            {
                roomSceneController.TryMoveThroughDoor(doorDirection); // 문 방향으로 이동 시도
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // 플레이어 진입 감지 함수
    {
        if (hasTriggeredMove) // 이미 이동을 실행했는지 확인
        {
            return; // 중복 이동 방지
        }

        PlayerStats playerStats = other.GetComponentInParent<PlayerStats>(); // 플레이어 스탯 검색

        if (playerStats == null) // 플레이어가 아닌지 확인
        {
            return; // 처리 중단
        }

        hasTriggeredMove = true; // 이동 실행 처리

        Debug.Log("Auto Move Door : " + doorDirection); // 자동 문 이동 로그

        if (roomSceneController != null) // 방 씬 관리자 확인
        {
            roomSceneController.TryMoveThroughDoor(doorDirection); // 문 방향으로 이동 시도
        }
    }

    private void OnTriggerExit2D(Collider2D other) // 플레이어 이탈 감지 함수
    {
        PlayerStats playerStats = other.GetComponentInParent<PlayerStats>(); // 플레이어 스탯 검색

        if (playerStats == null) // 플레이어가 아닌지 확인
        {
            return; // 처리 중단
        }

        Debug.Log("Exit Door : " + doorDirection); // 문 범위 이탈 로그
    }
}