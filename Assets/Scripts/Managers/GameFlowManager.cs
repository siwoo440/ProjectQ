using System.Collections.Generic; // 리스트 사용
using UnityEngine; // Unity 기본 기능 사용
using UnityEngine.SceneManagement; // 씬 이동 사용

public class GameFlowManager : MonoBehaviour // 게임 진행 관리 클래스
{
    public static GameFlowManager Instance; // 전역 인스턴스

    [Header("Scene Names")] // 씬 이름 제목
    public string mapSceneName = "MapScene"; // 맵 씬 이름
    public string battleSceneName = "GameScene"; // 전투 씬 이름

    [Header("Map Generation Settings")] // 맵 생성 설정 제목
    public int floorCount = 6; // 맵 층 수
    public int laneCount = 3; // 한 층의 노드 수

    [Header("Runtime Map Data")] // 실행 중 맵 데이터 제목
    public List<MapNodeData> mapNodes = new List<MapNodeData>(); // 맵 노드 목록
    public int currentClearedNodeId = -1; // 마지막 클리어 노드 ID
    public int activeNodeId = -1; // 현재 진행 노드 ID

    [Header("Selected Battle Data")] // 선택 전투 데이터 제목
    public StageNodeType selectedNodeType = StageNodeType.NormalBattle; // 선택 노드 타입
    public BattleType selectedBattleType = BattleType.Normal; // 선택 전투 타입
    public int selectedBattleNumber = 1; // 선택 전투 번호

    [Header("Rest Effect Values")] // 휴식 효과 수치 제목
    public int restHealAmount = 30; // 휴식 체력 회복량
    public int restShieldAmount = 2; // 휴식 보호막 획득량
    public int restCardDamageUpgradeAmount = 5; // 휴식 카드 강화량
   
    [Header("Event Effect Values")] // 이벤트 효과 수치 제목
    public int eventPrayerHealAmount = 20; // 기도 회복량
    public int eventBlessingShieldAmount = 3; // 축복 보호막량
    public int eventCardUpgradeCount = 1; // 이벤트 카드 강화 횟수

    [Header("Pending Rest Effects")] // 대기 중 휴식 효과 제목
    public int pendingHealAmount = 0; // 대기 중 체력 회복량
    public int pendingShieldAmount = 0; // 대기 중 보호막 획득량
    public int pendingCardDamageUpgradeCount = 0; // 대기 중 카드 강화 횟수

    private RestNodeUIController restNodeUIController; // 휴식 UI 컨트롤러
    private EventRoomUIController eventRoomUIController; // 이벤트 방 UI 컨트롤러

    private void Awake() // 오브젝트 생성 처리
    {
        if (Instance != null && Instance != this) // 중복 인스턴스 확인
        {
            Destroy(gameObject); // 중복 오브젝트 삭제
            return; // 초기화 중단
        }

        Instance = this; // 현재 오브젝트 등록
        DontDestroyOnLoad(gameObject); // 씬 이동 후 유지
    }

    public void RegisterRestNodeUI(RestNodeUIController newRestNodeUIController) // 휴식 UI 등록 함수
    {
        restNodeUIController = newRestNodeUIController; // 휴식 UI 저장
    }

    public void UnregisterRestNodeUI(RestNodeUIController targetRestNodeUIController) // 휴식 UI 해제 함수
    {
        if (restNodeUIController != targetRestNodeUIController) // 현재 등록 UI와 다른지 확인
        {
            return; // 해제 중단
        }

        restNodeUIController = null; // 휴식 UI 비우기
    }
    public void RegisterEventRoomUI(EventRoomUIController newEventRoomUIController) // 이벤트 방 UI 등록 함수
    {
        eventRoomUIController = newEventRoomUIController; // 이벤트 방 UI 저장
    }

    public void UnregisterEventRoomUI(EventRoomUIController targetEventRoomUIController) // 이벤트 방 UI 해제 함수
    {
        if (eventRoomUIController != targetEventRoomUIController) // 등록된 UI 비교
        {
            return; // 해제 중단
        }

        eventRoomUIController = null; // 이벤트 방 UI 비우기
    }
    public void EnsureMapExists() // 맵 존재 보장 함수
    {
        if (mapNodes.Count > 0) // 기존 맵 확인
        {
            return; // 생성 중단
        }

        GenerateNewMap(); // 새 맵 생성
    }

