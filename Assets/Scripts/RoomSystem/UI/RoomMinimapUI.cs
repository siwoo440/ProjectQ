using System.Collections.Generic; // List 사용
using TMPro; // TextMeshPro 사용
using UnityEngine; // Unity 기본 기능 사용

public class RoomMinimapUI : MonoBehaviour // 미니맵 전체 UI 관리 클래스
{
    [Header("References")] // 참조 제목
    public RectTransform cellParent; // 미니맵 칸 부모

    public RoomMinimapCellUI cellPrefab; // 미니맵 칸 프리팹

    public TextMeshProUGUI floorInfoText; // 현재 층 정보 텍스트

    [Header("Layout Settings")] // 배치 설정 제목
    public float cellSpacing = 42f; // 칸 간격
    public bool centerOnCurrentRoom = true; // 현재 방을 미니맵 중앙에 고정할지 여부

    private readonly List<RoomMinimapCellUI> spawnedCells = new List<RoomMinimapCellUI>(); // 생성된 칸 목록

    public void Refresh(RoomMapManager roomMapManager) // 미니맵 갱신 함수
    {
        ClearCells(); // 기존 칸 제거

        if (roomMapManager == null) // 맵 매니저 확인
        {
            return; // 갱신 중단
        }

        UpdateFloorInfo(roomMapManager); // 층 정보 갱신

        List<RoomData> visibleRooms = roomMapManager.GetVisibleRooms(); // 보이는 방 목록 가져오기

        for (int i = 0; i < visibleRooms.Count; i++) // 보이는 방 수만큼 반복
        {
            RoomData roomData = visibleRooms[i]; // 현재 방 데이터 가져오기

            CreateCell(roomMapManager, roomData); // 방 칸 생성
        }
    }

    private void CreateCell(RoomMapManager roomMapManager, RoomData roomData) // 방 칸 생성 함수
    {
        if (cellParent == null) // 부모 확인
        {
            return; // 생성 중단
        }

        if (cellPrefab == null) // 프리팹 확인
        {
            return; // 생성 중단
        }

        RoomMinimapCellUI newCell = Instantiate(cellPrefab, cellParent); // 새 칸 생성

        RectTransform cellRect = newCell.GetComponent<RectTransform>(); // RectTransform 가져오기

        if (cellRect != null) // RectTransform 확인
        {
            Vector2Int displayPosition = roomData.gridPosition; // 표시용 좌표 설정

            if (centerOnCurrentRoom) // 현재 방 중앙 고정 여부 확인
            {
                displayPosition = roomData.gridPosition - roomMapManager.currentRoomPosition; // 현재 방 기준 상대 좌표 계산
            }

            Vector2 cellPosition = new Vector2(displayPosition.x * cellSpacing, displayPosition.y * cellSpacing); // UI 좌표 계산

            cellRect.anchoredPosition = cellPosition; // UI 위치 적용
        }

        bool isCurrentRoom = roomMapManager.IsCurrentRoom(roomData); // 현재 방 여부 확인
        newCell.Setup(roomData, isCurrentRoom); // 칸 표시 설정
        spawnedCells.Add(newCell); // 생성 목록에 추가
    }

    private void UpdateFloorInfo(RoomMapManager roomMapManager) // 층 정보 텍스트 갱신 함수
    {
        if (floorInfoText == null) // 층 정보 텍스트 확인
        {
            return; // 갱신 중단
        }

        if (roomMapManager.currentFloorData == null) // 층 데이터 확인
        {
            floorInfoText.text = "Floor"; // 기본 문구 표시
            return; // 갱신 중단
        }

        floorInfoText.text = roomMapManager.currentFloorData.GetDisplayName() + " / " + roomMapManager.currentFloorData.targetRoomCount + " Rooms"; // 층 정보 표시
    }

    private void ClearCells() // 기존 미니맵 칸 제거 함수
    {
        for (int i = 0; i < spawnedCells.Count; i++) // 생성된 칸 수만큼 반복
        {
            if (spawnedCells[i] != null) // 칸 존재 확인
            {
                Destroy(spawnedCells[i].gameObject); // 칸 삭제
            }
        }

        spawnedCells.Clear(); // 목록 초기화
    }
}