using System.Collections; // 코루틴 사용
using System.Collections.Generic; // List 사용
using UnityEngine; // Unity 기본 기능 사용

public class RoomBattleController : MonoBehaviour // 방 전투 관리 클래스
{
    [Header("Spawn Point References")] // 스폰 포인트 참조 제목
    public Transform enemySpawnPointParent; // 적 스폰 포인트 부모

    public Transform enemyParent; // 생성된 적 부모

    [Header("Enemy Prefabs")] // 적 프리팹 제목
    public GameObject[] normalEnemyPrefabs; // 일반 전투방 적 프리팹

    public GameObject[] eliteEnemyPrefabs; // 정예 전투방 적 프리팹

    public GameObject[] midBossPrefabs; // 중간보스 프리팹

    public GameObject[] finalBossPrefabs; // 최종보스 프리팹

    [Header("Enemy Count Settings")] // 적 수 설정 제목
    public int normalEnemyCount = 3; // 일반 전투방 적 수

    public int eliteEnemyCount = 4; // 정예 전투방 적 수

    public int midBossEnemyCount = 1; // 중간보스 수

    public int finalBossEnemyCount = 1; // 최종보스 수

    [Header("Check Settings")] // 확인 설정 제목
    public float clearCheckInterval = 0.2f; // 클리어 확인 간격

    private RoomData currentRoomData; // 현재 방 데이터

    private RoomController roomController; // 방 컨트롤러

    private RoomMapManager roomMapManager; // 방 맵 관리자

    private readonly List<EnemyHealth> aliveEnemies = new List<EnemyHealth>(); // 살아있는 적 목록

    private bool isBattleRunning = false; // 전투 진행 여부
    
    [Header("Next Floor Portal")] // 다음층 포탈 제목
    public GameObject nextFloorPortalPrefab; // 다음층 이동 포탈 프리팹

    public Transform portalSpawnPoint; // 포탈 생성 위치

    private GameObject spawnedPortalObject; // 생성된 포탈 오브젝트
    public void Initialize(RoomData roomData, RoomController controller, RoomMapManager mapManager) // 전투 초기화 함수
    {
        currentRoomData = roomData; // 현재 방 데이터 저장
        roomController = controller; // 방 컨트롤러 저장
        roomMapManager = mapManager; // 맵 관리자 저장

        StopAllCoroutines(); // 기존 코루틴 정지
        ClearSpawnedEnemies(); // 기존 적 제거
        ClearSpawnedPortal(); // 기존 포탈 제거
        aliveEnemies.Clear(); // 적 목록 초기화

        isBattleRunning = false; // 전투 상태 초기화

        if (currentRoomData == null) // 방 데이터 확인
        {
            UnlockDoors(); // 문 잠금 해제
            return; // 초기화 중단
        }

        if (!currentRoomData.IsBattleRoom()) // 전투방이 아닌지 확인
        {
            UnlockDoors(); // 문 잠금 해제
            return; // 전투 시작 안 함
        }

        if (currentRoomData.isCleared) // 이미 클리어한 방인지 확인
        {
            UnlockDoors(); // 문 잠금 해제
            return; // 전투 시작 안 함
        }

        StartRoomBattle(); // 방 전투 시작
    }

    private void StartRoomBattle() // 방 전투 시작 함수
    {
        isBattleRunning = true; // 전투 진행 상태 설정

        LockDoors(); // 문 잠금

        SpawnEnemiesByRoomType(); // 방 타입에 맞는 적 생성

        if (aliveEnemies.Count == 0) // 생성된 적이 없는지 확인
        {
            CompleteRoomBattle(); // 적이 없으면 바로 클리어 처리
            return; // 전투 시작 중단
        }

        StartCoroutine(CheckBattleClearRoutine()); // 전투 클리어 확인 시작

        Debug.Log("Room Battle Start : " + currentRoomData.roomType); // 전투 시작 로그
    }

