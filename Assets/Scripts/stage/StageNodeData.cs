using System;
using UnityEngine;

[Serializable]
public class StageNodeData
{
    public string nodeName; // 노드 이름을 저장한다.
    public StageNodeType nodeType; // 노드 종류를 저장한다.
    public int battleNumber; // 기존 BattleManager에 전달할 전투 번호를 저장한다.
    public bool isCleared; // 노드 클리어 여부를 저장한다.
    public bool isUnlocked; // 노드 선택 가능 여부를 저장한다.

    public StageNodeData(string newNodeName, StageNodeType newNodeType, int newBattleNumber) // 노드 데이터를 초기화한다.
    {
        nodeName = newNodeName; // 노드 이름을 저장한다.
        nodeType = newNodeType; // 노드 종류를 저장한다.
        battleNumber = newBattleNumber; // 전투 번호를 저장한다.
        isCleared = false; // 처음에는 클리어하지 않은 상태로 설정한다.
        isUnlocked = false; // 처음에는 잠금 상태로 설정한다.
    }
}