    public void StartNewRun() // 새 진행 시작 함수
    {
        mapNodes.Clear(); // 기존 맵 삭제
        currentClearedNodeId = -1; // 마지막 클리어 노드 초기화
        activeNodeId = -1; // 현재 진행 노드 초기화
        ClearPendingRestEffects(); // 대기 효과 초기화
        GenerateNewMap(); // 새 맵 생성
        SceneManager.LoadScene(mapSceneName); // 맵 씬 이동
    }

    private void GenerateNewMap() // 랜덤 맵 생성 함수
    {
        mapNodes.Clear(); // 맵 노드 초기화

        int nodeId = 0; // 노드 ID 시작값

        for (int floor = 0; floor < floorCount; floor++) // 층 반복
        {
            bool isBossFloor = floor == floorCount - 1; // 보스 층 여부
            int nodesOnFloor = isBossFloor ? 1 : laneCount; // 층별 노드 수

            for (int lane = 0; lane < nodesOnFloor; lane++) // 노드 반복
            {
                int actualLane = isBossFloor ? laneCount / 2 : lane; // 실제 가로 위치
                StageNodeType nodeType = GetRandomNodeType(floor); // 노드 타입 생성
                int battleNumber = floor + 1; // 전투 번호 계산
                Vector2 position = GetNodePosition(floor, actualLane); // 노드 위치 계산
                MapNodeData nodeData = new MapNodeData(nodeId, floor, actualLane, nodeType, battleNumber, position); // 노드 데이터 생성

                mapNodes.Add(nodeData); // 노드 목록 추가
                nodeId++; // 노드 ID 증가
            }
        }

        CreateConnections(); // 노드 연결 생성
        Debug.Log("Map Generated. Node Count : " + mapNodes.Count); // 생성 로그
    }

    private StageNodeType GetRandomNodeType(int floor) // 층별 노드 타입 생성 함수
    {
        if (floor == floorCount - 1) // 마지막 층 확인
        {
            return StageNodeType.BossBattle; // 보스 노드 반환
        }

        if (floor == 0) // 첫 층 확인
        {
            return StageNodeType.BossBattle; // 보스 전투 테스트 반환
        }

        if (floor == floorCount - 2) // 보스 직전 층 확인
        {
            return Random.value < 0.5f ? StageNodeType.Rest : StageNodeType.EliteBattle; // 휴식 또는 엘리트 반환
        }

        float randomValue = Random.value; // 랜덤 값 생성

        if (randomValue < 0.6f) // 일반 전투 확률 확인
        {
            return StageNodeType.NormalBattle; // 일반 전투 반환
        }

        if (randomValue < 0.75f) // 이벤트 확률 확인
        {
            return StageNodeType.Event; // 이벤트 반환
        }

        if (randomValue < 0.9f) // 휴식 확률 확인
        {
            return StageNodeType.Rest; // 휴식 반환
        }

        return StageNodeType.EliteBattle; // 엘리트 반환
    }

    private Vector2 GetNodePosition(int floor, int lane) // 노드 위치 계산 함수
    {
        float x = laneCount <= 1 ? 0.5f : (float)lane / (laneCount - 1); // 가로 비율 계산
        float y = floorCount <= 1 ? 0.5f : (float)floor / (floorCount - 1); // 세로 비율 계산

        x = Mathf.Lerp(0.2f, 0.8f, x); // 가로 여백 적용
        y = Mathf.Lerp(0.1f, 0.9f, y); // 세로 여백 적용

        return new Vector2(x, y); // 위치 반환
    }

