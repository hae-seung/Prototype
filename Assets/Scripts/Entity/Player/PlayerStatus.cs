using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public event Action OnHealthChanged;
    public List<SkillPreset> playerPreset = new();  // 플레이어의 스킬 프리셋 리스트

    public SkillPreset skillPreset1;
    public SkillPreset skillPreset2;

    public int MaxHealth { get; private set; }
    public int curHealth;
    public int CurHealth
    {
        get { return curHealth; }
        private set { curHealth = value; }
    }

    public SkillPreset SkillPreset { get; set; }

    private void Awake()
    {
        MaxHealth = 100;
        CurHealth = 80;  // 플레이어 초기 체력
        SkillPreset = skillPreset1;  // 기본 스킬 프리셋 설정
    }

    private void Start()
    {
        // 스킬 프리셋 1에 스킬 추가
        skillPreset1.skills.Add(new PlayerSkill("Slash", 10, 30, 1));
        skillPreset1.skills.Add(new PlayerSkill("Heal", 15, -30, 2));
        skillPreset1.skills.Add(new PlayerSkill("FireBall", 20, 50, 3));
        skillPreset1.skills.Add(new PlayerSkill("Test", 10, 10, 4));

        // 스킬 프리셋 2에 스킬 추가
        skillPreset2.skills.Add(new PlayerSkill("A", 10, 10, 5));
        skillPreset2.skills.Add(new PlayerSkill("B", 10, 10, 6));
        skillPreset2.skills.Add(new PlayerSkill("C", 10, 10, 7));
        skillPreset2.skills.Add(new PlayerSkill("D", 10, 10, 8));

        // 플레이어의 모든 스킬 프리셋 리스트에 추가
        playerPreset.Add(skillPreset1);
        playerPreset.Add(skillPreset2);
    }

    // 체력 감소를 부드럽게 진행하는 코루틴
    public IEnumerator ReduceHealthOverTime(int damage)
    {
        int startHealth = curHealth;
        int targetHealth = Mathf.Max(curHealth - damage, 0);

        float duration = 1.0f;  // 체력 감소에 걸리는 시간
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            curHealth = (int)Mathf.Lerp(startHealth, targetHealth, elapsed / duration);

            OnHealthChanged?.Invoke();  // UI 업데이트

            yield return null;
        }

        curHealth = targetHealth;
        OnHealthChanged?.Invoke();

        if (curHealth <= 0)
        {
            Die();  // 플레이어 사망 처리
        }
    }


    // 플레이어가 데미지를 받을 때 호출
    public void OnDamage(int damage)
    {
        StartCoroutine(ReduceHealthOverTime(damage));  // 체력을 부드럽게 감소시키는 코루틴 실행
    }

    // 플레이어 사망 처리
    private void Die()
    {
        Debug.Log("플레이어가 사망했습니다.");
        Destroy(gameObject);  // 플레이어 오브젝트 제거
    }

    // 스킬 ID로 스킬 찾기
    public PlayerSkill GetSkillById(int skillID)
    {
        foreach (SkillPreset skillPreset in playerPreset)
        {
            for (int i = 0; i < skillPreset.skills.Count; i++)
            {
                if (skillPreset.skills[i].skillID == skillID)
                {
                    return skillPreset.skills[i];  // 일치하는 스킬 반환
                }
            }
        }
        Debug.Log("스킬을 찾을 수 없습니다.");
        return null;
    }
}
