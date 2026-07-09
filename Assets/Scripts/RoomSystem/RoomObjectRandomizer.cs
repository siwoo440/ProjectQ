using System.Collections.Generic; // List 사용
using UnityEngine; // Unity 기본 기능 사용

public class RoomObjectRandomizer : MonoBehaviour // 방 내부 구조물 랜덤 배치 클래스
{
    [Header("Spawn Point References")] // 스폰 위치 참조 제목
    public Transform obstacleSpawnPointParent; // 구조물 스폰 포인트 부모

    public Transform obstacleParent; // 생성된 구조물 부모

    [Header("Obstacle Prefabs")] // 구조물 프리팹 제목
    public GameObject[] obstaclePrefabs; // 랜덤 생성할 구조물 프리팹 배열

    [Header("Spawn Count Settings")] // 생성 개수 설정 제목
    public int minObstacleCount = 2; // 최소 구조물 개수

    public int maxObstacleCount = 5; // 최대 구조물 개수

    [Header("Random Settings")] // 랜덤 설정 제목
    public bool clearExistingObjects = true; // 기존 구조물 제거 여부

    public bool randomRotation = false; // 랜덤 회전 여부

    public void RandomizeObjects() // 구조물 랜덤 배치 실행 함수
    {
        if (clearExistingObjects) // 기존 구조물 제거 여부 확인
        {
            ClearSpawnedObjects(); // 기존 구조물 제거
        }

        if (obstacleSpawnPointParent == null) // 스폰 포인트 부모 확인
        {
            Debug.LogWarning("Obstacle Spawn Point Parent is missing."); // 경고 로그
            return; // 랜덤 배치 중단
        }

        if (obstacleParent == null) // 구조물 부모 확인
        {
            obstacleParent = transform; // 부모가 없으면 자기 자신 사용
        }

        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) // 구조물 프리팹 확인
        {
            Debug.LogWarning("Obstacle Prefabs are missing."); // 경고 로그
            return; // 랜덤 배치 중단
        }

        List<Transform> spawnPoints = GetSpawnPoints(); // 스폰 포인트 목록 가져오기

        if (spawnPoints.Count == 0) // 스폰 포인트 개수 확인
        {
            Debug.LogWarning("Obstacle Spawn Points are empty."); // 경고 로그
            return; // 랜덤 배치 중단
        }

        int spawnCount = Random.Range(minObstacleCount, maxObstacleCount + 1); // 생성할 구조물 개수 결정

        spawnCount = Mathf.Clamp(spawnCount, 0, spawnPoints.Count); // 스폰 포인트 수보다 많지 않게 보정

        ShuffleSpawnPoints(spawnPoints); // 스폰 포인트 순서 섞기

        for (int i = 0; i < spawnCount; i++) // 생성 개수만큼 반복
        {
            Transform spawnPoint = spawnPoints[i]; // 사용할 스폰 포인트 선택

            GameObject obstaclePrefab = GetRandomObstaclePrefab(); // 랜덤 구조물 프리팹 선택

            SpawnObstacle(obstaclePrefab, spawnPoint); // 구조물 생성
        }
    }

    private List<Transform> GetSpawnPoints() // 스폰 포인트 목록 반환 함수
    {
        List<Transform> spawnPoints = new List<Transform>(); // 스폰 포인트 목록 생성

        for (int i = 0; i < obstacleSpawnPointParent.childCount; i++) // 자식 수만큼 반복
        {
            Transform child = obstacleSpawnPointParent.GetChild(i); // 자식 Transform 가져오기

            spawnPoints.Add(child); // 목록에 추가
        }

        return spawnPoints; // 목록 반환
    }

    private void ShuffleSpawnPoints(List<Transform> spawnPoints) // 스폰 포인트 순서 섞기 함수
    {
        for (int i = 0; i < spawnPoints.Count; i++) // 스폰 포인트 수만큼 반복
        {
            int randomIndex = Random.Range(i, spawnPoints.Count); // 랜덤 인덱스 선택

            Transform temp = spawnPoints[i]; // 현재 값 임시 저장

            spawnPoints[i] = spawnPoints[randomIndex]; // 랜덤 위치 값을 현재 위치로 이동

            spawnPoints[randomIndex] = temp; // 임시 저장 값을 랜덤 위치로 이동
        }
    }

    private GameObject GetRandomObstaclePrefab() // 랜덤 구조물 프리팹 반환 함수
    {
        int randomIndex = Random.Range(0, obstaclePrefabs.Length); // 랜덤 인덱스 선택

        return obstaclePrefabs[randomIndex]; // 랜덤 구조물 프리팹 반환
    }

    private void SpawnObstacle(GameObject obstaclePrefab, Transform spawnPoint) // 구조물 생성 함수
    {
        if (obstaclePrefab == null) // 구조물 프리팹 확인
        {
            return; // 생성 중단
        }

        if (spawnPoint == null) // 스폰 포인트 확인
        {
            return; // 생성 중단
        }

        Quaternion spawnRotation = Quaternion.identity; // 기본 회전값 설정

        if (randomRotation) // 랜덤 회전 여부 확인
        {
            float randomZRotation = Random.Range(0, 4) * 90f; // 0, 90, 180, 270 중 하나 선택

            spawnRotation = Quaternion.Euler(0f, 0f, randomZRotation); // 랜덤 회전값 설정
        }

        GameObject obstacleObject = Instantiate(obstaclePrefab, spawnPoint.position, spawnRotation, obstacleParent); // 구조물 생성

        obstacleObject.name = obstaclePrefab.name; // 생성된 오브젝트 이름 정리
    }

    private void ClearSpawnedObjects() // 기존 구조물 제거 함수
    {
        if (obstacleParent == null) // 구조물 부모 확인
        {
            return; // 제거 중단
        }

        for (int i = obstacleParent.childCount - 1; i >= 0; i--) // 뒤에서부터 자식 반복
        {
            Transform child = obstacleParent.GetChild(i); // 자식 가져오기

            Destroy(child.gameObject); // 자식 오브젝트 삭제
        }
    }
}