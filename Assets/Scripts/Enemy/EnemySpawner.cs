using System.Collections.Generic; // 리스트 사용
using UnityEngine; // Unity 기본 기능 사용

public class EnemySpawner : MonoBehaviour // 적 생성 관리 클래스
{
    [Header("Spawn Settings")] // 스폰 설정 제목
    public GameObject enemyPrefab; // 기본 적 프리팹

    public GameObject[] enemyPrefabs; // 여러 적 프리팹 배열

    public Transform[] spawnPoints; // 일반 적 생성 위치 배열

    [Header("Battle Type Settings")] // 전투 타입 설정 제목
    public BattleType currentBattleType = BattleType.Normal; // 현재 전투 타입

    public float eliteEnemyCountMultiplier = 1.3f; // 엘리트 전투 적 수 배율

    public float eliteEnemyHealthMultiplier = 1.5f; // 엘리트 전투 적 체력 배율

    public float bossEnemyCountMultiplier = 0.7f; // 보스 전투 임시 적 수 배율

    public float bossEnemyHealthMultiplier = 2.5f; // 보스 전투 임시 적 체력 배율

    [Header("Difficulty Settings")] // 난이도 설정 제목
    public int baseEnemyCount = 3; // 기본 적 수

    public int enemyCountIncreasePerBattle = 1; // 전투마다 증가할 적 수

    public int baseEnemyHealth = 50; // 기본 적 체력

    public int enemyHealthIncreasePerBattle = 10; // 전투마다 증가할 적 체력

    [Header("Boss Settings")] // 보스 설정 제목
    public GameObject bossPrefab; // 보스 프리팹

    public Transform bossSpawnPoint; // 보스 생성 위치

    public int bossMaxHealth = 500; // 보스 최대 체력

    public string bossDisplayName = "BOSS"; // 보스 UI 표시 이름

    public BossHealthUIController bossHealthUIController; // 보스 체력 UI 컨트롤러

    private void Awake() // 초기화 함수
    {
        FindBossHealthUIIfNeeded(); // 보스 체력 UI 자동 검색
    }

