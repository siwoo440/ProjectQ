public enum EnemyAttackPattern
{
    Single, // 플레이어 방향으로 탄환 1발을 발사하는 패턴이다.
    Burst, // 짧은 간격으로 여러 발을 연속 발사하는 패턴이다.
    Spread, // 플레이어 방향을 기준으로 여러 발을 부채꼴로 발사하는 패턴이다.
    Circle // 적 중심에서 모든 방향으로 탄환을 발사하는 패턴이다.
}