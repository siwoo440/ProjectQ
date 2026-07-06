using System;
using UnityEngine;

[Serializable]
public class CardData
{
    public string cardName; // 카드 이름을 저장한다.
    public CardType cardType; // 카드 종류를 저장한다.

    [Header("Cost")]
    public float manaCost; // 카드 사용 MP 비용을 저장한다.
    public float cooldown; // 카드 기본 쿨타임을 저장한다.

    [Header("Bullet")]
    public int bulletDamage; // 카드 기본 탄환 피해량을 저장한다.
    public float bulletSpeed; // 카드 기본 탄환 속도를 저장한다.
    public float bulletLifeTime; // 카드 탄환 생존 시간을 저장한다.
    public int bulletCount; // 카드 탄환 수를 저장한다.
    public float spreadAngle; // 카드 탄환 퍼짐 각도를 저장한다.

    [Header("Runtime")]
    public float lastUseTime = -999f; // 마지막 사용 시간을 저장한다.

    public CardData(string newName, CardType newType, float newManaCost, float newCooldown, int newDamage, float newSpeed, float newLifeTime, int newBulletCount, float newSpreadAngle) // 카드 데이터를 생성한다.
    {
        cardName = newName; // 카드 이름을 설정한다.
        cardType = newType; // 카드 종류를 설정한다.
        manaCost = newManaCost; // MP 비용을 설정한다.
        cooldown = newCooldown; // 쿨타임을 설정한다.
        bulletDamage = newDamage; // 피해량을 설정한다.
        bulletSpeed = newSpeed; // 탄환 속도를 설정한다.
        bulletLifeTime = newLifeTime; // 탄환 생존 시간을 설정한다.
        bulletCount = newBulletCount; // 탄환 수를 설정한다.
        spreadAngle = newSpreadAngle; // 탄환 퍼짐 각도를 설정한다.
        lastUseTime = -999f; // 처음에는 바로 사용할 수 있게 설정한다.
    }
}