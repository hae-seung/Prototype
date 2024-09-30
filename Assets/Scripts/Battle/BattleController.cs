using System.Collections;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; private set; }
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Enemy enemyStatus;

    private bool playerTurn = true;  // 플레이어의 턴 여부
    private bool battleOver = false;  // 전투 종료 여부

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

    private void Start()
    {
        StartCoroutine(BattleLoop());
    }

    // 턴 기반 전투 루프
    // 턴 기반 전투 루프
    private IEnumerator BattleLoop()
    {
        while (!battleOver)
        {
            if (playerTurn)
            {
                Debug.Log("플레이어의 턴입니다. 스킬을 선택하세요.");
                // 플레이어가 스킬을 선택할 때까지 대기 (스킬 버튼 클릭 이벤트를 통해 UseSkill 호출)
                yield return new WaitUntil(() => !playerTurn);
            }
            else
            {
                // 적의 턴 - 적의 공격이 끝나고 플레이어의 체력이 닳때까지 대기
                yield return StartCoroutine(EnemyTurn());
            }

            yield return null;
        }
    }


    // 플레이어가 스킬을 사용할 때 호출 (스킬 버튼 이벤트를 통해 호출됨)
    public void UseSkill(int skillID)
    {
        if (playerTurn && !battleOver)
        {
            PlayerSkill skill = playerStatus.GetSkillById(skillID);
            if (skill != null)
            {
                Debug.Log($"플레이어가 {skill.skillName} 스킬을 사용했습니다.");
                enemyStatus.OnDamage(skill.damage);  // 적에게 데미지 적용
                CheckBattleStatus();
                playerTurn = false;  // 플레이어 턴 끝 -> 적의 턴으로 넘어감
            }
            else
            {
                Debug.Log("유효하지 않은 스킬");
            }
        }
    }
    
    // 적의 공격 로직
    private IEnumerator EnemyTurn()
    {
        if (!battleOver)
        {
            Debug.Log("적이 공격을 시작합니다.");
        
            // 적의 공격이 끝날 때까지 기다림 (코루틴 대기)
            yield return StartCoroutine(playerStatus.ReduceHealthOverTime(enemyStatus.enemySkill.damage));
        
            // 플레이어의 체력이 모두 줄어들고 나서 전투 상태 체크
            CheckBattleStatus();

            if (!battleOver)
            {
                Debug.Log("플레이어의 턴으로 넘어갑니다.");
                playerTurn = true;  // 적의 공격이 끝난 후에야 플레이어의 턴으로 넘어감
            }
        }
    }


    // 전투 상태 체크 (플레이어나 적의 체력이 0 이하가 되는지 확인)
    //사망 이벤트 호출을 통해서 간단하게 구현 가능
    private void CheckBattleStatus()
    {
        if (playerStatus.CurHealth <= 0)
        {
            battleOver = true;
            Debug.Log("플레이어가 패배했습니다.");
        }
        else if (enemyStatus.CurHealth <= 0)
        {
            battleOver = true;
            Debug.Log("플레이어가 승리했습니다.");
        }
    }
}
