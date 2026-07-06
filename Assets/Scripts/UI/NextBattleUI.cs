using UnityEngine;

public class NextBattleUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject nextBattleButtonObject; // Next Battle 버튼 오브젝트를 저장한다.

    [Header("Target References")]
    public BattleProgressManager battleProgressManager; // 전투 진행 관리자를 저장한다.

    private void Start() // 시작 시 버튼을 숨긴다.
    {
        Hide(); // Next Battle 버튼을 숨긴다.
    }

    public void Show() // Next Battle 버튼을 표시한다.
    {
        if (nextBattleButtonObject != null) // 버튼 오브젝트가 연결되어 있는지 확인한다.
        {
            nextBattleButtonObject.SetActive(true); // 버튼을 켠다.
        }

        Debug.Log("Next Battle Button Opened"); // 버튼 표시 로그를 출력한다.
    }

    public void Hide() // Next Battle 버튼을 숨긴다.
    {
        if (nextBattleButtonObject != null) // 버튼 오브젝트가 연결되어 있는지 확인한다.
        {
            nextBattleButtonObject.SetActive(false); // 버튼을 끈다.
        }
    }

    public void OnClickNextBattle() // Next Battle 버튼을 클릭했을 때 실행한다.
    {
        if (battleProgressManager == null) // BattleProgressManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("BattleProgressManager is not assigned."); // 오류 로그를 출력한다.
            return; // 다음 전투 시작을 중단한다.
        }

        Hide(); // 버튼을 숨긴다.
        battleProgressManager.StartNextBattle(); // 다음 전투를 시작한다.
    }
}