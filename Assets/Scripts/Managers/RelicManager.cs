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

    private void Start() // 시작 시 참조와 UI를 초기화한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            cardManager = FindFirstObjectByType<CardManager>(); // 씬에서 CardManager를 찾는다.
        }

        if (playerStats == null) // PlayerStats가 연결되지 않았는지 확인한다.
        {
            playerStats = FindFirstObjectByType<PlayerStats>(); // 씬에서 PlayerStats를 찾는다.
        }

        if (relicSlotUI == null) // RelicSlotUI가 연결되지 않았는지 확인한다.
        {
            relicSlotUI = FindFirstObjectByType<RelicSlotUI>(); // 씬에서 RelicSlotUI를 찾는다.
        }

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

        ownedRelics.Add(relicData); // 보유 유물 목록에 추가한다.

        ApplyRelicOnAcquire(relicData); // 획득 즉시 적용되는 유물 효과를 처리한다.
        UpdateRelicUI(); // 유물 UI를 갱신한다.

        Debug.Log("Relic Added : " + relicData.relicName); // 유물 획득 로그를 출력한다.
    }

    private void ApplyRelicOnAcquire(RelicData relicData) // 유물 획득 시 즉시 적용할 효과를 처리한다.
    {
        switch (relicData.relicType) // 유물 타입을 확인한다.
        {
            case RelicType.BloodCore: // Blood Core 유물인지 확인한다.
                ApplyBloodCore(relicData.value); // 모든 카드 데미지 증가 효과를 적용한다.
                break; // switch문을 종료한다.

            case RelicType.ManaStone: // Mana Stone 유물인지 확인한다.
                ApplyManaStone(relicData.value); // MP 회복 속도 증가 효과를 적용한다.
                break; // switch문을 종료한다.

            case RelicType.QuickGear: // Quick Gear 유물인지 확인한다.
                ApplyQuickGear(relicData.value); // 모든 카드 쿨타임 감소 효과를 적용한다.
                break; // switch문을 종료한다.

            case RelicType.BulletEngine: // Bullet Engine 유물인지 확인한다.
                ApplyBulletEngine(relicData.value); // 모든 카드 탄환 속도 증가 효과를 적용한다.
                break; // switch문을 종료한다.

            case RelicType.ShieldFragment: // Shield Fragment 유물인지 확인한다.
                Debug.Log("Shield Fragment will apply at battle start."); // 전투 시작 효과 로그를 출력한다.
                break; // switch문을 종료한다.
        }
    }

    public void ApplyRelicsOnBattleStart() // 전투 시작 시 적용되는 유물 효과를 처리한다.
    {
        for (int i = 0; i < ownedRelics.Count; i++) // 보유 유물 수만큼 반복한다.
        {
            RelicData relicData = ownedRelics[i]; // 현재 유물 데이터를 가져온다.

            if (relicData.relicType == RelicType.ShieldFragment) // Shield Fragment 유물인지 확인한다.
            {
                ApplyShieldFragment(relicData.value); // 전투 시작 보호막 효과를 적용한다.
            }
        }
    }

    private void ApplyBloodCore(float value) // Blood Core 효과를 적용한다.
    {
        if (cardManager == null) // CardManager가 연결되지 않았는지 확인한다.
        {
            Debug.LogError("CardManager is not assigned."); // 오류 로그를 출력한다.
            return; // 효과 적용을 중단한다.
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
}