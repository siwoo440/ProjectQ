using System.Collections; // 코루틴 사용
using UnityEngine; // Unity 기본 기능 사용

public class RoomSceneController : MonoBehaviour // 실제 방 프리팹 생성과 이동을 관리하는 클래스
{
    [Header("References")] // 참조 제목
    public RoomMapManager roomMapManager; // 랜덤 방 맵 관리자

    public GameObject defaultRoomPrefab; // 기본 테스트 방 프리팹

    public Transform roomParent; // 방 프리팹 부모

    public Transform playerTransform; // 플레이어 Transform

    [Header("Spawn Settings")] // 생성 설정 제목
    public Vector3 roomSpawnPosition = Vector3.zero; // 방 생성 위치

    public bool loadRoomOnStart = true; // 시작 시 방 로드 여부

    public bool disableKeyboardTestMovementOnStart = true; // 기존 방향키 테스트 이동 비활성화 여부

    private GameObject currentRoomObject; // 현재 생성된 방 오브젝트

    private RoomController currentRoomController; // 현재 방 컨트롤러

    private bool isChangingRoom = false; // 방 이동 중인지 확인하는 변수

    private IEnumerator Start() // 시작 코루틴
    {
        yield return null; // RoomMapManager의 Start가 먼저 실행될 시간을 줌

        FindReferencesIfNeeded(); // 필요한 참조 자동 검색

        if (roomMapManager != null && disableKeyboardTestMovementOnStart) // 맵 매니저와 비활성화 여부 확인
        {
            roomMapManager.enableKeyboardTestMovement = false; // 방향키 테스트 이동 비활성화
        }

        if (loadRoomOnStart) // 시작 시 방 로드 여부 확인
        {
            LoadCurrentRoom(RoomDirection.Down, true); // 시작 방 로드
        }
    }

    private void FindReferencesIfNeeded() // 참조 자동 검색 함수
    {
        if (roomMapManager == null) // 맵 매니저 확인
        {
            roomMapManager = FindFirstObjectByType<RoomMapManager>(); // 씬에서 맵 매니저 검색
        }

        if (playerTransform == null) // 플레이어 Transform 확인
        {
            PlayerStats playerStats = FindFirstObjectByType<PlayerStats>(); // 플레이어 검색

            if (playerStats != null) // 플레이어 확인
            {
                playerTransform = playerStats.transform; // 플레이어 Transform 저장
            }
        }
    }

    public void TryMoveThroughDoor(RoomDirection direction) // 문을 통한 방 이동 시도 함수
    {
        if (isChangingRoom) // 이미 방 이동 중인지 확인
        {
            return; // 중복 이동 방지
        }

        StartCoroutine(MoveThroughDoorRoutine(direction)); // 방 이동 코루틴 실행
    }
    private IEnumerator MoveThroughDoorRoutine(RoomDirection direction) // 문 이동 처리 코루틴
    {
        isChangingRoom = true; // 방 이동 중으로 설정

        if (roomMapManager == null) // 맵 매니저 확인
        {
            Debug.LogError("RoomMapManager is missing."); // 오류 로그
            isChangingRoom = false; // 방 이동 상태 해제
            yield break; // 이동 중단
        }

        bool moved = roomMapManager.TryMoveToRoom(RoomDirectionUtility.ToVector2Int(direction)); // 맵 데이터상 방 이동 시도

        if (!moved) // 이동 실패 확인
        {
            isChangingRoom = false; // 방 이동 상태 해제
            yield break; // 이동 중단
        }

        LoadCurrentRoom(direction, false); // 새 현재 방 로드

        yield return new WaitForSeconds(0.15f); // 연속 이동 방지 대기

        isChangingRoom = false; // 방 이동 상태 해제
    }
    private void LoadCurrentRoom(RoomDirection movedDirection, bool useDefaultEntry) // 현재 방 프리팹 로드 함수
    {
        if (defaultRoomPrefab == null) // 기본 방 프리팹 확인
        {
            Debug.LogError("Default Room Prefab is missing."); // 오류 로그
            return; // 로드 중단
        }

        if (currentRoomObject != null) // 기존 방 확인
        {
            Destroy(currentRoomObject); // 기존 방 삭제
        }

        Transform parent = roomParent != null ? roomParent : transform; // 부모 결정

        currentRoomObject = Instantiate(defaultRoomPrefab, roomSpawnPosition, Quaternion.identity, parent); // 새 방 생성

        currentRoomController = currentRoomObject.GetComponent<RoomController>(); // 방 컨트롤러 가져오기

        if (currentRoomController == null) // 방 컨트롤러 확인
        {
            Debug.LogError("RoomController is missing on room prefab."); // 오류 로그
            return; // 로드 중단
        }

        currentRoomController.Initialize(this, roomMapManager); // 현재 방 초기화

        MovePlayerToEntry(movedDirection, useDefaultEntry); // 플레이어 입장 위치 이동
    }

    private void MovePlayerToEntry(RoomDirection movedDirection, bool useDefaultEntry) // 플레이어 입장 위치 이동 함수
    {
        if (playerTransform == null) // 플레이어 확인
        {
            Debug.LogWarning("Player Transform is missing."); // 경고 로그
            return; // 이동 중단
        }

        if (currentRoomController == null) // 방 컨트롤러 확인
        {
            return; // 이동 중단
        }

        Transform entryPoint = useDefaultEntry ? currentRoomController.GetDefaultEntryPoint() : currentRoomController.GetEntryPointForArrival(movedDirection); // 입장 위치 결정

        if (entryPoint == null) // 입장 위치 확인
        {
            return; // 이동 중단
        }

        playerTransform.position = entryPoint.position; // 플레이어 위치 이동
    }
}