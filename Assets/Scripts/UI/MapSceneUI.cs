using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapSceneUI : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform mapArea; // 노드와 연결선이 배치될 전체 영역을 저장한다.
    public RectTransform lineParent; // 연결선들이 생성될 부모를 저장한다.
    public RectTransform nodeParent; // 노드 버튼들이 생성될 부모를 저장한다.
    public TextMeshProUGUI titleText; // 제목 텍스트를 저장한다.
    public MapNodeUI mapNodePrefab; // 노드 버튼 프리팹을 저장한다.

    [Header("Line Settings")]
    public float lineThickness = 4f; // 연결선의 두께를 저장한다.
    public Color lockedLineColor = new Color(0.35f, 0.35f, 0.35f, 0.7f); // 아직 선택할 수 없는 연결선 색상을 저장한다.
    public Color selectableLineColor = new Color(0.4f, 0.75f, 1f, 1f); // 현재 이동 가능한 연결선 색상을 저장한다.
    public Color clearedLineColor = new Color(0.4f, 0.9f, 0.45f, 1f); // 이미 지나간 연결선 색상을 저장한다.
    
    [Header("Scroll Map Reference")] // 스크롤 맵 참조 제목
    public ScrollRect mapScrollRect; // 맵 스크롤 뷰

    private readonly List<MapNodeUI> spawnedNodes = new List<MapNodeUI>(); // 생성된 노드 UI 목록을 저장한다.
    private readonly List<GameObject> spawnedLines = new List<GameObject>(); // 생성된 연결선 UI 목록을 저장한다.

    private void Start() // 씬이 시작될 때 실행한다.
    {
        if (GameFlowManager.Instance == null) // GameFlowManager가 없는지 확인한다.
        {
            Debug.LogError("GameFlowManager is missing."); // 오류 로그를 출력한다.
            return; // 맵 UI 생성을 중단한다.
        }

        GameFlowManager.Instance.EnsureMapExists(); // 맵 데이터가 없으면 생성한다.
        RefreshMap(); // 맵 UI를 갱신한다.
        StartCoroutine(SetMapScrollToBottom()); // 맵 시작 위치 아래 설정
    }

    public void RefreshMap() // 맵 UI를 새로 그린다.
    {
        ClearMapObjects(); // 기존 노드와 연결선을 제거한다.

        if (titleText != null) // 제목 텍스트가 연결되어 있는지 확인한다.
        {
            titleText.text = "Chapter 1 Map"; // 제목을 표시한다.
        }

        if (lineParent == null) // 연결선 부모가 연결되지 않았는지 확인한다.
        {
            lineParent = mapArea; // 연결선 부모를 맵 영역으로 대체한다.
        }

        if (nodeParent == null) // 노드 부모가 연결되지 않았는지 확인한다.
        {
            nodeParent = mapArea; // 노드 부모를 맵 영역으로 대체한다.
        }

        List<MapNodeData> mapNodes = GameFlowManager.Instance.mapNodes; // 맵 노드 목록을 가져온다.

        CreateConnectionLines(mapNodes); // 노드 사이 연결선을 먼저 생성한다.

        for (int i = 0; i < mapNodes.Count; i++) // 노드 수만큼 반복한다.
        {
            CreateNodeUI(mapNodes[i]); // 노드 UI를 생성한다.
        }
    }

    private void CreateConnectionLines(List<MapNodeData> mapNodes) // 모든 노드 연결선을 생성한다.
    {
        if (mapArea == null) // 맵 영역이 연결되지 않았는지 확인한다.
        {
            Debug.LogError("MapSceneUI : mapArea is not assigned."); // 오류 로그를 출력한다.
            return; // 연결선 생성을 중단한다.
        }

        for (int i = 0; i < mapNodes.Count; i++) // 모든 노드를 순회한다.
        {
            MapNodeData fromNode = mapNodes[i]; // 시작 노드를 가져온다.

            for (int c = 0; c < fromNode.connectedNodeIds.Count; c++) // 시작 노드의 연결 목록을 순회한다.
            {
                MapNodeData toNode = GameFlowManager.Instance.GetNodeById(fromNode.connectedNodeIds[c]); // 도착 노드를 가져온다.

                if (toNode == null) // 도착 노드가 없는지 확인한다.
                {
                    continue; // 잘못된 연결은 건너뛴다.
                }

                CreateLine(fromNode, toNode); // 두 노드 사이에 선을 생성한다.
            }
        }
    }

    private void CreateLine(MapNodeData fromNode, MapNodeData toNode) // 두 노드 사이의 연결선을 생성한다.
    {
        Vector2 startPosition = GetAnchoredPosition(fromNode); // 시작 노드의 UI 위치를 계산한다.
        Vector2 endPosition = GetAnchoredPosition(toNode); // 도착 노드의 UI 위치를 계산한다.
        Vector2 middlePosition = (startPosition + endPosition) * 0.5f; // 두 점의 중간 위치를 계산한다.
        Vector2 direction = endPosition - startPosition; // 시작에서 도착까지의 방향을 계산한다.
        float distance = direction.magnitude; // 두 점 사이의 거리를 계산한다.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 선이 회전해야 할 각도를 계산한다.

        GameObject lineObject = new GameObject("Line_" + fromNode.nodeId + "_To_" + toNode.nodeId); // 연결선 오브젝트를 생성한다.
        lineObject.transform.SetParent(lineParent, false); // 연결선을 lineParent 아래에 배치한다.

        Image lineImage = lineObject.AddComponent<Image>(); // Image 컴포넌트를 추가한다.
        lineImage.color = GetLineColor(fromNode, toNode); // 연결 상태에 맞는 색상을 적용한다.
        lineImage.raycastTarget = false; // 연결선이 클릭을 막지 않도록 설정한다.

        RectTransform lineRect = lineObject.GetComponent<RectTransform>(); // 연결선 RectTransform을 가져온다.
        lineRect.anchorMin = new Vector2(0.5f, 0.5f); // 중앙 기준 앵커를 설정한다.
        lineRect.anchorMax = new Vector2(0.5f, 0.5f); // 중앙 기준 앵커를 설정한다.
        lineRect.pivot = new Vector2(0.5f, 0.5f); // 중앙 피벗을 설정한다.
        lineRect.anchoredPosition = middlePosition; // 선의 위치를 두 노드의 중간으로 설정한다.
        lineRect.sizeDelta = new Vector2(distance, lineThickness); // 선의 길이와 두께를 설정한다.
        lineRect.localRotation = Quaternion.Euler(0f, 0f, angle); // 선을 두 노드 방향으로 회전시킨다.

        spawnedLines.Add(lineObject); // 생성한 선을 목록에 추가한다.
    }
    private Color GetLineColor(MapNodeData fromNode, MapNodeData toNode) // 연결선 상태에 맞는 색상을 반환한다.
    {
        if (fromNode.isCleared && toNode.isCleared) // 이미 지나간 경로인지 확인한다.
        {
            return clearedLineColor; // 클리어 경로 색상을 반환한다.
        }

        if (GameFlowManager.Instance.currentClearedNodeId == -1) // 아직 아무 노드도 클리어하지 않았는지 확인한다.
        {
            if (fromNode.floor == 0) // 시작 층 노드인지 확인한다.
            {
                return selectableLineColor; // 시작 층에서 이어지는 선을 선택 가능 색상으로 표시한다.
            }
        }

        if (fromNode.nodeId == GameFlowManager.Instance.currentClearedNodeId) // 현재 위치에서 이어지는 연결인지 확인한다.
        {
            if (GameFlowManager.Instance.IsNodeSelectable(toNode)) // 도착 노드가 선택 가능한지 확인한다.
            {
                return selectableLineColor; // 선택 가능한 연결선 색상을 반환한다.
            }
        }

        return lockedLineColor; // 기본 잠김 연결선 색상을 반환한다.
    }

    private void CreateNodeUI(MapNodeData nodeData) // 노드 UI 하나를 생성한다.
    {
        if (mapArea == null) // 맵 영역이 연결되지 않았는지 확인한다.
        {
            Debug.LogError("MapSceneUI : mapArea is not assigned."); // 오류 로그를 출력한다.
            return; // 생성을 중단한다.
        }

        if (mapNodePrefab == null) // 노드 프리팹이 연결되지 않았는지 확인한다.
        {
            Debug.LogError("MapSceneUI : mapNodePrefab is not assigned."); // 오류 로그를 출력한다.
            return; // 생성을 중단한다.
        }

        MapNodeUI nodeUI = Instantiate(mapNodePrefab, nodeParent); // 노드 UI를 생성한다.
        RectTransform nodeRect = nodeUI.GetComponent<RectTransform>(); // RectTransform을 가져온다.

        nodeRect.anchoredPosition = GetAnchoredPosition(nodeData); // 노드 위치를 적용한다.

        bool isSelectable = GameFlowManager.Instance.IsNodeSelectable(nodeData); // 선택 가능한 노드인지 확인한다.
        bool isCurrentCleared = nodeData.nodeId == GameFlowManager.Instance.currentClearedNodeId; // 현재 위치 노드인지 확인한다.

        nodeUI.Setup(nodeData, isSelectable, isCurrentCleared, OnClickNode); // 노드 UI를 설정한다.
        spawnedNodes.Add(nodeUI); // 생성된 노드 목록에 추가한다.
    }

    private Vector2 GetAnchoredPosition(MapNodeData nodeData) // 노드의 정규화 위치를 UI 좌표로 변환한다.
    {
        float mapWidth = mapArea.rect.width; // 맵 영역의 가로 크기를 가져온다.
        float mapHeight = mapArea.rect.height; // 맵 영역의 세로 크기를 가져온다.
        float x = (nodeData.normalizedPosition.x - 0.5f) * mapWidth; // 노드의 가로 위치를 계산한다.
        float y = (nodeData.normalizedPosition.y - 0.5f) * mapHeight; // 노드의 세로 위치를 계산한다.

        return new Vector2(x, y); // 계산된 UI 위치를 반환한다.
    }

    private void OnClickNode(MapNodeData nodeData) // 노드 버튼을 클릭했을 때 실행한다.
    {
        GameFlowManager.Instance.SelectNode(nodeData); // GameFlowManager에 노드 선택을 요청한다.
    }

    private void ClearMapObjects() // 기존 노드와 연결선 UI를 삭제한다.
    {
        for (int i = 0; i < spawnedNodes.Count; i++) // 생성된 노드 목록을 순회한다.
        {
            if (spawnedNodes[i] != null) // 노드가 존재하는지 확인한다.
            {
                Destroy(spawnedNodes[i].gameObject); // 노드 오브젝트를 삭제한다.
            }
        }

        spawnedNodes.Clear(); // 노드 목록을 비운다.

        for (int i = 0; i < spawnedLines.Count; i++) // 생성된 연결선 목록을 순회한다.
        {
            if (spawnedLines[i] != null) // 연결선이 존재하는지 확인한다.
            {
                Destroy(spawnedLines[i]); // 연결선 오브젝트를 삭제한다.
            }
        }

        spawnedLines.Clear(); // 연결선 목록을 비운다.
    }
    private IEnumerator SetMapScrollToBottom() // 맵 스크롤 아래 이동 함수
    {
        yield return null; // UI 생성 대기

        Canvas.ForceUpdateCanvases(); // 캔버스 갱신

        if (mapScrollRect == null) // 스크롤 뷰 확인
        {
            yield break; // 실행 중단
        }

        mapScrollRect.verticalNormalizedPosition = 0f; // 아래쪽 위치 설정
    }
}