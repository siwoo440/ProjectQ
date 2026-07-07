using UnityEngine;

public class NextBattleUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject nextBattlePanel; // 실제로 켜고 끌 Next Battle 패널을 저장한다.

    private void Awake() // 오브젝트가 생성될 때 실행한다.
    {
        Hide(); // 게임 시작 시 Next Battle 패널을 숨긴다.
    }

    public void Show() // 다음 전투 버튼 UI를 표시한다.
    {
        if (nextBattlePanel == null) // 패널이 연결되지 않았는지 확인한다.
        {
            Debug.LogError("NextBattleUI : nextBattlePanel is not assigned."); // 연결 오류를 출력한다.
            return; // 표시 처리를 중단한다.
        }

        nextBattlePanel.SetActive(true); // Next Battle 패널을 활성화한다.
        Debug.Log("NextBattleUI Show"); // Show 호출 로그를 출력한다.
    }

    public void Hide() // 다음 전투 버튼 UI를 숨긴다.
    {
        if (nextBattlePanel == null) // 패널이 연결되지 않았는지 확인한다.
        {
            return; // 아직 연결 전이면 숨김 처리를 하지 않는다.
        }

        nextBattlePanel.SetActive(false); // Next Battle 패널을 비활성화한다.
        Debug.Log("NextBattleUI Hide"); // Hide 호출 로그를 출력한다.
    }
}