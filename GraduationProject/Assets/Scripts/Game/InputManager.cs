using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : SingletonMono<InputManager>
{
    //是否允许输入
    public bool CharacterControlLock = false;

    private Vector3 lastPosition;
    private float timer;
    // x轴的移动
    public float XVelocity { get; private set; }
    private void Update()
    {
        if(Vector3.Distance(lastPosition, Input.mousePosition) <= 1)
            timer += Time.deltaTime;
        else
            timer = 0f;
        Cursor.visible = timer <= 2f;


        CharacterInput();
        OtherInput();
    }

    private void LateUpdate()
    {
        lastPosition = Input.mousePosition;
    }

    private void OtherInput()
    {
        if (UIManager.CurrentView == ScreenName.BattleView)
        {
            if(UIManager.CurrentWindow == ScreenName.None && Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.OpenWindow(ScreenName.PauseWindow);
            }

            if (UIManager.CurrentWindow == ScreenName.None && Input.GetKeyDown(KeyCode.Tab))
            {
                UIManager.OpenWindow(ScreenName.InventoryWindow);
            }

        }

    }

    private void CharacterInput()
    {
        if (CharacterControlLock)
        {
            XVelocity = 0;
            return;
        }

        // 获取按键移动
        XVelocity = Input.GetAxis("Horizontal");
        if (XVelocity != 0)
            EventManager.EmitEventData(EventName.PlayerInput_Move, XVelocity);
        // 攻击
        if (Input.GetKeyDown(KeyCode.X))
            EventManager.EmitEvent(EventName.PlayerInput_Attack);
        // 下落
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.C))
            EventManager.EmitEvent(EventName.PlayerInput_Fall);
        // 跳跃
        else if (Input.GetKeyDown(KeyCode.C))
            EventManager.EmitEvent(EventName.PlayerInput_Jump);
        // 冲刺
        if (Input.GetKeyDown(KeyCode.Z))
            EventManager.EmitEvent(EventName.PlayerInput_Dash);
        // 技能1
        if (Input.GetKeyDown(KeyCode.A))
            EventManager.EmitEvent(EventName.PlayerInput_Skill1);
        // 技能2
        if (Input.GetKeyDown(KeyCode.S))
            EventManager.EmitEvent(EventName.PlayerInput_Skill2);
        // 交换角色
        if (Input.GetKeyDown(KeyCode.Space))
            EventManager.EmitEvent(EventName.PlayerInput_Switch);
       
    }
}
