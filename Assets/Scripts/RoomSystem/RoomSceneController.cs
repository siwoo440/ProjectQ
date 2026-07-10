using System.Collections; // 코루틴 사용
using System.Collections.Generic; // Dictionary 사용
using UnityEngine; // Unity 기본 기능 사용

public class RoomSceneController : MonoBehaviour // 실제 방 프리팹 생성과 이동을 관리하는 클래스
{
    [Header("References")] // 참조 제목
    public RoomMapManager roomMapManager; // 랜덤 방 맵 관리자

    public Transform roomParent; // 방 프리팹 부모

    public Transform playerTransform; // 플레이어 Transform

    public Rigidbody2D playerRigidbody; // 플레이어 Rigidbody2D

    public Transform cameraTransform; // 카메라 Transform

    [Header("Room Prefab Database")] // 방 프리팹 데이터베이스 제목
    public RoomPrefabDatabase roomPrefabDatabase; // 지역과 방 타입별 프리팹 데이터베이스

    [Header("Fallback Room Prefab")] // 기본 방 프리팹 제목
    public GameObject defaultRoomPrefab; // 데이터베이스가 없을 때 사용할 기본 방 프리팹

    [Header("World Room Layout")] // 월드 방 배치 제목
    public Vector2 roomWorldSize = new Vector2(20f, 12f); // 방 하나의 월드 크기

    public Vector3 roomOriginPosition = Vector3.zero; // 시작 방 월드 기준 위치

    public int keepRoomDistance = 1; // 현재 방에서 몇 칸 거리까지 유지할지 설정

    [Header("Camera Transition")] // 카메라 전환 제목
    public float cameraMoveDuration = 0.35f; // 카메라 이동 시간

    public Vector3 cameraOffset = new Vector3(0f, 0f, -10f); // 카메라 오프셋

    [Header("Input Lock")] // 입력 잠금 제목
    public Behaviour[] behavioursToDisableDuringRoomTransition; // 방 이동 중 끌 컴포넌트 목록

    [Header("Start Settings")] // 시작 설정 제목
    public bool loadRoomOnStart = true; // 시작 시 방 로드 여부

    public bool disableKeyboardTestMovementOnStart = true; // 기존 방향키 테스트 이동 비활성화 여부

    private readonly Dictionary<Vector2Int, GameObject> spawnedRoomObjects = new Dictionary<Vector2Int, GameObject>(); // 생성된 방 오브젝트 목록

    private readonly Dictionary<Vector2Int, RoomController> spawnedRoomControllers = new Dictionary<Vector2Int, RoomController>(); // 생성된 방 컨트롤러 목록

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
            LoadRoomsAroundCurrentPosition(); // 현재 방 주변 방 로드

            SetCurrentRoomController(); // 현재 방 컨트롤러 설정

            MovePlayerToEntry(RoomDirection.Down, true); // 플레이어 시작 위치 배치

            MoveCameraInstantlyToCurrentRoom(); // 카메라를 현재 방으로 즉시 이동

            EnterCurrentRoom(); // 현재 방 입장 처리
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

        if (playerRigidbody == null && playerTransform != null) // 플레이어 Rigidbody 연결 확인
        {
            playerRigidbody = playerTransform.GetComponent<Rigidbody2D>(); // 플레이어 Rigidbody 자동 검색
        }

