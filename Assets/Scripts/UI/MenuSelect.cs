using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelect : MonoBehaviour
{
    public Button firstButton;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }
}
