using TMPro; // TextMeshPro 사용
using UnityEngine; // Unity 기본 기능 사용
using UnityEngine.UI; // Image 사용

public class RoomMinimapCellUI : MonoBehaviour // 미니맵 방 칸 UI 클래스
{
    [Header("UI References")] // UI 참조 제목
    public Image backgroundImage; // 방 칸 배경 이미지

    public TextMeshProUGUI markText; // 방 표시 텍스트

    [Header("Colors")] // 색상 제목
    public Color currentRoomColor = Color.white; // 현재 방 색상

    public Color clearedRoomColor = Color.gray; // 클리어 방 색상

    public Color unknownRoomColor = new Color(0.15f, 0.15f, 0.15f, 1f); // 미탐사 방 색상

    public Color bossRoomColor = new Color(0.7f, 0.1f, 0.1f, 1f); // 보스방 색상

    public Color startRoomColor = new Color(0.2f, 0.5f, 0.9f, 1f); // 시작 방 색상

    public void Setup(RoomData roomData, bool isCurrentRoom) // 방 칸 설정 함수
    {
        if (roomData == null) // 방 데이터 확인
        {
            return; // 설정 중단
        }

        string mark = GetRoomMark(roomData, isCurrentRoom); // 표시 문자 가져오기

        Color color = GetRoomColor(roomData, isCurrentRoom); // 표시 색상 가져오기

        if (markText != null) // 텍스트 연결 확인
        {
            markText.text = mark; // 표시 문자 적용
        }

        if (backgroundImage != null) // 배경 이미지 연결 확인
        {
            backgroundImage.color = color; // 색상 적용
        }
    }

    private string GetRoomMark(RoomData roomData, bool isCurrentRoom) // 방 표시 문자 반환 함수
    {
        if (isCurrentRoom) // 현재 방 확인
        {
            return "●"; // 현재 방 표시
        }

        if (!roomData.isVisited) // 아직 방문하지 않은 방 확인
        {
            if (roomData.IsBossRoom()) // 보스방 확인
            {
                return "B"; // 보스방 표시
            }

            return "?"; // 미탐사 방 표시
        }

        if (roomData.roomType == RoomType.Start) // 시작 방 확인
        {
            return "S"; // 시작 방 표시
        }

        if (roomData.roomType == RoomType.EliteBattle) // 정예 방 확인
        {
            return "E"; // 정예 방 표시
        }

        if (roomData.roomType == RoomType.Event) // 이벤트 방 확인
        {
            return "?"; // 이벤트 방 표시
        }

        if (roomData.roomType == RoomType.Rest) // 휴식 방 확인
        {
            return "R"; // 휴식 방 표시
        }

        if (roomData.roomType == RoomType.MidBoss || roomData.roomType == RoomType.FinalBoss) // 보스 방 확인
        {
            return "B"; // 보스 방 표시
        }

        return ""; // 일반 클리어 방은 빈칸
    }

    private Color GetRoomColor(RoomData roomData, bool isCurrentRoom) // 방 색상 반환 함수
    {
        if (isCurrentRoom) // 현재 방 확인
        {
            return currentRoomColor; // 현재 방 색상 반환
        }

        if (!roomData.isVisited) // 아직 방문하지 않은 방 확인
        {
            if (roomData.IsBossRoom()) // 보스방 확인
            {
                return bossRoomColor; // 보스방 색상 반환
            }

            return unknownRoomColor; // 미탐사 색상 반환
        }

        if (roomData.roomType == RoomType.Start) // 시작 방 확인
        {
            return startRoomColor; // 시작 방 색상 반환
        }

        if (roomData.IsBossRoom()) // 보스방 확인
        {
            return bossRoomColor; // 보스방 색상 반환
        }

        if (roomData.isCleared) // 클리어 여부 확인
        {
            return clearedRoomColor; // 클리어 방 색상 반환
        }

        return unknownRoomColor; // 기본 색상 반환
    }
}