using UnityEngine; // Unity 기본 기능 사용

[System.Serializable] // Inspector에서 보이도록 직렬화
public class RoomPrefabSet // 지역 하나의 방 프리팹 목록 클래스
{
    [Header("Region Settings")] // 지역 설정 제목
    public RegionType regionType; // 이 프리팹 세트가 담당하는 지역

    public GameObject fallbackRoomPrefab; // 해당 지역에서 사용할 기본 방 프리팹

    [Header("Room Prefabs By Type")] // 방 타입별 프리팹 제목
    public GameObject[] startRoomPrefabs; // 시작 방 프리팹 목록
    public GameObject[] normalBattleRoomPrefabs; // 일반 전투방 프리팹 목록
    public GameObject[] eliteBattleRoomPrefabs; // 정예 전투방 프리팹 목록
    public GameObject[] eventRoomPrefabs; // 이벤트 방 프리팹 목록
    public GameObject[] restRoomPrefabs; // 휴식 방 프리팹 목록
    public GameObject[] midBossRoomPrefabs; // 중간 보스방 프리팹 목록
    public GameObject[] finalBossRoomPrefabs; // 최종 보스방 프리팹 목록

    public GameObject[] GetPrefabs(RoomType roomType) // 방 타입에 맞는 프리팹 배열 반환 함수
    {
        if (roomType == RoomType.Start) // 시작 방 확인
        {
            return startRoomPrefabs; // 시작 방 프리팹 반환
        }

        if (roomType == RoomType.NormalBattle) // 일반 전투방 확인
        {
            return normalBattleRoomPrefabs; // 일반 전투방 프리팹 반환
        }

        if (roomType == RoomType.EliteBattle) // 정예 전투방 확인
        {
            return eliteBattleRoomPrefabs; // 정예 전투방 프리팹 반환
        }

        if (roomType == RoomType.Event) // 이벤트 방 확인
        {
            return eventRoomPrefabs; // 이벤트 방 프리팹 반환
        }

        if (roomType == RoomType.Rest) // 휴식 방 확인
        {
            return restRoomPrefabs; // 휴식 방 프리팹 반환
        }

        if (roomType == RoomType.MidBoss) // 중간 보스방 확인
        {
            return midBossRoomPrefabs; // 중간 보스방 프리팹 반환
        }

        if (roomType == RoomType.FinalBoss) // 최종 보스방 확인
        {
            return finalBossRoomPrefabs; // 최종 보스방 프리팹 반환
        }

        return null; // 해당 타입이 없으면 null 반환
    }

    public GameObject GetFallbackRoomPrefab() // 지역 기본 방 프리팹 반환 함수
    {
        return fallbackRoomPrefab; // 기본 방 프리팹 반환
    }
}