using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel; // 게임 오버 패널 오브젝트를 저장한다.

    [Header("Target References")]
    public BattleProgressManager battleProgressManager; // 전투 진행 관리자를 저장한다.

    private void Start() // 시작 시 게임 오버 UI를 숨긴다.
    {
        Hide(); // 게임 오버 패널을 숨긴다.
    }

    public void Show() // 게임 오버 UI를 표시한다.
    {
        if (gameOverPanel != null) // 게임 오버 패널이 연결되어 있는지 확인한다.
        {
            gameOverPanel.SetActive(true); // 게임 오버 패널을 켠다.
        }

        Debug.Log("Game Over UI Opened"); // 게임 오버 UI 표시 로그를 출력한다.
    }

    public void Hide() // 게임 오버 UI를 숨긴다.
    {
        if (gameOverPanel != null) // 게임 오버 패널이 연결되어 있는지 확인한다.
        {
            gameOverPanel.SetActive(false); // 게임 오버 패널을 끈다.
        }
    }

    public void OnClickRetry() // Retry 버튼을 클릭했을 때 실행한다.
    {
        if (battleProgressManager == null) // BattleProgressManager가 연결되지 않았는지 확인한다.
        {
            battleProgressManager = FindFirstObjectByType<BattleProgressManager>(); // 씬에서 BattleProgressManager를 찾는다.
        }

        if (battleProgressManager == null) // 자동으로도 찾지 못했는지 확인한다.
        {
            Debug.LogError("BattleProgressManager is not assigned."); // 오류 로그를 출력한다.
            return; // 재시작을 중단한다.
        }

        Hide(); // 게임 오버 UI를 숨긴다.
        battleProgressManager.RetryGame(); // 게임을 처음부터 다시 시작한다.
    }
}