        if (cameraTransform == null && Camera.main != null) // 카메라 Transform 연결 확인
        {
            cameraTransform = Camera.main.transform; // 메인 카메라 Transform 저장
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
    public void MoveToNextFloorFromPortal() // 포탈에서 다음층 이동 요청 함수
    {
        if (isChangingRoom) // 이미 이동 중인지 확인
        {
            return; // 중복 이동 방지
        }

        StartCoroutine(MoveToNextFloorRoutine()); // 다음층 이동 코루틴 실행
    }

    private IEnumerator MoveThroughDoorRoutine(RoomDirection direction) // 문 이동 처리 코루틴
    {
        isChangingRoom = true; // 방 이동 중으로 설정

        SetTransitionLock(true); // 조작 잠금

        if (roomMapManager == null) // 맵 매니저 확인
        {
            Debug.LogError("RoomMapManager is missing."); // 오류 로그
            SetTransitionLock(false); // 조작 잠금 해제
            isChangingRoom = false; // 이동 상태 해제
            yield break; // 이동 중단
        }

        bool moved = roomMapManager.TryMoveToRoom(RoomDirectionUtility.ToVector2Int(direction)); // 맵 데이터상 방 이동 시도

        if (!moved) // 이동 실패 확인
        {
            SetTransitionLock(false); // 조작 잠금 해제
            isChangingRoom = false; // 이동 상태 해제
            yield break; // 이동 중단
        }

        LoadRoomsAroundCurrentPosition(); // 새 현재 방 주변 방 로드

        SetCurrentRoomController(); // 현재 방 컨트롤러 갱신

        MovePlayerToEntry(direction, false); // 플레이어를 다음 방 입구에 배치

        yield return StartCoroutine(MoveCameraToCurrentRoomRoutine()); // 카메라를 현재 방으로 부드럽게 이동

        EnterCurrentRoom(); // 현재 방 입장 처리

        UnloadFarRooms(); // 멀어진 방 정리

        SetTransitionLock(false); // 조작 잠금 해제

        yield return new WaitForSeconds(0.05f); // 짧은 안정화 대기

        isChangingRoom = false; // 이동 상태 해제
    }

    private IEnumerator MoveToNextFloorRoutine() // 다음층 이동 처리 코루틴
    {
        isChangingRoom = true; // 이동 중 상태 설정

        SetTransitionLock(true); // 조작 잠금

        if (roomMapManager == null) // 맵 매니저 확인
        {
            Debug.LogError("RoomMapManager is missing."); // 오류 로그
            SetTransitionLock(false); // 조작 잠금 해제
            isChangingRoom = false; // 이동 상태 해제
            yield break; // 이동 중단
        }

        if (!roomMapManager.CanGoNextFloor()) // 다음층 이동 가능 여부 확인
        {
            Debug.Log("Game clear will be connected later."); // 게임 클리어 로그
            SetTransitionLock(false); // 조작 잠금 해제
            isChangingRoom = false; // 이동 상태 해제
            yield break; // 이동 중단
        }

        ClearAllSpawnedRooms(); // 기존 층 방 오브젝트 전체 제거

        roomMapManager.GoToNextFloor(); // 다음층 맵 생성

        LoadRoomsAroundCurrentPosition(); // 새 층 시작 방과 인접 방 로드

        SetCurrentRoomController(); // 현재 방 컨트롤러 갱신

        MovePlayerToEntry(RoomDirection.Down, true); // 플레이어를 시작 방 기본 위치로 이동

        MoveCameraInstantlyToCurrentRoom(); // 카메라를 새 시작 방으로 즉시 이동

        RecoverPlayerForNextFloor(); // 다음층 진입 회복 처리

        EnterCurrentRoom(); // 시작 방 입장 처리

        SetTransitionLock(false); // 조작 잠금 해제

        yield return new WaitForSeconds(0.05f); // 짧은 안정화 대기

        isChangingRoom = false; // 이동 상태 해제

        Debug.Log("Moved To Next Floor"); // 다음층 이동 로그
    }




    private void LoadRoomsAroundCurrentPosition() // 현재 방 주변 방 로드 함수
    {
        if (roomMapManager == null) // 맵 매니저 확인
        {
            return; // 처리 중단
        }

        Vector2Int currentPosition = roomMapManager.currentRoomPosition; // 현재 방 좌표 가져오기

        SpawnRoomIfNeeded(currentPosition); // 현재 방 생성
        SpawnRoomIfNeeded(currentPosition + Vector2Int.up); // 위쪽 방 생성
        SpawnRoomIfNeeded(currentPosition + Vector2Int.down); // 아래쪽 방 생성
        SpawnRoomIfNeeded(currentPosition + Vector2Int.left); // 왼쪽 방 생성
        SpawnRoomIfNeeded(currentPosition + Vector2Int.right); // 오른쪽 방 생성
    }

    private void SpawnRoomIfNeeded(Vector2Int gridPosition) // 방이 없으면 생성하는 함수
    {
        if (spawnedRoomObjects.ContainsKey(gridPosition)) // 이미 생성된 방인지 확인
        {
            return; // 생성하지 않음
        }

        if (roomMapManager == null) // 맵 매니저 확인
        {
            return; // 생성 중단
        }

        RoomData roomData = roomMapManager.GetRoom(gridPosition); // 해당 좌표의 방 데이터 가져오기

        if (roomData == null) // 해당 위치에 방이 없는지 확인
        {
            return; // 생성 중단
        }

        GameObject selectedRoomPrefab = SelectRoomPrefab(roomData); // 방 데이터에 맞는 프리팹 선택

        if (selectedRoomPrefab == null) // 선택된 프리팹 확인
        {
            Debug.LogError("Selected Room Prefab is missing."); // 오류 로그
            return; // 생성 중단
        }

        Transform parent = roomParent != null ? roomParent : transform; // 방 부모 결정

        Vector3 roomWorldPosition = GetRoomWorldPosition(gridPosition); // 방 월드 위치 계산

        GameObject roomObject = Instantiate(selectedRoomPrefab, roomWorldPosition, Quaternion.identity, parent); // 방 생성

        RoomController roomController = roomObject.GetComponent<RoomController>(); // 방 컨트롤러 가져오기

        if (roomController == null) // 방 컨트롤러 확인
        {
            Debug.LogError("RoomController is missing on room prefab : " + selectedRoomPrefab.name); // 오류 로그
            Destroy(roomObject); // 잘못 생성된 방 삭제
            return; // 생성 중단
        }

        roomController.Initialize(this, roomMapManager, roomData, false); // 방 초기화

        spawnedRoomObjects.Add(gridPosition, roomObject); // 생성된 방 오브젝트 등록

        spawnedRoomControllers.Add(gridPosition, roomController); // 생성된 방 컨트롤러 등록

        Debug.Log("Spawned Room : " + selectedRoomPrefab.name + " / " + gridPosition); // 방 생성 로그
    }

    private GameObject SelectRoomPrefab(RoomData roomData) // 방 데이터에 맞는 프리팹 선택 함수
    {
        RegionType currentRegionType = GetCurrentRegionType(); // 현재 지역 타입 가져오기

        if (roomData == null) // 방 데이터 확인
        {
            return GetFallbackRoomPrefab(currentRegionType); // 기본 방 반환
        }

        GameObject[] candidatePrefabs = GetCandidatePrefabs(currentRegionType, roomData.roomType); // 후보 프리팹 목록 가져오기

        if (candidatePrefabs == null || candidatePrefabs.Length == 0) // 후보 프리팹 확인
        {
            return GetFallbackRoomPrefab(currentRegionType); // 후보가 없으면 기본 방 반환
        }

        int prefabIndex = GetOrCreatePrefabIndex(roomData, candidatePrefabs.Length); // 방 데이터에 저장된 프리팹 인덱스 가져오기

        GameObject selectedPrefab = candidatePrefabs[prefabIndex]; // 선택된 프리팹 가져오기

        if (selectedPrefab == null) // 선택된 프리팹 확인
        {
            return GetFallbackRoomPrefab(currentRegionType); // 기본 방 반환
        }

        return selectedPrefab; // 선택된 프리팹 반환
    }

    private RegionType GetCurrentRegionType() // 현재 지역 타입 반환 함수
    {
        if (roomMapManager == null) // 맵 매니저 확인
        {
            return RegionType.Forest; // 기본 지역 반환
        }

        return roomMapManager.GetCurrentRegionType(); // 현재 지역 반환
    }

    private GameObject[] GetCandidatePrefabs(RegionType regionType, RoomType roomType) // 후보 프리팹 배열 반환 함수
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
            return Random.Range(0, prefabCount); // 임시 랜덤 반환
        }

