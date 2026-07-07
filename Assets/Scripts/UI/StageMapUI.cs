using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageMapUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject stageMapPanel; // 스테이지 맵 패널을 저장한다.
    public TextMeshProUGUI titleText; // 맵 제목 텍스트를 저장한다.
    public Transform nodeButtonParent; // 노드 버튼들이 생성될 부모를 저장한다.
    public StageNodeButtonUI nodeButtonPrefab; // 노드 버튼 프리팹을 저장한다.

    private readonly List<StageNodeButtonUI> nodeButtons = new List<StageNodeButtonUI>(); // 생성된 노드 버튼 목록을 저장한다.

    private void Awake() // 오브젝트가 생성될 때 실행한다.
    {
        if (stageMapPanel == null) // 패널이 연결되지 않았는지 확인한다.
        {
            stageMapPanel = gameObject; // 자기 자신을 패널로 사용한다.
        }
    }

    public void RefreshStageMap(List<StageNodeData> stageNodes, int currentNodeIndex) // 스테이지 맵을 갱신한다.
    {
        if (stageNodes == null) return; // 노드 목록이 없으면 실행하지 않는다.

        if (titleText != null) // 제목 텍스트가 연결되어 있는지 확인한다.
        {
            titleText.text = "Chapter 1 Map"; // 제목을 표시한다.
        }

        ClearNodeButtons(); // 기존 노드 버튼을 삭제한다.

        for (int i = 0; i < stageNodes.Count; i++) // 노드 개수만큼 반복한다.
        {
            CreateNodeButton(stageNodes[i], i, currentNodeIndex); // 노드 버튼을 생성한다.
        }
    }

    private void CreateNodeButton(StageNodeData nodeData, int nodeIndex, int currentNodeIndex) // 노드 버튼 하나를 생성한다.
    {
        if (nodeButtonPrefab == null) // 노드 버튼 프리팹이 연결되지 않았는지 확인한다.
        {
            Debug.LogError("StageMapUI : nodeButtonPrefab is not assigned."); // 오류 로그를 출력한다.
            return; // 생성 처리를 중단한다.
        }

        if (nodeButtonParent == null) // 노드 버튼 부모가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("StageMapUI : nodeButtonParent is not assigned."); // 오류 로그를 출력한다.
            return; // 생성 처리를 중단한다.
        }

        StageNodeButtonUI newButton = Instantiate(nodeButtonPrefab, nodeButtonParent); // 노드 버튼을 생성한다.
        newButton.Setup(nodeData, nodeIndex, currentNodeIndex); // 노드 버튼 정보를 설정한다.
        nodeButtons.Add(newButton); // 생성된 버튼을 목록에 추가한다.
    }

    private void ClearNodeButtons() // 기존 노드 버튼들을 삭제한다.
    {
        for (int i = 0; i < nodeButtons.Count; i++) // 생성된 버튼 목록을 순회한다.
        {
            if (nodeButtons[i] != null) // 버튼이 존재하는지 확인한다.
            {
                Destroy(nodeButtons[i].gameObject); // 버튼 오브젝트를 삭제한다.
            }
        }

        nodeButtons.Clear(); // 버튼 목록을 비운다.
    }

    public void Show() // 스테이지 맵 패널을 표시한다.
    {
        if (stageMapPanel != null) // 패널이 연결되어 있는지 확인한다.
        {
            stageMapPanel.SetActive(true); // 패널을 활성화한다.
        }
    }

    public void Hide() // 스테이지 맵 패널을 숨긴다.
    {
        if (stageMapPanel != null) // 패널이 연결되어 있는지 확인한다.
        {
            stageMapPanel.SetActive(false); // 패널을 비활성화한다.
        }
    }
}