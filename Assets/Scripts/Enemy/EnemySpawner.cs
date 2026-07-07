using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab; // 기존 단일 적 프리팹을 저장한다.
    public GameObject[] enemyPrefabs; // 여러 종류의 적 프리팹을 저장한다.
    public Transform[] spawnPoints; // 적이 생성될 위치들을 저장한다.

    [Header("Battle Type Settings")]
    public BattleType currentBattleType = BattleType.Normal; // 현재 전투 타입을 저장한다.
    public float eliteEnemyCountMultiplier = 1.3f; // 엘리트 전투의 적 수 배율을 저장한다.
    public float eliteEnemyHealthMultiplier = 1.5f; // 엘리트 전투의 적 체력 배율을 저장한다.
    public float bossEnemyCountMultiplier = 0.7f; // 보스 전투의 임시 적 수 배율을 저장한다.
    public float bossEnemyHealthMultiplier = 2.5f; // 보스 전투의 임시 적 체력 배율을 저장한다.

    [Header("Difficulty Settings")]
    public int baseEnemyCount = 3; // 첫 전투의 기본 적 수를 저장한다.
    public int enemyCountIncreasePerBattle = 1; // 전투마다 증가할 적 수를 저장한다.
    public int baseEnemyHealth = 50; // 첫 전투의 기본 적 체력을 저장한다.
    public int enemyHealthIncreasePerBattle = 10; // 전투마다 증가할 적 체력을 저장한다.

    public void SpawnEnemies(BattleManager battleManager, int battleNumber) // 전투 번호에 맞춰 적을 생성한다.
    {
        if (battleManager == null) // BattleManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("BattleManager is not assigned."); // 오류 로그를 출력한다.
            return; // 적 생성을 중단한다.
        }

        if (spawnPoints == null || spawnPoints.Length == 0) // 스폰 포인트가 없는지 확인한다.
        {
            Debug.LogError("Spawn Points are not assigned."); // 오류 로그를 출력한다.
            return; // 적 생성을 중단한다.
        }

        int enemyCount = GetEnemyCount(battleNumber); // 전투 번호와 전투 타입에 맞는 적 수를 계산한다.
        int enemyHealth = GetEnemyHealth(battleNumber); // 전투 번호와 전투 타입에 맞는 적 체력을 계산한다.

        Debug.Log("Current Battle Type : " + currentBattleType); // 현재 전투 타입을 로그로 출력한다.
        Debug.Log("Spawn Enemy Count : " + enemyCount); // 생성할 적 수를 로그로 출력한다.
        Debug.Log("Enemy HP For This Battle : " + enemyHealth); // 이번 전투 적 체력을 로그로 출력한다.

        for (int i = 0; i < enemyCount; i++) // 생성할 적 수만큼 반복한다.
        {
            GameObject selectedPrefab = GetEnemyPrefab(battleNumber); // 이번에 생성할 적 프리팹을 가져온다.

            if (selectedPrefab == null) // 선택된 프리팹이 없는지 확인한다.
            {
                Debug.LogError("Enemy Prefab is not assigned."); // 오류 로그를 출력한다.
                continue; // 이번 적 생성을 건너뛴다.
            }

            Transform spawnPoint = spawnPoints[i % spawnPoints.Length]; // 사용할 스폰 포인트를 선택한다.

            if (spawnPoint == null) // 스폰 포인트가 비어 있는지 확인한다.
            {
                Debug.LogWarning("Empty SpawnPoint. Index : " + i); // 경고 로그를 출력한다.
                continue; // 이번 적 생성을 건너뛴다.
            }

            GameObject enemyObject = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity); // 적을 생성한다.
            enemyObject.name = selectedPrefab.name + "_Battle" + battleNumber + "_" + (i + 1); // 생성된 적 이름을 설정한다.

            EnemyHealth enemyHealthComponent = enemyObject.GetComponent<EnemyHealth>(); // 적 체력 컴포넌트를 가져온다.

            if (enemyHealthComponent == null) // EnemyHealth가 없는지 확인한다.
            {
                Debug.LogError(enemyObject.name + " has no EnemyHealth."); // 오류 로그를 출력한다.
                continue; // 등록을 건너뛴다.
            }

            enemyHealthComponent.SetMaxHealth(enemyHealth); // 전투 번호와 전투 타입에 맞는 체력을 설정한다.
            battleManager.RegisterEnemy(enemyHealthComponent); // BattleManager에 적을 등록한다.
        }
    }

    public void SetBattleType(BattleType newBattleType) // 현재 전투 타입을 설정한다.
    {
        currentBattleType = newBattleType; // 전달받은 전투 타입을 저장한다.
        Debug.Log("EnemySpawner Battle Type Set : " + currentBattleType); // 전투 타입 변경 로그를 출력한다.
    }

    private GameObject GetEnemyPrefab(int battleNumber) // 전투 번호에 따라 사용할 수 있는 적 프리팹 중 하나를 고른다.
    {
        List<GameObject> availablePrefabs = new List<GameObject>(); // 이번 전투에서 사용할 수 있는 적 목록을 만든다.

        if (enemyPrefabs != null && enemyPrefabs.Length > 0) // 여러 적 프리팹 배열이 있는지 확인한다.
        {
            int unlockedCount = Mathf.Clamp(battleNumber, 1, enemyPrefabs.Length); // 전투 번호에 따라 해금된 적 수를 계산한다.

            for (int i = 0; i < unlockedCount; i++) // 해금된 적 수만큼 반복한다.
            {
                if (enemyPrefabs[i] != null) // 프리팹이 비어 있지 않은지 확인한다.
                {
                    availablePrefabs.Add(enemyPrefabs[i]); // 사용 가능 목록에 추가한다.
                }
            }
        }

        if (availablePrefabs.Count > 0) // 사용 가능한 적이 있는지 확인한다.
        {
            int randomIndex = Random.Range(0, availablePrefabs.Count); // 랜덤 인덱스를 뽑는다.
            return availablePrefabs[randomIndex]; // 랜덤 적 프리팹을 반환한다.
        }

        return enemyPrefab; // 여러 프리팹이 없으면 기존 단일 프리팹을 반환한다.
    }

    private int GetEnemyCount(int battleNumber) // 전투 번호와 전투 타입에 따른 적 수를 계산한다.
    {
        int count = baseEnemyCount + ((battleNumber - 1) * enemyCountIncreasePerBattle); // 기본 적 수를 계산한다.
        count = ApplyBattleTypeEnemyCount(count); // 전투 타입에 따라 적 수를 조정한다.
        return Mathf.Max(count, 1); // 최소 1마리는 나오도록 반환한다.
    }

    private int GetEnemyHealth(int battleNumber) // 전투 번호와 전투 타입에 따른 적 체력을 계산한다.
    {
        int health = baseEnemyHealth + ((battleNumber - 1) * enemyHealthIncreasePerBattle); // 기본 적 체력을 계산한다.
        health = ApplyBattleTypeEnemyHealth(health); // 전투 타입에 따라 적 체력을 조정한다.
        return Mathf.Max(health, 1); // 최소 체력 1 이상으로 반환한다.
    }

    private int ApplyBattleTypeEnemyCount(int baseEnemyCount) // 전투 타입에 따라 적 수를 조정한다.
    {
        if (currentBattleType == BattleType.Elite) // 엘리트 전투인지 확인한다.
        {
            return Mathf.Max(1, Mathf.RoundToInt(baseEnemyCount * eliteEnemyCountMultiplier)); // 엘리트 적 수를 반환한다.
        }

        if (currentBattleType == BattleType.Boss) // 보스 전투인지 확인한다.
        {
            return Mathf.Max(1, Mathf.RoundToInt(baseEnemyCount * bossEnemyCountMultiplier)); // 보스 전투 임시 적 수를 반환한다.
        }

        return baseEnemyCount; // 일반 전투는 기본 적 수를 그대로 반환한다.
    }

    private int ApplyBattleTypeEnemyHealth(int baseEnemyHealth) // 전투 타입에 따라 적 체력을 조정한다.
    {
        if (currentBattleType == BattleType.Elite) // 엘리트 전투인지 확인한다.
        {
            return Mathf.Max(1, Mathf.RoundToInt(baseEnemyHealth * eliteEnemyHealthMultiplier)); // 엘리트 적 체력을 반환한다.
        }

        if (currentBattleType == BattleType.Boss) // 보스 전투인지 확인한다.
        {
            return Mathf.Max(1, Mathf.RoundToInt(baseEnemyHealth * bossEnemyHealthMultiplier)); // 보스 전투 임시 적 체력을 반환한다.
        }

        return baseEnemyHealth; // 일반 전투는 기본 체력을 그대로 반환한다.
    }
}