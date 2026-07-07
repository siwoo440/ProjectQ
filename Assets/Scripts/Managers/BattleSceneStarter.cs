using System.Collections; // 코루틴 사용
using UnityEngine; // Unity 기본 기능 사용

public class BattleSceneStarter : MonoBehaviour // 전투 씬 시작 관리 클래스
{
    [Header("Manager References")] // 관리자 참조 제목
    public BattleManager battleManager; // 전투 관리자
    public EnemySpawner enemySpawner; // 적 스폰 관리자
    public RelicManager relicManager; // 유물 관리자
    public PlayerStats playerStats; // 플레이어 스탯
    public CardManager cardManager; // 카드 관리자

    private IEnumerator Start() // 전투 씬 시작 코루틴
    {
        FindReferencesIfNeeded(); // 필요한 참조 검색
        yield return null; // 다른 Start 실행 대기
        FindReferencesIfNeeded(); // 한 번 더 참조 검색
        StartSelectedBattle(); // 선택 전투 시작
    }

    private void FindReferencesIfNeeded() // 필요한 참조 자동 검색 함수
    {
        if (battleManager == null) // 전투 관리자 없음 확인
        {
            battleManager = FindFirstObjectByType<BattleManager>(); // 전투 관리자 검색
        }

        if (enemySpawner == null) // 적 스폰 관리자 없음 확인
        {
            enemySpawner = FindFirstObjectByType<EnemySpawner>(); // 적 스폰 관리자 검색
        }

        if (relicManager == null) // 유물 관리자 없음 확인
        {
            relicManager = FindFirstObjectByType<RelicManager>(); // 유물 관리자 검색
        }

        if (playerStats == null) // 플레이어 스탯 없음 확인
        {
            playerStats = FindFirstObjectByType<PlayerStats>(); // 플레이어 스탯 검색
        }

        if (cardManager == null) // 카드 관리자 없음 확인
        {
            cardManager = FindFirstObjectByType<CardManager>(); // 카드 관리자 검색
        }
    }

    private void StartSelectedBattle() // 선택 전투 시작 함수
    {
        if (GameFlowManager.Instance == null) // 진행 관리자 없음 확인
        {
            Debug.LogWarning("GameFlowManager is missing. BattleSceneStarter stopped."); // 경고 로그
            return; // 시작 중단
        }

        if (enemySpawner != null) // 적 스폰 관리자 확인
        {
            enemySpawner.SetBattleType(GameFlowManager.Instance.selectedBattleType); // 전투 타입 적용
        }

        GameFlowManager.Instance.ApplyPendingRestEffects(playerStats, cardManager); // 휴식 대기 효과 적용

        if (relicManager != null) // 유물 관리자 확인
        {
            relicManager.ApplyRelicsOnBattleStart(); // 전투 시작 유물 적용
        }

        if (battleManager == null) // 전투 관리자 없음 확인
        {
            Debug.LogError("BattleManager is not assigned."); // 오류 로그
            return; // 시작 중단
        }

        Debug.Log("Battle Scene Start : " + GameFlowManager.Instance.selectedNodeType + " / " + GameFlowManager.Instance.selectedBattleType); // 전투 시작 로그
        battleManager.StartBattle(GameFlowManager.Instance.selectedBattleNumber); // 전투 시작
    }
}