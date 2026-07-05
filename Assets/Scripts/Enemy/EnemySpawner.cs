using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab; // 생성할 적 프리팹을 저장한다.
    public Transform[] spawnPoints; // 적이 생성될 위치들을 저장한다.

    public void SpawnEnemies(BattleManager battleManager) // 적들을 생성한다.
    {
        if (battleManager == null) // BattleManager가 전달되지 않았는지 확인한다.
        {
            Debug.LogError("BattleManager가 전달되지 않았습니다."); // 오류 로그를 출력한다.
            return; // 적 생성을 중단한다.
        }

        if (enemyPrefab == null) // 적 프리팹이 연결되지 않았는지 확인한다.
        {
            Debug.LogError("Enemy Prefab이 연결되지 않았습니다."); // 오류 로그를 출력한다.
            return; // 적 생성을 중단한다.
        }

        if (spawnPoints == null || spawnPoints.Length == 0) // 스폰 포인트가 없는지 확인한다.
        {
            Debug.LogError("SpawnPoint가 설정되지 않았습니다."); // 오류 로그를 출력한다.
            return; // 적 생성을 중단한다.
        }

        for (int i = 0; i < spawnPoints.Length; i++) // 스폰 포인트 수만큼 반복한다.
        {
            Transform spawnPoint = spawnPoints[i]; // 현재 스폰 포인트를 가져온다.

            if (spawnPoint == null) // 현재 스폰 포인트가 비어 있는지 확인한다.
            {
                Debug.LogWarning("비어 있는 SpawnPoint가 있습니다. Index : " + i); // 경고 로그를 출력한다.
                continue; // 다음 스폰 포인트로 넘어간다.
            }

            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity); // 적 프리팹을 생성한다.
            enemyObject.name = enemyPrefab.name + "_" + (i + 1); // 생성된 적 이름을 정리한다.

            EnemyHealth enemyHealth = enemyObject.GetComponent<EnemyHealth>(); // 생성된 적에서 EnemyHealth를 가져온다.

            if (enemyHealth == null) // EnemyHealth가 없는지 확인한다.
            {
                Debug.LogError(enemyObject.name + "에 EnemyHealth가 없습니다."); // 오류 로그를 출력한다.
                continue; // 등록하지 않고 다음 적으로 넘어간다.
            }

            battleManager.RegisterEnemy(enemyHealth); // 생성된 적을 BattleManager에 등록한다.
        }
    }
}