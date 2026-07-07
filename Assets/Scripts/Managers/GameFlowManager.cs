using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance; // 전역에서 접근할 수 있는 GameFlowManager 인스턴스를 저장한다.

    [Header("Scene Names")]
    public string mapSceneName = "MapScene"; // 맵 씬 이름을 저장한다.
    public string battleSceneName = "GameScene"; // 전투 씬 이름을 저장한다.

    [Header("Map Generation Settings")]
    public int floorCount = 6; // 맵의 층 수를 저장한다.
    public int laneCount = 3; // 한 층의 가로 노드 수를 저장한다.

    [Header("Runtime Map Data")]
    public List<MapNodeData> mapNodes = new List<MapNodeData>(); // 현재 생성된 맵 노드 목록을 저장한다.
    public int currentClearedNodeId = -1; // 마지막으로 클리어한 노드 ID를 저장한다.
    public int activeNodeId = -1; // 현재 진행 중인 노드 ID를 저장한다.

    [Header("Selected Battle Data")]
    public StageNodeType selectedNodeType = StageNodeType.NormalBattle; // 현재 선택한 노드 타입을 저장한다.
    public BattleType selectedBattleType = BattleType.Normal; // 현재 선택한 전투 타입을 저장한다.
    public int selectedBattleNumber = 1; // 현재 선택한 전투 번호를 저장한다.

    private void Awake() // 오브젝트가 생성될 때 실행한다.
    {
        if (Instance != null && Instance != this) // 이미 다른 GameFlowManager가 있는지 확인한다.
        {
            Destroy(gameObject); // 중복 오브젝트를 제거한다.
            return; // 초기화를 중단한다.
        }

        Instance = this; // 현재 오브젝트를 전역 인스턴스로 저장한다.
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 삭제되지 않게 설정한다.
    }

    public void EnsureMapExists() // 맵 데이터가 없으면 새 맵을 생성한다.
    {
        if (mapNodes.Count > 0) return; // 이미 맵이 있으면 다시 만들지 않는다.

        GenerateNewMap(); // 새 맵을 생성한다.
    }

    public void StartNewRun() // 새 챕터 진행을 시작한다.
    {
        mapNodes.Clear(); // 기존 맵 데이터를 삭제한다.
        currentClearedNodeId = -1; // 마지막 클리어 노드를 초기화한다.
        activeNodeId = -1; // 현재 진행 노드를 초기화한다.
        GenerateNewMap(); // 새 맵을 생성한다.
        SceneManager.LoadScene(mapSceneName); // 맵 씬으로 이동한다.
    }

    private void GenerateNewMap() // 랜덤 노드맵을 생성한다.
    {
        mapNodes.Clear(); // 기존 노드 목록을 비운다.

        int nodeId = 0; // 노드 ID를 0부터 시작한다.

        for (int floor = 0; floor < floorCount; floor++) // 층 수만큼 반복한다.
        {
            bool isBossFloor = floor == floorCount - 1; // 마지막 층인지 확인한다.
            int nodesOnFloor = isBossFloor ? 1 : laneCount; // 보스 층은 노드 1개만 만든다.

            for (int lane = 0; lane < nodesOnFloor; lane++) // 해당 층의 노드 수만큼 반복한다.
            {
                int actualLane = isBossFloor ? laneCount / 2 : lane; // 보스 노드는 중앙에 배치한다.
                StageNodeType nodeType = GetRandomNodeType(floor); // 층에 맞는 노드 타입을 정한다.
                int battleNumber = floor + 1; // 전투 번호를 층 기준으로 설정한다.
                Vector2 position = GetNodePosition(floor, actualLane); // 노드 위치를 계산한다.

                MapNodeData nodeData = new MapNodeData(nodeId, floor, actualLane, nodeType, battleNumber, position); // 노드 데이터를 만든다.
                mapNodes.Add(nodeData); // 노드 목록에 추가한다.
                nodeId++; // 다음 노드 ID로 증가한다.
            }
        }

        CreateConnections(); // 노드 사이 연결을 만든다.
        Debug.Log("Map Generated. Node Count : " + mapNodes.Count); // 생성된 노드 수를 출력한다.
    }

    private StageNodeType GetRandomNodeType(int floor) // 층에 따라 랜덤 노드 타입을 반환한다.
    {
        if (floor == floorCount - 1) // 마지막 층인지 확인한다.
        {
            return StageNodeType.BossBattle; // 마지막 층은 보스 노드로 설정한다.
        }

        if (floor == 0) // 첫 번째 층인지 확인한다.
        {
            return StageNodeType.NormalBattle; // 첫 층은 일반 전투로 설정한다.
        }

        if (floor == floorCount - 2) // 보스 직전 층인지 확인한다.
        {
            return Random.value < 0.5f ? StageNodeType.Rest : StageNodeType.EliteBattle; // 휴식 또는 엘리트로 설정한다.
        }

        float randomValue = Random.value; // 랜덤 값을 뽑는다.

        if (randomValue < 0.6f) // 60% 확률인지 확인한다.
        {
            return StageNodeType.NormalBattle; // 일반 전투를 반환한다.
        }

        if (randomValue < 0.75f) // 15% 확률인지 확인한다.
        {
            return StageNodeType.Event; // 이벤트를 반환한다.
        }

        if (randomValue < 0.9f) // 15% 확률인지 확인한다.
        {
            return StageNodeType.Rest; // 휴식을 반환한다.
        }

        return StageNodeType.EliteBattle; // 나머지는 엘리트 전투를 반환한다.
    }

    private Vector2 GetNodePosition(int floor, int lane) // 맵 안에서 노드 위치를 계산한다.
    {
        float x = laneCount <= 1 ? 0.5f : (float)lane / (laneCount - 1); // 가로 위치를 0~1로 계산한다.
        float y = floorCount <= 1 ? 0.5f : (float)floor / (floorCount - 1); // 세로 위치를 0~1로 계산한다.

        x = Mathf.Lerp(0.2f, 0.8f, x); // 양쪽 여백을 두고 가로 위치를 조정한다.
        y = Mathf.Lerp(0.1f, 0.9f, y); // 위아래 여백을 두고 세로 위치를 조정한다.

        return new Vector2(x, y); // 계산된 위치를 반환한다.
    }

    private void CreateConnections() // 각 노드의 다음 연결을 생성한다.
    {
        for (int floor = 0; floor < floorCount - 1; floor++) // 마지막 층 전까지 반복한다.
        {
            List<MapNodeData> currentFloorNodes = GetNodesOnFloor(floor); // 현재 층 노드들을 가져온다.
            List<MapNodeData> nextFloorNodes = GetNodesOnFloor(floor + 1); // 다음 층 노드들을 가져온다.

            for (int i = 0; i < currentFloorNodes.Count; i++) // 현재 층 노드 수만큼 반복한다.
            {
                MapNodeData currentNode = currentFloorNodes[i]; // 현재 노드를 가져온다.

                if (nextFloorNodes.Count == 1) // 다음 층이 보스처럼 노드 1개인지 확인한다.
                {
                    currentNode.connectedNodeIds.Add(nextFloorNodes[0].nodeId); // 보스 노드에 연결한다.
                    continue; // 다음 노드로 넘어간다.
                }

                int connectionCount = Random.Range(1, 3); // 연결 개수를 1~2개로 정한다.

                for (int c = 0; c < connectionCount; c++) // 연결 개수만큼 반복한다.
                {
                    int targetLane = Mathf.Clamp(currentNode.lane + Random.Range(-1, 2), 0, laneCount - 1); // 인접한 다음 층 위치를 고른다.
                    MapNodeData targetNode = GetNodeOnFloorAndLane(floor + 1, targetLane); // 해당 위치의 다음 노드를 가져온다.

                    if (targetNode != null && currentNode.connectedNodeIds.Contains(targetNode.nodeId) == false) // 유효하고 중복이 아닌지 확인한다.
                    {
                        currentNode.connectedNodeIds.Add(targetNode.nodeId); // 연결 목록에 추가한다.
                    }
                }

                if (currentNode.connectedNodeIds.Count == 0) // 연결이 하나도 없는지 확인한다.
                {
                    MapNodeData fallbackNode = nextFloorNodes[Random.Range(0, nextFloorNodes.Count)]; // 임시 연결 노드를 고른다.
                    currentNode.connectedNodeIds.Add(fallbackNode.nodeId); // 최소 1개 연결을 보장한다.
                }
            }
        }
    }

    private List<MapNodeData> GetNodesOnFloor(int floor) // 특정 층의 노드 목록을 반환한다.
    {
        List<MapNodeData> result = new List<MapNodeData>(); // 결과 목록을 만든다.

        for (int i = 0; i < mapNodes.Count; i++) // 전체 노드를 순회한다.
        {
            if (mapNodes[i].floor == floor) // 같은 층인지 확인한다.
            {
                result.Add(mapNodes[i]); // 결과 목록에 추가한다.
            }
        }

        return result; // 결과 목록을 반환한다.
    }

    private MapNodeData GetNodeOnFloorAndLane(int floor, int lane) // 특정 층과 위치의 노드를 반환한다.
    {
        for (int i = 0; i < mapNodes.Count; i++) // 전체 노드를 순회한다.
        {
            if (mapNodes[i].floor == floor && mapNodes[i].lane == lane) // 층과 위치가 같은지 확인한다.
            {
                return mapNodes[i]; // 해당 노드를 반환한다.
            }
        }

        return null; // 찾지 못하면 null을 반환한다.
    }

    public MapNodeData GetNodeById(int nodeId) // 노드 ID로 노드를 찾는다.
    {
        for (int i = 0; i < mapNodes.Count; i++) // 전체 노드를 순회한다.
        {
            if (mapNodes[i].nodeId == nodeId) // ID가 같은지 확인한다.
            {
                return mapNodes[i]; // 해당 노드를 반환한다.
            }
        }

        return null; // 찾지 못하면 null을 반환한다.
    }

    public bool IsNodeSelectable(MapNodeData nodeData) // 노드를 선택할 수 있는지 확인한다.
    {
        if (nodeData == null) return false; // 노드가 없으면 선택할 수 없다.
        if (activeNodeId != -1) return false; // 진행 중인 노드가 있으면 선택할 수 없다.
        if (nodeData.isCleared) return false; // 이미 클리어한 노드는 선택할 수 없다.

        if (currentClearedNodeId == -1) // 아직 클리어한 노드가 없는지 확인한다.
        {
            return nodeData.floor == 0; // 첫 층 노드만 선택할 수 있다.
        }

        MapNodeData currentNode = GetNodeById(currentClearedNodeId); // 마지막으로 클리어한 노드를 가져온다.

        if (currentNode == null) return false; // 현재 노드가 없으면 선택할 수 없다.

        return currentNode.connectedNodeIds.Contains(nodeData.nodeId); // 연결된 노드인지 반환한다.
    }

    public void SelectNode(MapNodeData nodeData) // 맵에서 노드를 선택한다.
    {
        if (IsNodeSelectable(nodeData) == false) // 선택 가능한 노드인지 확인한다.
        {
            Debug.Log("This node is not selectable."); // 선택 불가 로그를 출력한다.
            return; // 선택을 중단한다.
        }

        activeNodeId = nodeData.nodeId; // 현재 진행 중인 노드로 저장한다.
        selectedNodeType = nodeData.nodeType; // 선택한 노드 타입을 저장한다.
        selectedBattleNumber = nodeData.battleNumber; // 선택한 전투 번호를 저장한다.
        selectedBattleType = ConvertNodeTypeToBattleType(nodeData.nodeType); // 전투 타입으로 변환한다.

        Debug.Log("Selected Node : " + nodeData.nodeId + " / " + nodeData.nodeType); // 선택한 노드 정보를 출력한다.

        if (nodeData.nodeType == StageNodeType.Rest || nodeData.nodeType == StageNodeType.Event) // 비전투 노드인지 확인한다.
        {
            HandleNonBattleNode(nodeData); // 비전투 노드를 처리한다.
            return; // 전투 씬 이동을 하지 않는다.
        }

        SceneManager.LoadScene(battleSceneName); // 전투 씬으로 이동한다.
    }

    private BattleType ConvertNodeTypeToBattleType(StageNodeType nodeType) // 노드 타입을 전투 타입으로 변환한다.
    {
        if (nodeType == StageNodeType.EliteBattle) // 엘리트 전투 노드인지 확인한다.
        {
            return BattleType.Elite; // 엘리트 전투 타입을 반환한다.
        }

        if (nodeType == StageNodeType.BossBattle) // 보스 전투 노드인지 확인한다.
        {
            return BattleType.Boss; // 보스 전투 타입을 반환한다.
        }

        return BattleType.Normal; // 기본은 일반 전투 타입을 반환한다.
    }

    private void HandleNonBattleNode(MapNodeData nodeData) // 휴식이나 이벤트 노드를 임시 처리한다.
    {
        Debug.Log("Non Battle Node Reached : " + nodeData.nodeType); // 비전투 노드 도착 로그를 출력한다.
        Debug.Log("Detail UI will be implemented later."); // 상세 UI는 나중에 구현한다고 출력한다.

        CompleteActiveNodeAndReturnToMap(); // 현재 노드를 클리어하고 맵으로 돌아간다.
    }

    public void CompleteActiveNodeAndReturnToMap() // 현재 진행 중인 노드를 완료하고 맵으로 돌아간다.
    {
        if (activeNodeId == -1) // 진행 중인 노드가 없는지 확인한다.
        {
            SceneManager.LoadScene(mapSceneName); // 안전하게 맵 씬으로 이동한다.
            return; // 처리를 종료한다.
        }

        MapNodeData activeNode = GetNodeById(activeNodeId); // 현재 진행 중인 노드를 가져온다.

        if (activeNode != null) // 노드가 존재하는지 확인한다.
        {
            activeNode.isCleared = true; // 노드를 클리어 처리한다.
            currentClearedNodeId = activeNode.nodeId; // 마지막 클리어 노드로 저장한다.
        }

        activeNodeId = -1; // 진행 중인 노드를 초기화한다.

        if (selectedNodeType == StageNodeType.BossBattle) // 보스 노드를 클리어했는지 확인한다.
        {
            Debug.Log("Chapter Clear"); // 챕터 클리어 로그를 출력한다.
        }

        SceneManager.LoadScene(mapSceneName); // 맵 씬으로 돌아간다.
    }
}