    private void SpawnEnemiesByRoomType() // 방 타입에 맞는 적 생성 함수
    {
        if (currentRoomData.roomType == RoomType.NormalBattle) // 일반 전투방 확인
        {
            SpawnEnemies(normalEnemyPrefabs, normalEnemyCount); // 일반 적 생성
            return; // 생성 종료
        }

        if (currentRoomData.roomType == RoomType.EliteBattle) // 정예 전투방 확인
        {
            SpawnEnemies(eliteEnemyPrefabs, eliteEnemyCount); // 정예 적 생성
            return; // 생성 종료
        }

        if (currentRoomData.roomType == RoomType.MidBoss) // 중간보스방 확인
        {
            SpawnEnemies(midBossPrefabs, midBossEnemyCount); // 중간보스 생성
            return; // 생성 종료
        }

        if (currentRoomData.roomType == RoomType.FinalBoss) // 최종보스방 확인
        {
            SpawnEnemies(finalBossPrefabs, finalBossEnemyCount); // 최종보스 생성
        }
    }

    private void SpawnEnemies(GameObject[] enemyPrefabs, int enemyCount) // 적 생성 함수
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) // 적 프리팹 확인
        {
            Debug.LogWarning("Enemy Prefabs are missing for room battle."); // 경고 로그
            return; // 생성 중단
        }

        List<Transform> spawnPoints = GetEnemySpawnPoints(); // 적 스폰 포인트 가져오기

        if (spawnPoints.Count == 0) // 스폰 포인트 확인
        {
            Debug.LogWarning("Enemy Spawn Points are empty."); // 경고 로그
            return; // 생성 중단
        }

        for (int i = 0; i < enemyCount; i++) // 적 수만큼 반복
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Count]; // 스폰 위치 선택

            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; // 랜덤 적 프리팹 선택

            SpawnEnemy(enemyPrefab, spawnPoint); // 적 생성
        }
    }

    private List<Transform> GetEnemySpawnPoints() // 적 스폰 포인트 목록 반환 함수
    {
        List<Transform> spawnPoints = new List<Transform>(); // 스폰 포인트 목록 생성

        if (enemySpawnPointParent == null) // 스폰 포인트 부모 확인
        {
            return spawnPoints; // 빈 목록 반환
        }

        for (int i = 0; i < enemySpawnPointParent.childCount; i++) // 자식 수만큼 반복
        {
            Transform child = enemySpawnPointParent.GetChild(i); // 자식 가져오기

            spawnPoints.Add(child); // 목록에 추가
        }

        return spawnPoints; // 목록 반환
    }

    private void SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint) // 적 하나 생성 함수
    {
        if (enemyPrefab == null) // 적 프리팹 확인
        {
            return; // 생성 중단
        }

        if (spawnPoint == null) // 스폰 포인트 확인
        {
            return; // 생성 중단
        }

        Transform parent = enemyParent != null ? enemyParent : transform; // 적 부모 결정

        GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, parent); // 적 생성

        EnemyHealth enemyHealth = enemyObject.GetComponent<EnemyHealth>(); // 적 체력 가져오기

        if (enemyHealth == null) // 적 체력 컴포넌트 확인
        {
            Debug.LogWarning("EnemyHealth is missing on spawned enemy : " + enemyObject.name); // 경고 로그
            return; // 목록 추가 중단
        }

        aliveEnemies.Add(enemyHealth); // 살아있는 적 목록에 추가
    }

    private IEnumerator CheckBattleClearRoutine() // 전투 클리어 확인 코루틴
    {
        while (isBattleRunning) // 전투 중 반복
        {
            RemoveDeadEnemiesFromList(); // 죽은 적 목록 제거

            if (aliveEnemies.Count == 0) // 살아있는 적이 없는지 확인
            {
                CompleteRoomBattle(); // 방 전투 클리어
                yield break; // 코루틴 종료
            }

            yield return new WaitForSeconds(clearCheckInterval); // 다음 확인까지 대기
        }
    }

    private void RemoveDeadEnemiesFromList() // 죽은 적 제거 함수
    {
        for (int i = aliveEnemies.Count - 1; i >= 0; i--) // 뒤에서부터 반복
        {
            EnemyHealth enemy = aliveEnemies[i]; // 적 가져오기

            if (enemy == null) // 적 오브젝트가 삭제되었는지 확인
            {
                aliveEnemies.RemoveAt(i); // 목록에서 제거
                continue; // 다음 적 확인
            }

            if (enemy.currentHealth <= 0) // 체력이 0 이하인지 확인
            {
                aliveEnemies.RemoveAt(i); // 목록에서 제거
            }
        }
    }

    private void CompleteRoomBattle() // 방 전투 클리어 함수
    {
        if (!isBattleRunning) // 전투 진행 상태 확인
        {
            return; // 중복 클리어 방지
        }

        isBattleRunning = false; // 전투 종료 상태 설정

        if (currentRoomData != null) // 현재 방 데이터 확인
        {
            currentRoomData.isVisited = true; // 방문 처리
            currentRoomData.isCleared = true; // 클리어 처리
        }

        UnlockDoors(); // 문 잠금 해제

        if (roomMapManager != null) // 맵 매니저 확인
        {
            roomMapManager.RefreshMinimap(); // 미니맵 갱신
        }

        if (ShouldSpawnNextFloorPortal()) // 다음층 포탈 생성 조건 확인
        {
            SpawnNextFloorPortal(); // 다음층 포탈 생성
        }

        Debug.Log("Room Battle Clear"); // 방 클리어 로그
    }

    private void LockDoors() // 문 잠금 함수
    {
        if (roomController != null) // 방 컨트롤러 확인
        {
            roomController.SetDoorsLocked(true); // 모든 문 잠금
        }
    }

    private void UnlockDoors() // 문 잠금 해제 함수
    {
        if (roomController != null) // 방 컨트롤러 확인
        {
            roomController.SetDoorsLocked(false); // 모든 문 잠금 해제
        }
    }

    private void ClearSpawnedEnemies() // 기존 적 제거 함수
    {
        if (enemyParent == null) // 적 부모 확인
        {
            return; // 제거 중단
        }

        for (int i = enemyParent.childCount - 1; i >= 0; i--) // 적 부모 자식 수만큼 반복
        {
            Transform child = enemyParent.GetChild(i); // 자식 가져오기

            Destroy(child.gameObject); // 적 오브젝트 삭제
        }
    }

    private bool ShouldSpawnNextFloorPortal() // 다음층 포탈을 생성해야 하는지 확인하는 함수
    {
        if (currentRoomData == null) // 현재 방 데이터 확인
        {
            return false; // 포탈 생성 안 함
        }

        if (!currentRoomData.isEndRoom) // 현재 층 끝방인지 확인
        {
            return false; // 포탈 생성 안 함
        }

        if (spawnedPortalObject != null) // 이미 포탈이 있는지 확인
        {
            return false; // 중복 생성 방지
        }

        return true; // 포탈 생성 가능
    }

    private void SpawnNextFloorPortal() // 다음층 포탈 생성 함수
    {
        if (nextFloorPortalPrefab == null) // 포탈 프리팹 확인
        {
            Debug.LogWarning("Next Floor Portal Prefab is missing."); // 경고 로그
            return; // 생성 중단
        }

        Vector3 spawnPosition = transform.position; // 기본 생성 위치 설정

        if (portalSpawnPoint != null) // 포탈 생성 위치 확인
        {
            spawnPosition = portalSpawnPoint.position; // 지정된 위치 사용
        }

        Transform parent = enemyParent != null ? enemyParent : transform; // 포탈 부모 결정

        spawnedPortalObject = Instantiate(nextFloorPortalPrefab, spawnPosition, Quaternion.identity, parent); // 포탈 생성

        RoomPortal roomPortal = spawnedPortalObject.GetComponent<RoomPortal>(); // 포탈 스크립트 가져오기

        if (roomPortal != null) // 포탈 스크립트 확인
        {
            RoomSceneController sceneController = FindFirstObjectByType<RoomSceneController>(); // 방 씬 관리자 검색

            roomPortal.Initialize(sceneController); // 포탈 초기화
        }

        Debug.Log("Next Floor Portal Spawned"); // 포탈 생성 로그
    }

    private void ClearSpawnedPortal() // 기존 포탈 제거 함수
    {
        if (spawnedPortalObject == null) // 생성된 포탈 확인
        {
            return; // 제거 중단
        }

        Destroy(spawnedPortalObject); // 포탈 제거

        spawnedPortalObject = null; // 포탈 참조 초기화
    }
}