        if (roomData.roomPrefabIndex < 0 || roomData.roomPrefabIndex >= prefabCount) // 인덱스 유효성 확인
        {
            roomData.roomPrefabIndex = Random.Range(0, prefabCount); // 랜덤 인덱스 저장
        }

        return roomData.roomPrefabIndex; // 저장된 인덱스 반환
    }

    private void SetCurrentRoomController() // 현재 방 컨트롤러 설정 함수
    {
        if (roomMapManager == null) // 맵 매니저 확인
        {
            currentRoomController = null; // 현재 방 컨트롤러 초기화
            return; // 처리 중단
        }

        Vector2Int currentPosition = roomMapManager.currentRoomPosition; // 현재 방 좌표 가져오기

        if (spawnedRoomControllers.ContainsKey(currentPosition)) // 현재 방 컨트롤러 존재 확인
        {
            currentRoomController = spawnedRoomControllers[currentPosition]; // 현재 방 컨트롤러 저장
            return; // 처리 완료
        }

        currentRoomController = null; // 현재 방 컨트롤러 없음
    }

    private void EnterCurrentRoom() // 현재 방 입장 처리 함수
    {
        if (currentRoomController == null) // 현재 방 컨트롤러 확인
        {
            return; // 처리 중단
        }

        currentRoomController.EnterRoom(); // 현재 방 입장 처리
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

    private Vector3 GetRoomWorldPosition(Vector2Int gridPosition) // 방 좌표를 월드 좌표로 변환하는 함수
    {
        float worldX = gridPosition.x * roomWorldSize.x; // 월드 X 계산

        float worldY = gridPosition.y * roomWorldSize.y; // 월드 Y 계산

        return roomOriginPosition + new Vector3(worldX, worldY, 0f); // 최종 월드 좌표 반환
    }

    private Vector3 GetCurrentRoomCameraPosition() // 현재 방 카메라 위치 반환 함수
    {
        if (roomMapManager == null) // 맵 매니저 확인
        {
            return roomOriginPosition + cameraOffset; // 기본 위치 반환
        }

        Vector3 roomPosition = GetRoomWorldPosition(roomMapManager.currentRoomPosition); // 현재 방 월드 위치 가져오기

        return roomPosition + cameraOffset; // 카메라 위치 반환
    }

    private void MoveCameraInstantlyToCurrentRoom() // 카메라 즉시 이동 함수
    {
        if (cameraTransform == null) // 카메라 확인
        {
            return; // 이동 중단
        }

        cameraTransform.position = GetCurrentRoomCameraPosition(); // 현재 방 카메라 위치로 이동
    }

    private IEnumerator MoveCameraToCurrentRoomRoutine() // 카메라 부드러운 이동 코루틴
    {
        if (cameraTransform == null) // 카메라 확인
        {
            yield break; // 코루틴 종료
        }

        Vector3 startPosition = cameraTransform.position; // 시작 위치 저장

        Vector3 targetPosition = GetCurrentRoomCameraPosition(); // 목표 위치 저장

        float elapsedTime = 0f; // 경과 시간 초기화

        while (elapsedTime < cameraMoveDuration) // 이동 시간 동안 반복
        {
            elapsedTime += Time.deltaTime; // 경과 시간 증가

            float t = Mathf.Clamp01(elapsedTime / cameraMoveDuration); // 보간값 계산

            float smoothT = t * t * (3f - 2f * t); // 부드러운 보간값 계산

            cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, smoothT); // 카메라 위치 보간

            yield return null; // 다음 프레임까지 대기
        }

        cameraTransform.position = targetPosition; // 최종 위치 보정
    }

    private void SetTransitionLock(bool locked) // 방 이동 중 조작 잠금 함수
    {
        for (int i = 0; i < behavioursToDisableDuringRoomTransition.Length; i++) // 컴포넌트 수만큼 반복
        {
            Behaviour behaviour = behavioursToDisableDuringRoomTransition[i]; // 컴포넌트 가져오기

            if (behaviour == null) // 컴포넌트 확인
            {
                continue; // 다음 컴포넌트 확인
            }

            behaviour.enabled = !locked; // 잠금 상태에 따라 컴포넌트 활성화 변경
        }

        if (playerRigidbody != null) // 플레이어 Rigidbody 확인
        {
            playerRigidbody.linearVelocity = Vector2.zero; // 플레이어 속도 초기화
        }
    }

    private void UnloadFarRooms() // 멀어진 방 정리 함수
    {
        if (roomMapManager == null) // 맵 매니저 확인
        {
            return; // 처리 중단
        }

        Vector2Int currentPosition = roomMapManager.currentRoomPosition; // 현재 방 좌표 가져오기

        List<Vector2Int> positionsToRemove = new List<Vector2Int>(); // 삭제할 좌표 목록 생성

        foreach (Vector2Int position in spawnedRoomObjects.Keys) // 생성된 방 좌표 반복
        {
            int distance = Mathf.Abs(position.x - currentPosition.x) + Mathf.Abs(position.y - currentPosition.y); // 현재 방과의 거리 계산

            if (distance > keepRoomDistance) // 유지 거리보다 먼지 확인
            {
                positionsToRemove.Add(position); // 삭제 목록에 추가
            }
        }

        for (int i = 0; i < positionsToRemove.Count; i++) // 삭제 목록 반복
        {
            Vector2Int position = positionsToRemove[i]; // 삭제할 좌표 가져오기

            if (spawnedRoomObjects.ContainsKey(position)) // 오브젝트 존재 확인
            {
                Destroy(spawnedRoomObjects[position]); // 방 오브젝트 삭제

                spawnedRoomObjects.Remove(position); // 오브젝트 목록에서 제거
            }

            if (spawnedRoomControllers.ContainsKey(position)) // 컨트롤러 존재 확인
            {
                spawnedRoomControllers.Remove(position); // 컨트롤러 목록에서 제거
            }
        }
    }

    private void ClearAllSpawnedRooms() // 생성된 모든 방 제거 함수
    {
        foreach (GameObject roomObject in spawnedRoomObjects.Values) // 생성된 방 오브젝트 반복
        {
            if (roomObject == null) // 방 오브젝트 확인
            {
                continue; // 다음 방 확인
            }

            Destroy(roomObject); // 방 오브젝트 제거
        }

        spawnedRoomObjects.Clear(); // 방 오브젝트 목록 초기화

        spawnedRoomControllers.Clear(); // 방 컨트롤러 목록 초기화

        currentRoomController = null; // 현재 방 컨트롤러 초기화
    }

    private void RecoverPlayerForNextFloor() // 다음층 진입 시 플레이어 회복 함수
    {
        if (playerTransform == null) // 플레이어 Transform 확인
        {
            return; // 회복 중단
        }

        playerTransform.SendMessage("RecoverForNextFloor", SendMessageOptions.DontRequireReceiver); // PlayerStats에 회복 요청
    }
}