    private void CreateConnections() // 노드 연결 생성 함수
    {
        for (int floor = 0; floor < floorCount - 1; floor++) // 마지막 전 층까지 반복
        {
            List<MapNodeData> currentFloorNodes = GetNodesOnFloor(floor); // 현재 층 노드 목록
            List<MapNodeData> nextFloorNodes = GetNodesOnFloor(floor + 1); // 다음 층 노드 목록

            for (int i = 0; i < currentFloorNodes.Count; i++) // 현재 층 노드 반복
            {
                MapNodeData currentNode = currentFloorNodes[i]; // 현재 노드

                if (nextFloorNodes.Count == 1) // 다음 층 노드 1개 확인
                {
                    currentNode.connectedNodeIds.Add(nextFloorNodes[0].nodeId); // 보스 노드 연결
                    continue; // 다음 노드 진행
                }

                int connectionCount = Random.Range(1, 3); // 연결 개수 생성

                for (int c = 0; c < connectionCount; c++) // 연결 개수 반복
                {
                    int targetLane = Mathf.Clamp(currentNode.lane + Random.Range(-1, 2), 0, laneCount - 1); // 목표 가로 위치
                    MapNodeData targetNode = GetNodeOnFloorAndLane(floor + 1, targetLane); // 목표 노드 검색

                    if (targetNode != null && currentNode.connectedNodeIds.Contains(targetNode.nodeId) == false) // 유효 연결 확인
                    {
                        currentNode.connectedNodeIds.Add(targetNode.nodeId); // 연결 추가
                    }
                }

                if (currentNode.connectedNodeIds.Count == 0) // 연결 없음 확인
                {
                    MapNodeData fallbackNode = nextFloorNodes[Random.Range(0, nextFloorNodes.Count)]; // 예비 노드 선택
                    currentNode.connectedNodeIds.Add(fallbackNode.nodeId); // 예비 연결 추가
                }
            }
        }
    }

    private List<MapNodeData> GetNodesOnFloor(int floor) // 특정 층 노드 목록 함수
    {
        List<MapNodeData> result = new List<MapNodeData>(); // 결과 목록 생성

        for (int i = 0; i < mapNodes.Count; i++) // 전체 노드 반복
        {
            if (mapNodes[i].floor == floor) // 같은 층 확인
            {
                result.Add(mapNodes[i]); // 결과 추가
            }
        }

        return result; // 결과 반환
    }

    private MapNodeData GetNodeOnFloorAndLane(int floor, int lane) // 층과 위치로 노드 검색 함수
    {
        for (int i = 0; i < mapNodes.Count; i++) // 전체 노드 반복
        {
            if (mapNodes[i].floor == floor && mapNodes[i].lane == lane) // 층과 위치 확인
            {
                return mapNodes[i]; // 노드 반환
            }
        }

        return null; // 없음 반환
    }

    public MapNodeData GetNodeById(int nodeId) // ID로 노드 검색 함수
    {
        for (int i = 0; i < mapNodes.Count; i++) // 전체 노드 반복
        {
            if (mapNodes[i].nodeId == nodeId) // ID 확인
            {
                return mapNodes[i]; // 노드 반환
            }
        }

        return null; // 없음 반환
    }

    public bool IsNodeSelectable(MapNodeData nodeData) // 노드 선택 가능 확인 함수
    {
        if (nodeData == null) // 노드 없음 확인
        {
            return false; // 선택 불가 반환
        }

        if (activeNodeId != -1) // 진행 중 노드 확인
        {
            return false; // 선택 불가 반환
        }

        if (nodeData.isCleared) // 클리어 노드 확인
        {
            return false; // 선택 불가 반환
        }

        if (currentClearedNodeId == -1) // 첫 선택 확인
        {
            return nodeData.floor == 0; // 첫 층만 선택 가능
        }

        MapNodeData currentNode = GetNodeById(currentClearedNodeId); // 현재 위치 노드 검색

        if (currentNode == null) // 현재 위치 없음 확인
        {
            return false; // 선택 불가 반환
        }

        return currentNode.connectedNodeIds.Contains(nodeData.nodeId); // 연결 노드 여부 반환
    }

    public void SelectNode(MapNodeData nodeData) // 노드 선택 함수
    {
        if (IsNodeSelectable(nodeData) == false) // 선택 가능 여부 확인
        {
            Debug.Log("This node is not selectable."); // 선택 불가 로그
            return; // 선택 중단
        }

        activeNodeId = nodeData.nodeId; // 현재 진행 노드 저장
        selectedNodeType = nodeData.nodeType; // 선택 노드 타입 저장
        selectedBattleNumber = nodeData.battleNumber; // 선택 전투 번호 저장
        selectedBattleType = ConvertNodeTypeToBattleType(nodeData.nodeType); // 전투 타입 변환

        Debug.Log("Selected Node : " + nodeData.nodeId + " / " + nodeData.nodeType); // 선택 로그

        if (nodeData.nodeType == StageNodeType.Rest) // 휴식 노드 확인
        {
            HandleRestNode(); // 휴식 노드 처리
            return; // 전투 씬 이동 중단
        }

        if (nodeData.nodeType == StageNodeType.Event) // 이벤트 노드 확인
        {
            HandleEventNode(); // 이벤트 노드 처리
            return; // 전투 씬 이동 중단
        }

        SceneManager.LoadScene(battleSceneName); // 전투 씬 이동
    }

