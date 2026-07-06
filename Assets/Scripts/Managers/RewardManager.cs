using System.Collections.Generic;
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

    private List<RewardData> rewardPool = new List<RewardData>(); // 전체 보상 후보를 저장한다.
    private List<RewardData> currentRewards = new List<RewardData>(); // 현재 화면에 표시된 보상을 저장한다.

    private void Awake() // 오브젝트가 시작될 때 보상 풀을 만든다.
    {
        CreateRewardPool(); // 보상 후보 목록을 생성한다.
    }

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

    private void CreateRewardPool() // 전체 보상 후보를 생성한다.
    {
        rewardPool.Clear(); // 기존 보상 후보를 비운다.

        rewardPool.Add(new RewardData("Card Damage Up", "All Card Damage +5", RewardType.PixelShotDamageUp, RewardRarity.Common, 5f)); // 모든 카드 피해량 증가 보상을 추가한다.
        rewardPool.Add(new RewardData("Quick Reload", "All Card Cooldown -0.1s", RewardType.PixelShotCooldownDown, RewardRarity.Common, 0.1f)); // 모든 카드 쿨타임 감소 보상을 추가한다.
        rewardPool.Add(new RewardData("Mana Expansion", "Max MP +1", RewardType.MaxManaUp, RewardRarity.Common, 1f)); // 최대 MP 증가 보상을 추가한다.
        rewardPool.Add(new RewardData("Vital Boost", "Max HP +20", RewardType.MaxHealthUp, RewardRarity.Common, 20f)); // 최대 HP 증가 보상을 추가한다.
        rewardPool.Add(new RewardData("Shield Core", "Shield +1", RewardType.ShieldUp, RewardRarity.Common, 1f)); // 보호막 증가 보상을 추가한다.
        rewardPool.Add(new RewardData("Bullet Speed Up", "All Card Bullet Speed +1", RewardType.BulletSpeedUp, RewardRarity.Common, 1f)); // 모든 카드 탄환 속도 증가 보상을 추가한다.
        rewardPool.Add(new RewardData("Triple Focus", "All Card Damage +10", RewardType.PixelShotDamageUp, RewardRarity.Rare, 10f)); // 강한 모든 카드 피해량 증가 보상을 추가한다.
        rewardPool.Add(new RewardData("Mana Recovery", "MP Regen +0.5", RewardType.ManaRegenUp, RewardRarity.Rare, 0.5f)); // MP 회복 증가 보상을 추가한다.
        rewardPool.Add(new RewardData("Emergency Guard", "Shield +2", RewardType.ShieldUp, RewardRarity.Rare, 2f)); // 강한 보호막 보상을 추가한다.
    }

    public void ShowRewardPanel() // 보상 패널을 표시한다.
    {
        SelectRandomRewards(); // 랜덤 보상 3개를 선택한다.
        ApplyRewardsToButtons(); // 선택된 보상을 버튼에 표시한다.

        if (rewardPanel != null) // 보상 패널이 연결되어 있는지 확인한다.
        {
            rewardPanel.SetActive(true); // 보상 패널을 켠다.
        }

        Time.timeScale = 0f; // 보상을 선택하는 동안 게임 시간을 정지한다.

        Debug.Log("Random Reward UI Opened"); // 보상 UI 표시 로그를 출력한다.
    }

    public void HideRewardPanel() // 보상 패널을 숨긴다.
    {
        if (rewardPanel != null) // 보상 패널이 연결되어 있는지 확인한다.
        {
            rewardPanel.SetActive(false); // 보상 패널을 끈다.
        }
    }

    private void SelectRandomRewards() // 보상 풀에서 중복 없이 3개를 선택한다.
    {
        currentRewards.Clear(); // 현재 보상 목록을 비운다.

        List<RewardData> tempPool = new List<RewardData>(rewardPool); // 원본 보상 풀을 복사한다.

        int rewardCount = Mathf.Min(3, tempPool.Count); // 표시할 보상 수를 계산한다.

        for (int i = 0; i < rewardCount; i++) // 필요한 보상 수만큼 반복한다.
        {
            int randomIndex = Random.Range(0, tempPool.Count); // 랜덤 인덱스를 선택한다.
            RewardData selectedReward = tempPool[randomIndex]; // 선택된 보상을 가져온다.

            currentRewards.Add(selectedReward); // 현재 보상 목록에 추가한다.
            tempPool.RemoveAt(randomIndex); // 중복 방지를 위해 임시 목록에서 제거한다.
        }
    }

    private void ApplyRewardsToButtons() // 선택된 보상을 버튼에 적용한다.
    {
        if (currentRewards.Count < 3) // 보상 수가 3개보다 적은지 확인한다.
        {
            Debug.LogError("Not enough rewards to display."); // 오류 로그를 출력한다.
            return; // 버튼 적용을 중단한다.
        }

        if (rewardButton01 != null) rewardButton01.SetReward(currentRewards[0]); // 첫 번째 버튼에 보상을 설정한다.
        if (rewardButton02 != null) rewardButton02.SetReward(currentRewards[1]); // 두 번째 버튼에 보상을 설정한다.
        if (rewardButton03 != null) rewardButton03.SetReward(currentRewards[2]); // 세 번째 버튼에 보상을 설정한다.
    }

    public void SelectReward(RewardData rewardData) // 선택된 보상을 적용한다.
    {
        if (rewardData == null) // 보상 데이터가 없는지 확인한다.
        {
            Debug.LogError("Selected reward is null."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        ApplyReward(rewardData); // 보상 효과를 적용한다.
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

        Debug.Log("Reward Selected : " + rewardData.rewardName); // 선택한 보상을 로그로 출력한다.
    }

    private void ApplyReward(RewardData rewardData) // 보상 종류에 따라 효과를 적용한다.
    {
        switch (rewardData.rewardType) // 선택된 보상 종류를 확인한다.
        {
            case RewardType.PixelShotDamageUp: // 픽셀 샷 데미지 증가 보상인지 확인한다.
                ApplyPixelShotDamageUp(rewardData.value); // 픽셀 샷 데미지 증가를 적용한다.
                break; // switch문을 종료한다.

            case RewardType.PixelShotCooldownDown: // 픽셀 샷 쿨타임 감소 보상인지 확인한다.
                ApplyPixelShotCooldownDown(rewardData.value); // 픽셀 샷 쿨타임 감소를 적용한다.
                break; // switch문을 종료한다.

            case RewardType.MaxManaUp: // 최대 마나 증가 보상인지 확인한다.
                ApplyMaxManaUp(rewardData.value); // 최대 마나 증가를 적용한다.
                break; // switch문을 종료한다.

            case RewardType.MaxHealthUp: // 최대 체력 증가 보상인지 확인한다.
                ApplyMaxHealthUp(rewardData.value); // 최대 체력 증가를 적용한다.
                break; // switch문을 종료한다.

            case RewardType.ShieldUp: // 보호막 증가 보상인지 확인한다.
                ApplyShieldUp(rewardData.value); // 보호막 증가를 적용한다.
                break; // switch문을 종료한다.

            case RewardType.BulletSpeedUp: // 탄환 속도 증가 보상인지 확인한다.
                ApplyBulletSpeedUp(rewardData.value); // 탄환 속도 증가를 적용한다.
                break; // switch문을 종료한다.

            case RewardType.ManaRegenUp: // 마나 회복 증가 보상인지 확인한다.
                ApplyManaRegenUp(rewardData.value); // 마나 회복 증가를 적용한다.
                break; // switch문을 종료한다.
        }
    }

    private void ApplyPixelShotDamageUp(float value) // 픽셀 샷 데미지 증가 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        cardManager.IncreaseBulletDamage(Mathf.RoundToInt(value)); // 픽셀 샷 데미지를 증가시킨다.
    }

    private void ApplyPixelShotCooldownDown(float value) // 픽셀 샷 쿨타임 감소 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        cardManager.ReduceCooldown(value); // 픽셀 샷 쿨타임을 감소시킨다.
    }

    private void ApplyMaxManaUp(float value) // 최대 마나 증가 보상을 적용한다.
    {
        if (playerStats == null) // PlayerStats가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("PlayerStats is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        playerStats.IncreaseMaxMana(value); // 최대 마나를 증가시킨다.
    }

    private void ApplyMaxHealthUp(float value) // 최대 체력 증가 보상을 적용한다.
    {
        if (playerStats == null) // PlayerStats가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("PlayerStats is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        playerStats.IncreaseMaxHealth(Mathf.RoundToInt(value)); // 최대 체력을 증가시킨다.
    }

    private void ApplyShieldUp(float value) // 보호막 증가 보상을 적용한다.
    {
        if (playerStats == null) // PlayerStats가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("PlayerStats is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        playerStats.AddShield(Mathf.RoundToInt(value)); // 보호막을 증가시킨다.
    }

    private void ApplyBulletSpeedUp(float value) // 탄환 속도 증가 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        cardManager.IncreaseBulletSpeed(value); // 탄환 속도를 증가시킨다.
    }

    private void ApplyManaRegenUp(float value) // 마나 회복 증가 보상을 적용한다.
    {
        if (playerStats == null) // PlayerStats가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("PlayerStats is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        playerStats.IncreaseManaRegen(value); // 마나 회복 속도를 증가시킨다.
    }
}