using System.Collections.Generic; // List와 Dictionary 사용
using UnityEngine; // Unity 기본 기능 사용

public class RoomMapManager : MonoBehaviour // 랜덤 방 맵 생성과 현재 방 이동을 관리하는 클래스
{
    [Header("Floor Settings")] // 층 설정 제목
    public int currentFloorNumber = 1; // 현재 층 번호

    [Header("Debug Settings")] // 디버그 설정 제목
    public bool generateOnStart = true; // 시작 시 맵 생성 여부

    public bool enableKeyboardTestMovement = true; // 키보드 이동 테스트 여부

    public bool autoClearEnteredRoomForTest = true; // 테스트용 입장 방 자동 클리어 여부

    [Header("UI Reference")] // UI 참조 제목
    public RoomMinimapUI roomMinimapUI; // 미니맵 UI

    public FloorData currentFloorData; // 현재 층 데이터

    public List<RoomData> generatedRooms = new List<RoomData>(); // 생성된 방 목록

    public Vector2Int currentRoomPosition = Vector2Int.zero; // 현재 방 좌표

    private Dictionary<Vector2Int, RoomData> roomLookup = new Dictionary<Vector2Int, RoomData>(); // 좌표로 방을 찾기 위한 딕셔너리

    private readonly Vector2Int[] directions = new Vector2Int[] // 상하좌우 방향 배열
    {
        Vector2Int.up, // 위
        Vector2Int.down, // 아래
        Vector2Int.left, // 왼쪽
        Vector2Int.right // 오른쪽
    };

    private void Start() // 시작 함수
    {
        if (generateOnStart) // 시작 시 생성 여부 확인
        {
            GenerateFloor(currentFloorNumber); // 현재 층 맵 생성
        }
    }

    private void Update() // 매 프레임 입력 확인 함수
    {
        if (!enableKeyboardTestMovement) // 키보드 테스트 여부 확인
        {
            return; // 테스트 이동 중단
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) // 위 이동 입력 확인
        {
            TryMoveToRoom(Vector2Int.up); // 위 방으로 이동 시도
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) // 아래 이동 입력 확인
        {
            TryMoveToRoom(Vector2Int.down); // 아래 방으로 이동 시도
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) // 왼쪽 이동 입력 확인
        {
            TryMoveToRoom(Vector2Int.left); // 왼쪽 방으로 이동 시도
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) // 오른쪽 이동 입력 확인
        {
            TryMoveToRoom(Vector2Int.right); // 오른쪽 방으로 이동 시도
        }
    }

    public void GenerateFloor(int floorNumber) // 층 맵 생성 함수
    {
        generatedRooms.Clear(); // 기존 방 목록 초기화
        roomLookup.Clear(); // 기존 방 딕셔너리 초기화

        currentFloorNumber = Mathf.Clamp(floorNumber, 1, 15); // 층 번호 보정

        FloorType floorType = FloorData.GetFloorTypeByFloor(currentFloorNumber); // 현재 층 타입 계산

        int roomCount = GetRandomRoomCount(floorType); // 층 타입에 맞는 방 개수 랜덤 결정

        currentFloorData = new FloorData(currentFloorNumber, roomCount); // 현재 층 데이터 생성

        CreateConnectedRoomMap(roomCount); // 연결된 방 맵 생성

        CalculateRoomDistances(); // 시작 방 기준 거리 계산

        AssignSpecialEndRoom(); // 가장 먼 방을 끝방으로 지정

        AssignRandomRoomTypes(); // 나머지 방 타입 랜덤 배정

        SetupStartRoom(); // 시작 방 설정

        RefreshMinimap(); // 미니맵 갱신

        Debug.Log("Generated Floor : " + currentFloorData.GetDisplayName() + " / Room Count : " + generatedRooms.Count); // 생성 로그
    }

    private int GetRandomRoomCount(FloorType floorType) // 층 타입별 랜덤 방 개수 반환 함수
    {
        if (floorType == FloorType.Normal) // 일반층 확인
        {
            return Random.Range(8, 11); // 8~10개 반환
        }

        if (floorType == FloorType.MidBoss) // 중간보스층 확인
        {
            return Random.Range(10, 16); // 10~15개 반환
        }

        return Random.Range(12, 19); // 최종보스층은 12~18개 반환
    }

