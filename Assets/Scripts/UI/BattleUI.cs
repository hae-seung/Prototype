using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelState
{
    public bool[] panelStates;  // 패널의 활성화 상태 저장 배열

    public PanelState(int size)  // 생성자
    {
        panelStates = new bool[size];
    }
}

public class BattleUI : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerStatus playerStatus;
    public GameObject[] panels;  // 패널 배열
    public Button[] skills;
    private Stack<PanelState> panelStateStack = new Stack<PanelState>();

    private void Start()
    {
        // 플레이어 체력 초기화
        healthSlider.maxValue = playerStatus.MaxHealth;
        healthSlider.value = playerStatus.CurHealth;
        playerStatus.OnHealthChanged += ChangePlayerUI;
        // 패널의 초기 상태 저장
        SaveCurrentPanelState();
    }

    public void SetSkillToButton()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (i < playerStatus.playerSkills.Count)
            {
                PlayerSkill skill = playerStatus.playerSkills[i];
                Text buttonText = skills[i].GetComponentInChildren<Text>();

                if (buttonText != null)
                {
                    buttonText.text = skill.skillName;  // 스킬 이름을 버튼 텍스트로 설정
                }

                // 클릭 시 스킬 ID를 전달
                int skillID = skill.skillID;
                skills[i].onClick.AddListener(() => OnSkillButtonClick(skillID));
            }
        }
    }

    private void OnSkillButtonClick(int skillID)
    {
        // BattleController에 스킬 ID 전달
        BattleController.Instance.UseSkill(skillID);
    }
    private void Update()
    {
        // X키로 이전 패널 상태 복원
        if (Input.GetKeyDown(KeyCode.X))
        {
            RestorePreviousPanel();
        }

        // Z키로 현재 선택된 버튼 클릭 및 패널 상태 저장
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ClickSelectedButton();
        }
    }

    // Z키로 버튼 클릭 처리
    private void ClickSelectedButton()
    {
        // 현재 선택된 UI 오브젝트 가져오기
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected != null)
        {
            // 선택된 오브젝트가 버튼이라면 클릭 이벤트 실행
            Button button = selected.GetComponent<Button>();
            if (button != null)
            {
                // 버튼 클릭 트리거
                button.onClick.Invoke();
                
                // 버튼 클릭 후 패널 상태가 변했는지 체크
                if (HasPanelStateChanged())
                {
                    SaveCurrentPanelState();  // 패널 상태가 변경된 경우에만 저장
                }
            }
        }
    }

// 패널 상태 변경 여부 확인 함수
    private bool HasPanelStateChanged()
    {
        if (panelStateStack.Count == 0) return true;  // 스택이 비어있으면 상태가 변경된 것으로 간주

        PanelState currentState = new PanelState(panels.Length);

        // 현재 패널 상태를 수집
        for (int i = 0; i < panels.Length; i++)
        {
            currentState.panelStates[i] = panels[i].activeSelf;
        }

        // 스택의 마지막 상태와 현재 상태 비교
        PanelState previousState = panelStateStack.Peek();
        for (int i = 0; i < panels.Length; i++)
        {
            if (currentState.panelStates[i] != previousState.panelStates[i])
            {
                return true;  // 상태가 다르면 변경된 것으로 간주
            }
        }

        return false;  // 상태가 동일하다면 변경되지 않음
    }

    // 패널 상태 스택에서 이전 상태 복원
    private void RestorePreviousPanel()
    {
        if (panelStateStack.Count > 1)
        {
            // 현재 상태 제거
            panelStateStack.Pop();

            // 이전 상태 가져오기
            PanelState previousState = panelStateStack.Peek();
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(previousState.panelStates[i]);  // 저장된 패널 상태로 되돌림
            }

            Debug.Log("이전 패널 상태로 복원되었습니다.");
        }
        else
        {
            Debug.Log("더 이상 이전 상태가 없습니다.");
        }
    }

    // 플레이어 체력 UI 업데이트
    public void ChangePlayerUI()
    {
        healthSlider.value = playerStatus.CurHealth;
    }

    // 현재 패널 상태를 스택에 저장
    private void SaveCurrentPanelState()
    {
        PanelState currentState = new PanelState(panels.Length);
        for (int i = 0; i < panels.Length; i++)
        {
            currentState.panelStates[i] = panels[i].activeSelf;  // 각 패널의 활성화 상태 저장
        }
        panelStateStack.Push(currentState);  // 스택에 상태 저장

        Debug.Log("현재 패널 상태가 저장되었습니다.");
    }
}
