[System.Serializable] // Inspector 확인이 가능하도록 직렬화
public class FloorData // 현재 층 정보를 저장하는 클래스
{
    public int floorNumber; // 현재 층 번호

    public RegionType regionType; // 현재 지역

    public FloorType floorType; // 현재 층 타입

    public int targetRoomCount; // 생성할 방 개수

    public FloorData(int newFloorNumber, int newTargetRoomCount) // 층 데이터 생성자
    {
        floorNumber = newFloorNumber; // 층 번호 저장
        regionType = GetRegionTypeByFloor(newFloorNumber); // 층 번호로 지역 계산
        floorType = GetFloorTypeByFloor(newFloorNumber); // 층 번호로 층 타입 계산
        targetRoomCount = newTargetRoomCount; // 목표 방 개수 저장
    }

    public static RegionType GetRegionTypeByFloor(int floorNumber) // 층 번호로 지역 반환 함수
    {
        if (floorNumber <= 3) // 1~3층 확인
        {
            return RegionType.Forest; // 숲 반환
        }

        if (floorNumber <= 6) // 4~6층 확인
        {
            return RegionType.Beach; // 해변 반환
        }

        if (floorNumber <= 9) // 7~9층 확인
        {
            return RegionType.Desert; // 사막 반환
        }

        if (floorNumber <= 12) // 10~12층 확인
        {
            return RegionType.Snowfield; // 설원 반환
        }

        return RegionType.ServerRoom; // 13~15층은 서버실 반환
    }

    public static FloorType GetFloorTypeByFloor(int floorNumber) // 층 번호로 층 타입 반환 함수
    {
        int floorStep = ((floorNumber - 1) % 3) + 1; // 지역 안에서 몇 번째 층인지 계산

        if (floorStep == 1) // 첫 번째 층 확인
        {
            return FloorType.Normal; // 일반층 반환
        }

        if (floorStep == 2) // 두 번째 층 확인
        {
            return FloorType.MidBoss; // 중간보스층 반환
        }

        return FloorType.FinalBoss; // 세 번째 층은 최종보스층 반환
    }

    public string GetDisplayName() // 화면에 표시할 층 이름 반환 함수
    {
        string regionName = GetRegionDisplayName(regionType); // 지역 이름 가져오기

        int regionFloorNumber = ((floorNumber - 1) % 3) + 1; // 지역 안 층 번호 계산

        return regionName + " " + regionFloorNumber; // 표시 이름 반환
    }

    private string GetRegionDisplayName(RegionType type) // 지역 이름 반환 함수
    {
        if (type == RegionType.Forest) // 숲 확인
        {
            return "Forest"; // 숲 이름 반환
        }

        if (type == RegionType.Beach) // 해변 확인
        {
            return "Beach"; // 해변 이름 반환
        }

        if (type == RegionType.Desert) // 사막 확인
        {
            return "Desert"; // 사막 이름 반환
        }

        if (type == RegionType.Snowfield) // 설원 확인
        {
            return "Snowfield"; // 설원 이름 반환
        }

        return "Server Room"; // 서버실 이름 반환
    }
}