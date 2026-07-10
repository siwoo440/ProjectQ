using UnityEngine; // Vector2Int 사용

[System.Serializable] // Inspector 확인이 가능하도록 직렬화
public class RoomData // 방 하나의 정보를 저장하는 클래스
{
    public Vector2Int gridPosition; // 방의 격자 좌표
    public RoomType roomType; // 방 종류
    public bool isVisited; // 방문 여부
    public bool isCleared; // 클리어 여부
    public int distanceFromStart; // 시작 방으로부터의 거리
    public int roomPrefabIndex; // 현재 방에 선택된 프리팹 인덱스
    public bool isEndRoom; // 현재 층의 끝방 여부

    public RoomData(Vector2Int position, RoomType type) // 방 데이터 생성자
    {
        gridPosition = position; // 방 좌표 저장
        roomType = type; // 방 종류 저장
        isVisited = false; // 기본 방문 여부 초기화
        isCleared = false; // 기본 클리어 여부 초기화
        distanceFromStart = 0; // 기본 거리 초기화
        roomPrefabIndex = -1; // 선택된 방 프리팹 인덱스 초기화
        isEndRoom = false; // 기본 끝방 여부 초기화
    }

    public bool IsBossRoom() // 보스방 여부 반환 함수
    {
        return roomType == RoomType.MidBoss || roomType == RoomType.FinalBoss; // 중간보스 또는 최종보스 확인
    }

    public bool IsBattleRoom() // 전투방 여부 반환 함수
    {
        return roomType == RoomType.NormalBattle || roomType == RoomType.EliteBattle || roomType == RoomType.MidBoss || roomType == RoomType.FinalBoss; // 전투 방인지 확인
    }
}