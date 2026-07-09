using System.Collections; // 코루틴 사용
using UnityEngine; // Unity 기본 기능 사용

public class RoomSceneController : MonoBehaviour // 실제 방 프리팹 생성과 이동을 관리하는 클래스
{
    [Header("References")] // 참조 제목
    public RoomMapManager roomMapManager; // 랜덤 방 맵 관리자

    public Transform roomParent; // 방 프리팹 부모

    public Transform playerTransform; // 플레이어 Transform

    [Header("Room Prefab Database")] // 방 프리팹 데이터베이스 제목
    public RoomPrefabDatabase roomPrefabDatabase; // 지역과 방 타입별 프리팹 데이터베이스

    [Header("Fallback Room Prefab")] // 기본 방 프리팹 제목
    public GameObject defaultRoomPrefab; // 데이터베이스가 없을 때 사용할 기본 방 프리팹

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

        if (roomMapManager != null && disableKeyboardTestMovementOnStart) // 키보드 테스트 이동 비활성화 여부 확인
        {
            roomMapManager.enableKeyboardTestMovement = false; // 방향키 방 이동 테스트 비활성화
        }

        if (loadRoomOnStart) // 시작 시 방 로드 여부 확인
        {
            LoadCurrentRoom(RoomDirection.Down, true); // 현재 방 로드
        }
    }

    private void FindReferencesIfNeeded() // 참조 자동 검색 함수
    {
        if (roomMapManager == null) // 맵 매니저 연결 확인
        {
            roomMapManager = FindFirstObjectByType<RoomMapManager>(); // 맵 매니저 자동 검색
        }

        if (playerTransform == null) // 플레이어 Transform 연결 확인
        {
            PlayerStats playerStats = FindFirstObjectByType<PlayerStats>(); // 플레이어 스탯 자동 검색

            if (playerStats != null) // 플레이어 스탯 확인
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
        GameObject selectedRoomPrefab = SelectRoomPrefabForCurrentRoom(); // 현재 방에 맞는 프리팹 선택

        if (selectedRoomPrefab == null) // 선택된 방 프리팹 확인
        {
            Debug.LogError("Selected Room Prefab is missing."); // 오류 로그
            return; // 방 로드 중단
        }

        if (currentRoomObject != null) // 기존 방 오브젝트 확인
        {
            Destroy(currentRoomObject); // 기존 방 삭제
        }

        Transform parent = roomParent != null ? roomParent : transform; // 방 부모 결정

        currentRoomObject = Instantiate(selectedRoomPrefab, roomSpawnPosition, Quaternion.identity, parent); // 방 프리팹 생성

        currentRoomController = currentRoomObject.GetComponent<RoomController>(); // 방 컨트롤러 가져오기

        if (currentRoomController == null) // 방 컨트롤러 확인
        {
            Debug.LogError("RoomController is missing on room prefab : " + selectedRoomPrefab.name); // 오류 로그
            return; // 방 로드 중단
        }

        currentRoomController.Initialize(this, roomMapManager); // 방 초기화

        MovePlayerToEntry(movedDirection, useDefaultEntry); // 플레이어 입장 위치 이동

        Debug.Log("Loaded Room Prefab : " + selectedRoomPrefab.name); // 로드된 방 프리팹 로그
    }

    private GameObject SelectRoomPrefabForCurrentRoom() // 현재 방에 맞는 방 프리팹 선택 함수
    {
        RoomData currentRoomData = GetCurrentRoomData(); // 현재 방 데이터 가져오기

        RegionType currentRegionType = GetCurrentRegionType(); // 현재 지역 타입 가져오기

        if (currentRoomData == null) // 현재 방 데이터 확인
        {
            return GetFallbackRoomPrefab(currentRegionType); // 기본 방 반환
        }

        GameObject[] candidatePrefabs = GetCandidatePrefabs(currentRegionType, currentRoomData.roomType); // 후보 프리팹 목록 가져오기

        if (candidatePrefabs == null || candidatePrefabs.Length == 0) // 후보 프리팹 확인
        {
            return GetFallbackRoomPrefab(currentRegionType); // 후보가 없으면 기본 방 반환
        }

        int prefabIndex = GetOrCreatePrefabIndex(currentRoomData, candidatePrefabs.Length); // 방 데이터에 저장된 프리팹 인덱스 가져오기

        GameObject selectedPrefab = candidatePrefabs[prefabIndex]; // 선택된 프리팹 가져오기

        if (selectedPrefab == null) // 선택된 프리팹이 비어 있는지 확인
        {
            return GetFallbackRoomPrefab(currentRegionType); // 비어 있으면 기본 방 반환
        }

        return selectedPrefab; // 선택된 프리팹 반환
    }

    private RoomData GetCurrentRoomData() // 현재 방 데이터 반환 함수
    {
        if (roomMapManager == null) // 맵 매니저 확인
        {
            return null; // 현재 방 데이터 없음
        }

        return roomMapManager.GetRoom(roomMapManager.currentRoomPosition); // 현재 좌표의 방 데이터 반환
    }

    private RegionType GetCurrentRegionType() // 현재 지역 타입 반환 함수
    {
        if (roomMapManager == null) // 맵 매니저 확인
        {
            return RegionType.Forest; // 기본 지역 반환
        }

        return roomMapManager.GetCurrentRegionType(); // 맵 매니저에서 현재 지역 반환
    }

    private GameObject[] GetCandidatePrefabs(RegionType regionType, RoomType roomType) // 데이터베이스에서 후보 프리팹 배열 반환 함수
    {
        if (roomPrefabDatabase == null) // 데이터베이스 연결 확인
        {
            return null; // 후보 없음
        }

        return roomPrefabDatabase.GetCandidatePrefabs(regionType, roomType); // 지역과 방 타입에 맞는 후보 반환
    }

    private GameObject GetFallbackRoomPrefab(RegionType regionType) // 기본 방 프리팹 반환 함수
    {
        if (roomPrefabDatabase != null) // 데이터베이스 확인
        {
            GameObject databaseFallback = roomPrefabDatabase.GetFallbackPrefab(regionType); // 데이터베이스 기본 방 가져오기

            if (databaseFallback != null) // 데이터베이스 기본 방 확인
            {
                return databaseFallback; // 데이터베이스 기본 방 반환
            }
        }

        return defaultRoomPrefab; // Inspector 기본 방 반환
    }

    private int GetOrCreatePrefabIndex(RoomData roomData, int prefabCount) // 방 프리팹 인덱스 반환 함수
    {
        if (prefabCount <= 0) // 프리팹 개수 확인
        {
            return 0; // 기본 인덱스 반환
        }

        if (roomData == null) // 방 데이터 확인
        {
            return Random.Range(0, prefabCount); // 방 데이터가 없으면 임시 랜덤 반환
        }

        if (roomData.roomPrefabIndex < 0 || roomData.roomPrefabIndex >= prefabCount) // 아직 프리팹이 선택되지 않았는지 확인
        {
            roomData.roomPrefabIndex = Random.Range(0, prefabCount); // 랜덤 프리팹 인덱스 저장
        }

        return roomData.roomPrefabIndex; // 저장된 프리팹 인덱스 반환
    }

    private void MovePlayerToEntry(RoomDirection movedDirection, bool useDefaultEntry) // 플레이어 입장 위치 이동 함수
    {
        if (playerTransform == null) // 플레이어 Transform 확인
        {
            Debug.LogWarning("Player Transform is missing."); // 경고 로그
            return; // 이동 중단
        }

        if (currentRoomController == null) // 현재 방 컨트롤러 확인
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