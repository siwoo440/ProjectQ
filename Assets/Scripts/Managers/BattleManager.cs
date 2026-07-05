using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum BattleState // 전투 상태 종류를 정의한다.
    {
        Ready, // 전투 시작 전 상태다.
        Battle, // 전투 진행 중 상태다.
        Clear, // 전투 클리어 상태다.
        Failed // 전투 실패 상태다.
    }

    [Header("Battle References")]
    public EnemySpawner enemySpawner; // 적 생성 관리자를 저장한다.
    public BattleClearUI battleClearUI; // 전투 클리어 UI를 저장한다.
    public RewardManager rewardManager; // 보상 관리자를 저장한다.

    [Header("Battle State")]
    public BattleState currentState = BattleState.Ready; // 현재 전투 상태를 저장한다.
    public int aliveEnemyCount = 0; // 현재 살아있는 적 수를 저장한다.

    private void Start() // 게임 시작 시 전투를 시작한다.
    {
        if (battleClearUI != null) // 클리어 UI가 연결되어 있는지 확인한다.
        {
            battleClearUI.Hide(); // 시작 시 클리어 UI를 숨긴다.
        }

        StartBattle(); // 전투를 시작한다.
    }

    public void StartBattle() // 전투 시작 처리를 실행한다.
    {
        if (currentState == BattleState.Battle) return; // 이미 전투 중이면 다시 시작하지 않는다.

        currentState = BattleState.Battle; // 현재 상태를 전투 중으로 변경한다.
        aliveEnemyCount = 0; // 살아있는 적 수를 초기화한다.

        Debug.Log("Battle Start"); // 전투 시작 로그를 출력한다.

        if (battleClearUI != null) // 클리어 UI가 연결되어 있는지 확인한다.
        {
            battleClearUI.Hide(); // 전투 시작 시 클리어 UI를 숨긴다.
        }

        if (enemySpawner == null) // 적 생성 관리자가 없는지 확인한다.
        {
            Debug.LogError("EnemySpawner가 BattleManager에 연결되지 않았습니다."); // 오류 로그를 출력한다.
            return; // 전투 시작 처리를 중단한다.
        }

        enemySpawner.SpawnEnemies(this); // 적 생성 관리자에게 적 생성을 요청한다.

        if (aliveEnemyCount <= 0) // 생성된 적이 없는지 확인한다.
        {
            Debug.LogWarning("생성된 적이 없습니다."); // 경고 로그를 출력한다.
        }
    }

    public void RegisterEnemy(EnemyHealth enemyHealth) // 생성된 적을 전투 관리 대상에 등록한다.
    {
        if (enemyHealth == null) return; // 적 체력 컴포넌트가 없으면 실행하지 않는다.

        aliveEnemyCount += 1; // 살아있는 적 수를 1 증가시킨다.
        enemyHealth.SetBattleManager(this); // 적에게 BattleManager를 알려준다.

        Debug.Log("Enemy Registered. Alive Enemy Count : " + aliveEnemyCount); // 현재 적 수를 로그로 출력한다.
    }

    public void OnEnemyDead(EnemyHealth enemyHealth) // 적이 죽었을 때 호출된다.
    {
        if (currentState != BattleState.Battle) return; // 전투 중이 아니면 처리하지 않는다.

        aliveEnemyCount -= 1; // 살아있는 적 수를 1 감소시킨다.
        aliveEnemyCount = Mathf.Max(aliveEnemyCount, 0); // 적 수가 0 아래로 내려가지 않게 한다.

        Debug.Log("Enemy Dead. Alive Enemy Count : " + aliveEnemyCount); // 남은 적 수를 로그로 출력한다.

        CheckBattleClear(); // 전투 클리어 여부를 확인한다.
    }

    private void CheckBattleClear() // 전투 클리어 조건을 확인한다.
    {
        if (aliveEnemyCount > 0) return; // 살아있는 적이 남아 있으면 클리어하지 않는다.

        ClearBattle(); // 전투 클리어 처리를 실행한다.
    }

    private void ClearBattle() // 전투 클리어 처리를 실행한다.
    {
        currentState = BattleState.Clear; // 현재 상태를 클리어로 변경한다.

        Debug.Log("Battle Clear"); // 전투 클리어 로그를 출력한다.

        StartCoroutine(ClearBattleRoutine()); // 클리어 연출과 보상 표시를 순서대로 실행한다.
    }

    private IEnumerator ClearBattleRoutine() // 전투 클리어 후 보상 UI를 표시한다.
    {
        if (battleClearUI != null) // 클리어 UI가 연결되어 있는지 확인한다.
        {
            battleClearUI.Show("BATTLE CLEAR"); // 클리어 UI를 표시한다.
        }

        yield return new WaitForSeconds(1f); // 1초 동안 클리어 문구를 보여준다.

        if (rewardManager != null) // 보상 관리자가 연결되어 있는지 확인한다.
        {
            rewardManager.ShowRewardPanel(); // 보상 선택 UI를 표시한다.
        }
        else // 보상 관리자가 없는 경우를 처리한다.
        {
            Debug.LogWarning("RewardManager가 연결되지 않았습니다."); // 경고 로그를 출력한다.
        }
    }
}