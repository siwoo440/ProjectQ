using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardSlotUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI cardSlotsText; // 카드 슬롯 표시 텍스트를 저장한다.

    public void UpdateCardSlots(List<CardData> cards, int selectedIndex) // 카드 슬롯 UI를 갱신한다.
    {
        if (cardSlotsText == null) return; // 카드 슬롯 텍스트가 없으면 실행하지 않는다.

        if (cards == null || cards.Count == 0) // 보유 카드가 없는지 확인한다.
        {
            cardSlotsText.text = "No Cards"; // 카드 없음 문구를 표시한다.
            return; // UI 갱신을 종료한다.
        }

        string displayText = "Cards\n"; // 표시 문장을 초기화한다.

        for (int i = 0; i < cards.Count; i++) // 보유 카드 수만큼 반복한다.
        {
            string selectedMark = i == selectedIndex ? "> " : "  "; // 선택된 카드 표시를 만든다.
            displayText += selectedMark + "[" + (i + 1) + "] " + cards[i].cardName + "\n"; // 카드 한 줄을 추가한다.
        }

        cardSlotsText.text = displayText; // 완성된 카드 목록을 UI에 표시한다.
    }
}