    private void CreateConnectedRoomMap(int targetRoomCount) // 연결된 랜덤 방 맵 생성 함수
    {
        AddRoom(Vector2Int.zero, RoomType.Start); // 시작 방 생성

        int safetyCount = 0; // 무한 반복 방지 카운트

        while (generatedRooms.Count < targetRoomCount && safetyCount < 1000) // 목표 방 개수까지 반복
        {
            safetyCount++; // 안전 카운트 증가

            RoomData baseRoom = generatedRooms[Random.Range(0, generatedRooms.Count)]; // 기존 방 중 하나 선택

            Vector2Int randomDirection = directions[Random.Range(0, directions.Length)]; // 랜덤 방향 선택

            Vector2Int newPosition = baseRoom.gridPosition + randomDirection; // 새 방 좌표 계산

            if (roomLookup.ContainsKey(newPosition)) // 이미 방이 있는지 확인
            {
                continue; // 다음 반복으로 이동
            }

            AddRoom(newPosition, RoomType.NormalBattle); // 새 방 추가
        }
    }

    private void AddRoom(Vector2Int position, RoomType roomType) // 방 추가 함수
    {
        RoomData newRoom = new RoomData(position, roomType); // 새 방 데이터 생성

        generatedRooms.Add(newRoom); // 방 목록에 추가

        roomLookup.Add(position, newRoom); // 딕셔너리에 추가
    }

    private void CalculateRoomDistances() // 시작 방 기준 거리 계산 함수
    {
        Queue<RoomData> queue = new Queue<RoomData>(); // BFS 큐 생성

        HashSet<Vector2Int> visitedPositions = new HashSet<Vector2Int>(); // 방문 좌표 저장

        RoomData startRoom = GetRoom(Vector2Int.zero); // 시작 방 가져오기

        if (startRoom == null) // 시작 방 확인
        {
            return; // 거리 계산 중단
        }

        startRoom.distanceFromStart = 0; // 시작 방 거리 설정

        queue.Enqueue(startRoom); // 시작 방 큐에 추가

        visitedPositions.Add(startRoom.gridPosition); // 시작 좌표 방문 처리

        while (queue.Count > 0) // 큐가 빌 때까지 반복
        {
            RoomData currentRoom = queue.Dequeue(); // 현재 방 꺼내기

            for (int i = 0; i < directions.Length; i++) // 네 방향 반복
            {
                Vector2Int nextPosition = currentRoom.gridPosition + directions[i]; // 다음 좌표 계산

                if (!roomLookup.ContainsKey(nextPosition)) // 방이 없는지 확인
                {
                    continue; // 다음 방향으로 이동
                }

                if (visitedPositions.Contains(nextPosition)) // 이미 방문했는지 확인
                {
                    continue; // 다음 방향으로 이동
                }

                RoomData nextRoom = roomLookup[nextPosition]; // 다음 방 가져오기

                nextRoom.distanceFromStart = currentRoom.distanceFromStart + 1; // 거리 설정

                visitedPositions.Add(nextPosition); // 방문 처리

                queue.Enqueue(nextRoom); // 큐에 추가
            }
        }
    }

    private void AssignSpecialEndRoom() // 끝방 지정 함수
    {
        RoomData farthestRoom = null; // 가장 먼 방 저장 변수

        for (int i = 0; i < generatedRooms.Count; i++) // 생성된 방 수만큼 반복
        {
            RoomData room = generatedRooms[i]; // 현재 방 가져오기

            if (room.gridPosition == Vector2Int.zero) // 시작 방 확인
            {
                continue; // 시작 방 제외
            }

            if (farthestRoom == null || room.distanceFromStart > farthestRoom.distanceFromStart) // 더 먼 방인지 확인
            {
                farthestRoom = room; // 가장 먼 방 갱신
            }
        }

        if (farthestRoom == null) // 가장 먼 방 확인
        {
            return; // 끝방 지정 중단
        }

        if (currentFloorData.floorType == FloorType.Normal) // 일반층 확인
        {
            farthestRoom.roomType = RoomType.EliteBattle; // 일반층 끝방은 정예방
        }
        else if (currentFloorData.floorType == FloorType.MidBoss) // 중간보스층 확인
        {
            farthestRoom.roomType = RoomType.MidBoss; // 중간보스방 지정
        }
        else // 최종보스층 처리
        {
            farthestRoom.roomType = RoomType.FinalBoss; // 최종보스방 지정
        }
    }

    private void AssignRandomRoomTypes() // 일반 방 타입 랜덤 배정 함수
    {
        for (int i = 0; i < generatedRooms.Count; i++) // 생성된 방 수만큼 반복
        {
            RoomData room = generatedRooms[i]; // 현재 방 가져오기

            if (room.roomType == RoomType.Start) // 시작 방 확인
            {
                continue; // 시작 방 제외
            }

            if (room.roomType == RoomType.MidBoss || room.roomType == RoomType.FinalBoss) // 보스방 확인
            {
                continue; // 보스방 제외
            }

            room.roomType = GetWeightedRandomRoomType(); // 추천 비율에 따라 방 타입 배정
        }
    }