    private BattleType ConvertNodeTypeToBattleType(StageNodeType nodeType) // 전투 타입 변환 함수
    {
        if (nodeType == StageNodeType.EliteBattle) // 엘리트 노드 확인
        {
            return BattleType.Elite; // 엘리트 반환
        }

        if (nodeType == StageNodeType.BossBattle) // 보스 노드 확인
        {
            return BattleType.Boss; // 보스 반환
        }

        return BattleType.Normal; // 일반 반환
    }

    private void HandleRestNode() // 휴식 노드 처리 함수
    {
        if (restNodeUIController == null) // 휴식 UI 없음 확인
        {
            restNodeUIController = FindFirstObjectByType<RestNodeUIController>(); // 씬에서 휴식 UI 검색
        }

        if (restNodeUIController == null) // 휴식 UI 검색 실패 확인
        {
            Debug.LogError("RestNodeUIController is missing in MapScene."); // 오류 로그
            activeNodeId = -1; // 진행 노드 해제
            return; // 휴식 처리 중단
        }

        restNodeUIController.Show(); // 휴식 UI 표시
    }

    private void HandleEventNode() // 이벤트 노드 처리 함수
    {
        if (eventRoomUIController == null) // 이벤트 방 UI 없음 확인
        {
            eventRoomUIController = FindFirstObjectByType<EventRoomUIController>(); // 이벤트 방 UI 검색
        }

        if (eventRoomUIController == null) // 이벤트 방 UI 검색 실패 확인
        {
            Debug.LogError("EventRoomUIController is missing in MapScene."); // 오류 로그
            activeNodeId = -1; // 진행 노드 해제
            return; // 이벤트 처리 중단
        }

        eventRoomUIController.ShowSampleEvent(); // 샘플 이벤트 표시
    }

    public void SelectRestHealChoice() // 휴식 체력 회복 선택 함수
    {
        pendingHealAmount += restHealAmount; // 대기 회복량 추가
        Debug.Log("Rest Pending Heal +" + restHealAmount); // 회복 선택 로그
        CompleteActiveNodeAndReturnToMap(); // 휴식 노드 완료
    }

    public void SelectRestShieldChoice() // 휴식 보호막 선택 함수
    {
        pendingShieldAmount += restShieldAmount; // 대기 보호막 추가
        Debug.Log("Rest Pending Shield +" + restShieldAmount); // 보호막 선택 로그
        CompleteActiveNodeAndReturnToMap(); // 휴식 노드 완료
    }

    public void SelectRestCardUpgradeChoice() // 휴식 카드 강화 선택 함수
    {
        pendingCardDamageUpgradeCount += 1; // 대기 카드 강화 추가
        Debug.Log("Rest Pending Random Card Damage Upgrade +" + restCardDamageUpgradeAmount); // 카드 강화 로그
        CompleteActiveNodeAndReturnToMap(); // 휴식 노드 완료
    }
    public void SelectEventPrayerChoice() // 이벤트 기도 선택 함수
    {
        pendingHealAmount += eventPrayerHealAmount; // 회복 대기 효과 추가
        Debug.Log("Event Pending Heal +" + eventPrayerHealAmount); // 회복 로그
        CompleteActiveNodeAndReturnToMap(); // 이벤트 노드 완료
    }

    public void SelectEventBlessingChoice() // 이벤트 축복 선택 함수
    {
        pendingShieldAmount += eventBlessingShieldAmount; // 보호막 대기 효과 추가
        Debug.Log("Event Pending Shield +" + eventBlessingShieldAmount); // 보호막 로그
        CompleteActiveNodeAndReturnToMap(); // 이벤트 노드 완료
    }

    public void SelectEventStrangeCardChoice() // 이벤트 카드 선택 함수
    {
        pendingCardDamageUpgradeCount += eventCardUpgradeCount; // 카드 강화 대기 효과 추가
        Debug.Log("Event Pending Card Upgrade +" + eventCardUpgradeCount); // 카드 강화 로그
        CompleteActiveNodeAndReturnToMap(); // 이벤트 노드 완료
    }