    public void SpawnEnemies(BattleManager battleManager, int battleNumber) // 전투 번호에 맞춰 적 생성 함수
    {
        if (battleManager == null) // BattleManager 연결 확인
        {
            Debug.LogError("BattleManager is not assigned."); // 오류 로그 출력
            return; // 적 생성 중단
        }

        FindBossHealthUIIfNeeded(); // 보스 체력 UI 자동 검색

        if (bossHealthUIController != null) // 보스 체력 UI 확인
        {
            bossHealthUIController.Hide(); // 전투 시작 시 보스 체력 UI 숨김
        }

        if (currentBattleType == BattleType.Boss) // 보스 전투 확인
        {
            SpawnBoss(battleManager); // 보스 생성
            return; // 일반 적 생성 중단
        }

        if (spawnPoints == null || spawnPoints.Length == 0) // 스폰 포인트 확인
        {
            Debug.LogError("Spawn Points are not assigned."); // 오류 로그 출력
            return; // 적 생성 중단
        }

        int enemyCount = GetEnemyCount(battleNumber); // 전투 번호별 적 수 계산

        int enemyHealth = GetEnemyHealth(battleNumber); // 전투 번호별 적 체력 계산

        Debug.Log("Current Battle Type : " + currentBattleType); // 현재 전투 타입 로그

        Debug.Log("Spawn Enemy Count : " + enemyCount); // 적 수 로그

        Debug.Log("Enemy HP For This Battle : " + enemyHealth); // 적 체력 로그

        for (int i = 0; i < enemyCount; i++) // 적 수만큼 반복
        {
            GameObject selectedPrefab = GetEnemyPrefab(battleNumber); // 생성할 적 프리팹 선택

            if (selectedPrefab == null) // 적 프리팹 확인
            {
                Debug.LogError("Enemy Prefab is not assigned."); // 오류 로그 출력
                continue; // 이번 생성 건너뛰기
            }

            Transform spawnPoint = spawnPoints[i % spawnPoints.Length]; // 사용할 스폰 위치 선택

            if (spawnPoint == null) // 스폰 위치 확인
            {
                Debug.LogWarning("Empty SpawnPoint. Index : " + i); // 경고 로그 출력
                continue; // 이번 생성 건너뛰기
            }

            GameObject enemyObject = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity); // 적 오브젝트 생성

            enemyObject.name = selectedPrefab.name + "_Battle" + battleNumber + "_" + (i + 1); // 적 이름 설정

            EnemyHealth enemyHealthComponent = enemyObject.GetComponent<EnemyHealth>(); // 적 체력 컴포넌트 가져오기

            if (enemyHealthComponent == null) // 적 체력 컴포넌트 확인
            {
                Debug.LogError(enemyObject.name + " has no EnemyHealth."); // 오류 로그 출력
                continue; // 등록 건너뛰기
            }

            enemyHealthComponent.SetMaxHealth(enemyHealth); // 적 최대 체력 설정

            battleManager.RegisterEnemy(enemyHealthComponent); // BattleManager에 적 등록
        }
    }

    public void SetBattleType(BattleType newBattleType) // 전투 타입 설정 함수
    {
        currentBattleType = newBattleType; // 전투 타입 저장

        Debug.Log("EnemySpawner Battle Type Set : " + currentBattleType); // 전투 타입 로그
    }

    private void SpawnBoss(BattleManager battleManager) // 보스 생성 함수
    {
        if (bossPrefab == null) // 보스 프리팹 확인
        {
            Debug.LogError("Boss Prefab is not assigned."); // 오류 로그 출력
            return; // 보스 생성 중단
        }

        if (bossSpawnPoint == null) // 보스 생성 위치 확인
        {
            Debug.LogError("Boss Spawn Point is not assigned."); // 오류 로그 출력
            return; // 보스 생성 중단
        }

        GameObject bossObject = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity); // 보스 오브젝트 생성

        bossObject.name = bossPrefab.name + "_Boss"; // 보스 이름 설정

        EnemyHealth bossHealth = bossObject.GetComponent<EnemyHealth>(); // 보스 체력 컴포넌트 가져오기

        if (bossHealth == null) // 보스 체력 컴포넌트 확인
        {
            Debug.LogError(bossObject.name + " has no EnemyHealth."); // 오류 로그 출력
            return; // 보스 등록 중단
        }

        bossHealth.SetMaxHealth(bossMaxHealth); // 보스 최대 체력 설정

        battleManager.RegisterEnemy(bossHealth); // BattleManager에 보스 등록

        FindBossHealthUIIfNeeded(); // 보스 체력 UI 자동 검색

        if (bossHealthUIController != null) // 보스 체력 UI 확인
        {
            bossHealthUIController.Show(bossHealth, bossDisplayName); // 보스 체력 UI 표시
        }
        else // 보스 체력 UI 없음
        {
            Debug.LogWarning("Boss Health UI Controller is not assigned."); // 경고 로그 출력
        }

        Debug.Log("Boss Spawned : " + bossObject.name); // 보스 생성 로그
    }

    private void FindBossHealthUIIfNeeded() // 보스 체력 UI 자동 검색 함수
    {
        if (bossHealthUIController != null) // 이미 연결된 경우 확인
        {
            return; // 검색 중단
        }

        bossHealthUIController = FindFirstObjectByType<BossHealthUIController>(); // 씬에서 보스 UI 검색
    }

    private GameObject GetEnemyPrefab(int battleNumber) // 전투 번호별 적 프리팹 선택 함수
    {
        List<GameObject> availablePrefabs = new List<GameObject>(); // 사용 가능한 적 목록 생성

        if (enemyPrefabs != null && enemyPrefabs.Length > 0) // 여러 적 프리팹 확인
        {
            int unlockedCount = Mathf.Clamp(battleNumber, 1, enemyPrefabs.Length); // 해금된 적 수 계산

            for (int i = 0; i < unlockedCount; i++) // 해금된 적 수만큼 반복
            {
                if (enemyPrefabs[i] != null) // 적 프리팹 확인
                {
                    availablePrefabs.Add(enemyPrefabs[i]); // 사용 가능 목록 추가
                }
            }
        }

        if (availablePrefabs.Count > 0) // 사용 가능 적 존재 확인
        {
            int randomIndex = Random.Range(0, availablePrefabs.Count); // 랜덤 인덱스 선택

            return availablePrefabs[randomIndex]; // 랜덤 적 반환
        }

        return enemyPrefab; // 기본 적 반환
    }

    private int GetEnemyCount(int battleNumber) // 전투 번호별 적 수 계산 함수
    {
        int count = baseEnemyCount + ((battleNumber - 1) * enemyCountIncreasePerBattle); // 기본 적 수 계산

        count = ApplyBattleTypeEnemyCount(count); // 전투 타입별 적 수 적용

        return Mathf.Max(count, 1); // 최소 적 수 보정
    }

    private int GetEnemyHealth(int battleNumber) // 전투 번호별 적 체력 계산 함수
    {
        int health = baseEnemyHealth + ((battleNumber - 1) * enemyHealthIncreasePerBattle); // 기본 체력 계산

        health = ApplyBattleTypeEnemyHealth(health); // 전투 타입별 체력 적용

        return Mathf.Max(health, 1); // 최소 체력 보정
    }

    private int ApplyBattleTypeEnemyCount(int baseCount) // 전투 타입별 적 수 적용 함수
    {
        if (currentBattleType == BattleType.Elite) // 엘리트 전투 확인
        {
            return Mathf.Max(1, Mathf.RoundToInt(baseCount * eliteEnemyCountMultiplier)); // 엘리트 적 수 반환
        }

        if (currentBattleType == BattleType.Boss) // 보스 전투 확인
        {
            return 1; // 보스 전투 적 수 반환
        }

        return baseCount; // 일반 전투 적 수 반환
    }

    private int ApplyBattleTypeEnemyHealth(int baseHealth) // 전투 타입별 적 체력 적용 함수
    {
        if (currentBattleType == BattleType.Elite) // 엘리트 전투 확인
        {
            return Mathf.Max(1, Mathf.RoundToInt(baseHealth * eliteEnemyHealthMultiplier)); // 엘리트 체력 반환
        }

        if (currentBattleType == BattleType.Boss) // 보스 전투 확인
        {
            return bossMaxHealth; // 보스 체력 반환
        }

        return baseHealth; // 일반 전투 체력 반환
    }
}