    private RoomType GetWeightedRandomRoomType() // 가중치 기반 랜덤 방 타입 반환 함수
    {
        int randomValue = Random.Range(0, 100); // 0~99 랜덤 값 생성

        if (randomValue < 60) // 60% 확인
        {
            return RoomType.NormalBattle; // 일반 전투방 반환
        }

        if (randomValue < 80) // 20% 확인
        {
            return RoomType.EliteBattle; // 정예 전투방 반환
        }

        if (randomValue < 90) // 10% 확인
        {
            return RoomType.Event; // 이벤트방 반환
        }

        return RoomType.Rest; // 나머지 10%는 휴식방 반환
    }

    private void SetupStartRoom() // 시작 방 설정 함수
    {
        currentRoomPosition = Vector2Int.zero; // 현재 위치를 시작 방으로 설정

        RoomData startRoom = GetRoom(currentRoomPosition); // 시작 방 가져오기

        if (startRoom == null) // 시작 방 확인
        {
            return; // 설정 중단
        }

        startRoom.roomType = RoomType.Start; // 시작 방 타입 설정

        startRoom.isVisited = true; // 시작 방 방문 처리

        startRoom.isCleared = true; // 시작 방 클리어 처리
    }

    public bool TryMoveToRoom(Vector2Int direction) // 인접 방 이동 시도 함수
    {
        Vector2Int targetPosition = currentRoomPosition + direction; // 이동 목표 좌표 계산

        if (!roomLookup.ContainsKey(targetPosition)) // 목표 방 존재 확인
        {
            Debug.Log("No room in this direction : " + direction); // 방 없음 로그
            return false; // 이동 실패 반환
        }

        currentRoomPosition = targetPosition; // 현재 방 좌표 변경

        RoomData currentRoom = GetRoom(currentRoomPosition); // 현재 방 가져오기

        if (currentRoom != null) // 현재 방 확인
        {
            currentRoom.isVisited = true; // 방문 처리

            if (autoClearEnteredRoomForTest) // 테스트 자동 클리어 여부 확인
            {
                currentRoom.isCleared = true; // 테스트용 클리어 처리
            }
        }

        RefreshMinimap(); // 미니맵 갱신

        Debug.Log("Moved Room : " + currentRoomPosition); // 이동 로그

        return true; // 이동 성공 반환
    }

    public RoomData GetRoom(Vector2Int position) // 좌표로 방 반환 함수
    {
        if (!roomLookup.ContainsKey(position)) // 방 존재 확인
        {
            return null; // 없으면 null 반환
        }

        return roomLookup[position]; // 방 반환
    }

    public List<RoomData> GetVisibleRooms() // 미니맵에 보일 방 목록 반환 함수
    {
        List<RoomData> visibleRooms = new List<RoomData>(); // 보이는 방 목록 생성

        for (int i = 0; i < generatedRooms.Count; i++) // 전체 방 수만큼 반복
        {
            RoomData room = generatedRooms[i]; // 현재 방 가져오기

            if (room.isVisited || IsAdjacentToVisitedRoom(room.gridPosition)) // 방문했거나 방문 방과 인접했는지 확인
            {
                visibleRooms.Add(room); // 보이는 방 목록에 추가
            }
        }

        return visibleRooms; // 보이는 방 목록 반환
    }

    public bool IsCurrentRoom(RoomData room) // 현재 방 여부 확인 함수
    {
        if (room == null) // 방 데이터 확인
        {
            return false; // 현재 방 아님
        }

        return room.gridPosition == currentRoomPosition; // 좌표 비교 결과 반환
    }

    private bool IsAdjacentToVisitedRoom(Vector2Int position) // 방문한 방과 인접한지 확인하는 함수
    {
        for (int i = 0; i < directions.Length; i++) // 네 방향 반복
        {
            Vector2Int neighborPosition = position + directions[i]; // 이웃 좌표 계산

            RoomData neighborRoom = GetRoom(neighborPosition); // 이웃 방 가져오기

            if (neighborRoom == null) // 이웃 방 확인
            {
                continue; // 다음 방향으로 이동
            }

            if (neighborRoom.isVisited) // 이웃 방 방문 여부 확인
            {
                return true; // 인접한 방문 방 있음
            }
        }

        return false; // 인접한 방문 방 없음
    }

    public void RefreshMinimap() // 미니맵 갱신 함수
    {
        if (roomMinimapUI != null) // 미니맵 UI 연결 확인
        {
            roomMinimapUI.Refresh(this); // 미니맵 갱신
        }
    }

    public RegionType GetCurrentRegionType() // 현재 지역 타입 반환 함수
    {
        if (currentFloorData == null) // 현재 층 데이터 확인
        {
            return RegionType.Forest; // 기본 지역 반환
        }

        return currentFloorData.regionType; // 현재 층의 지역 타입 반환
    }
}