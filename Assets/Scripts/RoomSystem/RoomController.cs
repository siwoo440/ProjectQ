using UnityEngine; // Unity 기본 기능 사용

public class RoomController : MonoBehaviour // 방 프리팹 내부 관리 클래스
{
    [Header("Door References")] // 문 참조 제목
    public RoomDoor doorTop; // 위쪽 문

    public RoomDoor doorBottom; // 아래쪽 문

    public RoomDoor doorLeft; // 왼쪽 문

    public RoomDoor doorRight; // 오른쪽 문

    [Header("Randomizer Reference")] // 랜덤 배치 참조 제목
    public RoomObjectRandomizer roomObjectRandomizer; // 방 구조물 랜덤 배치 관리자

    [Header("Battle Reference")] // 전투 참조 제목
    public RoomBattleController roomBattleController; // 방 전투 관리자

    [Header("Entry Points")] // 입장 위치 제목
    public Transform entryFromTop; // 위쪽에서 들어왔을 때 위치

    public Transform entryFromBottom; // 아래쪽에서 들어왔을 때 위치

    public Transform entryFromLeft; // 왼쪽에서 들어왔을 때 위치

    public Transform entryFromRight; // 오른쪽에서 들어왔을 때 위치

    public Transform entryDefault; // 기본 시작 위치

    public void Initialize(RoomSceneController sceneController, RoomMapManager mapManager) // 방 초기화 함수
    {
        SetupDoor(doorTop, RoomDirection.Up, sceneController, mapManager); // 위쪽 문 설정
        SetupDoor(doorBottom, RoomDirection.Down, sceneController, mapManager); // 아래쪽 문 설정
        SetupDoor(doorLeft, RoomDirection.Left, sceneController, mapManager); // 왼쪽 문 설정
        SetupDoor(doorRight, RoomDirection.Right, sceneController, mapManager); // 오른쪽 문 설정

        if (roomObjectRandomizer != null) // 구조물 랜덤 배치 관리자 확인
        {
            roomObjectRandomizer.RandomizeObjects(); // 방 내부 구조물 랜덤 배치 실행
        }

        RoomData currentRoomData = null; // 현재 방 데이터 변수

        if (mapManager != null) // 맵 매니저 확인
        {
            currentRoomData = mapManager.GetRoom(mapManager.currentRoomPosition); // 현재 방 데이터 가져오기
        }

        if (roomBattleController != null) // 방 전투 관리자 확인
        {
            roomBattleController.Initialize(currentRoomData, this, mapManager); // 방 전투 초기화
        }
    }

    private void SetupDoor(RoomDoor door, RoomDirection direction, RoomSceneController sceneController, RoomMapManager mapManager) // 문 설정 함수
    {
        if (door == null) // 문 확인
        {
            return; // 설정 중단
        }

        bool isConnected = false; // 연결 여부 기본값

        if (mapManager != null) // 맵 매니저 확인
        {
            Vector2Int targetPosition = mapManager.currentRoomPosition + RoomDirectionUtility.ToVector2Int(direction); // 연결될 방 좌표 계산

            isConnected = mapManager.GetRoom(targetPosition) != null; // 해당 방향 방 존재 여부 확인
        }

        door.doorDirection = direction; // 문 방향 설정

        door.Initialize(sceneController, isConnected); // 문 초기화
    }

    public Transform GetEntryPointForArrival(RoomDirection movedDirection) // 이동 방향에 따른 입장 위치 반환 함수
    {
        if (movedDirection == RoomDirection.Up) // 위쪽 방으로 이동했는지 확인
        {
            return entryFromBottom != null ? entryFromBottom : GetDefaultEntryPoint(); // 아래쪽 입장 위치 반환
        }

        if (movedDirection == RoomDirection.Down) // 아래쪽 방으로 이동했는지 확인
        {
            return entryFromTop != null ? entryFromTop : GetDefaultEntryPoint(); // 위쪽 입장 위치 반환
        }

        if (movedDirection == RoomDirection.Left) // 왼쪽 방으로 이동했는지 확인
        {
            return entryFromRight != null ? entryFromRight : GetDefaultEntryPoint(); // 오른쪽 입장 위치 반환
        }

        return entryFromLeft != null ? entryFromLeft : GetDefaultEntryPoint(); // 왼쪽 입장 위치 반환
    }

    public Transform GetDefaultEntryPoint() // 기본 입장 위치 반환 함수
    {
        if (entryDefault != null) // 기본 위치 확인
        {
            return entryDefault; // 기본 위치 반환
        }

        return transform; // 없으면 방 루트 위치 반환
    }

    public void SetDoorsLocked(bool locked) // 모든 문 잠금 설정 함수
    {
        if (doorTop != null) // 위쪽 문 확인
        {
            doorTop.SetLocked(locked); // 위쪽 문 잠금 설정
        }

        if (doorBottom != null) // 아래쪽 문 확인
        {
            doorBottom.SetLocked(locked); // 아래쪽 문 잠금 설정
        }

        if (doorLeft != null) // 왼쪽 문 확인
        {
            doorLeft.SetLocked(locked); // 왼쪽 문 잠금 설정
        }

        if (doorRight != null) // 오른쪽 문 확인
        {
            doorRight.SetLocked(locked); // 오른쪽 문 잠금 설정
        }
    }
}