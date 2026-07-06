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
    public GameOverUI gameOverUI; // 게임 오버 UI를 저장한다.
    public BattleInfoUI battleInfoUI; // 전투 번호 UI를 저장한다.

    [Header("Battle State")]
    public BattleState currentState = BattleState.Ready; // 현재 전투 상태를 저장한다.
    public int aliveEnemyCount = 0; // 현재 살아있는 적 수를 저장한다.
    public int currentBattleNumber = 0; // 현재 전투 번호를 저장한다.

    private Coroutine clearRoutine; // 클리어 처리 코루틴을 저장한다.

    private void Start() // 시작 시 UI만 정리한다.
    {
        if (battleClearUI != null) // 클리어 UI가 연결되어 있는지 확인한다.
        {
            battleClearUI.Hide(); // 시작 시 클리어 UI를 숨긴다.
        }

        if (gameOverUI != null) // 게임 오버 UI가 연결되어 있는지 확인한다.
        {
            gameOverUI.Hide(); // 시작 시 게임 오버 UI를 숨긴다.
        }
    }

    public void StartBattle(int battleNumber) // 지정된 전투 번호로 전투를 시작한다.
    {
        currentBattleNumber = battleNumber; // 현재 전투 번호를 저장한다.

        ResetBattleState(); // 전투 상태를 초기화한다.
        ClearOldBattleObjects(); // 이전 전투에서 남은 적과 탄환을 정리한다.

        currentState = BattleState.Battle; // 현재 상태를 전투 중으로 변경한다.
        aliveEnemyCount = 0; // 살아있는 적 수를 초기화한다.

        if (battleInfoUI != null) // 전투 번호 UI가 연결되어 있는지 확인한다.
        {
            battleInfoUI.Show(); // 전투 번호 UI를 표시한다.
            battleInfoUI.SetBattleNumber(currentBattleNumber); // 현재 전투 번호를 표시한다.
        }

        Debug.Log("Battle Start : " + currentBattleNumber); // 전투 시작 로그를 출력한다.

        if (enemySpawner == null) // 적 생성 관리자가 없는지 확인한다.
        {
            Debug.LogError("EnemySpawner is not assigned."); // 오류 로그를 출력한다.
            return; // 전투 시작 처리를 중단한다.
        }

        enemySpawner.SpawnEnemies(this, currentBattleNumber); // 전투 번호에 맞게 적 생성을 요청한다.

        if (aliveEnemyCount <= 0) // 생성된 적이 없는지 확인한다.
        {
            Debug.LogWarning("No enemies were spawned."); // 경고 로그를 출력한다.
        }

        if (battleInfoUI != null) // 전투 정보 UI가 연결되어 있는지 확인한다.
        {
            battleInfoUI.SetBattleInfo(currentBattleNumber, aliveEnemyCount); // 전투 번호와 남은 적 수를 갱신한다.
        }

    }

    private void ResetBattleState() // 전투 시작 전 UI와 상태를 정리한다.
    {
        currentState = BattleState.Ready; // 상태를 준비 상태로 변경한다.

        if (clearRoutine != null) // 이전 클리어 코루틴이 있는지 확인한다.
        {
            StopCoroutine(clearRoutine); // 이전 클리어 코루틴을 중지한다.
            clearRoutine = null; // 코루틴 참조를 비운다.
        }

        if (battleClearUI != null) // 클리어 UI가 연결되어 있는지 확인한다.
        {
            battleClearUI.Hide(); // 클리어 UI를 숨긴다.
        }

        if (rewardManager != null) // 보상 관리자가 연결되어 있는지 확인한다.
        {
            rewardManager.HideRewardPanel(); // 보상 패널을 숨긴다.
        }

        if (gameOverUI != null) // 게임 오버 UI가 연결되어 있는지 확인한다.
        {
            gameOverUI.Hide(); // 게임 오버 UI를 숨긴다.
        }

        Time.timeScale = 1f; // 혹시 멈춰 있던 게임 시간을 다시 흐르게 한다.
    }

    private void ClearOldBattleObjects() // 이전 전투에서 남은 오브젝트를 정리한다.
    {
        EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None); // 씬에 남아 있는 적을 모두 찾는다.

        for (int i = 0; i < enemies.Length; i++) // 남은 적 수만큼 반복한다.
        {
            Destroy(enemies[i].gameObject); // 남은 적을 삭제한다.
        }

        Bullet[] playerBullets = FindObjectsByType<Bullet>(FindObjectsSortMode.None); // 씬에 남아 있는 플레이어 탄환을 모두 찾는다.

        for (int i = 0; i < playerBullets.Length; i++) // 남은 플레이어 탄환 수만큼 반복한다.
        {
            Destroy(playerBullets[i].gameObject); // 남은 플레이어 탄환을 삭제한다.
        }

        EnemyBullet[] enemyBullets = FindObjectsByType<EnemyBullet>(FindObjectsSortMode.None); // 씬에 남아 있는 적 탄환을 모두 찾는다.

        for (int i = 0; i < enemyBullets.Length; i++) // 남은 적 탄환 수만큼 반복한다.
        {
            Destroy(enemyBullets[i].gameObject); // 남은 적 탄환을 삭제한다.
        }
    }

    public void RegisterEnemy(EnemyHealth enemyHealth) // 생성된 적을 전투 관리 대상에 등록한다.
    {
        if (enemyHealth == null) return; // 적 체력 컴포넌트가 없으면 실행하지 않는다.

        aliveEnemyCount += 1; // 살아있는 적 수를 1 증가시킨다.
        enemyHealth.SetBattleManager(this); // 적에게 BattleManager를 알려준다.

        if (battleInfoUI != null) // 전투 정보 UI가 연결되어 있는지 확인한다.
        {
            battleInfoUI.SetBattleInfo(currentBattleNumber, aliveEnemyCount); // 전투 번호와 남은 적 수를 갱신한다.
        }

        Debug.Log("Enemy Registered. Alive Enemy Count : " + aliveEnemyCount); // 현재 적 수를 로그로 출력한다.
    }

    public void OnEnemyDead(EnemyHealth enemyHealth) // 적이 죽었을 때 호출된다.
    {
        if (currentState != BattleState.Battle) return; // 전투 중이 아니면 처리하지 않는다.

        aliveEnemyCount -= 1; // 살아있는 적 수를 1 감소시킨다.
        aliveEnemyCount = Mathf.Max(aliveEnemyCount, 0); // 적 수가 0 아래로 내려가지 않게 한다.

        if (battleInfoUI != null) // 전투 정보 UI가 연결되어 있는지 확인한다.
        {
            battleInfoUI.SetBattleInfo(currentBattleNumber, aliveEnemyCount); // 전투 번호와 남은 적 수를 갱신한다.
        }

        Debug.Log("Enemy Dead. Alive Enemy Count : " + aliveEnemyCount); // 남은 적 수를 로그로 출력한다.

        CheckBattleClear(); // 전투 클리어 여부를 확인한다.
    }

    public void OnPlayerDead() // 플레이어가 죽었을 때 호출된다.
    {
        if (currentState != BattleState.Battle) return; // 전투 중이 아니면 처리하지 않는다.

        FailBattle(); // 전투 실패 처리를 실행한다.
    }

    private void CheckBattleClear() // 전투 클리어 조건을 확인한다.
    {
        if (aliveEnemyCount > 0) return; // 살아있는 적이 남아 있으면 클리어하지 않는다.

        ClearBattle(); // 전투 클리어 처리를 실행한다.
    }

    private void ClearBattle() // 전투 클리어 처리를 실행한다.
    {
        currentState = BattleState.Clear; // 현재 상태를 클리어로 변경한다.

        Debug.Log("Battle Clear : " + currentBattleNumber); // 전투 클리어 로그를 출력한다.

        clearRoutine = StartCoroutine(ClearBattleRoutine()); // 클리어 연출과 보상 표시를 순서대로 실행한다.
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
            Debug.LogWarning("RewardManager is not assigned."); // 경고 로그를 출력한다.
        }
    }

    private void FailBattle() // 전투 실패 처리를 실행한다.
    {
        currentState = BattleState.Failed; // 현재 상태를 실패로 변경한다.

        Debug.Log("Battle Failed : " + currentBattleNumber); // 전투 실패 로그를 출력한다.

        if (clearRoutine != null) // 클리어 코루틴이 실행 중인지 확인한다.
        {
            StopCoroutine(clearRoutine); // 클리어 코루틴을 중지한다.
            clearRoutine = null; // 코루틴 참조를 비운다.
        }

        ClearOldBattleObjects(); // 남아 있는 적과 탄환을 정리한다.

        if (rewardManager != null) // 보상 관리자가 연결되어 있는지 확인한다.
        {
            rewardManager.HideRewardPanel(); // 보상 패널을 숨긴다.
        }

        if (battleClearUI != null) // 클리어 UI가 연결되어 있는지 확인한다.
        {
            battleClearUI.Hide(); // 클리어 UI를 숨긴다.
        }

        if (gameOverUI != null) // 게임 오버 UI가 연결되어 있는지 확인한다.
        {
            gameOverUI.Show(); // 게임 오버 UI를 표시한다.
        }

        Time.timeScale = 0f; // 게임 오버 상태에서 게임 시간을 멈춘다.
    }
}