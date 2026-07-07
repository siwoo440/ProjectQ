using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats; // 플레이어 스탯을 저장한다.
    public GameObject bulletPrefab; // 생성할 탄환 프리팹을 저장한다.
    public Transform firePoint; // 탄환이 생성될 위치를 저장한다.
    public Camera mainCamera; // 마우스 위치 계산에 사용할 카메라를 저장한다.
    public CardSlotUI cardSlotUI; // 카드 슬롯 UI를 저장한다.

    [Header("Runtime Cards")]
    public List<CardData> ownedCards = new List<CardData>(); // 현재 보유한 카드 목록을 저장한다.
    public int selectedCardIndex = 0; // 현재 선택한 카드 번호를 저장한다.

    [Header("Reward Modifiers")]
    public int bonusBulletDamage = 0; // 보상으로 증가한 추가 피해량을 저장한다.
    public float bonusBulletSpeed = 0f; // 보상으로 증가한 추가 탄환 속도를 저장한다.
    public float cooldownReduction = 0f; // 보상으로 감소한 쿨타임 수치를 저장한다.

    private void Start() // 시작 시 참조와 기본 카드를 설정한다.
    {
        if (mainCamera == null) // 카메라가 연결되지 않았는지 확인한다.
        {
            mainCamera = Camera.main; // MainCamera 태그가 붙은 카메라를 가져온다.
        }

        if (playerStats == null) // 플레이어 스탯이 연결되지 않았는지 확인한다.
        {
            playerStats = FindFirstObjectByType<PlayerStats>(); // 씬에서 PlayerStats를 찾는다.
        }

        if (cardSlotUI == null) // 카드 슬롯 UI가 연결되지 않았는지 확인한다.
        {
            cardSlotUI = FindFirstObjectByType<CardSlotUI>(); // 씬에서 CardSlotUI를 찾는다.
        }

        CreateDefaultCards(); // 기본 카드 3개를 생성한다.
        UpdateCardUI(); // 카드 UI를 갱신한다.
    }

    private void Update() // 매 프레임 카드 선택과 사용 입력을 확인한다.
    {
        HandleCardSelectionInput(); // 카드 선택 입력을 처리한다.

        if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭을 눌렀는지 확인한다.
        {
            TryUseSelectedCard(); // 현재 선택한 카드 사용을 시도한다.
        }
    }

    private void CreateDefaultCards() // 기본 카드 3개를 생성한다.
    {
        if (ownedCards.Count > 0) return; // 이미 카드가 있으면 다시 만들지 않는다.

        ownedCards.Add(new CardData("Pixel Shot", CardType.PixelShot, 1f, 1f, 10, 10f, 3f, 3, 10f)); // 기본 3발 카드를 추가한다.
        ownedCards.Add(new CardData("Focus Shot", CardType.FocusShot, 2f, 1.4f, 25, 14f, 3f, 1, 0f)); // 강한 단일 탄환 카드를 추가한다.
        ownedCards.Add(new CardData("Wide Shot", CardType.WideShot, 2f, 1.8f, 8, 9f, 3f, 5, 15f)); // 넓게 퍼지는 5발 카드를 추가한다.
    }

    private void HandleCardSelectionInput() // 카드 선택 입력을 처리한다.
    {
        if (ownedCards.Count == 0) return; // 보유 카드가 없으면 실행하지 않는다.

        for (int i = 0; i < ownedCards.Count && i < 9; i++) // 보유 카드 수만큼 숫자키 입력을 확인한다.
        {
            KeyCode keyCode = (KeyCode)((int)KeyCode.Alpha1 + i); // 1번 키부터 순서대로 KeyCode를 만든다.

            if (Input.GetKeyDown(keyCode)) // 해당 숫자키를 눌렀는지 확인한다.
            {
                SelectCard(i); // 해당 번호의 카드를 선택한다.
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) SelectPreviousCard(); // Q 키로 이전 카드를 선택한다.
        if (Input.GetKeyDown(KeyCode.E)) SelectNextCard(); // E 키로 다음 카드를 선택한다.

        float scroll = Input.mouseScrollDelta.y; // 마우스 휠 입력을 가져온다.

        if (scroll > 0f) // 휠을 위로 올렸는지 확인한다.
        {
            SelectPreviousCard(); // 이전 카드를 선택한다.
        }
        else if (scroll < 0f) // 휠을 아래로 내렸는지 확인한다.
        {
            SelectNextCard(); // 다음 카드를 선택한다.
        }
    }

    private void SelectCard(int index) // 카드 선택을 처리한다.
    {
        if (index < 0 || index >= ownedCards.Count) return; // 선택 번호가 올바르지 않으면 실행하지 않는다.

        selectedCardIndex = index; // 선택한 카드 번호를 저장한다.
        UpdateCardUI(); // 카드 UI를 갱신한다.
    }

    private void SelectNextCard() // 다음 카드를 선택한다.
    {
        if (ownedCards.Count == 0) return; // 보유 카드가 없으면 실행하지 않는다.

        selectedCardIndex += 1; // 선택 번호를 1 증가시킨다.

        if (selectedCardIndex >= ownedCards.Count) // 마지막 카드를 넘었는지 확인한다.
        {
            selectedCardIndex = 0; // 첫 번째 카드로 돌아간다.
        }

        UpdateCardUI(); // 카드 UI를 갱신한다.
    }

    private void SelectPreviousCard() // 이전 카드를 선택한다.
    {
        if (ownedCards.Count == 0) return; // 보유 카드가 없으면 실행하지 않는다.

        selectedCardIndex -= 1; // 선택 번호를 1 감소시킨다.

        if (selectedCardIndex < 0) // 첫 번째 카드보다 앞인지 확인한다.
        {
            selectedCardIndex = ownedCards.Count - 1; // 마지막 카드로 이동한다.
        }

        UpdateCardUI(); // 카드 UI를 갱신한다.
    }

    private void TryUseSelectedCard() // 현재 선택한 카드 사용을 시도한다.
    {
        if (ownedCards.Count == 0) return; // 보유 카드가 없으면 실행하지 않는다.

        CardData selectedCard = ownedCards[selectedCardIndex]; // 현재 선택한 카드를 가져온다.
        float finalCooldown = GetFinalCooldown(selectedCard); // 보상 효과가 반영된 최종 쿨타임을 계산한다.

        if (Time.time < selectedCard.lastUseTime + finalCooldown) // 아직 쿨타임 중인지 확인한다.
        {
            Debug.Log("Card Cooldown : " + selectedCard.cardName); // 쿨타임 로그를 출력한다.
            return; // 카드 사용을 중단한다.
        }

        if (playerStats == null) // 플레이어 스탯이 없는지 확인한다.
        {
            Debug.LogError("PlayerStats is not assigned."); // 오류 로그를 출력한다.
            return; // 카드 사용을 중단한다.
        }

        bool canUseMana = playerStats.UseMana(selectedCard.manaCost); // 카드 MP 비용 사용을 시도한다.

        if (canUseMana == false) // MP가 부족한지 확인한다.
        {
            return; // 카드 사용을 중단한다.
        }

        CastCard(selectedCard); // 선택한 카드를 사용한다.
        selectedCard.lastUseTime = Time.time; // 마지막 사용 시간을 갱신한다.
    }

    private void CastCard(CardData cardData) // 카드 종류에 따라 카드 효과를 실행한다.
    {
        switch (cardData.cardType) // 카드 종류를 확인한다.
        {
            case CardType.PixelShot: // Pixel Shot인지 확인한다.
                FireSpread(cardData); // 퍼짐 탄환을 발사한다.
                break; // switch문을 종료한다.

            case CardType.FocusShot: // Focus Shot인지 확인한다.
                FireFocus(cardData); // 단일 강탄을 발사한다.
                break; // switch문을 종료한다.

            case CardType.WideShot: // Wide Shot인지 확인한다.
                FireSpread(cardData); // 넓은 퍼짐 탄환을 발사한다.
                break; // switch문을 종료한다.

            case CardType.RapidShot: // Rapid Shot인지 확인한다.
                FireFocus(cardData); // 빠른 단일 탄환을 발사한다.
                break; // switch문을 종료한다.

            case CardType.HeavyShot: // Heavy Shot인지 확인한다.
                FireFocus(cardData); // 강한 단일 탄환을 발사한다.
                break; // switch문을 종료한다.
        }
    }

    private void FireSpread(CardData cardData) // 여러 발의 퍼짐 탄환을 발사한다.
    {
        Vector2 baseDirection = GetMouseDirection(); // 마우스 방향을 기준 방향으로 가져온다.
        float centerIndex = (cardData.bulletCount - 1) / 2f; // 가운데 탄환 기준 인덱스를 계산한다.

        for (int i = 0; i < cardData.bulletCount; i++) // 탄환 수만큼 반복한다.
        {
            float angleOffset = (i - centerIndex) * cardData.spreadAngle; // 각 탄환의 각도 차이를 계산한다.
            Vector2 shotDirection = RotateVector(baseDirection, angleOffset); // 기준 방향을 회전한다.

            CreateBullet(cardData, shotDirection); // 탄환을 생성한다.
        }
    }

    private void FireFocus(CardData cardData) // 강한 단일 탄환을 발사한다.
    {
        Vector2 direction = GetMouseDirection(); // 마우스 방향을 가져온다.

        CreateBullet(cardData, direction); // 단일 탄환을 생성한다.
    }

    private Vector2 GetMouseDirection() // 마우스 방향을 계산한다.
    {
        if (mainCamera == null) // 카메라가 없는지 확인한다.
        {
            mainCamera = Camera.main; // MainCamera를 다시 찾는다.
        }

        Vector3 mousePosition = Input.mousePosition; // 화면상의 마우스 위치를 가져온다.
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition); // 마우스 위치를 월드 좌표로 변환한다.
        mouseWorldPosition.z = 0f; // 2D 게임이므로 z값을 0으로 맞춘다.

        Vector2 direction = mouseWorldPosition - firePoint.position; // 발사 위치에서 마우스까지의 방향을 계산한다.

        if (direction.sqrMagnitude <= 0.001f) // 방향 값이 너무 작은지 확인한다.
        {
            direction = Vector2.right; // 기본 방향을 오른쪽으로 설정한다.
        }

        return direction.normalized; // 정규화된 방향을 반환한다.
    }

    private Vector2 RotateVector(Vector2 vector, float angle) // 벡터를 지정 각도만큼 회전한다.
    {
        float radian = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환한다.
        float cos = Mathf.Cos(radian); // 코사인 값을 계산한다.
        float sin = Mathf.Sin(radian); // 사인 값을 계산한다.

        float x = vector.x * cos - vector.y * sin; // 회전된 x값을 계산한다.
        float y = vector.x * sin + vector.y * cos; // 회전된 y값을 계산한다.

        return new Vector2(x, y).normalized; // 회전된 방향을 반환한다.
    }

    private void CreateBullet(CardData cardData, Vector2 direction) // 탄환을 생성한다.
    {
        if (bulletPrefab == null) // 탄환 프리팹이 없는지 확인한다.
        {
            Debug.LogError("Bullet Prefab is not assigned."); // 오류 로그를 출력한다.
            return; // 탄환 생성을 중단한다.
        }

        if (firePoint == null) // 발사 위치가 없는지 확인한다.
        {
            Debug.LogError("FirePoint is not assigned."); // 오류 로그를 출력한다.
            return; // 탄환 생성을 중단한다.
        }

        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity); // 탄환 프리팹을 생성한다.
        Bullet bullet = bulletObject.GetComponent<Bullet>(); // 생성된 탄환의 Bullet 컴포넌트를 가져온다.

        if (bullet == null) // Bullet 컴포넌트가 없는지 확인한다.
        {
            Debug.LogError("Generated bullet has no Bullet component."); // 오류 로그를 출력한다.
            return; // 탄환 설정을 중단한다.
        }

        int finalDamage = cardData.bulletDamage + bonusBulletDamage; // 카드 기본 데미지에 전체 데미지 보너스를 더한다.
        float finalBulletSpeed = cardData.bulletSpeed + bonusBulletSpeed; // 카드 기본 탄속에 전체 탄속 보너스를 더한다.

        bullet.InitializeSpecial(direction, finalBulletSpeed, finalDamage, cardData.bulletLifeTime, cardData.bulletEffectType,
            cardData.pierceCount, cardData.explosionRadius, cardData.homingRange, cardData.homingTurnSpeed); // 카드 효과 정보를 포함해 탄환을 초기화한다.
    }

    private int GetFinalDamage(CardData cardData) // 보상 효과가 반영된 최종 피해량을 계산한다.
    {
        return Mathf.Max(cardData.bulletDamage + bonusBulletDamage, 1); // 최소 1 이상의 피해량을 반환한다.
    }

    private float GetFinalBulletSpeed(CardData cardData) // 보상 효과가 반영된 최종 탄환 속도를 계산한다.
    {
        return Mathf.Max(cardData.bulletSpeed + bonusBulletSpeed, 1f); // 최소 1 이상의 속도를 반환한다.
    }

    private float GetFinalCooldown(CardData cardData) // 보상 효과가 반영된 최종 쿨타임을 계산한다.
    {
        return Mathf.Max(cardData.cooldown - cooldownReduction, 0.2f); // 최소 쿨타임을 0.2초로 제한한다.
    }

    private void UpdateCardUI() // 카드 UI를 갱신한다.
    {
        if (cardSlotUI == null) return; // 카드 슬롯 UI가 없으면 실행하지 않는다.

        cardSlotUI.UpdateCardSlots(ownedCards, selectedCardIndex, bonusBulletDamage, bonusBulletSpeed, cooldownReduction); // 보너스 수치를 포함하여 카드 UI를 갱신한다.
    }

    public void IncreaseBulletDamage(int value) // 모든 카드 추가 데미지를 증가시킨다.
    {
        bonusBulletDamage += value; // 추가 데미지 보너스를 증가시킨다.
        UpdateCardUI(); // 카드 UI를 갱신한다.
    }

    public void ReduceCooldown(float value) // 모든 카드 쿨타임 감소량을 증가시킨다.
    {
        cooldownReduction += value; // 쿨타임 감소량을 증가시킨다.
        UpdateCardUI(); // 카드 UI를 갱신한다.
    }

    public void IncreaseBulletSpeed(float value) // 모든 카드 탄환 속도를 증가시킨다.
    {
        bonusBulletSpeed += value; // 추가 탄환 속도 보너스를 증가시킨다.
        UpdateCardUI(); // 카드 UI를 갱신한다.
    }
    public void AddCardByType(CardType cardType) // 카드 타입에 따라 새 카드를 추가한다.
    {
        if (HasCard(cardType)) // 이미 같은 카드를 가지고 있는지 확인한다.
        {
            UpgradeCardDamage(cardType, 5); // 중복 카드는 데미지 강화로 처리한다.
            return; // 카드 추가를 종료한다.
        }

        CardData newCard = CreateCardByType(cardType); // 카드 타입에 맞는 새 카드를 만든다.

        if (newCard == null) // 새 카드 생성에 실패했는지 확인한다.
        {
            Debug.LogWarning("Card creation failed : " + cardType); // 카드 생성 실패 로그를 출력한다.
            return; // 카드 추가를 종료한다.
        }

        ownedCards.Add(newCard); // 보유 카드 목록에 새 카드를 추가한다.
        selectedCardIndex = ownedCards.Count - 1; // 새로 얻은 카드를 선택한다.
        UpdateCardUI(); // 카드 UI를 갱신한다.

        Debug.Log("New Card Added : " + newCard.cardName); // 새 카드 추가 로그를 출력한다.
    }

    public bool HasCard(CardType cardType) // 특정 카드를 이미 가지고 있는지 확인한다.
    {
        for (int i = 0; i < ownedCards.Count; i++) // 보유 카드 수만큼 반복한다.
        {
            if (ownedCards[i].cardType == cardType) // 같은 카드 타입인지 확인한다.
            {
                return true; // 이미 가지고 있다고 반환한다.
            }
        }

        return false; // 가지고 있지 않다고 반환한다.
    }

    private CardData CreateCardByType(CardType cardType) // 카드 종류에 맞는 카드 데이터를 생성한다.
    {
        switch (cardType) // 생성할 카드 종류를 확인한다.
        {
            case CardType.PixelShot: // Pixel Shot인지 확인한다.
                return new CardData("Pixel Shot", CardType.PixelShot, 1f, 1f, 10, 10f, 3f, 3, 10f); // Pixel Shot 데이터를 반환한다.

            case CardType.FocusShot: // Focus Shot인지 확인한다.
                return new CardData("Focus Shot", CardType.FocusShot, 2f, 1.4f, 25, 14f, 3f, 1, 0f); // Focus Shot 데이터를 반환한다.

            case CardType.WideShot: // Wide Shot인지 확인한다.
                return new CardData("Wide Shot", CardType.WideShot, 2f, 1.8f, 8, 9f, 3f, 5, 15f); // Wide Shot 데이터를 반환한다.

            case CardType.RapidShot: // Rapid Shot인지 확인한다.
                return new CardData("Rapid Shot", CardType.RapidShot, 0.5f, 0.35f, 5, 13f, 2.5f, 1, 0f); // Rapid Shot 데이터를 반환한다.

            case CardType.HeavyShot: // Heavy Shot인지 확인한다.
                return new CardData("Heavy Shot", CardType.HeavyShot, 3f, 2.2f, 45, 8f, 4f, 1, 0f); // Heavy Shot 데이터를 반환한다.

            case CardType.PierceShot: // Pierce Shot 카드인지 확인한다.
            {
                 CardData cardData = new CardData("Pierce Shot", CardType.PierceShot, 2f, 1.7f, 13, 10.5f, 3f, 1, 0f); // Pierce Shot 데이터를 만든다.
                 cardData.bulletEffectType = BulletEffectType.Pierce; // 관통 탄환으로 설정한다.
                 cardData.pierceCount = 3; // 최대 3회 관통하도록 설정한다.
                 return cardData; // Pierce Shot 데이터를 반환한다.
            }

            case CardType.BombShot: // Bomb Shot 카드인지 확인한다.
            {
                 CardData cardData = new CardData("Bomb Shot", CardType.BombShot, 3f, 2.6f, 16, 7f, 3.5f, 1, 0f); // Bomb Shot 데이터를 만든다.
                 cardData.bulletEffectType = BulletEffectType.Bomb; // 폭발 탄환으로 설정한다.
                 cardData.explosionRadius = 2.0f; // 폭발 범위를 설정한다.
                 return cardData; // Bomb Shot 데이터를 반환한다.
            }

            case CardType.HomingShot: // Homing Shot 카드인지 확인한다.
            {
                 CardData cardData = new CardData("Homing Shot", CardType.HomingShot, 2f, 1.9f, 9, 8f, 4f, 1, 0f); // Homing Shot 데이터를 만든다.
                 cardData.bulletEffectType = BulletEffectType.Homing; // 유도 탄환으로 설정한다.
                 cardData.homingRange = 7f; // 유도 탐색 범위를 설정한다.
                 cardData.homingTurnSpeed = 4f; // 유도 회전 속도를 설정한다.
                 return cardData; // Homing Shot 데이터를 반환한다.
            }
        }
        return null; // 해당하는 카드가 없으면 null을 반환한다.
    }

    public void UpgradeCardDamage(CardType cardType, int value) // 특정 카드 데미지를 강화한다.
    {
        CardData cardData = FindCard(cardType); // 강화할 카드를 찾는다.

        if (cardData == null) return; // 카드가 없으면 실행하지 않는다.

        cardData.bulletDamage += value; // 카드 데미지를 증가시킨다.
        UpdateCardUI(); // 카드 UI를 갱신한다.
    }

    public void UpgradeCardCooldown(CardType cardType, float value) // 특정 카드 쿨타임을 감소시킨다.
    {
        CardData cardData = FindCard(cardType); // 강화할 카드를 찾는다.

        if (cardData == null) return; // 카드가 없으면 실행하지 않는다.

        cardData.cooldown = Mathf.Max(0.05f, cardData.cooldown - value); // 카드 쿨타임을 감소시킨다.
        UpdateCardUI(); // 카드 UI를 갱신한다.
    }

    public void UpgradeCardBulletCount(CardType cardType, int value) // 특정 카드 탄환 수를 강화한다.
    {
        CardData cardData = FindCard(cardType); // 강화할 카드를 찾는다.

        if (cardData == null) return; // 카드가 없으면 실행하지 않는다.

        cardData.bulletCount += value; // 카드 탄환 수를 증가시킨다.
        UpdateCardUI(); // 카드 UI를 갱신한다.
    }

    private CardData FindCard(CardType cardType) // 보유 카드 목록에서 특정 카드를 찾는다.
    {
        for (int i = 0; i < ownedCards.Count; i++) // 보유 카드 수만큼 반복한다.
        {
            if (ownedCards[i].cardType == cardType) // 찾는 카드 타입과 같은지 확인한다.
            {
                return ownedCards[i]; // 찾은 카드를 반환한다.
            }
        }

        return null; // 찾지 못하면 null을 반환한다.
    }

    public void UpgradeCardPierceCount(CardType cardType, int value) // 특정 카드의 관통 수를 강화한다.
    {
        CardData cardData = FindCard(cardType); // 강화할 카드를 찾는다.

        if (cardData == null) return; // 카드가 없으면 실행하지 않는다.

        cardData.pierceCount += value; // 관통 수를 증가시킨다.
        UpdateCardUI(); // 카드 UI를 갱신한다.

        Debug.Log(cardData.cardName + " Pierce Count +" + value); // 강화 로그를 출력한다.
    }

    public void UpgradeCardExplosionRadius(CardType cardType, float value) // 특정 카드의 폭발 범위를 강화한다.
    {
        CardData cardData = FindCard(cardType); // 강화할 카드를 찾는다.

        if (cardData == null) return; // 카드가 없으면 실행하지 않는다.

        cardData.explosionRadius += value; // 폭발 범위를 증가시킨다.
        UpdateCardUI(); // 카드 UI를 갱신한다.

        Debug.Log(cardData.cardName + " Explosion Radius +" + value); // 강화 로그를 출력한다.
    }

    public void UpgradeCardHomingTurnSpeed(CardType cardType, float value) // 특정 카드의 유도 회전 속도를 강화한다.
    {
        CardData cardData = FindCard(cardType); // 강화할 카드를 찾는다.

        if (cardData == null) return; // 카드가 없으면 실행하지 않는다.

        cardData.homingTurnSpeed += value; // 유도 회전 속도를 증가시킨다.
        UpdateCardUI(); // 카드 UI를 갱신한다.

        Debug.Log(cardData.cardName + " Homing Turn Speed +" + value); // 강화 로그를 출력한다.
    }
}