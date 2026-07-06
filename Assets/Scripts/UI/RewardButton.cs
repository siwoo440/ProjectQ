using TMPro;
using UnityEngine;

public class RewardButton : MonoBehaviour
{
    [Header("Reward Data")]
    public RewardData rewardData; // 이 버튼이 가진 보상 데이터를 저장한다.

    [Header("References")]
    public RewardManager rewardManager; // 보상을 처리할 RewardManager를 저장한다.
    public TextMeshProUGUI rewardText; // 버튼에 표시할 텍스트를 저장한다.

    public void SetReward(RewardData newRewardData) // 버튼의 보상 정보를 설정한다.
    {
        rewardData = newRewardData; // 보상 데이터를 저장한다.

        if (rewardText != null && rewardData != null) // 보상 텍스트와 데이터가 있는지 확인한다.
        {
            rewardText.text = rewardData.GetDisplayText(); // 보상 표시 문장을 설정한다.
        }
    }

    public void OnClickReward() // 보상 버튼을 클릭했을 때 실행한다.
    {
        if (rewardManager == null) // RewardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("RewardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 처리를 중단한다.
        }

        if (rewardData == null) // 보상 데이터가 없는지 확인한다.
        {
            Debug.LogError("RewardData is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 처리를 중단한다.
        }

        rewardManager.SelectReward(rewardData); // RewardManager에게 선택된 보상 데이터를 전달한다.
    }
}