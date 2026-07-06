using UnityEngine;

public class BattleProgressManager : MonoBehaviour
{
    [Header("Battle References")]
    public BattleManager battleManager; // 실제 전투를 시작할 BattleManager를 저장한다.
    public NextBattleUI nextBattleUI; // 다음 전투 버튼 UI를 저장한다.
    public PlayerStats playerStats; // 플레이어 스탯을 저장한다.
    public BattleClearUI battleClearUI; // 프로토타입 클리어 문구를 표시할 UI를 저장한다.
    public RelicManager relicManager; // 전투 시작 시 유물 효과를 적용할 RelicManager를 저장한다.

    [Header("Progress")]
    public int currentBattleNumber = 0; // 현재 전투 번호를 저장한다.
    public int maxBattleNumber = 6; // 1차 프로토타입 최대 전투 수를 저장한다.

    private void Start() // 게임 시작 시 첫 번째 전투를 시작한다.
    {
        if (nextBattleUI != null) // NextBattleUI가 연결되어 있는지 확인한다.
        {
            nextBattleUI.Hide(); // 시작 시 Next Battle 버튼을 숨긴다.
        }

        StartNextBattle(); // 첫 번째 전투를 시작한다.
    }

    public void StartNextBattle() // 다음 전투를 시작한다.
    {
        if (battleManager == null) // BattleManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("BattleManager is not assigned."); // 오류 로그를 출력한다.
            return; // 전투 시작을 중단한다.
        }

        if (currentBattleNumber >= maxBattleNumber) // 최대 전투 수를 이미 클리어했는지 확인한다.
        {
            ShowPrototypeClear(); // 프로토타입 클리어 처리를 실행한다.
            return; // 더 이상 전투를 시작하지 않는다.
        }

        currentBattleNumber += 1; // 전투 번호를 1 증가시킨다.

        Debug.Log("Start Battle : " + currentBattleNumber); // 시작할 전투 번호를 로그로 출력한다.

        if (relicManager == null) // RelicManager가 연결되지 않았는지 확인한다.
        {
            relicManager = FindFirstObjectByType<RelicManager>(); // 씬에서 RelicManager를 찾는다.
        }

        if (relicManager != null) // RelicManager가 있는지 확인한다.
        {
            relicManager.ApplyRelicsOnBattleStart(); // 전투 시작 유물 효과를 적용한다.
        }

        battleManager.StartBattle(currentBattleNumber); // BattleManager에게 해당 번호의 전투 시작을 요청한다.
    }

    public void RetryGame() // 게임 오버 후 처음부터 다시 시작한다.
    {
        Time.timeScale = 1f; // 멈춰 있던 게임 시간을 다시 흐르게 한다.

        currentBattleNumber = 0; // 전투 번호를 초기화한다.

        if (nextBattleUI != null) // NextBattleUI가 연결되어 있는지 확인한다.
        {
            nextBattleUI.Hide(); // Next Battle 버튼을 숨긴다.
        }

        if (playerStats == null) // PlayerStats가 연결되지 않았는지 확인한다.
        {
            playerStats = FindFirstObjectByType<PlayerStats>(); // 씬에서 PlayerStats를 찾는다.
        }

        if (playerStats != null) // PlayerStats가 있는지 확인한다.
        {
            playerStats.ResetStats(); // 플레이어 스탯을 초기화한다.
        }

        Debug.Log("Retry Game"); // 재시작 로그를 출력한다.

        StartNextBattle(); // Battle 1부터 다시 시작한다.
    }

    private void ShowPrototypeClear() // 1차 프로토타입 클리어 처리를 실행한다.
    {
        Time.timeScale = 1f; // 게임 시간을 정상 속도로 유지한다.

        if (nextBattleUI != null) // NextBattleUI가 연결되어 있는지 확인한다.
        {
            nextBattleUI.Hide(); // Next Battle 버튼을 숨긴다.
        }

        if (battleClearUI != null) // 클리어 UI가 연결되어 있는지 확인한다.
        {
            battleClearUI.Show("PROTOTYPE CLEAR"); // 프로토타입 클리어 문구를 표시한다.
        }

        Debug.Log("Prototype Clear"); // 프로토타입 클리어 로그를 출력한다.
    }
}