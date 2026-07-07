using System.Collections; // 코루틴 사용
using UnityEngine; // Unity 기본 기능 사용

public class BossController : MonoBehaviour // 보스 행동 관리 클래스
{
    [Header("Reference Settings")] // 참조 설정 제목
    public Transform firePoint; // 탄환 발사 위치

    public GameObject bossBulletPrefab; // 보스 탄환 프리팹

    private Transform player; // 플레이어 위치

    private EnemyHealth enemyHealth; // 보스 체력 정보

    [Header("Pattern Time Settings")] // 패턴 시간 설정 제목
    public float normalPatternInterval = 2.5f; // 일반 패턴 간격

    public float enragedPatternInterval = 1.5f; // 강화 패턴 간격

    private float patternTimer = 0f; // 패턴 타이머

    private int patternIndex = 0; // 현재 패턴 번호

    private bool isAttacking = false; // 공격 중 여부

    [Header("Circle Pattern Settings")] // 원형 탄막 설정 제목
    public int circleBulletCount = 16; // 원형 탄환 개수

    [Header("Fan Pattern Settings")] // 부채꼴 탄막 설정 제목
    public int fanBulletCount = 7; // 부채꼴 탄환 개수

    public float fanAngle = 70f; // 부채꼴 각도

    [Header("Aimed Burst Settings")] // 조준 연사 설정 제목
    public int aimedBurstCount = 4; // 조준 연사 횟수

    public float aimedBurstDelay = 0.2f; // 조준 연사 간격

    private void Awake() // 초기화 함수
    {
        enemyHealth = GetComponent<EnemyHealth>(); // 보스 체력 컴포넌트 가져오기
    }

    private void Start() // 시작 함수
    {
        FindPlayer(); // 플레이어 검색
    }

    private void Update() // 매 프레임 갱신 함수
    {
        if (bossBulletPrefab == null) // 탄환 프리팹 확인
        {
            return; // 패턴 실행 중단
        }

        if (firePoint == null) // 발사 위치 확인
        {
            return; // 패턴 실행 중단
        }

        if (player == null) // 플레이어 확인
        {
            FindPlayer(); // 플레이어 재검색
            return; // 이번 프레임 중단
        }

        if (isAttacking) // 공격 중인지 확인
        {
            return; // 중복 패턴 방지
        }

        patternTimer += Time.deltaTime; // 패턴 타이머 증가

        if (patternTimer >= GetCurrentPatternInterval()) // 패턴 실행 시간 확인
        {
            patternTimer = 0f; // 타이머 초기화
            ExecuteNextPattern(); // 다음 패턴 실행
        }
    }

    private void FindPlayer() // 플레이어 검색 함수
    {
        PlayerStats playerStats = FindFirstObjectByType<PlayerStats>(); // 플레이어 스탯 검색

        if (playerStats == null) // 플레이어 없음 확인
        {
            return; // 검색 중단
        }

        player = playerStats.transform; // 플레이어 위치 저장
    }

    private float GetCurrentPatternInterval() // 현재 패턴 간격 반환 함수
    {
        if (enemyHealth == null) // 체력 컴포넌트 없음 확인
        {
            return normalPatternInterval; // 일반 간격 반환
        }

        if (enemyHealth.currentHealth <= enemyHealth.maxHealth * 0.5f) // 체력 50퍼센트 이하 확인
        {
            return enragedPatternInterval; // 강화 간격 반환
        }

        return normalPatternInterval; // 일반 간격 반환
    }

    private void ExecuteNextPattern() // 다음 패턴 실행 함수
    {
        if (patternIndex == 0) // 첫 번째 패턴 확인
        {
            FireCirclePattern(); // 원형 탄막 실행
        }
        else if (patternIndex == 1) // 두 번째 패턴 확인
        {
            FireFanPattern(); // 부채꼴 탄막 실행
        }
        else // 세 번째 패턴 처리
        {
            StartCoroutine(FireAimedBurstPattern()); // 조준 연사 실행
        }

        patternIndex++; // 패턴 번호 증가

        if (patternIndex > 2) // 마지막 패턴 이후 확인
        {
            patternIndex = 0; // 첫 패턴으로 순환
        }
    }

    private void FireCirclePattern() // 원형 탄막 함수
    {
        for (int i = 0; i < circleBulletCount; i++) // 탄환 개수만큼 반복
        {
            float angle = 360f / circleBulletCount * i; // 탄환 각도 계산

            Vector2 direction = GetDirectionFromAngle(angle); // 각도 방향 계산

            SpawnBullet(direction); // 탄환 생성
        }

        Debug.Log("Boss Pattern : Circle"); // 패턴 로그
    }

    private void FireFanPattern() // 부채꼴 탄막 함수
    {
        Vector2 directionToPlayer = player.position - firePoint.position; // 플레이어 방향 계산

        float centerAngle = GetAngleFromDirection(directionToPlayer); // 중심 각도 계산

        float startAngle = centerAngle - fanAngle * 0.5f; // 시작 각도 계산

        float angleStep = fanBulletCount <= 1 ? 0f : fanAngle / (fanBulletCount - 1); // 각도 간격 계산

        for (int i = 0; i < fanBulletCount; i++) // 탄환 개수만큼 반복
        {
            float angle = startAngle + angleStep * i; // 현재 탄환 각도 계산

            Vector2 direction = GetDirectionFromAngle(angle); // 각도 방향 계산

            SpawnBullet(direction); // 탄환 생성
        }

        Debug.Log("Boss Pattern : Fan"); // 패턴 로그
    }

    private IEnumerator FireAimedBurstPattern() // 조준 연사 탄막 함수
    {
        isAttacking = true; // 공격 중 설정

        for (int i = 0; i < aimedBurstCount; i++) // 연사 횟수만큼 반복
        {
            if (player == null) // 플레이어 없음 확인
            {
                break; // 연사 중단
            }

            Vector2 direction = player.position - firePoint.position; // 플레이어 방향 계산

            SpawnBullet(direction); // 탄환 생성

            yield return new WaitForSeconds(aimedBurstDelay); // 연사 간격 대기
        }

        isAttacking = false; // 공격 중 해제

        Debug.Log("Boss Pattern : Aimed Burst"); // 패턴 로그
    }

    private void SpawnBullet(Vector2 direction) // 탄환 생성 함수
    {
        GameObject bulletObject = Instantiate(bossBulletPrefab, firePoint.position, Quaternion.identity); // 탄환 생성

        BossBullet bossBullet = bulletObject.GetComponent<BossBullet>(); // 보스 탄환 컴포넌트 가져오기

        if (bossBullet == null) // 보스 탄환 컴포넌트 확인
        {
            Debug.LogError("BossBullet component is missing."); // 오류 로그
            Destroy(bulletObject); // 잘못된 탄환 삭제
            return; // 생성 처리 중단
        }

        bossBullet.SetDirection(direction); // 탄환 방향 설정
    }

    private Vector2 GetDirectionFromAngle(float angle) // 각도로 방향 계산 함수
    {
        float radian = angle * Mathf.Deg2Rad; // 라디안 변환

        float x = Mathf.Cos(radian); // X 방향 계산

        float y = Mathf.Sin(radian); // Y 방향 계산

        return new Vector2(x, y).normalized; // 방향 반환
    }

    private float GetAngleFromDirection(Vector2 direction) // 방향으로 각도 계산 함수
    {
        Vector2 normalizedDirection = direction.normalized; // 방향 정규화

        float angle = Mathf.Atan2(normalizedDirection.y, normalizedDirection.x) * Mathf.Rad2Deg; // 각도 계산

        return angle; // 각도 반환
    }
}