    public void ApplyPendingRestEffects(PlayerStats playerStats, CardManager cardManager) // 대기 휴식 효과 적용 함수
    {
        ApplyPendingHeal(playerStats); // 체력 회복 적용
        ApplyPendingShield(playerStats); // 보호막 적용
        ApplyPendingCardUpgrade(cardManager); // 카드 강화 적용
        ClearPendingRestEffects(); // 대기 효과 초기화
    }

    private void ApplyPendingHeal(PlayerStats playerStats) // 대기 체력 회복 적용 함수
    {
        if (playerStats == null) // 플레이어 스탯 없음 확인
        {
            return; // 적용 중단
        }

        if (pendingHealAmount <= 0) // 회복량 없음 확인
        {
            return; // 적용 중단
        }

        playerStats.currentHealth += pendingHealAmount; // 현재 체력 증가
        playerStats.currentHealth = Mathf.Min(playerStats.currentHealth, playerStats.maxHealth); // 최대 체력 제한
        Debug.Log("Pending Heal Applied : " + pendingHealAmount); // 적용 로그
    }

    private void ApplyPendingShield(PlayerStats playerStats) // 대기 보호막 적용 함수
    {
        if (playerStats == null) // 플레이어 스탯 없음 확인
        {
            return; // 적용 중단
        }

        if (pendingShieldAmount <= 0) // 보호막 없음 확인
        {
            return; // 적용 중단
        }

        playerStats.AddShield(pendingShieldAmount); // 보호막 추가
        Debug.Log("Pending Shield Applied : " + pendingShieldAmount); // 적용 로그
    }

    private void ApplyPendingCardUpgrade(CardManager cardManager) // 대기 카드 강화 적용 함수
    {
        if (cardManager == null) // 카드 관리자 없음 확인
        {
            return; // 적용 중단
        }

        if (pendingCardDamageUpgradeCount <= 0) // 강화 횟수 없음 확인
        {
            return; // 적용 중단
        }

        for (int i = 0; i < pendingCardDamageUpgradeCount; i++) // 강화 횟수 반복
        {
            ApplyRandomCardDamageUpgrade(cardManager); // 랜덤 카드 강화 적용
        }
    }

    private void ApplyRandomCardDamageUpgrade(CardManager cardManager) // 랜덤 카드 피해량 강화 함수
    {
        if (cardManager.ownedCards == null) // 보유 카드 목록 없음 확인
        {
            return; // 강화 중단
        }

        if (cardManager.ownedCards.Count == 0) // 보유 카드 없음 확인
        {
            Debug.LogWarning("No owned card to upgrade."); // 경고 로그
            return; // 강화 중단
        }

        int randomIndex = Random.Range(0, cardManager.ownedCards.Count); // 랜덤 카드 번호
        CardData targetCard = cardManager.ownedCards[randomIndex]; // 강화 대상 카드

        if (targetCard == null) // 대상 카드 없음 확인
        {
            return; // 강화 중단
        }

        cardManager.UpgradeCardDamage(targetCard.cardType, restCardDamageUpgradeAmount); // 카드 피해량 강화
        Debug.Log("Pending Card Upgrade Applied : " + targetCard.cardName + " +" + restCardDamageUpgradeAmount); // 적용 로그
    }

    private void ClearPendingRestEffects() // 대기 휴식 효과 초기화 함수
    {
        pendingHealAmount = 0; // 회복량 초기화
        pendingShieldAmount = 0; // 보호막 초기화
        pendingCardDamageUpgradeCount = 0; // 카드 강화 횟수 초기화
    }

    public void CompleteActiveNodeAndReturnToMap() // 현재 노드 완료 후 맵 복귀 함수
    {
        Time.timeScale = 1f; // 시간 흐름 복구

        if (activeNodeId == -1) // 진행 노드 없음 확인
        {
            SceneManager.LoadScene(mapSceneName); // 맵 씬 이동
            return; // 처리 중단
        }

        MapNodeData activeNode = GetNodeById(activeNodeId); // 진행 노드 검색

        if (activeNode != null) // 진행 노드 존재 확인
        {
            activeNode.isCleared = true; // 노드 클리어 처리
            currentClearedNodeId = activeNode.nodeId; // 현재 위치 갱신
        }

        activeNodeId = -1; // 진행 노드 초기화

        if (selectedNodeType == StageNodeType.BossBattle) // 보스 클리어 확인
        {
            Debug.Log("Chapter Clear"); // 챕터 클리어 로그
        }

        SceneManager.LoadScene(mapSceneName); // 맵 씬 이동
    }
}