using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; private set; }

    [SerializeField]private PlayerStatus playerStatus;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UseSkill(int id)
    {
        PlayerSkill skill = playerStatus.GetSkillById(id);
        if (skill != null)
        {
            Debug.Log(skill.skillName);
        }
        else
        {
            Debug.Log("유효하지 않은 스킬");
        }
    }
    
}
