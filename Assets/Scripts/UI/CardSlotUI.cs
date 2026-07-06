using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardSlotUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI cardText; // 보유 카드 목록 텍스트를 저장한다.
    public TextMeshProUGUI cardDetailText; // 선택한 카드 상세 정보 텍스트를 저장한다.

    [Header("Display Settings")]
    public bool showFinalStats = true; // 보너스가 적용된 최종 수치를 표시할지 저장한다.

    public void UpdateCardSlots(List<CardData> cards, int selectedIndex) // 기존 방식으로 카드 UI를 갱신한다.
    {
        UpdateCardSlots(cards, selectedIndex, 0, 0f, 0f); // 보너스 수치 없이 카드 UI를 갱신한다.
    }

    public void UpdateCardSlots(List<CardData> cards, int selectedIndex, int bonusDamage, float bonusSpeed, float cooldownReduction) // 보너스 수치를 포함하여 카드 UI를 갱신한다.
    {
        UpdateCardList(cards, selectedIndex); // 카드 목록 UI를 갱신한다.
        UpdateCardDetail(cards, selectedIndex, bonusDamage, bonusSpeed, cooldownReduction); // 카드 상세 UI를 갱신한다.
    }

    private void UpdateCardList(List<CardData> cards, int selectedIndex) // 보유 카드 목록을 갱신한다.
    {
        if (cardText == null) return; // 카드 목록 텍스트가 없으면 실행하지 않는다.

        if (cards == null || cards.Count == 0) // 보유 카드가 없는지 확인한다.
        {
            cardText.text = "Cards\nNone"; // 카드가 없다는 문구를 표시한다.
            return; // 카드 목록 갱신을 종료한다.
        }

        string displayText = "Cards\n"; // 카드 목록 표시 문장을 만든다.

        for (int i = 0; i < cards.Count; i++) // 보유 카드 수만큼 반복한다.
        {
            string selectMark = i == selectedIndex ? "> " : "  "; // 선택된 카드에는 표시를 붙인다.
            displayText += selectMark + "[" + (i + 1) + "] " + cards[i].cardName + "\n"; // 카드 번호와 이름을 추가한다.
        }

        cardText.text = displayText; // 완성된 카드 목록을 표시한다.
    }

    private void UpdateCardDetail(List<CardData> cards, int selectedIndex, int bonusDamage, float bonusSpeed, float cooldownReduction) // 선택 카드 상세 정보를 갱신한다.
    {
        if (cardDetailText == null) return; // 카드 상세 텍스트가 없으면 실행하지 않는다.

        if (cards == null || cards.Count == 0) // 보유 카드가 없는지 확인한다.
        {
            cardDetailText.text = "Selected Card\nNone"; // 선택 카드가 없다는 문구를 표시한다.
            return; // 상세 정보 갱신을 종료한다.
        }

        if (selectedIndex < 0 || selectedIndex >= cards.Count) // 선택 번호가 올바른 범위인지 확인한다.
        {
            selectedIndex = 0; // 안전을 위해 첫 번째 카드를 선택한다.
        }

        CardData cardData = cards[selectedIndex]; // 선택된 카드 데이터를 가져온다.

        int displayDamage = Mathf.RoundToInt(cardData.bulletDamage); // 표시할 기본 데미지를 계산한다.
        float displaySpeed = cardData.bulletSpeed; // 표시할 기본 탄환 속도를 저장한다.
        float displayCooldown = cardData.cooldown; // 표시할 기본 쿨타임을 저장한다.

        if (showFinalStats) // 최종 수치 표시를 사용하는지 확인한다.
        {
            displayDamage += bonusDamage; // 전체 데미지 보너스를 더한다.
            displaySpeed += bonusSpeed; // 전체 탄환 속도 보너스를 더한다.
            displayCooldown = Mathf.Max(0.05f, displayCooldown - cooldownReduction); // 전체 쿨타임 감소를 적용한다.
        }

        string detailText = ""; // 상세 정보 문장을 만든다.
        detailText += "Selected Card\n"; // 제목을 추가한다.
        detailText += "Name: " + cardData.cardName + "\n"; // 카드 이름을 추가한다.
        detailText += "Type: " + cardData.cardType + "\n"; // 카드 타입을 추가한다.
        detailText += "Role: " + GetCardRoleText(cardData.cardType) + "\n\n"; // 카드 역할을 추가한다.

        detailText += "MP Cost: " + cardData.manaCost.ToString("0.##") + "\n"; // MP 소모량을 추가한다.
        detailText += "Cooldown: " + displayCooldown.ToString("0.##") + "s\n"; // 쿨타임을 추가한다.
        detailText += "Damage: " + displayDamage + "\n"; // 데미지를 추가한다.
        detailText += "Bullet Count: " + cardData.bulletCount + "\n"; // 탄환 수를 추가한다.
        detailText += "Bullet Speed: " + displaySpeed.ToString("0.##") + "\n"; // 탄환 속도를 추가한다.
        detailText += "Life Time: " + cardData.bulletLifeTime.ToString("0.##") + "s\n"; // 탄환 지속 시간을 추가한다.
        detailText += "Spread: " + cardData.spreadAngle.ToString("0.##"); // 탄퍼짐 각도를 추가한다.
        detailText += "\nEffect: " + cardData.bulletEffectType; // 탄환 효과 타입을 추가한다.

        if (cardData.bulletEffectType == BulletEffectType.Pierce) // 관통 탄환인지 확인한다.
        {
            detailText += "\nPierce Count: " + cardData.pierceCount; // 관통 수를 추가한다.
        }

        if (cardData.bulletEffectType == BulletEffectType.Bomb) // 폭발 탄환인지 확인한다.
        {
            detailText += "\nExplosion Radius: " + cardData.explosionRadius.ToString("0.##"); // 폭발 범위를 추가한다.
        }

        if (cardData.bulletEffectType == BulletEffectType.Homing) // 유도 탄환인지 확인한다.
        {
            detailText += "\nHoming Range: " + cardData.homingRange.ToString("0.##"); // 유도 범위를 추가한다.
            detailText += "\nTurn Speed: " + cardData.homingTurnSpeed.ToString("0.##"); // 유도 회전 속도를 추가한다.
        }

        cardDetailText.text = detailText; // 완성된 상세 정보를 표시한다.
    }

    private string GetCardRoleText(CardType cardType) // 카드 타입에 따른 역할 문구를 반환한다.
    {
        switch (cardType) // 카드 타입을 확인한다.
        {
            case CardType.PixelShot: // Pixel Shot인지 확인한다.
                return "Basic"; // 기본 공격 카드라고 반환한다.

            case CardType.FocusShot: // Focus Shot인지 확인한다.
                return "High Damage"; // 고화력 카드라고 반환한다.

            case CardType.WideShot: // Wide Shot인지 확인한다.
                return "Wide Area"; // 넓은 범위 카드라고 반환한다.

            case CardType.RapidShot: // Rapid Shot인지 확인한다.
                return "Fast Fire"; // 빠른 연사 카드라고 반환한다.

            case CardType.HeavyShot: // Heavy Shot인지 확인한다.
                return "Heavy Damage"; // 강한 한 방 카드라고 반환한다.

            case CardType.PierceShot: // Pierce Shot인지 확인한다.
                return "Pierce"; // 관통 카드라고 반환한다.

            case CardType.BombShot: // Bomb Shot인지 확인한다.
                return "Explosion"; // 폭발 카드라고 반환한다.

            case CardType.HomingShot: // Homing Shot인지 확인한다.
                return "Homing"; // 유도 카드라고 반환한다.

            default: // 정의되지 않은 카드 타입인 경우다.
                return "Unknown"; // 알 수 없는 역할이라고 반환한다.
        }
    }
}