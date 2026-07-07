using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Stage Settings")]
    public bool startRunOnPlay = true; // 게임 시작 시 자동으로 스테이지를 시작할지 저장한다.

    [Header("Stage Nodes")]
    public List<StageNodeData> stageNodes = new List<StageNodeData>(); // 현재 챕터의 노드 목록을 저장한다.

    [Header("Manager References")]
    public BattleManager battleManager; // 실제 전투 시작을 담당하는 BattleManager를 저장한다.
    public RelicManager relicManager; // 전투 시작 시 유물 효과를 적용할 RelicManager를 저장한다.
    public EnemySpawner enemySpawner; // 전투 타입을 전달할 EnemySpawner를 저장한다.

    [Header("UI References")]
    public StageInfoUI stageInfoUI; // 현재 스테이지 정보를 표시할 UI를 저장한다.
    public StageMapUI stageMapUI; // 노드맵 UI를 저장한다.
    public NextBattleUI nextBattleUI; // 다음 전투 버튼 UI를 저장한다.

    private int currentNodeIndex = -1; // 현재 진행 중인 노드 번호를 저장한다.
    private bool isChapterCleared = false; // 챕터 클리어 여부를 저장한다.

    private void Awake() // 오브젝트가 생성될 때 기본 노드를 준비한다.
    {
        CreateDefaultStageNodes(); // 기본 스테이지 노드를 생성한다.
    }

    private void Start() // 게임 시작 시 실행한다.
    {
        FindReferencesIfNeeded(); // 필요한 참조를 자동으로 찾는다.

        HideNextBattleUI(); // 시작 시 다음 전투 버튼을 숨긴다.

        if (startRunOnPlay) // 자동 시작 설정이 켜져 있는지 확인한다.
        {
            StartRun(); // 스테이지 진행을 시작한다.
        }
    }

    private void FindReferencesIfNeeded() // 필요한 매니저 참조를 자동으로 찾는다.
    {
        if (battleManager == null) // BattleManager가 연결되지 않았는지 확인한다.
        {
            battleManager = FindFirstObjectByType<BattleManager>(); // 씬에서 BattleManager를 찾는다.
        }

        if (relicManager == null) // RelicManager가 연결되지 않았는지 확인한다.
        {
            relicManager = FindFirstObjectByType<RelicManager>(); // 씬에서 RelicManager를 찾는다.
        }

        if (enemySpawner == null) // EnemySpawner가 연결되지 않았는지 확인한다.
        {
            enemySpawner = FindFirstObjectByType<EnemySpawner>(); // 씬에서 EnemySpawner를 찾는다.
        }

        if (stageInfoUI == null) // StageInfoUI가 연결되지 않았는지 확인한다.
        {
            stageInfoUI = FindFirstObjectByType<StageInfoUI>(); // 씬에서 StageInfoUI를 찾는다.
        }

        if (stageMapUI == null) // StageMapUI가 연결되지 않았는지 확인한다.
        {
            stageMapUI = FindFirstObjectByType<StageMapUI>(); // 씬에서 StageMapUI를 찾는다.
        }

        if (nextBattleUI == null) // NextBattleUI가 연결되지 않았는지 확인한다.
        {
            nextBattleUI = FindFirstObjectByType<NextBattleUI>(); // 씬에서 NextBattleUI를 찾는다.
        }
    }

    private void CreateDefaultStageNodes() // 기본 스테이지 노드 목록을 만든다.
    {
        if (stageNodes.Count > 0) return; // 이미 노드가 있으면 새로 만들지 않는다.

        stageNodes.Add(new StageNodeData("First Contact", StageNodeType.NormalBattle, 1)); // 첫 번째 일반 전투 노드를 추가한다.
        stageNodes.Add(new StageNodeData("Second Wave", StageNodeType.NormalBattle, 2)); // 두 번째 일반 전투 노드를 추가한다.
        stageNodes.Add(new StageNodeData("Elite Gate", StageNodeType.EliteBattle, 3)); // 엘리트 전투 노드를 추가한다.
        stageNodes.Add(new StageNodeData("Temporary Shelter", StageNodeType.Rest, 0)); // 휴식 노드를 추가한다.
        stageNodes.Add(new StageNodeData("Boss Gate", StageNodeType.BossBattle, 4)); // 보스 전투 노드를 추가한다.

        if (stageNodes.Count > 0) // 노드가 하나 이상 있는지 확인한다.
        {
            stageNodes[0].isUnlocked = true; // 첫 번째 노드를 해금한다.
        }
    }

    public void StartRun() // 챕터 진행을 처음부터 시작한다.
    {
        if (stageNodes.Count == 0) // 노드가 없는지 확인한다.
        {
            Debug.LogError("Stage nodes are empty."); // 오류 로그를 출력한다.
            return; // 스테이지 시작을 중단한다.
        }

        isChapterCleared = false; // 챕터 클리어 상태를 초기화한다.
        currentNodeIndex = 0; // 첫 번째 노드부터 시작한다.

        for (int i = 0; i < stageNodes.Count; i++) // 모든 노드를 순회한다.
        {
            stageNodes[i].isCleared = false; // 모든 노드를 미클리어 상태로 초기화한다.
            stageNodes[i].isUnlocked = i == 0; // 첫 번째 노드만 해금한다.
        }

        StartCurrentNode(); // 현재 노드를 시작한다.
    }

    public void StartNextNode() // 다음 노드로 이동한다.
    {
        HideNextBattleUI(); // 다음 노드로 넘어갈 때 다음 전투 버튼을 숨긴다.

        if (isChapterCleared) return; // 이미 챕터가 클리어되었으면 실행하지 않는다.

        if (currentNodeIndex >= 0 && currentNodeIndex < stageNodes.Count) // 현재 노드 번호가 올바른지 확인한다.
        {
            stageNodes[currentNodeIndex].isCleared = true; // 현재 노드를 클리어 처리한다.
        }

        currentNodeIndex++; // 다음 노드 번호로 이동한다.

        if (currentNodeIndex >= stageNodes.Count) // 마지막 노드 이후인지 확인한다.
        {
            ChapterClear(); // 챕터 클리어 처리를 한다.
            return; // 다음 노드 시작을 중단한다.
        }

        stageNodes[currentNodeIndex].isUnlocked = true; // 다음 노드를 해금한다.
        StartCurrentNode(); // 다음 노드를 시작한다.
    }

    public void RetryCurrentNode() // 현재 노드를 다시 시작한다.
    {
        HideNextBattleUI(); // 재시작 시 다음 전투 버튼을 숨긴다.

        if (currentNodeIndex < 0 || currentNodeIndex >= stageNodes.Count) // 현재 노드 번호가 올바르지 않은지 확인한다.
        {
            StartRun(); // 안전을 위해 처음부터 시작한다.
            return; // 재시작 처리를 종료한다.
        }

        StartCurrentNode(); // 현재 노드를 다시 시작한다.
    }

    private void StartCurrentNode() // 현재 노드 종류에 따라 진행을 시작한다.
    {
        HideNextBattleUI(); // 새 노드가 시작될 때 다음 전투 버튼을 숨긴다.

        if (currentNodeIndex < 0 || currentNodeIndex >= stageNodes.Count) // 현재 노드 번호가 올바르지 않은지 확인한다.
        {
            Debug.LogError("Invalid node index : " + currentNodeIndex); // 오류 로그를 출력한다.
            return; // 노드 시작을 중단한다.
        }

        StageNodeData currentNode = stageNodes[currentNodeIndex]; // 현재 노드 데이터를 가져온다.

        UpdateStageUIs(currentNode); // 스테이지 관련 UI를 갱신한다.

        Debug.Log("Start Stage Node : " + currentNode.nodeName + " / " + currentNode.nodeType); // 현재 노드 시작 로그를 출력한다.

        switch (currentNode.nodeType) // 현재 노드 타입을 확인한다.
        {
            case StageNodeType.NormalBattle: // 일반 전투 노드인지 확인한다.
                StartBattleNode(currentNode, BattleType.Normal); // 일반 전투를 시작한다.
                break; // switch문을 종료한다.

            case StageNodeType.EliteBattle: // 엘리트 전투 노드인지 확인한다.
                StartBattleNode(currentNode, BattleType.Elite); // 엘리트 전투를 시작한다.
                break; // switch문을 종료한다.

            case StageNodeType.Rest: // 휴식 노드인지 확인한다.
                StartRestNode(currentNode); // 휴식 노드를 시작한다.
                break; // switch문을 종료한다.

            case StageNodeType.BossBattle: // 보스 전투 노드인지 확인한다.
                StartBattleNode(currentNode, BattleType.Boss); // 보스 전투를 시작한다.
                break; // switch문을 종료한다.

            case StageNodeType.Event: // 이벤트 노드인지 확인한다.
                StartEventNode(currentNode); // 이벤트 노드를 시작한다.
                break; // switch문을 종료한다.
        }
    }

    private void StartBattleNode(StageNodeData nodeData, BattleType battleType) // 전투 노드를 시작한다.
    {
        Time.timeScale = 1f; // 게임 시간을 정상 속도로 되돌린다.

        if (enemySpawner != null) // EnemySpawner가 연결되어 있는지 확인한다.
        {
            enemySpawner.SetBattleType(battleType); // EnemySpawner에 전투 타입을 전달한다.
        }

        if (relicManager != null) // RelicManager가 연결되어 있는지 확인한다.
        {
            relicManager.ApplyRelicsOnBattleStart(); // 전투 시작 시 발동하는 유물 효과를 적용한다.
        }

        if (battleManager == null) // BattleManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("BattleManager is not assigned."); // 오류 로그를 출력한다.
            return; // 전투 시작을 중단한다.
        }

        Debug.Log("Start Battle Type : " + battleType); // 전투 타입 로그를 출력한다.
        battleManager.StartBattle(nodeData.battleNumber); // 기존 BattleManager에 전투 시작을 요청한다.
    }

    private void StartRestNode(StageNodeData nodeData) // 휴식 노드를 처리한다.
    {
        Debug.Log("Rest Node Reached : " + nodeData.nodeName); // 휴식 노드 도착 로그를 출력한다.
        Debug.Log("Rest node detail will be implemented later."); // 휴식 노드 상세 기능은 나중에 구현한다고 출력한다.

        StartNextNode(); // 16일차에서는 휴식 노드를 임시로 자동 통과한다.
    }

    private void StartEventNode(StageNodeData nodeData) // 이벤트 노드를 처리한다.
    {
        Debug.Log("Event Node Reached : " + nodeData.nodeName); // 이벤트 노드 도착 로그를 출력한다.
        Debug.Log("Event node detail will be implemented later."); // 이벤트 노드 상세 기능은 나중에 구현한다고 출력한다.

        StartNextNode(); // 16일차에서는 이벤트 노드를 임시로 자동 통과한다.
    }

    private void ChapterClear() // 챕터 클리어 처리를 한다.
    {
        isChapterCleared = true; // 챕터 클리어 상태로 변경한다.
        Time.timeScale = 1f; // 게임 시간을 정상 속도로 되돌린다.

        HideNextBattleUI(); // 챕터 클리어 시 다음 전투 버튼을 숨긴다.

        if (stageInfoUI != null) // StageInfoUI가 연결되어 있는지 확인한다.
        {
            stageInfoUI.ShowChapterClear(); // 챕터 클리어 UI를 표시한다.
        }

        if (stageMapUI != null) // StageMapUI가 연결되어 있는지 확인한다.
        {
            stageMapUI.RefreshStageMap(stageNodes, currentNodeIndex); // 노드맵 UI를 갱신한다.
        }

        Debug.Log("Chapter Clear"); // 챕터 클리어 로그를 출력한다.
    }

    private void UpdateStageUIs(StageNodeData currentNode) // 스테이지 관련 UI를 갱신한다.
    {
        if (stageInfoUI != null) // StageInfoUI가 연결되어 있는지 확인한다.
        {
            stageInfoUI.UpdateStageInfo(currentNode, currentNodeIndex, stageNodes.Count); // 현재 노드 정보를 UI에 표시한다.
        }

        if (stageMapUI != null) // StageMapUI가 연결되어 있는지 확인한다.
        {
            stageMapUI.RefreshStageMap(stageNodes, currentNodeIndex); // 노드맵 UI를 갱신한다.
        }
    }

    private void HideNextBattleUI() // 다음 전투 버튼 UI를 숨긴다.
    {
        if (nextBattleUI != null) // NextBattleUI가 연결되어 있는지 확인한다.
        {
            nextBattleUI.Hide(); // 다음 전투 버튼 UI를 숨긴다.
        }
    }

    public StageNodeData GetCurrentNode() // 현재 노드 데이터를 반환한다.
    {
        if (currentNodeIndex < 0 || currentNodeIndex >= stageNodes.Count) return null; // 현재 노드가 없으면 null을 반환한다.

        return stageNodes[currentNodeIndex]; // 현재 노드 데이터를 반환한다.
    }

    public int GetCurrentNodeIndex() // 현재 노드 번호를 반환한다.
    {
        return currentNodeIndex; // 현재 노드 번호를 반환한다.
    }

    public bool IsChapterCleared() // 챕터 클리어 여부를 반환한다.
    {
        return isChapterCleared; // 챕터 클리어 여부를 반환한다.
    }

    public List<StageNodeData> GetStageNodes() // 현재 스테이지 노드 목록을 반환한다.
    {
        return stageNodes; // 스테이지 노드 목록을 반환한다.
    }
}