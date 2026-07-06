using TMPro;
using UnityEngine;

public class BattleInfoUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI battleNumberText; // 현재 전투 번호 텍스트를 저장한다.

    public void SetBattleNumber(int battleNumber) // 현재 전투 번호를 표시한다.
    {
        if (battleNumberText == null) return; // 전투 번호 텍스트가 없으면 실행하지 않는다.

        battleNumberText.text = "Battle " + battleNumber; // 전투 번호 문구를 설정한다.
    }

    public void Hide() // 전투 번호 UI를 숨긴다.
    {
        if (battleNumberText == null) return; // 전투 번호 텍스트가 없으면 실행하지 않는다.

        battleNumberText.gameObject.SetActive(false); // 전투 번호 텍스트를 끈다.
    }

    public void Show() // 전투 번호 UI를 표시한다.
    {
        if (battleNumberText == null) return; // 전투 번호 텍스트가 없으면 실행하지 않는다.

        battleNumberText.gameObject.SetActive(true); // 전투 번호 텍스트를 켠다.
    }
}