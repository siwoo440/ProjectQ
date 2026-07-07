using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapNodeData
{
    public int nodeId; // 노드 고유 번호를 저장한다.
    public int floor; // 노드가 위치한 층 번호를 저장한다.
    public int lane; // 노드의 가로 위치 번호를 저장한다.
    public StageNodeType nodeType; // 노드 종류를 저장한다.
    public int battleNumber; // 전투 씬에 전달할 전투 번호를 저장한다.
    public Vector2 normalizedPosition; // 맵 안에서의 정규화된 위치를 저장한다.
    public List<int> connectedNodeIds = new List<int>(); // 다음으로 이동 가능한 노드 ID 목록을 저장한다.
    public bool isCleared; // 클리어 여부를 저장한다.

    public MapNodeData(int newNodeId, int newFloor, int newLane, StageNodeType newNodeType, int newBattleNumber, Vector2 newPosition) // 맵 노드 데이터를 초기화한다.
    {
        nodeId = newNodeId; // 노드 고유 번호를 저장한다.
        floor = newFloor; // 층 번호를 저장한다.
        lane = newLane; // 가로 위치 번호를 저장한다.
        nodeType = newNodeType; // 노드 종류를 저장한다.
        battleNumber = newBattleNumber; // 전투 번호를 저장한다.
        normalizedPosition = newPosition; // 위치를 저장한다.
        isCleared = false; // 처음에는 클리어하지 않은 상태로 설정한다.
    }
}