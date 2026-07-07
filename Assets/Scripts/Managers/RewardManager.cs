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
    public RelicManager relicManager; // 유물 획득을 처리할 RelicManager를 저장한다.

    [Header("Rarity Weight Settings")] // 전체 100 중 : Common 70,Rare 25,Epic 5
    public int commonWeight = 70; // Common 등급 보상 등장 가중치를 저장한다.
    public int rareWeight = 25; // Rare 등급 보상 등장 가중치를 저장한다.
    public int epicWeight = 5; // Epic 등급 보상 등장 가중치를 저장한다.
   

    private List<RewardData> rewardPool = new List<RewardData>(); // 전체 보상 후보를 저장한다.
    private List<RewardData> currentRewards = new List<RewardData>(); // 현재 화면에 표시된 보상을 저장한다.

    private void Awake() // 오브젝트가 시작될 때 보상 풀을 만든다.
    {
        CreateRewardPool(); // 보상 후보 목록을 생성한다.
    }

    private void Start() // 시작 시 보상 UI를 숨긴다.
    {
        HideRewardPanel(); // 보상 패널을 숨긴다.
    }

    private void ConnectButtons() // 각 보상 버튼에 RewardManager를 연결한다.
    {
        if (rewardButton01 != null) rewardButton01.Setup(currentRewards[0], this, 0); // 첫 번째 버튼에 보상을 설정한다.
        if (rewardButton02 != null) rewardButton02.Setup(currentRewards[1], this, 1); // 두 번째 버튼에 보상을 설정한다.
        if (rewardButton03 != null) rewardButton03.Setup(currentRewards[2], this, 2); // 세 번째 버튼에 보상을 설정한다.
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
        rewardPool.Add(new RewardData("Upgrade: Pixel Shot", "Pixel Shot Damage +3", RewardType.UpgradePixelShotDamage, RewardRarity.Common, 3f)); // Pixel Shot 강화 보상을 추가한다.

        rewardPool.Add(new RewardData("Triple Focus", "All Card Damage +10", RewardType.PixelShotDamageUp, RewardRarity.Rare, 10f)); // 강한 모든 카드 피해량 증가 보상을 추가한다.
        rewardPool.Add(new RewardData("Mana Recovery", "MP Regen +0.5", RewardType.ManaRegenUp, RewardRarity.Rare, 0.5f)); // MP 회복 증가 보상을 추가한다.
        rewardPool.Add(new RewardData("Emergency Guard", "Shield +2", RewardType.ShieldUp, RewardRarity.Rare, 2f)); // 강한 보호막 보상을 추가한다.
        rewardPool.Add(new RewardData("New Card: Rapid Shot", "Add Rapid Shot to your deck", RewardType.NewRapidShot, RewardRarity.Rare, 0f)); // Rapid Shot 획득 보상을 추가한다.
        rewardPool.Add(new RewardData("New Card: Heavy Shot", "Add Heavy Shot to your deck", RewardType.NewHeavyShot, RewardRarity.Rare, 0f)); // Heavy Shot 획득 보상을 추가한다.
        rewardPool.Add(new RewardData("Upgrade: Focus Shot", "Focus Shot Damage +10", RewardType.UpgradeFocusShotDamage, RewardRarity.Rare, 10f)); // Focus Shot 강화 보상을 추가한다.
        rewardPool.Add(new RewardData("Upgrade: Wide Shot", "Wide Shot Bullet Count +1", RewardType.UpgradeWideShotBulletCount, RewardRarity.Rare, 1f)); // Wide Shot 탄환 수 강화 보상을 추가한다.
        rewardPool.Add(new RewardData("Upgrade: Rapid Shot", "Rapid Shot Cooldown -0.05s", RewardType.UpgradeRapidShotCooldown, RewardRarity.Rare, 0.05f)); // Rapid Shot 쿨타임 강화 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Blood Core", "All Card Damage +2", RewardType.RelicBloodCore, RewardRarity.Rare, 2f)); // Blood Core 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Mana Stone", "MP Regen +0.3", RewardType.RelicManaStone, RewardRarity.Rare, 0.3f)); // Mana Stone 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Shield Fragment", "Gain Shield +1 at battle start", RewardType.RelicShieldFragment, RewardRarity.Rare, 1f)); // Shield Fragment 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Quick Gear", "All Card Cooldown -0.05s", RewardType.RelicQuickGear, RewardRarity.Rare, 0.05f)); // Quick Gear 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Bullet Engine", "All Card Bullet Speed +0.5", RewardType.RelicBulletEngine, RewardRarity.Rare, 0.5f)); // Bullet Engine 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Pixel Lens", "Pixel Shot Damage +5", RewardType.RelicPixelLens, RewardRarity.Rare, 5f)); // Pixel Lens 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Focus Lens", "Focus Shot Damage +15", RewardType.RelicFocusLens, RewardRarity.Rare, 15f)); // Focus Lens 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Wide Barrel", "Wide Shot Bullet Count +2", RewardType.RelicWideBarrel, RewardRarity.Rare, 2f)); // Wide Barrel 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Rapid Battery", "Rapid Shot Cooldown -0.08s", RewardType.RelicRapidBattery, RewardRarity.Rare, 0.08f)); // Rapid Battery 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("New Card: Pierce Shot", "Add Pierce Shot to your deck", RewardType.NewPierceShot, RewardRarity.Rare, 0f)); // Pierce Shot 획득 보상을 추가한다.
        rewardPool.Add(new RewardData("New Card: Bomb Shot", "Add Bomb Shot to your deck", RewardType.NewBombShot, RewardRarity.Rare, 0f)); // Bomb Shot 획득 보상을 추가한다.
        rewardPool.Add(new RewardData("New Card: Homing Shot", "Add Homing Shot to your deck", RewardType.NewHomingShot, RewardRarity.Rare, 0f)); // Homing Shot 획득 보상을 추가한다.
        rewardPool.Add(new RewardData("Upgrade: Pierce Shot", "Pierce Shot Damage +8", RewardType.UpgradePierceShotDamage, RewardRarity.Rare, 8f)); // Pierce Shot 데미지 강화 보상을 추가한다.
        rewardPool.Add(new RewardData("Upgrade: Bomb Shot", "Bomb Shot Damage +10", RewardType.UpgradeBombShotDamage, RewardRarity.Rare, 10f)); // Bomb Shot 데미지 강화 보상을 추가한다.
        rewardPool.Add(new RewardData("Upgrade: Homing Shot", "Homing Shot Damage +6", RewardType.UpgradeHomingShotDamage, RewardRarity.Rare, 6f)); // Homing Shot 데미지 강화 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Blast Core", "Bomb Shot Damage +12", RewardType.RelicBlastCore, RewardRarity.Rare, 12f)); // Blast Core 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Smart Chip", "Homing Shot Turn Speed +1", RewardType.RelicSmartChip, RewardRarity.Rare, 1f)); // Smart Chip 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Pierce Engine", "Pierce Shot Damage +6", RewardType.RelicPierceEngine, RewardRarity.Rare, 6f)); // Pierce Engine 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Piercing Needle", "Pierce Shot Pierce Count +1", RewardType.RelicPiercingNeedle, RewardRarity.Rare, 1f)); // Piercing Needle 유물 보상을 추가한다.

        rewardPool.Add(new RewardData("Upgrade: Pierce Core", "Pierce Shot Pierce Count +1", RewardType.UpgradePierceShotPierceCount, RewardRarity.Epic, 1f)); // Pierce Shot 관통 수 강화 보상을 추가한다.
        rewardPool.Add(new RewardData("Upgrade: Bomb Radius", "Bomb Shot Explosion Radius +0.5", RewardType.UpgradeBombShotRadius, RewardRarity.Epic, 0.5f)); // Bomb Shot 폭발 범위 강화 보상을 추가한다.
        rewardPool.Add(new RewardData("Upgrade: Homing Core", "Homing Shot Turn Speed +1", RewardType.UpgradeHomingShotTurnSpeed, RewardRarity.Epic, 1f)); // Homing Shot 유도 회전 속도 강화 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Heavy Core", "Heavy Shot Damage +20", RewardType.RelicHeavyCore, RewardRarity.Epic, 20f)); // Heavy Core 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Upgrade: Heavy Shot", "Heavy Shot Damage +15", RewardType.UpgradeHeavyShotDamage, RewardRarity.Epic, 15f)); // Heavy Shot 피해량 강화 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Piercing Needle", "Pierce Shot Pierce Count +1", RewardType.RelicPiercingNeedle, RewardRarity.Rare, 1f)); // Piercing Needle 유물 보상을 추가한다.
        rewardPool.Add(new RewardData("Relic: Blast Powder", "Bomb Shot Explosion Radius +0.5", RewardType.RelicBlastPowder, RewardRarity.Epic, 0.5f)); // Blast Powder 유물 보상을 추가한다.
       
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

    private void SelectRandomRewards() // 등급 확률을 적용하여 보상 3개를 중복 없이 선택한다.
    {
        currentRewards.Clear(); // 현재 보상 목록을 비운다.

        List<RewardData> availablePool = GetAvailableRewardPool(); // 현재 상황에서 등장 가능한 보상 목록을 가져온다.

        if (availablePool.Count < 3) // 등장 가능한 보상이 3개보다 적은지 확인한다.
        {
            Debug.LogWarning("Available rewards are less than 3. Use full reward pool instead."); // 경고 로그를 출력한다.
            availablePool = new List<RewardData>(rewardPool); // 안전을 위해 전체 보상 풀을 사용한다.
        }

        int rewardCount = Mathf.Min(3, availablePool.Count); // 표시할 보상 수를 계산한다.

        for (int i = 0; i < rewardCount; i++) // 필요한 보상 수만큼 반복한다.
        {
            RewardData selectedReward = GetRandomRewardByRarity(availablePool); // 등급 확률을 적용하여 보상 하나를 선택한다.

            if (selectedReward == null) // 선택된 보상이 없는지 확인한다.
            {
                Debug.LogWarning("Selected reward is null."); // 경고 로그를 출력한다.
                continue; // 이번 선택을 건너뛴다.
            }

            currentRewards.Add(selectedReward); // 현재 보상 목록에 추가한다.
            availablePool.Remove(selectedReward); // 중복 방지를 위해 사용 가능한 보상 목록에서 제거한다.
        }

        Debug.Log("Selected Reward Count : " + currentRewards.Count); // 선택된 보상 수를 로그로 출력한다.
    }
    private RewardData GetRandomRewardByRarity(List<RewardData> availablePool) // 등급 확률을 적용하여 보상 하나를 선택한다.
    {
        if (availablePool == null || availablePool.Count == 0) return null; // 사용 가능한 보상이 없으면 null을 반환한다.

        RewardRarity selectedRarity = GetWeightedRandomRarity(); // 가중치에 따라 보상 등급을 먼저 선택한다.
        List<RewardData> rarityRewards = GetRewardsByRarity(availablePool, selectedRarity); // 선택된 등급의 보상 목록을 가져온다.

        if (rarityRewards.Count == 0) // 해당 등급의 보상이 없는지 확인한다.
        {
            rarityRewards = availablePool; // 해당 등급 보상이 없으면 전체 보상 목록에서 선택한다.
        }

        int randomIndex = Random.Range(0, rarityRewards.Count); // 랜덤 인덱스를 선택한다.
        RewardData selectedReward = rarityRewards[randomIndex]; // 선택된 보상을 가져온다.

        Debug.Log("Reward Rarity Rolled : " + selectedRarity + " / Selected : " + selectedReward.rewardName); // 선택 결과를 로그로 출력한다.

        return selectedReward; // 선택한 보상을 반환한다.
    }



    private RewardRarity GetWeightedRandomRarity() // 가중치 기반 보상 등급 선택 함수
    {
        int safeCommonWeight = Mathf.Max(commonWeight, 0); // Common 음수 방지
        int safeRareWeight = Mathf.Max(rareWeight, 0); // Rare 음수 방지
        int safeEpicWeight = Mathf.Max(epicWeight, 0); // Epic 음수 방지

        int totalWeight = safeCommonWeight + safeRareWeight + safeEpicWeight; // 전체 가중치 계산

        if (totalWeight <= 0) return RewardRarity.Common; // 전체 가중치 없음 -> Common 반환
        int randomValue = Random.Range(0, totalWeight); // 랜덤 값 생성

        if (randomValue < safeCommonWeight) return RewardRarity.Common; // Common 범위 -> Common 반환
        randomValue -= safeCommonWeight; // Common 범위 제외

        if (randomValue < safeRareWeight) return RewardRarity.Rare; // Rare 범위 -> Rare 반환

        return RewardRarity.Epic; // 남은 범위 -> Epic 반환
    }
    private List<RewardData> GetRewardsByRarity(List<RewardData> rewardList, RewardRarity rarity) // 특정 등급의 보상만 골라낸다.
    {
        List<RewardData> result = new List<RewardData>(); // 결과 목록을 만든다.

        for (int i = 0; i < rewardList.Count; i++) // 보상 목록 수만큼 반복한다.
        {
            if (rewardList[i].rarity == rarity) // 원하는 등급과 같은지 확인한다.
            {
                result.Add(rewardList[i]); // 결과 목록에 추가한다.
            }
        }

        return result; // 특정 등급 보상 목록을 반환한다.
    }
    private List<RewardData> GetAvailableRewardPool() // 현재 보유 카드 상태에 맞는 보상 목록을 만든다.
    {
        List<RewardData> availableRewards = new List<RewardData>(); // 사용 가능한 보상 목록을 만든다.

        for (int i = 0; i < rewardPool.Count; i++) // 전체 보상 후보 수만큼 반복한다.
        {
            RewardData rewardData = rewardPool[i]; // 현재 확인할 보상 데이터를 가져온다.

            if (IsRewardAvailable(rewardData)) // 현재 상황에서 등장 가능한 보상인지 확인한다.
            {
                availableRewards.Add(rewardData); // 사용 가능한 보상 목록에 추가한다.
            }
        }

        return availableRewards; // 필터링된 보상 목록을 반환한다.
    }
    private bool IsRewardAvailable(RewardData rewardData) // 보상 등장 가능 확인 함수
    {
        if (rewardData == null) return false; // 보상 데이터 없음 -> 등장 불가

        if (cardManager == null) return IsCardSpecificReward(rewardData.rewardType) == false; // CardManager 없음 -> 카드 전용 보상 제외

        switch (rewardData.rewardType) // 보상 타입 확인
        {
            case RewardType.NewRapidShot: return cardManager.HasCard(CardType.RapidShot) == false; // Rapid Shot 없음 -> 등장 가능
            case RewardType.NewHeavyShot: return cardManager.HasCard(CardType.HeavyShot) == false; // Heavy Shot 없음 -> 등장 가능
            case RewardType.UpgradePixelShotDamage: return cardManager.HasCard(CardType.PixelShot); // Pixel Shot 보유 -> 등장 가능
            case RewardType.UpgradeFocusShotDamage: return cardManager.HasCard(CardType.FocusShot); // Focus Shot 보유 -> 등장 가능
            case RewardType.UpgradeWideShotBulletCount: return cardManager.HasCard(CardType.WideShot); // Wide Shot 보유 -> 등장 가능
            case RewardType.UpgradeRapidShotCooldown: return cardManager.HasCard(CardType.RapidShot); // Rapid Shot 보유 -> 등장 가능
            case RewardType.UpgradeHeavyShotDamage: return cardManager.HasCard(CardType.HeavyShot); // Heavy Shot 보유 -> 등장 가능
            
            case RewardType.RelicBloodCore: return IsRelicAvailable(RelicType.BloodCore); // Blood Core 없음 -> 등장 가능
            case RewardType.RelicManaStone: return IsRelicAvailable(RelicType.ManaStone); // Mana Stone 없음 -> 등장 가능
            case RewardType.RelicShieldFragment: return IsRelicAvailable(RelicType.ShieldFragment); // Shield Fragment 없음 -> 등장 가능
            case RewardType.RelicQuickGear: return IsRelicAvailable(RelicType.QuickGear); // Quick Gear 없음 -> 등장 가능
            case RewardType.RelicBulletEngine: return IsRelicAvailable(RelicType.BulletEngine); // Bullet Engine 없음 -> 등장 가능
            case RewardType.RelicPixelLens: return IsRelicAvailable(RelicType.PixelLens) && cardManager.HasCard(CardType.PixelShot); // Pixel Lens 없음 / Pixel Shot 보유 -> 등장 가능
            case RewardType.RelicFocusLens: return IsRelicAvailable(RelicType.FocusLens) && cardManager.HasCard(CardType.FocusShot); // Focus Lens 없음 / Focus Shot 보유 -> 등장 가능
            case RewardType.RelicWideBarrel: return IsRelicAvailable(RelicType.WideBarrel) && cardManager.HasCard(CardType.WideShot); // Wide Barrel 없음 / Wide Shot 보유 -> 등장 가능
            case RewardType.RelicRapidBattery: return IsRelicAvailable(RelicType.RapidBattery) && cardManager.HasCard(CardType.RapidShot); // Rapid Battery 없음 / Rapid Shot 보유 -> 등장 가능
            case RewardType.RelicHeavyCore: return IsRelicAvailable(RelicType.HeavyCore) && cardManager.HasCard(CardType.HeavyShot); // Heavy Core 없음 / Heavy Shot 보유 -> 등장 가능
            
            case RewardType.NewPierceShot: return cardManager.HasCard(CardType.PierceShot) == false; // Pierce Shot 없음 -> 등장 가능
            case RewardType.NewBombShot: return cardManager.HasCard(CardType.BombShot) == false; // Bomb Shot 없음 -> 등장 가능
            case RewardType.NewHomingShot: return cardManager.HasCard(CardType.HomingShot) == false; // Homing Shot 없음 -> 등장 가능
            case RewardType.UpgradePierceShotDamage: return cardManager.HasCard(CardType.PierceShot); // Pierce Shot 보유 -> 등장 가능
            case RewardType.UpgradePierceShotPierceCount: return cardManager.HasCard(CardType.PierceShot); // Pierce Shot 보유 -> 등장 가능
            case RewardType.UpgradeBombShotDamage: return cardManager.HasCard(CardType.BombShot); // Bomb Shot 보유 -> 등장 가능
            case RewardType.UpgradeBombShotRadius: return cardManager.HasCard(CardType.BombShot); // Bomb Shot 보유 -> 등장 가능
            case RewardType.UpgradeHomingShotDamage: return cardManager.HasCard(CardType.HomingShot); // Homing Shot 보유 -> 등장 가능
            case RewardType.UpgradeHomingShotTurnSpeed: return cardManager.HasCard(CardType.HomingShot); // Homing Shot 보유 -> 등장 가능

            case RewardType.RelicPiercingNeedle: return IsRelicAvailable(RelicType.PiercingNeedle) && cardManager.HasCard(CardType.PierceShot); // Piercing Needle 없음 / Pierce Shot 보유 -> 등장 가능
            case RewardType.RelicPierceEngine: return IsRelicAvailable(RelicType.PierceEngine) && cardManager.HasCard(CardType.PierceShot); // Pierce Engine 없음 / Pierce Shot 보유 -> 등장 가능
            case RewardType.RelicBlastPowder: return IsRelicAvailable(RelicType.BlastPowder) && cardManager.HasCard(CardType.BombShot); // Blast Powder 없음 / Bomb Shot 보유 -> 등장 가능
            case RewardType.RelicBlastCore: return IsRelicAvailable(RelicType.BlastCore) && cardManager.HasCard(CardType.BombShot); // Blast Core 없음 / Bomb Shot 보유 -> 등장 가능
            case RewardType.RelicSmartChip: return IsRelicAvailable(RelicType.SmartChip) && cardManager.HasCard(CardType.HomingShot); // Smart Chip 없음 / Homing Shot 보유 -> 등장 가능

            default: return true; // 일반 보상 -> 항상 등장 가능
        }
    }
    private bool IsRelicAvailable(RelicType relicType) // 특정 유물이 보상으로 등장 가능한지 확인한다.
    {
        if (relicManager == null) // RelicManager가 연결되지 않았는지 확인한다.
        {
            relicManager = FindFirstObjectByType<RelicManager>(); // 씬에서 RelicManager를 찾는다.
        }

        if (relicManager == null) // RelicManager를 찾지 못했는지 확인한다.
        {
            return true; // 안전을 위해 유물 보상을 등장 가능 처리한다.
        }

        return relicManager.HasRelic(relicType) == false; // 이미 보유한 유물이 아닐 때만 등장 가능하다고 반환한다.
    }
    private bool IsCardSpecificReward(RewardType rewardType) // 카드 보상인지 확인한다.
    {
        switch (rewardType) // 보상 타입을 확인한다.
        {
            case RewardType.NewRapidShot:
            case RewardType.NewHeavyShot:
            case RewardType.UpgradePixelShotDamage:
            case RewardType.UpgradeFocusShotDamage:
            case RewardType.UpgradeWideShotBulletCount:
            case RewardType.UpgradeRapidShotCooldown:
            case RewardType.UpgradeHeavyShotDamage:
            case RewardType.RelicPixelLens:
            case RewardType.RelicFocusLens:
            case RewardType.RelicWideBarrel:
            case RewardType.RelicRapidBattery:
            case RewardType.RelicHeavyCore:
            case RewardType.NewPierceShot:
            case RewardType.NewBombShot:
            case RewardType.NewHomingShot:
            case RewardType.UpgradePierceShotDamage:
            case RewardType.UpgradePierceShotPierceCount:
            case RewardType.UpgradeBombShotDamage:
            case RewardType.UpgradeBombShotRadius:
            case RewardType.UpgradeHomingShotDamage:
            case RewardType.UpgradeHomingShotTurnSpeed:
            case RewardType.RelicPiercingNeedle:
            case RewardType.RelicPierceEngine:
            case RewardType.RelicBlastPowder:
            case RewardType.RelicBlastCore:
            case RewardType.RelicSmartChip:
                return true; // 카드 보유 여부가 필요한 유물 보상이라고 반환한다.

            default:
                return false; // 카드 관련 보상이 아니라고 반환한다.
        }
    }
    private void ApplyRewardsToButtons() // 선택된 보상을 버튼에 적용한다.
    {
        if (currentRewards.Count < 3) // 보상 수가 3개보다 적은지 확인한다.
        {
            Debug.LogError("Not enough rewards to display."); // 오류 로그를 출력한다.
            return; // 버튼 적용을 중단한다.
        }

        if (rewardButton01 != null) rewardButton01.Setup(currentRewards[0], this, 0); // 첫 번째 버튼에 보상을 설정한다.
        if (rewardButton02 != null) rewardButton02.Setup(currentRewards[1], this, 1); // 두 번째 버튼에 보상을 설정한다.
        if (rewardButton03 != null) rewardButton03.Setup(currentRewards[2], this, 2); // 세 번째 버튼에 보상을 설정한다.
    }

    public void SelectReward(int rewardIndex) // 선택한 보상 번호를 받아 보상을 적용한다.
    {
        if (rewardIndex < 0 || rewardIndex >= currentRewards.Count) // 보상 번호가 올바른 범위인지 확인한다.
        {
            Debug.LogError("Invalid reward index : " + rewardIndex); // 잘못된 보상 번호 로그를 출력한다.
            return; // 보상 선택을 중단한다.
        }

        RewardData rewardData = currentRewards[rewardIndex]; // 선택한 번호에 해당하는 보상 데이터를 가져온다.

        if (rewardData == null) // 보상 데이터가 없는지 확인한다.
        {
            Debug.LogError("Selected reward is null."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        ApplyReward(rewardData); // 보상 효과를 적용한다.
        HideRewardPanel(); // 보상 패널을 숨긴다.

        Time.timeScale = 1f; // 게임 시간을 다시 흐르게 한다.

        if (GameFlowManager.Instance != null) // 맵 진행 관리자가 있는지 확인한다.
        {  GameFlowManager.Instance.CompleteActiveNodeAndReturnToMap(); }// 현재 노드를 완료하고 맵 씬으로 돌아간다.
        
        else if (nextBattleUI != null) // 맵 진행 관리자가 없고 기존 NextBattleUI가 있는지 확인한다.
        {  nextBattleUI.Show(); }// 기존 방식으로 다음 전투 버튼을 표시한다.
        
        else // 다음 전투 UI가 없는 경우를 처리한다.
        { Debug.LogWarning("NextBattleUI is not assigned."); }// 경고 로그를 출력한다.
        
        Debug.Log("Reward Selected : " + rewardData.rewardName); // 선택한 보상을 로그로 출력한다.
    }

    private void ApplyReward(RewardData rewardData) // 보상 종류에 따라 효과를 적용한다.
    {
        switch (rewardData.rewardType) // 선택된 보상 종류를 확인한다.
        {
            case RewardType.PixelShotDamageUp: // 모든 카드 피해량 증가 보상인지 확인한다.
                ApplyPixelShotDamageUp(rewardData.value); // 모든 카드 피해량 증가를 적용한다.
                break; // switch문을 종료한다.

            case RewardType.PixelShotCooldownDown: // 모든 카드 쿨타임 감소 보상인지 확인한다.
                ApplyPixelShotCooldownDown(rewardData.value); // 모든 카드 쿨타임 감소를 적용한다.
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

            case RewardType.BulletSpeedUp: // 모든 카드 탄환 속도 증가 보상인지 확인한다.
                ApplyBulletSpeedUp(rewardData.value); // 모든 카드 탄환 속도 증가를 적용한다.
                break; // switch문을 종료한다.

            case RewardType.ManaRegenUp: // 마나 회복 증가 보상인지 확인한다.
                ApplyManaRegenUp(rewardData.value); // 마나 회복 증가를 적용한다.
                break; // switch문을 종료한다.

            case RewardType.NewRapidShot: // Rapid Shot 새 카드 보상인지 확인한다.
                ApplyNewCard(CardType.RapidShot); // Rapid Shot을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.NewHeavyShot: // Heavy Shot 새 카드 보상인지 확인한다.
                ApplyNewCard(CardType.HeavyShot); // Heavy Shot을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.UpgradePixelShotDamage: // Pixel Shot 피해량 강화 보상인지 확인한다.
                ApplyCardDamageUpgrade(CardType.PixelShot, rewardData.value); // Pixel Shot 피해량을 강화한다.
                break; // switch문을 종료한다.

            case RewardType.UpgradeFocusShotDamage: // Focus Shot 피해량 강화 보상인지 확인한다.
                ApplyCardDamageUpgrade(CardType.FocusShot, rewardData.value); // Focus Shot 피해량을 강화한다.
                break; // switch문을 종료한다.

            case RewardType.UpgradeWideShotBulletCount: // Wide Shot 탄환 수 강화 보상인지 확인한다.
                ApplyCardBulletCountUpgrade(CardType.WideShot, rewardData.value); // Wide Shot 탄환 수를 강화한다.
                break; // switch문을 종료한다.

            case RewardType.UpgradeRapidShotCooldown: // Rapid Shot 쿨타임 강화 보상인지 확인한다.
                ApplyCardCooldownUpgrade(CardType.RapidShot, rewardData.value); // Rapid Shot 쿨타임을 강화한다.
                break; // switch문을 종료한다.

            case RewardType.UpgradeHeavyShotDamage: // Heavy Shot 피해량 강화 보상인지 확인한다.
                ApplyCardDamageUpgrade(CardType.HeavyShot, rewardData.value); // Heavy Shot 피해량을 강화한다.
                break; // switch문을 종료한다.

            case RewardType.RelicBloodCore: // Blood Core 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Blood Core", "All Card Damage +2", RelicType.BloodCore, rewardData.value)); // Blood Core 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicManaStone: // Mana Stone 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Mana Stone", "MP Regen +0.3", RelicType.ManaStone, rewardData.value)); // Mana Stone 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicShieldFragment: // Shield Fragment 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Shield Fragment", "Gain Shield +1 at battle start", RelicType.ShieldFragment, rewardData.value)); // Shield Fragment 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicQuickGear: // Quick Gear 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Quick Gear", "All Card Cooldown -0.05s", RelicType.QuickGear, rewardData.value)); // Quick Gear 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicBulletEngine: // Bullet Engine 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Bullet Engine", "All Card Bullet Speed +0.5", RelicType.BulletEngine, rewardData.value)); // Bullet Engine 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicPixelLens: // Pixel Lens 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Pixel Lens", "Pixel Shot Damage +5", RelicType.PixelLens, rewardData.value)); // Pixel Lens 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicFocusLens: // Focus Lens 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Focus Lens", "Focus Shot Damage +15", RelicType.FocusLens, rewardData.value)); // Focus Lens 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicWideBarrel: // Wide Barrel 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Wide Barrel", "Wide Shot Bullet Count +2", RelicType.WideBarrel, rewardData.value)); // Wide Barrel 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicRapidBattery: // Rapid Battery 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Rapid Battery", "Rapid Shot Cooldown -0.08s", RelicType.RapidBattery, rewardData.value)); // Rapid Battery 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicHeavyCore: // Heavy Core 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Heavy Core", "Heavy Shot Damage +20", RelicType.HeavyCore, rewardData.value)); // Heavy Core 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.NewPierceShot: // Pierce Shot 새 카드 보상인지 확인한다.
                ApplyNewCard(CardType.PierceShot); // Pierce Shot을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.NewBombShot: // Bomb Shot 새 카드 보상인지 확인한다.
                ApplyNewCard(CardType.BombShot); // Bomb Shot을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.NewHomingShot: // Homing Shot 새 카드 보상인지 확인한다.
                ApplyNewCard(CardType.HomingShot); // Homing Shot을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.UpgradePierceShotDamage: // Pierce Shot 데미지 강화 보상인지 확인한다.
                ApplyCardDamageUpgrade(CardType.PierceShot, rewardData.value); // Pierce Shot 데미지를 강화한다.
                break; // switch문을 종료한다.

            case RewardType.UpgradePierceShotPierceCount: // Pierce Shot 관통 수 강화 보상인지 확인한다.
                ApplyCardPierceCountUpgrade(CardType.PierceShot, rewardData.value); // Pierce Shot 관통 수를 강화한다.
                break; // switch문을 종료한다.

            case RewardType.UpgradeBombShotDamage: // Bomb Shot 데미지 강화 보상인지 확인한다.
                ApplyCardDamageUpgrade(CardType.BombShot, rewardData.value); // Bomb Shot 데미지를 강화한다.
                break; // switch문을 종료한다.

            case RewardType.UpgradeBombShotRadius: // Bomb Shot 폭발 범위 강화 보상인지 확인한다.
                ApplyCardExplosionRadiusUpgrade(CardType.BombShot, rewardData.value); // Bomb Shot 폭발 범위를 강화한다.
                break; // switch문을 종료한다.

            case RewardType.UpgradeHomingShotDamage: // Homing Shot 데미지 강화 보상인지 확인한다.
                ApplyCardDamageUpgrade(CardType.HomingShot, rewardData.value); // Homing Shot 데미지를 강화한다.
                break; // switch문을 종료한다.

            case RewardType.UpgradeHomingShotTurnSpeed: // Homing Shot 유도 회전 속도 강화 보상인지 확인한다.
                ApplyCardHomingTurnSpeedUpgrade(CardType.HomingShot, rewardData.value); // Homing Shot 유도 회전 속도를 강화한다.
                break; // switch문을 종료한다.

            case RewardType.RelicPiercingNeedle: // Piercing Needle 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Piercing Needle", "Pierce Shot Pierce Count +1", RelicType.PiercingNeedle, rewardData.value)); // Piercing Needle 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicPierceEngine: // Pierce Engine 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Pierce Engine", "Pierce Shot Damage +6", RelicType.PierceEngine, rewardData.value)); // Pierce Engine 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicBlastPowder: // Blast Powder 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Blast Powder", "Bomb Shot Explosion Radius +0.5", RelicType.BlastPowder, rewardData.value)); // Blast Powder 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicBlastCore: // Blast Core 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Blast Core", "Bomb Shot Damage +12", RelicType.BlastCore, rewardData.value)); // Blast Core 유물을 추가한다.
                break; // switch문을 종료한다.

            case RewardType.RelicSmartChip: // Smart Chip 유물 보상인지 확인한다.
                ApplyRelicReward(new RelicData("Smart Chip", "Homing Shot Turn Speed +1", RelicType.SmartChip, rewardData.value)); // Smart Chip 유물을 추가한다.
                break; // switch문을 종료한다.

            default: // 정의되지 않은 보상 타입인 경우다.
                Debug.LogWarning("Unknown Reward Type : " + rewardData.rewardType); // 알 수 없는 보상 타입 로그를 출력한다.
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
    private void ApplyNewCard(CardType cardType) // 새 카드 획득 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        cardManager.AddCardByType(cardType); // CardManager에 새 카드 추가를 요청한다.
    }

    private void ApplyCardDamageUpgrade(CardType cardType, float value) // 특정 카드 피해량 강화 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        cardManager.UpgradeCardDamage(cardType, Mathf.RoundToInt(value)); // 특정 카드 피해량을 강화한다.
    }

    private void ApplyCardCooldownUpgrade(CardType cardType, float value) // 특정 카드 쿨타임 감소 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        cardManager.UpgradeCardCooldown(cardType, value); // 특정 카드 쿨타임을 감소시킨다.
    }

    private void ApplyCardBulletCountUpgrade(CardType cardType, float value) // 특정 카드 탄환 수 강화 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        cardManager.UpgradeCardBulletCount(cardType, Mathf.RoundToInt(value)); // 특정 카드 탄환 수를 증가시킨다.
    }

    private void ApplyRelicReward(RelicData relicData) // 유물 보상을 적용한다.
    {
        if (relicManager == null) // RelicManager가 연결되지 않았는지 확인한다.
        {
            relicManager = FindFirstObjectByType<RelicManager>(); // 씬에서 RelicManager를 찾는다.
        }

        if (relicManager == null) // 자동으로도 찾지 못했는지 확인한다.
        {
            Debug.LogError("RelicManager is not assigned."); // 오류 로그를 출력한다.
            return; // 유물 보상을 중단한다.
        }

        relicManager.AddRelic(relicData); // 유물을 획득 처리한다.
    }

    private void ApplyCardPierceCountUpgrade(CardType cardType, float value) // 특정 카드 관통 수 강화 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        cardManager.UpgradeCardPierceCount(cardType, Mathf.RoundToInt(value)); // 특정 카드 관통 수를 증가시킨다.
    }

    private void ApplyCardExplosionRadiusUpgrade(CardType cardType, float value) // 특정 카드 폭발 범위 강화 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }

        cardManager.UpgradeCardExplosionRadius(cardType, value); // 특정 카드 폭발 범위를 증가시킨다.
    }

    private void ApplyCardHomingTurnSpeedUpgrade(CardType cardType, float value) // 특정 카드 유도 회전 속도 강화 보상을 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 보상 적용을 중단한다.
        }
        cardManager.UpgradeCardHomingTurnSpeed(cardType, value); // 특정 카드 유도 회전 속도를 증가시킨다.
    }
}