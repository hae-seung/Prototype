using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth { get; private set; }
    public int curHealth;
    public PlayerSkill enemySkill;
    
    public void Awake()
    {
        MaxHealth = 100;
        curHealth = 100;
    }

    public void Start()
    {
        enemySkill = new PlayerSkill("bodyAttack", 0, 20, 10);
    }

    public int CurHealth
    {
        get { return curHealth; }
        private set{ curHealth = value; }
    }
    
    public void OnDamage(int damage)
    {
        CurHealth -= damage;
        Debug.Log("남은 적 HP" + curHealth);
        if (CurHealth <= 0)
        {
            //원래는 Die 관련 함수 실행
            Destroy(gameObject);
            return;
        }
    }
}
