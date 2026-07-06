using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab; // 생성할 적 프리팹을 저장한다.
    public Transform[] spawnPoints; // 적이 생성될 위치들을 저장한다.

    [Header("Difficulty Settings")]
    public int baseEnemyCount = 3; // 첫 전투의 기본 적 수를 저장한다.
    public int enemyCountIncreasePerBattle = 1; // 전투마다 증가할 적 수를 저장한다.
    public int baseEnemyHealth = 50; // 첫 전투의 기본 적 체력을 저장한다.
    public int enemyHealthIncreasePerBattle = 10; // 전투마다 증가할 적 체력을 저장한다.

    public void SpawnEnemies(BattleManager battleManager, int battleNumber) // 전투 번호에 맞춰 적들을 생성한다.
    {
        if (battleManager == null) // BattleManager가 전달되지 않았는지 확인한다.
        {
            Debug.LogError("BattleManager is not assigned."); // 오류 로그를 출력한다.
            return; // 적 생성을 중단한다.
        }

        if (enemyPrefab == null) // 적 프리팹이 연결되지 않았는지 확인한다.
        {
            Debug.LogError("Enemy Prefab is not assigned."); // 오류 로그를 출력한다.
            return; // 적 생성을 중단한다.
        }

        if (spawnPoints == null || spawnPoints.Length == 0) // 스폰 포인트가 없는지 확인한다.
        {
            Debug.LogError("Spawn Points are not assigned."); // 오류 로그를 출력한다.
            return; // 적 생성을 중단한다.
        }

        int enemyCount = GetEnemyCount(battleNumber); // 전투 번호에 따른 적 수를 계산한다.
        int enemyHealth = GetEnemyHealth(battleNumber); // 전투 번호에 따른 적 체력을 계산한다.

        Debug.Log("Spawn Enemy Count : " + enemyCount); // 생성할 적 수를 로그로 출력한다.
        Debug.Log("Enemy HP For This Battle : " + enemyHealth); // 이번 전투 적 체력을 로그로 출력한다.

        for (int i = 0; i < enemyCount; i++) // 생성할 적 수만큼 반복한다.
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length]; // 스폰 포인트를 순서대로 반복 사용한다.

            if (spawnPoint == null) // 현재 스폰 포인트가 비어 있는지 확인한다.
            {
                Debug.LogWarning("Empty SpawnPoint. Index : " + i); // 비어 있는 스폰 포인트 경고를 출력한다.
                continue; // 다음 생성으로 넘어간다.
            }

            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity); // 적 프리팹을 생성한다.
            enemyObject.name = enemyPrefab.name + "_Battle" + battleNumber + "_" + (i + 1); // 생성된 적 이름을 정리한다.

            EnemyHealth enemyHealthComponent = enemyObject.GetComponent<EnemyHealth>(); // 생성된 적에서 EnemyHealth를 가져온다.

            if (enemyHealthComponent == null) // EnemyHealth가 없는지 확인한다.
            {
                Debug.LogError(enemyObject.name + " has no EnemyHealth."); // 오류 로그를 출력한다.
                continue; // 등록하지 않고 다음 적으로 넘어간다.
            }

            enemyHealthComponent.SetMaxHealth(enemyHealth); // 이번 전투에 맞는 체력을 설정한다.
            battleManager.RegisterEnemy(enemyHealthComponent); // 생성된 적을 BattleManager에 등록한다.
        }
    }

    private int GetEnemyCount(int battleNumber) // 전투 번호에 따른 적 수를 계산한다.
    {
        int count = baseEnemyCount + ((battleNumber - 1) * enemyCountIncreasePerBattle); // 전투 번호에 따라 적 수를 증가시킨다.
        return Mathf.Max(count, 1); // 적 수가 최소 1 이상이 되도록 한다.
    }

    private int GetEnemyHealth(int battleNumber) // 전투 번호에 따른 적 체력을 계산한다.
    {
        int health = baseEnemyHealth + ((battleNumber - 1) * enemyHealthIncreasePerBattle); // 전투 번호에 따라 적 체력을 증가시킨다.
        return Mathf.Max(health, 1); // 체력이 최소 1 이상이 되도록 한다.
    }
}