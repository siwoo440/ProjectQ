using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicSlotUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI relicText; // 보유 유물 목록 텍스트를 저장한다.

    public void UpdateRelicSlots(List<RelicData> relics) // 보유 유물 UI를 갱신한다.
    {
        if (relicText == null) return; // 유물 텍스트가 없으면 실행하지 않는다.

        if (relics == null || relics.Count == 0) // 보유 유물이 없는지 확인한다.
        {
            relicText.text = "Relics\nNone"; // 유물이 없다는 문구를 표시한다.
            return; // UI 갱신을 종료한다.
        }

        string displayText = "Relics\n"; // 표시 문장을 초기화한다.

        for (int i = 0; i < relics.Count; i++) // 보유 유물 수만큼 반복한다.
        {
            displayText += "- " + relics[i].relicName + "\n"; // 유물 이름을 한 줄씩 추가한다.
        }

        relicText.text = displayText; // 완성된 문장을 UI에 표시한다.
    }
}