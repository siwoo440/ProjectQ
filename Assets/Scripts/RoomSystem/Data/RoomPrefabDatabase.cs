using UnityEngine; // Unity 기본 기능 사용

[CreateAssetMenu(fileName = "RoomPrefabDatabase", menuName = "Project Q/Room/Room Prefab Database")] // 에셋 생성 메뉴 등록
public class RoomPrefabDatabase : ScriptableObject // 방 프리팹 데이터베이스 클래스
{
    [Header("Global Fallback")] // 전체 기본값 제목
    public GameObject globalFallbackRoomPrefab; // 모든 지역에서 사용할 수 있는 최종 기본 방 프리팹

    [Header("Region Prefab Sets")] // 지역별 프리팹 세트 제목
    public RoomPrefabSet[] regionPrefabSets; // 지역별 방 프리팹 세트 목록

    public GameObject[] GetCandidatePrefabs(RegionType regionType, RoomType roomType) // 지역과 방 타입에 맞는 후보 프리팹 배열 반환 함수
    {
        RoomPrefabSet prefabSet = GetPrefabSet(regionType); // 지역 프리팹 세트 가져오기

        if (prefabSet == null) // 지역 세트 확인
        {
            return null; // 후보 없음
        }

        return prefabSet.GetPrefabs(roomType); // 방 타입에 맞는 프리팹 배열 반환
    }

    public GameObject GetFallbackPrefab(RegionType regionType) // 지역 기본 프리팹 반환 함수
    {
        RoomPrefabSet prefabSet = GetPrefabSet(regionType); // 지역 프리팹 세트 가져오기

        if (prefabSet != null && prefabSet.GetFallbackRoomPrefab() != null) // 지역 기본 프리팹 확인
        {
            return prefabSet.GetFallbackRoomPrefab(); // 지역 기본 프리팹 반환
        }

        return globalFallbackRoomPrefab; // 전체 기본 프리팹 반환
    }

    private RoomPrefabSet GetPrefabSet(RegionType regionType) // 지역에 맞는 프리팹 세트 반환 함수
    {
        if (regionPrefabSets == null) // 지역 세트 배열 확인
        {
            return null; // 세트 없음
        }

        for (int i = 0; i < regionPrefabSets.Length; i++) // 지역 세트 수만큼 반복
        {
            RoomPrefabSet prefabSet = regionPrefabSets[i]; // 현재 지역 세트 가져오기

            if (prefabSet == null) // 지역 세트 확인
            {
                continue; // 다음 세트 확인
            }

            if (prefabSet.regionType == regionType) // 지역 타입 일치 확인
            {
                return prefabSet; // 일치하는 지역 세트 반환
            }
        }

        return null; // 일치하는 지역 세트 없음
    }
}