using TMPro;
using UnityEngine;

public class BattleInfoUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI battleNumberText; // 전투 정보 텍스트를 저장한다.

    private int currentBattleNumber = 0; // 현재 전투 번호를 저장한다.
    private int currentEnemyCount = 0; // 현재 남은 적 수를 저장한다.

    public void SetBattleNumber(int battleNumber) // 현재 전투 번호를 표시한다.
    {
        currentBattleNumber = battleNumber; // 전투 번호를 저장한다.
        RefreshText(); // 전투 정보 텍스트를 갱신한다.
    }

    public void SetEnemyCount(int enemyCount) // 현재 남은 적 수를 표시한다.
    {
        currentEnemyCount = enemyCount; // 남은 적 수를 저장한다.
        RefreshText(); // 전투 정보 텍스트를 갱신한다.
    }

    public void SetBattleInfo(int battleNumber, int enemyCount) // 전투 번호와 적 수를 함께 표시한다.
    {
        currentBattleNumber = battleNumber; // 전투 번호를 저장한다.
        currentEnemyCount = enemyCount; // 남은 적 수를 저장한다.
        RefreshText(); // 전투 정보 텍스트를 갱신한다.
    }

    private void RefreshText() // 전투 정보 텍스트를 갱신한다.
    {
        if (battleNumberText == null) return; // 텍스트가 없으면 실행하지 않는다.

        battleNumberText.text = "Battle " + currentBattleNumber + "\nEnemies: " + currentEnemyCount; // 전투 번호와 남은 적 수를 표시한다.
    }

    public void Show() // 전투 정보 UI를 표시한다.
    {
        if (battleNumberText == null) return; // 텍스트가 없으면 실행하지 않는다.

        battleNumberText.gameObject.SetActive(true); // 전투 정보 텍스트를 켠다.
    }

    public void Hide() // 전투 정보 UI를 숨긴다.
    {
        if (battleNumberText == null) return; // 텍스트가 없으면 실행하지 않는다.

        battleNumberText.gameObject.SetActive(false); // 전투 정보 텍스트를 끈다.
    }
}