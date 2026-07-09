using UnityEngine; // Unity 기본 기능 사용

public class RoomDoor : MonoBehaviour // 방 문 상호작용 클래스
{
    [Header("Door Settings")] // 문 설정 제목
    public RoomDirection doorDirection; // 문 방향

    [Header("Door Visual Reference")] // 문 시각 참조 제목
    public GameObject visualRoot; // 문 시각 오브젝트

    public SpriteRenderer doorVisualRenderer; // 문 SpriteRenderer

    [Header("Door Color Settings")] // 문 색상 설정 제목
    public Color unlockedColor = Color.green; // 열린 문 색상

    public Color lockedColor = Color.red; // 잠긴 문 색상

    [Header("Door Sprite Settings")] // 문 이미지 설정 제목
    public Sprite unlockedSprite; // 열린 문 이미지

    public Sprite lockedSprite; // 잠긴 문 이미지

    [Header("Door Lock Reference")] // 문 잠금 참조 제목
    public Collider2D lockedCollider; // 잠겼을 때 켜지는 충돌 Collider

    private RoomSceneController roomSceneController; // 방 씬 관리자

    private bool isConnected = false; // 연결된 방이 있는지 여부

    private bool isLocked = false; // 문 잠금 여부

    private bool hasTriggeredMove = false; // 문 이동 실행 여부

    public void Initialize(RoomSceneController controller, bool connected) // 문 초기화 함수
    {
        roomSceneController = controller; // 방 씬 관리자 저장

        isConnected = connected; // 연결 여부 저장

        hasTriggeredMove = false; // 문 이동 실행 여부 초기화

        gameObject.SetActive(isConnected); // 연결된 방이 있을 때만 문 활성화

        if (visualRoot != null) // 문 시각 오브젝트 확인
        {
            visualRoot.SetActive(isConnected); // 문 표시 여부 설정
        }

        if (doorVisualRenderer == null && visualRoot != null) // 문 Renderer 자동 검색
        {
            doorVisualRenderer = visualRoot.GetComponentInChildren<SpriteRenderer>(); // 문 SpriteRenderer 가져오기
        }

        SetLocked(false); // 초기 문 상태를 열림으로 설정
    }

    public void SetLocked(bool locked) // 문 잠금 상태 설정 함수
    {
        isLocked = locked; // 잠금 상태 저장

        hasTriggeredMove = false; // 잠금 상태 변경 시 이동 실행 여부 초기화

        if (lockedCollider != null) // 잠금 Collider 확인
        {
            lockedCollider.enabled = locked; // 잠겼을 때만 충돌 켜기
        }

        if (doorVisualRenderer != null) // 문 Renderer 확인
        {
            doorVisualRenderer.color = locked ? lockedColor : unlockedColor; // 잠금 상태에 따라 색상 변경

            if (locked && lockedSprite != null) // 잠김 이미지 확인
            {
                doorVisualRenderer.sprite = lockedSprite; // 잠긴 문 이미지 적용
            }
            else if (!locked && unlockedSprite != null) // 열림 이미지 확인
            {
                doorVisualRenderer.sprite = unlockedSprite; // 열린 문 이미지 적용
            }
        }

        Debug.Log("Door Lock State : " + doorDirection + " / " + locked); // 문 잠금 상태 로그
    }

    private void OnTriggerEnter2D(Collider2D other) // 플레이어 진입 감지 함수
    {
        if (!isConnected) // 연결된 방이 없는 문인지 확인
        {
            return; // 이동 처리 중단
        }

        if (isLocked) // 문이 잠겼는지 확인
        {
            return; // 잠긴 문은 이동 처리 안 함
        }

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
}