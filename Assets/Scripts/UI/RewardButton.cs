using TMPro;
using UnityEngine;

public class RewardButton : MonoBehaviour
{
    public enum RewardType // 보상 종류를 정의한다.
    {
        PixelShotDamageUp, // 픽셀 샷 데미지 증가 보상이다.
        PixelShotCooldownDown, // 픽셀 샷 쿨타임 감소 보상이다.
        MaxManaUp // 최대 마나 증가 보상이다.
    }

    [Header("Reward Data")]
    public RewardType rewardType; // 이 버튼이 가진 보상 종류를 저장한다.

    [Header("References")]
    public RewardManager rewardManager; // 보상을 처리할 RewardManager를 저장한다.
    public TextMeshProUGUI rewardText; // 버튼에 표시할 텍스트를 저장한다.

    public void SetReward(RewardType newRewardType, string title, string description) // 버튼의 보상 정보를 설정한다.
    {
        rewardType = newRewardType; // 보상 종류를 저장한다.

        if (rewardText != null) // 보상 텍스트가 연결되어 있는지 확인한다.
        {
            rewardText.text = title + "\n\n" + description; // 제목과 설명을 버튼에 표시한다.
        }
    }

    public void OnClickReward() // 보상 버튼을 클릭했을 때 실행한다.
    {
        if (rewardManager == null) // RewardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("RewardManager가 연결되지 않았습니다."); // 오류 로그를 출력한다.
            return; // 보상 처리를 중단한다.
        }

        rewardManager.SelectReward(rewardType); // RewardManager에게 선택된 보상을 전달한다.
    }
}