using UnityEngine;

public class BattleSceneStarter : MonoBehaviour
{
    [Header("Manager References")]
    public BattleManager battleManager; // 전투 시작을 담당할 BattleManager를 저장한다.
    public EnemySpawner enemySpawner; // 전투 타입을 적용할 EnemySpawner를 저장한다.
    public RelicManager relicManager; // 전투 시작 유물 효과를 적용할 RelicManager를 저장한다.

    private void Start() // 전투 씬이 시작될 때 실행한다.
    {
        FindReferencesIfNeeded(); // 필요한 참조를 찾는다.
        StartSelectedBattle(); // 선택된 노드에 맞는 전투를 시작한다.
    }

    private void FindReferencesIfNeeded() // 필요한 참조를 자동으로 찾는다.
    {
        if (battleManager == null) // BattleManager가 연결되지 않았는지 확인한다.
        {
            battleManager = FindFirstObjectByType<BattleManager>(); // 씬에서 BattleManager를 찾는다.
        }

        if (enemySpawner == null) // EnemySpawner가 연결되지 않았는지 확인한다.
        {
            enemySpawner = FindFirstObjectByType<EnemySpawner>(); // 씬에서 EnemySpawner를 찾는다.
        }

        if (relicManager == null) // RelicManager가 연결되지 않았는지 확인한다.
        {
            relicManager = FindFirstObjectByType<RelicManager>(); // 씬에서 RelicManager를 찾는다.
        }
    }

    private void StartSelectedBattle() // GameFlowManager가 가진 선택 노드 정보로 전투를 시작한다.
    {
        if (GameFlowManager.Instance == null) // GameFlowManager가 없는지 확인한다.
        {
            Debug.LogWarning("GameFlowManager is missing. BattleSceneStarter stopped."); // 경고 로그를 출력한다.
            return; // 전투 시작을 중단한다.
        }

        if (enemySpawner != null) // EnemySpawner가 연결되어 있는지 확인한다.
        {
            enemySpawner.SetBattleType(GameFlowManager.Instance.selectedBattleType); // 선택된 전투 타입을 적용한다.
        }

        if (relicManager != null) // RelicManager가 연결되어 있는지 확인한다.
        {
            relicManager.ApplyRelicsOnBattleStart(); // 전투 시작 유물 효과를 적용한다.
        }

        if (battleManager == null) // BattleManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("BattleManager is not assigned."); // 오류 로그를 출력한다.
            return; // 전투 시작을 중단한다.
        }

        Debug.Log("Battle Scene Start : " + GameFlowManager.Instance.selectedNodeType + " / " + GameFlowManager.Instance.selectedBattleType); // 시작 전투 정보를 출력한다.
        battleManager.StartBattle(GameFlowManager.Instance.selectedBattleNumber); // 선택된 전투 번호로 전투를 시작한다.
    }
}