using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [Header("Target")]
    public PlayerStats playerStats; // 표시할 플레이어 스탯을 저장한다.

    [Header("Texts")]
    public TextMeshProUGUI hpText; // HP 텍스트를 저장한다.
    public TextMeshProUGUI mpText; // MP 텍스트를 저장한다.
    public TextMeshProUGUI shieldText; // Shield 텍스트를 저장한다.

    private void Update() // 매 프레임 UI 텍스트를 갱신한다.
    {
        if (playerStats == null) return; // 플레이어 스탯이 없으면 실행하지 않는다.

        UpdateHPText(); // HP 텍스트를 갱신한다.
        UpdateMPText(); // MP 텍스트를 갱신한다.
        UpdateShieldText(); // Shield 텍스트를 갱신한다.
    }

    private void UpdateHPText() // HP 텍스트를 갱신한다.
    {
        if (hpText == null) return; // HP 텍스트가 없으면 실행하지 않는다.

        hpText.text = "HP : " + playerStats.currentHealth + " / " + playerStats.maxHealth; // HP 표시 문장을 만든다.
    }

    private void UpdateMPText() // MP 텍스트를 갱신한다.
    {
        if (mpText == null) return; // MP 텍스트가 없으면 실행하지 않는다.

        int currentMP = Mathf.FloorToInt(playerStats.currentMana); // 현재 마나를 정수로 변환한다.
        int maxMP = Mathf.FloorToInt(playerStats.maxMana); // 최대 마나를 정수로 변환한다.

        mpText.text = "MP : " + currentMP + " / " + maxMP; // MP 표시 문장을 만든다.
    }

    private void UpdateShieldText() // Shield 텍스트를 갱신한다.
    {
        if (shieldText == null) return; // Shield 텍스트가 없으면 실행하지 않는다.

        shieldText.text = "Shield : " + playerStats.shield; // Shield 표시 문장을 만든다.
    }
}