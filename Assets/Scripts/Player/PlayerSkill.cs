using UnityEngine;

[System.Serializable]
public class PlayerSkill
{
    public int skillID;
    public string skillName;  // 스킬 이름
    public int fatigueCost;   // 스킬 사용 시 소모되는 피로도
    public int damage;        // 스킬이 입히는 데미지

    // 생성자
    public PlayerSkill(string name, int fatigue, int dmg, int id)
    {
        skillName = name;
        fatigueCost = fatigue;
        damage = dmg;
        skillID = id;
    }
}