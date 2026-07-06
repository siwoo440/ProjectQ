using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject rewardPanel; // 보상 패널 오브젝트를 저장한다.
    public RewardButton rewardButton01; // 첫 번째 보상 버튼을 저장한다.
    public RewardButton rewardButton02; // 두 번째 보상 버튼을 저장한다.
    public RewardButton rewardButton03; // 세 번째 보상 버튼을 저장한다.
    public NextBattleUI nextBattleUI; // 다음 전투 버튼 UI를 저장한다.

    [Header("Target References")]
    public CardManager cardManager; // 카드 수치를 변경할 CardManager를 저장한다.
    public PlayerStats playerStats; // 플레이어 수치를 변경할 PlayerStats를 저장한다.

    private void Start() // 시작 시 보상 UI를 숨긴다.
    {
        HideRewardPanel(); // 보상 패널을 숨긴다.
        ConnectButtons(); // 보상 버튼에 RewardManager를 연결한다.
    }

    private void ConnectButtons() // 각 보상 버튼에 RewardManager를 연결한다.
    {
        if (rewardButton01 != null) rewardButton01.rewardManager = this; // 첫 번째 버튼에 RewardManager를 연결한다.
        if (rewardButton02 != null) rewardButton02.rewardManager = this; // 두 번째 버튼에 RewardManager를 연결한다.
        if (rewardButton03 != null) rewardButton03.rewardManager = this; // 세 번째 버튼에 RewardManager를 연결한다.
    }

    public void ShowRewardPanel() // 보상 패널을 표시한다.
    {
        SetupFixedRewards(); // 고정 보상 3개를 설정한다.

        if (rewardPanel != null) // 보상 패널이 연결되어 있는지 확인한다.
        {
            rewardPanel.SetActive(true); // 보상 패널을 켠다.
        }

        Time.timeScale = 0f; // 보상을 선택하는 동안 게임 시간을 정지한다.

        Debug.Log("Reward UI Opened"); // 보상 UI 표시 로그를 출력한다.
    }

    public void HideRewardPanel() // 보상 패널을 숨긴다.
    {
        if (rewardPanel != null) // 보상 패널이 연결되어 있는지 확인한다.
        {
            rewardPanel.SetActive(false); // 보상 패널을 끈다.
        }
    }

    private void SetupFixedRewards() // 고정 보상 3개를 설정한다.
    {
        if (rewardButton01 != null) // 첫 번째 보상 버튼이 연결되어 있는지 확인한다.
        {
            rewardButton01.SetReward(RewardButton.RewardType.PixelShotDamageUp, "Pixel Shot Upgrade", "Pixel Shot Damage +5"); // 첫 번째 보상을 설정한다.
        }

        if (rewardButton02 != null) // 두 번째 보상 버튼이 연결되어 있는지 확인한다.
        {
            rewardButton02.SetReward(RewardButton.RewardType.PixelShotCooldownDown, "Quick Reload", "Pixel Shot Cooldown -0.1s"); // 두 번째 보상을 설정한다.
        }

        if (rewardButton03 != null) // 세 번째 보상 버튼이 연결되어 있는지 확인한다.
        {
            rewardButton03.SetReward(RewardButton.RewardType.MaxManaUp, "Mana Expansion", "Max MP +1"); // 세 번째 보상을 설정한다.
        }
    }

    public void SelectReward(RewardButton.RewardType rewardType) // 선택된 보상을 적용한다.
    {
        ApplyReward(rewardType); // 보상 효과를 적용한다.
        HideRewardPanel(); // 보상 패널을 숨긴다.

        Time.timeScale = 1f; // 게임 시간을 다시 흐르게 한다.

        if (nextBattleUI != null) // 다음 전투 UI가 연결되어 있는지 확인한다.
        {
            nextBattleUI.Show(); // Next Battle 버튼을 표시한다.
        }
        else // 다음 전투 UI가 없는 경우를 처리한다.
        {
            Debug.LogWarning("NextBattleUI is not assigned."); // 경고 로그를 출력한다.
        }

        Debug.Log("Reward Selected"); // 보상 선택 완료 로그를 출력한다.
    }

    private void ApplyReward(RewardButton.RewardType rewardType) // 보상 종류에 따라 효과를 적용한다.
    {
        switch (rewardType) // 선택된 보상 종류를 확인한다.
        {
            case RewardButton.RewardType.PixelShotDamageUp: // 픽셀 샷 데미지 증가 보상인지 확인한다.
                ApplyPixelShotDamageUp(); // 픽셀 샷 데미지 증가를 적용한다.
                break; // switch문을 종료한다.

            case RewardButton.RewardType.PixelShotCooldownDown: // 픽셀 샷 쿨타임 감소 보상인지 확인한다.
                ApplyPixelShotCooldownDown(); // 픽셀 샷 쿨타임 감소를 적용한다.
                break; // switch문을 종료한다.

            case RewardButton.RewardType.MaxManaUp: // 최대 마나 증가 보상인지 확인한다.
                ApplyMaxManaUp(); // 최대 마나 증가를 적용한다.
                break; // switch문을 종료한다.
        }
    }

    private void ApplyPixelShotDamageUp() // 픽셀 샷 데미지 증가 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        cardManager.IncreaseBulletDamage(5); // 픽셀 샷 데미지를 5 증가시킨다.
    }

    private void ApplyPixelShotCooldownDown() // 픽셀 샷 쿨타임 감소 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        cardManager.ReduceCooldown(0.1f); // 픽셀 샷 쿨타임을 0.1초 감소시킨다.
    }

    private void ApplyMaxManaUp() // 최대 마나 증가 보상을 적용한다.
    {
        if (playerStats == null) // PlayerStats가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("PlayerStats is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        playerStats.IncreaseMaxMana(1f); // 최대 마나를 1 증가시킨다.
    }
}