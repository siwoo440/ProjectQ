using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    [Header("Target References")]
    public CardManager cardManager; // 카드 관련 효과를 적용할 CardManager를 저장한다.
    public PlayerStats playerStats; // 플레이어 관련 효과를 적용할 PlayerStats를 저장한다.

    [Header("UI References")]
    public RelicSlotUI relicSlotUI; // 보유 유물 UI를 저장한다.

    [Header("Owned Relics")]
    public List<RelicData> ownedRelics = new List<RelicData>(); // 현재 보유한 유물 목록을 저장한다.
    private List<RelicComboType> activatedCombos = new List<RelicComboType>(); // 이미 발동된 유물 조합 목록을 저장한다.

    private void Start() // 시작 시 참조와 UI를 초기화한다.
    {
        if (cardManager == null) cardManager = FindFirstObjectByType<CardManager>(); // CardManager 없음 -> 씬에서 검색
        if (playerStats == null) playerStats = FindFirstObjectByType<PlayerStats>(); // PlayerStats 없음 -> 씬에서 검색
        if (relicSlotUI == null) relicSlotUI = FindFirstObjectByType<RelicSlotUI>(); // RelicSlotUI 없음 -> 씬에서 검색
        UpdateRelicUI(); // 유물 UI를 갱신한다.
    }

    public bool HasRelic(RelicType relicType) // 특정 유물을 이미 가지고 있는지 확인한다.
    {
        for (int i = 0; i < ownedRelics.Count; i++) // 보유 유물 수만큼 반복한다.
        {
            if (ownedRelics[i].relicType == relicType) // 같은 유물 타입인지 확인한다.
            {
                return true; // 이미 가지고 있다고 반환한다.
            }
        }

        return false; // 가지고 있지 않다고 반환한다.
    }

    public void AddRelic(RelicData relicData) // 새 유물을 획득한다.
    {
        if (relicData == null) return; // 유물 데이터가 없으면 실행하지 않는다.

        if (HasRelic(relicData.relicType)) // 이미 같은 유물을 가지고 있는지 확인한다.
        {
            Debug.Log("Already Has Relic : " + relicData.relicName); // 중복 유물 로그를 출력한다.
            return; // 중복 유물은 추가하지 않는다.
        }

        ownedRelics.Add(relicData); // 유물을 보유 목록에 추가한다.
        ApplyRelicOnAcquire(relicData); // 유물 획득 효과를 적용한다.
        CheckRelicCombos(); // 유물 획득 후 조합 효과를 확인한다.
        UpdateRelicUI(); // 유물 UI를 갱신한다.

        Debug.Log("Relic Added : " + relicData.relicName); // 유물 획득 로그를 출력한다.
    }

    private void ApplyRelicOnAcquire(RelicData relicData) // 유물 획득 시 즉시 적용 효과 처리
    {
        switch (relicData.relicType) // 유물 타입 확인
        {
            case RelicType.BloodCore:       ApplyBloodCore(relicData.value);        break; // Blood Core -> 모든 카드 데미지 증가
            case RelicType.ManaStone:       ApplyManaStone(relicData.value);        break; // Mana Stone -> MP 회복 속도 증가
            case RelicType.QuickGear:       ApplyQuickGear(relicData.value);        break; // Quick Gear -> 모든 카드 쿨타임 감소
            case RelicType.BulletEngine:    ApplyBulletEngine(relicData.value);     break; // Bullet Engine -> 모든 카드 탄환 속도 증가
            case RelicType.ShieldFragment:  Debug.Log("Shield Fragment will apply at battle start."); break; // Shield Fragment -> 전투 시작 시 적용 예정
            case RelicType.PixelLens:       ApplyPixelLens(relicData.value);        break; // Pixel Lens -> Pixel Shot 데미지 증가
            case RelicType.FocusLens:       ApplyFocusLens(relicData.value);        break; // Focus Lens -> Focus Shot 데미지 증가
            case RelicType.WideBarrel:      ApplyWideBarrel(relicData.value);       break; // Wide Barrel -> Wide Shot 탄환 수 증가
            case RelicType.RapidBattery:    ApplyRapidBattery(relicData.value);     break; // Rapid Battery -> Rapid Shot 쿨타임 감소
            case RelicType.HeavyCore:       ApplyHeavyCore(relicData.value);        break; // Heavy Core -> Heavy Shot 데미지 증가
            case RelicType.PiercingNeedle:  ApplyPiercingNeedle(relicData.value);   break; // Piercing Needle -> Pierce Shot 관통 수 증가
            case RelicType.PierceEngine:    ApplyPierceEngine(relicData.value);     break; // Pierce Engine -> Pierce Shot 데미지 증가
            case RelicType.BlastPowder:     ApplyBlastPowder(relicData.value);      break; // Blast Powder -> Bomb Shot 폭발 범위 증가
            case RelicType.BlastCore:       ApplyBlastCore(relicData.value);        break; // Blast Core -> Bomb Shot 데미지 증가
            case RelicType.SmartChip:       ApplySmartChip(relicData.value);        break; // Smart Chip -> Homing Shot 유도 회전 속도 증가
        }
    }

    public void ApplyRelicsOnBattleStart() // 전투 시작 시 적용되는 유물 효과를 처리한다.
    {
        for (int i = 0; i < ownedRelics.Count; i++) // 보유 유물 수만큼 반복한다.
        {
            RelicData relicData = ownedRelics[i]; // 현재 유물 데이터를 가져온다.

            if (relicData.relicType == RelicType.ShieldFragment) // Shield Fragment 유물인지 확인한다.
            {  ApplyShieldFragment(relicData.value); } // 전투 시작 보호막 효과를 적용한다.
        }
    }

    private void ApplyBloodCore(float value) // Blood Core 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); return; // 오류 로그를 출력한다. -> 효과 적용을 중단한다.
        }
        cardManager.IncreaseBulletDamage(Mathf.RoundToInt(value)); // 모든 카드 추가 데미지를 증가시킨다.
    }

    private void ApplyManaStone(float value) // Mana Stone 효과를 적용한다.
    {
        if (playerStats == null) // PlayerStats가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("PlayerStats is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }
        playerStats.IncreaseManaRegen(value); // MP 회복 속도를 증가시킨다.
    }

    private void ApplyQuickGear(float value) // Quick Gear 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }
        cardManager.ReduceCooldown(value); // 모든 카드 쿨타임 감소량을 증가시킨다.
    }
    private void ApplyPixelLens(float value) // Pixel Lens 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        cardManager.UpgradeCardDamage(CardType.PixelShot, Mathf.RoundToInt(value)); // Pixel Shot 데미지를 증가시킨다.
        Debug.Log("Pixel Lens Applied : Pixel Shot Damage +" + value); // 적용 로그를 출력한다.
    }

    private void ApplyFocusLens(float value) // Focus Lens 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        cardManager.UpgradeCardDamage(CardType.FocusShot, Mathf.RoundToInt(value)); // Focus Shot 데미지를 증가시킨다.
        Debug.Log("Focus Lens Applied : Focus Shot Damage +" + value); // 적용 로그를 출력한다.
    }

    private void ApplyWideBarrel(float value) // Wide Barrel 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        cardManager.UpgradeCardBulletCount(CardType.WideShot, Mathf.RoundToInt(value)); // Wide Shot 탄환 수를 증가시킨다.
        Debug.Log("Wide Barrel Applied : Wide Shot Bullet Count +" + value); // 적용 로그를 출력한다.
    }

    private void ApplyRapidBattery(float value) // Rapid Battery 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        cardManager.UpgradeCardCooldown(CardType.RapidShot, value); // Rapid Shot 쿨타임을 감소시킨다.
        Debug.Log("Rapid Battery Applied : Rapid Shot Cooldown -" + value); // 적용 로그를 출력한다.
    }

    private void ApplyHeavyCore(float value) // Heavy Core 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        cardManager.UpgradeCardDamage(CardType.HeavyShot, Mathf.RoundToInt(value)); // Heavy Shot 데미지를 증가시킨다.
        Debug.Log("Heavy Core Applied : Heavy Shot Damage +" + value); // 적용 로그를 출력한다.
    }
    private void ApplyBulletEngine(float value) // Bullet Engine 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        cardManager.IncreaseBulletSpeed(value); // 모든 카드 탄환 속도를 증가시킨다.
    }

    private void ApplyShieldFragment(float value) // Shield Fragment 효과를 적용한다.
    {
        if (playerStats == null) // PlayerStats가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("PlayerStats is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        playerStats.AddShield(Mathf.RoundToInt(value)); // 보호막을 추가한다.
        Debug.Log("Shield Fragment Applied : +" + value); // 보호막 적용 로그를 출력한다.
    }

    private void UpdateRelicUI() // 유물 UI를 갱신한다.
    {
        if (relicSlotUI == null) return; // 유물 UI가 없으면 실행하지 않는다.

        relicSlotUI.UpdateRelicSlots(ownedRelics); // 유물 UI에 보유 유물 목록을 전달한다.
    }

    private void ApplyPiercingNeedle(float value) // Piercing Needle 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        cardManager.UpgradeCardPierceCount(CardType.PierceShot, Mathf.RoundToInt(value)); // Pierce Shot 관통 수를 증가시킨다.
        Debug.Log("Piercing Needle Applied : Pierce Shot Pierce Count +" + value); // 적용 로그를 출력한다.
    }

    private void ApplyPierceEngine(float value) // Pierce Engine 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        cardManager.UpgradeCardDamage(CardType.PierceShot, Mathf.RoundToInt(value)); // Pierce Shot 데미지를 증가시킨다.
        Debug.Log("Pierce Engine Applied : Pierce Shot Damage +" + value); // 적용 로그를 출력한다.
    }

    private void ApplyBlastPowder(float value) // Blast Powder 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        cardManager.UpgradeCardExplosionRadius(CardType.BombShot, value); // Bomb Shot 폭발 범위를 증가시킨다.
        Debug.Log("Blast Powder Applied : Bomb Shot Explosion Radius +" + value); // 적용 로그를 출력한다.
    }

    private void ApplyBlastCore(float value) // Blast Core 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        cardManager.UpgradeCardDamage(CardType.BombShot, Mathf.RoundToInt(value)); // Bomb Shot 데미지를 증가시킨다.
        Debug.Log("Blast Core Applied : Bomb Shot Damage +" + value); // 적용 로그를 출력한다.
    }

    private void ApplySmartChip(float value) // Smart Chip 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
        }

        cardManager.UpgradeCardHomingTurnSpeed(CardType.HomingShot, value); // Homing Shot 유도 회전 속도를 증가시킨다.
        Debug.Log("Smart Chip Applied : Homing Shot Turn Speed +" + value); // 적용 로그를 출력한다.
    }
    private void CheckRelicCombos() // 현재 보유 유물과 카드 상태를 확인하여 조합 효과를 발동한다.
    {
        CheckPiercingArsenalCombo(); // 관통 빌드 조합을 확인한다.
        CheckVolatileReactorCombo(); // 폭발 빌드 조합을 확인한다.
        CheckSmartGuidanceCombo(); // 유도 빌드 조합을 확인한다.
    }
    private bool HasActivatedCombo(RelicComboType comboType) // 이미 발동한 조합인지 확인한다.
    {
        return activatedCombos.Contains(comboType); // 발동된 조합 목록에 있는지 반환한다.
    }

    private void AddActivatedCombo(RelicComboType comboType) // 조합 발동 목록에 조합을 추가한다.
    {
        if (HasActivatedCombo(comboType)) return; // 이미 발동한 조합이면 추가하지 않는다.

        activatedCombos.Add(comboType); // 발동된 조합 목록에 추가한다.
    }
    private void CheckPiercingArsenalCombo() // Piercing Arsenal 조합 조건을 확인한다.
    {
        if (HasActivatedCombo(RelicComboType.PiercingArsenal)) return; // 이미 발동했다면 실행하지 않는다.

        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 조합 확인을 중단한다.
        }

        if (cardManager.HasCard(CardType.PierceShot) == false) return; // Pierce Shot이 없으면 발동하지 않는다.
        if (HasRelic(RelicType.PiercingNeedle) == false) return; // Piercing Needle이 없으면 발동하지 않는다.
        if (HasRelic(RelicType.PierceEngine) == false) return; // Pierce Engine이 없으면 발동하지 않는다.

        cardManager.UpgradeCardDamage(CardType.PierceShot, 4); // Pierce Shot 데미지를 추가로 증가시킨다.
        cardManager.UpgradeCardPierceCount(CardType.PierceShot, 1); // Pierce Shot 관통 수를 추가로 증가시킨다.

        AddActivatedCombo(RelicComboType.PiercingArsenal); // 조합 발동 상태를 저장한다.

        Debug.Log("Combo Activated : Piercing Arsenal"); // 조합 발동 로그를 출력한다.
    }
    private void CheckVolatileReactorCombo() // Volatile Reactor 조합 조건을 확인한다.
    {
        if (HasActivatedCombo(RelicComboType.VolatileReactor)) return; // 이미 발동했다면 실행하지 않는다.

        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 조합 확인을 중단한다.
        }

        if (cardManager.HasCard(CardType.BombShot) == false) return; // Bomb Shot이 없으면 발동하지 않는다.
        if (HasRelic(RelicType.BlastPowder) == false) return; // Blast Powder가 없으면 발동하지 않는다.
        if (HasRelic(RelicType.BlastCore) == false) return; // Blast Core가 없으면 발동하지 않는다.

        cardManager.UpgradeCardExplosionRadius(CardType.BombShot, 0.3f); // Bomb Shot 폭발 반경을 추가로 증가시킨다.
        cardManager.UpgradeCardCooldown(CardType.BombShot, 0.2f); // Bomb Shot 쿨타임을 추가로 감소시킨다.

        AddActivatedCombo(RelicComboType.VolatileReactor); // 조합 발동 상태를 저장한다.

        Debug.Log("Combo Activated : Volatile Reactor"); // 조합 발동 로그를 출력한다.
    }

    private void CheckSmartGuidanceCombo() // Smart Guidance 조합 조건을 확인한다.
    {
        if (HasActivatedCombo(RelicComboType.SmartGuidance)) return; // 이미 발동했다면 실행하지 않는다.

        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 조합 확인을 중단한다.
        }

        if (cardManager.HasCard(CardType.HomingShot) == false) return; // Homing Shot이 없으면 발동하지 않는다.
        if (HasRelic(RelicType.SmartChip) == false) return; // Smart Chip이 없으면 발동하지 않는다.
        if (HasRelic(RelicType.BulletEngine) == false) return; // Bullet Engine이 없으면 발동하지 않는다.

        cardManager.UpgradeCardDamage(CardType.HomingShot, 4); // Homing Shot 데미지를 추가로 증가시킨다.
        cardManager.UpgradeCardHomingTurnSpeed(CardType.HomingShot, 0.7f); // Homing Shot 유도 회전 속도를 추가로 증가시킨다.

        AddActivatedCombo(RelicComboType.SmartGuidance); // 조합 발동 상태를 저장한다.

        Debug.Log("Combo Activated : Smart Guidance"); // 조합 발동 로그를 출력한다.
    }
    public int GetActivatedComboCount() // 발동된 조합 수를 반환한다.
    {
        return activatedCombos.Count; // 발동된 조합 목록 개수를 반환한다.
    }
}