using System;
using UnityEngine;

[Serializable]
public class RewardData
{
    public string rewardName; // 보상 이름을 저장한다.
    public string description; // 보상 설명을 저장한다.
    public RewardType rewardType; // 보상 종류를 저장한다.
    public RewardRarity rarity; // 보상 등급을 저장한다.
    public float value; // 보상 수치를 저장한다.

    public RewardData(string newName, string newDescription, RewardType newType, RewardRarity newRarity, float newValue) // 보상 데이터를 생성한다.
    {
        rewardName = newName; // 보상 이름을 설정한다.
        description = newDescription; // 보상 설명을 설정한다.
        rewardType = newType; // 보상 종류를 설정한다.
        rarity = newRarity; // 보상 등급을 설정한다.
        value = newValue; // 보상 수치를 설정한다.
    }

    public string GetDisplayText() // UI에 표시할 문장을 만든다.
    {
        return "[" + rarity + "]\n" + rewardName + "\n\n" + description; // 등급, 이름, 설명을 합쳐서 반환한다.
    }
}