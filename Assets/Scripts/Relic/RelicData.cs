using System;
using UnityEngine;

[Serializable]
public class RelicData
{
    public string relicName; // 유물 이름을 저장한다.
    public string description; // 유물 설명을 저장한다.
    public RelicType relicType; // 유물 종류를 저장한다.
    public float value; // 유물 효과 수치를 저장한다.

    public RelicData(string newName, string newDescription, RelicType newType, float newValue) // 유물 데이터를 생성한다.
    {
        relicName = newName; // 유물 이름을 설정한다.
        description = newDescription; // 유물 설명을 설정한다.
        relicType = newType; // 유물 종류를 설정한다.
        value = newValue; // 유물 수치를 설정한다.
    }

    public string GetDisplayText() // UI에 표시할 문장을 만든다.
    {
        return relicName + " - " + description; // 유물 이름과 설명을 합쳐서 반환한다.
    }
}