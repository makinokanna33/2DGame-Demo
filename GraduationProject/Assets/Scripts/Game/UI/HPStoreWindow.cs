using GameFrameWork;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPStoreWindow : BaseWindow
{
    public TextMeshProUGUI text;
    private int gold;

    protected override void Awake()
    {
        base.Awake();
        EventManager.StartListening(EventName.OnLevelLoaded, OnLevelLoaded);
        gold = 500;
    }

    private void OnLevelLoaded()
    {
        gold = 500;
    }

    protected override void OnShowIng()
    {
        base.OnShowIng();
        text.text = "消耗" + gold + "金币回复50生命值";
        InputManager.Instance.CharacterControlLock = true;
    }

    protected override void OnClosed()
    {
        base.OnClosed();
        InputManager.Instance.CharacterControlLock = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(GameManager.Instance.UseGold(gold))
            {
                gold *= 2;
                PlayerController.Instance.currentCharacter.AddCurrentHp(50);
                Tips.ShowTips("回复成功");
                UIManager.CloseWindow(ScreenName.HPStoreWindow);
            }
            else
            {
                Tips.ShowTips("金币不足");
            }
        }
        else if(Input.GetKeyDown(KeyCode.X) && UIManager.CurrentWindow == ScreenName.HPStoreWindow)
        {
            UIManager.CloseWindow(ScreenName.HPStoreWindow);
        }
    }
}
