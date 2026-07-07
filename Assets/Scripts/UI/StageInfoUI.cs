using TMPro;
using UnityEngine;

public class StageInfoUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI stageText; // 현재 스테이지 정보를 표시할 텍스트를 저장한다.

    public void UpdateStageInfo(StageNodeData nodeData, int currentIndex, int totalCount) // 현재 노드 정보를 UI에 표시한다.
    {
        if (stageText == null) return; // 텍스트가 연결되지 않았으면 실행하지 않는다.

        if (nodeData == null) // 노드 데이터가 없는지 확인한다.
        {
            stageText.text = "Stage\nNone"; // 노드가 없다는 문구를 표시한다.
            return; // UI 갱신을 종료한다.
        }

        string displayText = ""; // 표시할 문장을 만든다.
        displayText += "Chapter 1\n"; // 챕터 정보를 추가한다.
        displayText += "Node " + (currentIndex + 1) + " / " + totalCount + "\n"; // 현재 노드 번호를 추가한다.
        displayText += "Type: " + nodeData.nodeType + "\n"; // 노드 타입을 추가한다.
        displayText += "Name: " + nodeData.nodeName + "\n"; // 노드 이름을 추가한다.
        displayText += "Battle No: " + nodeData.battleNumber; // 전투 번호를 추가한다.

        stageText.text = displayText; // 완성된 문장을 표시한다.
    }

    public void ShowChapterClear() // 챕터 클리어 문구를 표시한다.
    {
        if (stageText == null) return; // 텍스트가 연결되지 않았으면 실행하지 않는다.

        stageText.text = "Chapter 1 Clear"; // 챕터 클리어 문구를 표시한다.
    }
}