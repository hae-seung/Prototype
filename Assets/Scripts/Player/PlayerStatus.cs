using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public event Action OnHealthChanged;
    public List<PlayerSkill> playerSkills = new List<PlayerSkill>();
    public int MaxHealth { get; private set; }
    public int curHealth;
    public int CurHealth
    {
        get { return curHealth; }
        set { curHealth = value; }
    }

    private void Awake()
    {
        MaxHealth = 100;
        CurHealth = 80;
    }

    private void Start()
    {
        playerSkills.Add(new PlayerSkill("Slash", 10,30,1));
        playerSkills.Add(new PlayerSkill("Heal", 15,-30,2));
        playerSkills.Add(new PlayerSkill("FireBall", 20,50,3));
        playerSkills.Add(new PlayerSkill("Test", 10,10,4));
    }
    
    public void OnDamage(int damage)
    {
        CurHealth -= damage;
        if (CurHealth <= 0)
        {
            //원래는 Die 관련 함수 실행
            Destroy(gameObject);
            return;
        }
        
        OnHealthChanged?.Invoke();
    }
    
    
    public PlayerSkill GetSkillById(int skillID)
    {
        // 스킬 리스트를 순회하며 일치하는 ID의 스킬을 반환
        foreach (PlayerSkill skill in playerSkills)
        {
            if (skill.skillID == skillID)
            {
                return skill;
            }
        }

        // 일치하는 스킬이 없을 경우 null 반환
        Debug.LogWarning("해당 스킬 ID를 찾을 수 없습니다: " + skillID);
        return null;
    }
}
