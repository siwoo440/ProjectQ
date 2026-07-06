using UnityEngine;

public class BattleProgressManager : MonoBehaviour
{
    [Header("Battle References")]
    public BattleManager battleManager; // 실제 전투를 시작할 BattleManager를 저장한다.
    public NextBattleUI nextBattleUI; // 다음 전투 버튼 UI를 저장한다.

    [Header("Progress")]
    public int currentBattleNumber = 0; // 현재 전투 번호를 저장한다.
    public int maxBattleNumber = 5; // 임시 최대 전투 수를 저장한다.

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

        if (currentBattleNumber >= maxBattleNumber) // 최대 전투 수에 도달했는지 확인한다.
        {
            Debug.Log("All Battles Cleared"); // 모든 전투 클리어 로그를 출력한다.
            return; // 더 이상 전투를 시작하지 않는다.
        }

        currentBattleNumber += 1; // 전투 번호를 1 증가시킨다.

        Debug.Log("Start Battle : " + currentBattleNumber); // 시작할 전투 번호를 로그로 출력한다.

        battleManager.StartBattle(currentBattleNumber); // BattleManager에게 해당 번호의 전투 시작을 요청한다.
    }
}