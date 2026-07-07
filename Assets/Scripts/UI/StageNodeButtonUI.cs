using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageNodeButtonUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nodeText; // 노드 정보를 표시할 텍스트를 저장한다.
    public Image backgroundImage; // 노드 배경 이미지를 저장한다.
    public Button nodeButton; // 노드 버튼을 저장한다.

    [Header("Node Colors")]
    public Color currentColor = new Color(0.3f, 0.6f, 1f, 1f); // 현재 노드 색상을 저장한다.
    public Color clearedColor = new Color(0.3f, 0.9f, 0.4f, 1f); // 클리어 노드 색상을 저장한다.
    public Color lockedColor = new Color(0.35f, 0.35f, 0.35f, 1f); // 잠긴 노드 색상을 저장한다.
    public Color normalColor = new Color(0.8f, 0.8f, 0.8f, 1f); // 기본 노드 색상을 저장한다.

    public void Setup(StageNodeData nodeData, int nodeIndex, int currentNodeIndex) // 노드 버튼 표시를 설정한다.
    {
        if (nodeData == null) return; // 노드 데이터가 없으면 실행하지 않는다.

        bool isCurrent = nodeIndex == currentNodeIndex && nodeData.isCleared == false; // 현재 노드인지 확인한다.
        string typeText = GetNodeTypeText(nodeData.nodeType); // 노드 타입 표시 문구를 가져온다.
        string stateText = GetNodeStateText(nodeData, isCurrent); // 노드 상태 표시 문구를 가져온다.

        if (nodeText != null) // 텍스트가 연결되어 있는지 확인한다.
        {
            nodeText.text = stateText + "\n" + typeText + "\n" + nodeData.nodeName; // 노드 정보를 표시한다.
        }

        if (backgroundImage != null) // 배경 이미지가 연결되어 있는지 확인한다.
        {
            backgroundImage.color = GetNodeColor(nodeData, isCurrent); // 노드 상태에 맞는 색상을 적용한다.
        }

        if (nodeButton != null) // 버튼이 연결되어 있는지 확인한다.
        {
            nodeButton.interactable = false; // 16일차에서는 클릭용이 아니라 표시용으로 사용한다.
        }
    }

    private string GetNodeTypeText(StageNodeType nodeType) // 노드 타입 표시 문구를 반환한다.
    {
        switch (nodeType) // 노드 타입을 확인한다.
        {
            case StageNodeType.NormalBattle: // 일반 전투 노드인지 확인한다.
                return "Normal Battle"; // 일반 전투 문구를 반환한다.

            case StageNodeType.EliteBattle: // 엘리트 전투 노드인지 확인한다.
                return "Elite Battle"; // 엘리트 전투 문구를 반환한다.

            case StageNodeType.Rest: // 휴식 노드인지 확인한다.
                return "Rest"; // 휴식 문구를 반환한다.

            case StageNodeType.BossBattle: // 보스 전투 노드인지 확인한다.
                return "Boss Battle"; // 보스 전투 문구를 반환한다.

            case StageNodeType.Event: // 이벤트 노드인지 확인한다.
                return "Event"; // 이벤트 문구를 반환한다.

            default: // 정의되지 않은 타입인 경우이다.
                return "Unknown"; // 알 수 없음 문구를 반환한다.
        }
    }

    private string GetNodeStateText(StageNodeData nodeData, bool isCurrent) // 노드 상태 표시 문구를 반환한다.
    {
        if (nodeData.isCleared) // 이미 클리어한 노드인지 확인한다.
        {
            return "[Clear]"; // 클리어 문구를 반환한다.
        }

        if (isCurrent) // 현재 노드인지 확인한다.
        {
            return "[Current]"; // 현재 노드 문구를 반환한다.
        }

        if (nodeData.isUnlocked == false) // 잠긴 노드인지 확인한다.
        {
            return "[Locked]"; // 잠김 문구를 반환한다.
        }

        return "[Open]"; // 열려 있는 노드 문구를 반환한다.
    }

    private Color GetNodeColor(StageNodeData nodeData, bool isCurrent) // 노드 상태에 맞는 색상을 반환한다.
    {
        if (nodeData.isCleared) // 클리어한 노드인지 확인한다.
        {
            return clearedColor; // 클리어 색상을 반환한다.
        }

        if (isCurrent) // 현재 노드인지 확인한다.
        {
            return currentColor; // 현재 노드 색상을 반환한다.
        }

        if (nodeData.isUnlocked == false) // 잠긴 노드인지 확인한다.
        {
            return lockedColor; // 잠긴 노드 색상을 반환한다.
        }

        return normalColor; // 기본 색상을 반환한다.
    }
}