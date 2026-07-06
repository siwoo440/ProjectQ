using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicSlotUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI relicText; // 보유 유물 목록 텍스트를 저장한다.

    [Header("Display Settings")]
    public int maxDisplayCount = 10; // 화면에 표시할 최대 유물 개수를 저장한다.

    public void UpdateRelicSlots(List<RelicData> relics) // 보유 유물 UI를 갱신한다.
    {
        if (relicText == null) return; // 유물 텍스트가 없으면 실행하지 않는다.

        if (relics == null || relics.Count == 0) // 보유 유물이 없는지 확인한다.
        {
            relicText.text = "Relics 0/" + maxDisplayCount + "\nNone"; // 유물이 없다는 문구를 표시한다.
            return; // UI 갱신을 종료한다.
        }

        string displayText = "Relics " + relics.Count + "/" + maxDisplayCount + "\n"; // 유물 개수 문구를 만든다.

        int displayCount = Mathf.Min(relics.Count, maxDisplayCount); // 실제 표시할 유물 개수를 계산한다.

        for (int i = 0; i < displayCount; i++) // 표시할 유물 수만큼 반복한다.
        {
            displayText += "- " + relics[i].relicName + "\n"; // 유물 이름을 한 줄씩 추가한다.
        }

        if (relics.Count > maxDisplayCount) // 최대 표시 개수를 넘었는지 확인한다.
        {
            int hiddenCount = relics.Count - maxDisplayCount; // 숨겨진 유물 개수를 계산한다.
            displayText += "... +" + hiddenCount + " more"; // 숨겨진 유물이 있다는 문구를 추가한다.
        }

        relicText.text = displayText; // 완성된 문장을 UI에 표시한다.
    }
}