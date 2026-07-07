using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    [Header("UI References")]
    public Button button; // 클릭할 버튼 컴포넌트를 저장한다.
    public TextMeshProUGUI rewardText; // 보상 정보를 표시할 텍스트를 저장한다.
    public Image backgroundImage; // 보상 등급에 따라 색상을 바꿀 배경 이미지를 저장한다.

    private RewardManager rewardManager; // 보상 선택 처리를 담당할 RewardManager를 저장한다.
    private int rewardIndex; // 현재 버튼이 몇 번째 보상인지 저장한다.
    private RewardData rewardData; // 현재 버튼에 표시할 보상 데이터를 저장한다.

    public void Setup(RewardData newRewardData, RewardManager newRewardManager, int newRewardIndex) // 보상 버튼 정보를 설정한다.
    {
        rewardData = newRewardData; // 보상 데이터를 저장한다.
        rewardManager = newRewardManager; // RewardManager를 저장한다.
        rewardIndex = newRewardIndex; // 보상 번호를 저장한다.

        UpdateText(); // 보상 텍스트를 갱신한다.
        UpdateColor(); // 보상 등급 색상을 갱신한다.
        UpdateButtonEvent(); // 버튼 클릭 이벤트를 갱신한다.
    }

    private void UpdateText() // 보상 버튼 텍스트를 갱신한다.
    {
        if (rewardText == null) return; // 텍스트가 연결되지 않았으면 실행하지 않는다.
        if (rewardData == null) return; // 보상 데이터가 없으면 실행하지 않는다.

        string rarityText = "[" + rewardData.rarity + "]"; // 보상 등급 문구를 만든다.
        string categoryText = "[" + GetRewardCategoryText(rewardData.rewardType) + "]"; // 보상 종류 문구를 만든다.

        rewardText.text = rarityText + " " + categoryText + "\n" + rewardData.rewardName + "\n\n" + rewardData.description; // 최종 보상 문구를 표시한다.
    }

    private string GetRewardCategoryText(RewardType rewardType) // 보상 타입에 따른 분류 문구를 반환한다.
    {
        switch (rewardType) // 보상 타입을 확인한다.
        {
            case RewardType.NewRapidShot: // Rapid Shot 새 카드 보상인지 확인한다.
            case RewardType.NewHeavyShot: // Heavy Shot 새 카드 보상인지 확인한다.
            case RewardType.NewPierceShot: // Pierce Shot 새 카드 보상인지 확인한다.
            case RewardType.NewBombShot: // Bomb Shot 새 카드 보상인지 확인한다.
            case RewardType.NewHomingShot: // Homing Shot 새 카드 보상인지 확인한다.
                return "New Card"; // 새 카드 보상이라고 반환한다.

            case RewardType.UpgradePixelShotDamage: // Pixel Shot 강화 보상인지 확인한다.
            case RewardType.UpgradeFocusShotDamage: // Focus Shot 강화 보상인지 확인한다.
            case RewardType.UpgradeWideShotBulletCount: // Wide Shot 강화 보상인지 확인한다.
            case RewardType.UpgradeRapidShotCooldown: // Rapid Shot 강화 보상인지 확인한다.
            case RewardType.UpgradeHeavyShotDamage: // Heavy Shot 강화 보상인지 확인한다.
            case RewardType.UpgradePierceShotDamage: // Pierce Shot 강화 보상인지 확인한다.
            case RewardType.UpgradePierceShotPierceCount: // Pierce Shot 관통 수 강화 보상인지 확인한다.
            case RewardType.UpgradeBombShotDamage: // Bomb Shot 강화 보상인지 확인한다.
            case RewardType.UpgradeBombShotRadius: // Bomb Shot 폭발 범위 강화 보상인지 확인한다.
            case RewardType.UpgradeHomingShotDamage: // Homing Shot 강화 보상인지 확인한다.
            case RewardType.UpgradeHomingShotTurnSpeed: // Homing Shot 유도 회전 속도 강화 보상인지 확인한다.
                return "Card Upgrade"; // 카드 강화 보상이라고 반환한다.

            case RewardType.RelicBloodCore: // Blood Core 유물 보상인지 확인한다.
            case RewardType.RelicManaStone: // Mana Stone 유물 보상인지 확인한다.
            case RewardType.RelicShieldFragment: // Shield Fragment 유물 보상인지 확인한다.
            case RewardType.RelicQuickGear: // Quick Gear 유물 보상인지 확인한다.
            case RewardType.RelicBulletEngine: // Bullet Engine 유물 보상인지 확인한다.
            case RewardType.RelicPixelLens: // Pixel Lens 유물 보상인지 확인한다.
            case RewardType.RelicFocusLens: // Focus Lens 유물 보상인지 확인한다.
            case RewardType.RelicWideBarrel: // Wide Barrel 유물 보상인지 확인한다.
            case RewardType.RelicRapidBattery: // Rapid Battery 유물 보상인지 확인한다.
            case RewardType.RelicHeavyCore: // Heavy Core 유물 보상인지 확인한다.
            case RewardType.RelicPiercingNeedle: // Piercing Needle 유물 보상인지 확인한다.
            case RewardType.RelicPierceEngine: // Pierce Engine 유물 보상인지 확인한다.
            case RewardType.RelicBlastPowder: // Blast Powder 유물 보상인지 확인한다.
            case RewardType.RelicBlastCore: // Blast Core 유물 보상인지 확인한다.
            case RewardType.RelicSmartChip: // Smart Chip 유물 보상인지 확인한다.
                return "Relic"; // 유물 보상이라고 반환한다.

            default: // 그 외 보상인 경우다.
                return "Stat"; // 스탯 보상이라고 반환한다.
        }
    }

    private void UpdateColor() // 보상 등급에 따라 버튼 배경 색상을 변경한다.
    {
        if (backgroundImage == null) return; // 배경 이미지가 연결되지 않았으면 실행하지 않는다.
        if (rewardData == null) return; // 보상 데이터가 없으면 실행하지 않는다.

        switch (rewardData.rarity) // 보상 등급을 확인한다.
        {
            case RewardRarity.Common: // Common 등급인지 확인한다.
                backgroundImage.color = new Color(0.85f, 0.85f, 0.85f, 1f); // 회색 계열로 표시한다.
                break; // switch문을 종료한다.

            case RewardRarity.Rare: // Rare 등급인지 확인한다.
                backgroundImage.color = new Color(0.45f, 0.65f, 1f, 1f); // 파란색 계열로 표시한다.
                break; // switch문을 종료한다.

            case RewardRarity.Epic: // Epic 등급인지 확인한다.
                backgroundImage.color = new Color(0.75f, 0.45f, 1f, 1f); // 보라색 계열로 표시한다.
                break; // switch문을 종료한다.
        }
    }

    private void UpdateButtonEvent() // 버튼 클릭 이벤트를 갱신한다.
    {
        if (button == null) return; // 버튼이 연결되지 않았으면 실행하지 않는다.

        button.onClick.RemoveAllListeners(); // 기존 클릭 이벤트를 모두 제거한다.
        button.onClick.AddListener(OnClickRewardButton); // 새 클릭 이벤트를 등록한다.
    }

    private void OnClickRewardButton() // 보상 버튼을 눌렀을 때 실행한다.
    {
        if (rewardManager == null) return; // RewardManager가 없으면 실행하지 않는다.

        rewardManager.SelectReward(rewardIndex); // 해당 번호의 보상을 선택한다.
    }
}