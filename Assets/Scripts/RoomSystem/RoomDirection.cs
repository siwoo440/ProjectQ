using UnityEngine; // Vector2Int 사용

public enum RoomDirection // 방 이동 방향 enum
{
    Up, // 위쪽
    Down, // 아래쪽
    Left, // 왼쪽
    Right // 오른쪽
}

public static class RoomDirectionUtility // 방 이동 방향 보조 기능 클래스
{
    public static Vector2Int ToVector2Int(RoomDirection direction) // 방향을 좌표값으로 변환하는 함수
    {
        if (direction == RoomDirection.Up) // 위쪽 확인
        {
            return Vector2Int.up; // 위쪽 좌표 반환
        }

        if (direction == RoomDirection.Down) // 아래쪽 확인
        {
            return Vector2Int.down; // 아래쪽 좌표 반환
        }

        if (direction == RoomDirection.Left) // 왼쪽 확인
        {
            return Vector2Int.left; // 왼쪽 좌표 반환
        }

        return Vector2Int.right; // 오른쪽 좌표 반환
    }

    public static RoomDirection GetOpposite(RoomDirection direction) // 반대 방향을 반환하는 함수
    {
        if (direction == RoomDirection.Up) // 위쪽 확인
        {
            return RoomDirection.Down; // 아래쪽 반환
        }

        if (direction == RoomDirection.Down) // 아래쪽 확인
        {
            return RoomDirection.Up; // 위쪽 반환
        }

        if (direction == RoomDirection.Left) // 왼쪽 확인
        {
            return RoomDirection.Right; // 오른쪽 반환
        }

        return RoomDirection.Left; // 왼쪽 반환
    }
}