using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapNodeUI : MonoBehaviour
{
    [Header("UI References")]
    public Button nodeButton; // 노드 선택 버튼을 저장한다.
    public TextMeshProUGUI nodeText; // 노드 정보를 표시할 텍스트를 저장한다.
    public Image backgroundImage; // 노드 배경 이미지를 저장한다.

    [Header("Colors")]
    public Color selectableColor = new Color(0.35f, 0.65f, 1f, 1f); // 선택 가능한 노드 색상을 저장한다.
    public Color clearedColor = new Color(0.4f, 0.9f, 0.45f, 1f); // 클리어한 노드 색상을 저장한다.
    public Color lockedColor = new Color(0.25f, 0.25f, 0.25f, 1f); // 잠긴 노드 색상을 저장한다.
    public Color bossColor = new Color(0.9f, 0.35f, 0.35f, 1f); // 보스 노드 색상을 저장한다.

    private MapNodeData nodeData; // 이 UI가 표시하는 노드 데이터를 저장한다.
    private Action<MapNodeData> onClickAction; // 클릭 시 실행할 함수를 저장한다.

    public void Setup(MapNodeData newNodeData, bool isSelectable, bool isCurrentCleared, Action<MapNodeData> newClickAction) // 노드 UI를 설정한다.
    {
        nodeData = newNodeData; // 노드 데이터를 저장한다.
        onClickAction = newClickAction; // 클릭 함수를 저장한다.

        if (nodeText != null) // 텍스트가 연결되어 있는지 확인한다.
        {
            nodeText.text = GetNodeText(nodeData, isSelectable, isCurrentCleared); // 노드 텍스트를 표시한다.
        }

        if (backgroundImage != null) // 배경 이미지가 연결되어 있는지 확인한다.
        {
            backgroundImage.color = GetNodeColor(nodeData, isSelectable); // 노드 색상을 적용한다.
        }

        if (nodeButton != null) // 버튼이 연결되어 있는지 확인한다.
        {
            nodeButton.interactable = isSelectable; // 선택 가능 여부에 따라 버튼 활성화를 설정한다.
            nodeButton.onClick.RemoveAllListeners(); // 기존 클릭 이벤트를 제거한다.
            nodeButton.onClick.AddListener(OnClickNode); // 클릭 이벤트를 등록한다.
        }
    }

    private void OnClickNode() // 노드 버튼을 클릭했을 때 실행한다.
    {
        if (onClickAction == null) return; // 클릭 함수가 없으면 실행하지 않는다.

        onClickAction.Invoke(nodeData); // 노드 선택 함수를 실행한다.
    }

    private string GetNodeText(MapNodeData data, bool isSelectable, bool isCurrentCleared) // 노드 표시 문구를 만든다.
    {
        string stateText = "[Locked]"; // 기본 상태 문구를 잠김으로 설정한다.

        if (data.isCleared) // 클리어한 노드인지 확인한다.
        {
            stateText = "[Clear]"; // 클리어 문구를 설정한다.
        }
        else if (isSelectable) // 선택 가능한 노드인지 확인한다.
        {
            stateText = "[Select]"; // 선택 가능 문구를 설정한다.
        }

        if (isCurrentCleared) // 현재 위치 노드인지 확인한다.
        {
            stateText = "[Current]"; // 현재 위치 문구를 설정한다.
        }

        return stateText + "\n" + GetTypeText(data.nodeType); // 상태와 타입 문구를 반환한다.
    }

    private string GetTypeText(StageNodeType nodeType) // 노드 타입 문구를 반환한다.
    {
        switch (nodeType) // 노드 타입을 확인한다.
        {
            case StageNodeType.NormalBattle: // 일반 전투인지 확인한다.
                return "Battle"; // 일반 전투 문구를 반환한다.

            case StageNodeType.EliteBattle: // 엘리트 전투인지 확인한다.
                return "Elite"; // 엘리트 문구를 반환한다.

            case StageNodeType.Rest: // 휴식인지 확인한다.
                return "Rest"; // 휴식 문구를 반환한다.

            case StageNodeType.Event: // 이벤트인지 확인한다.
                return "Event"; // 이벤트 문구를 반환한다.

            case StageNodeType.BossBattle: // 보스 전투인지 확인한다.
                return "Boss"; // 보스 문구를 반환한다.

            default: // 정의되지 않은 타입인 경우이다.
                return "Unknown"; // 알 수 없음 문구를 반환한다.
        }
    }

    private Color GetNodeColor(MapNodeData data, bool isSelectable) // 노드 색상을 반환한다.
    {
        if (data.nodeType == StageNodeType.BossBattle) // 보스 노드인지 확인한다.
        {
            return bossColor; // 보스 색상을 반환한다.
        }

        if (data.isCleared) // 클리어한 노드인지 확인한다.
        {
            return clearedColor; // 클리어 색상을 반환한다.
        }

        if (isSelectable) // 선택 가능한 노드인지 확인한다.
        {
            return selectableColor; // 선택 가능 색상을 반환한다.
        }

        return lockedColor; // 잠긴 색상을 반환한다.
    }
}