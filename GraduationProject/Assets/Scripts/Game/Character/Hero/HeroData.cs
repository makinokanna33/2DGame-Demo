using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "MyCreate/Character/HeroData")]
public class HeroData : CharacterData
{
    [LabelText("英雄姓名")]
    public string HeroName;
    [LabelText("战斗界面_图标")]
    public Sprite BattleView_Icon;
    [LabelText("背包界面_格子图标")]
    public Sprite Invenotry_SlotIcon;
    [LabelText("背包界面_详情图标")]
    public Sprite Inventory_DetailIcon;
    [LabelText("场景_图标")]
    public Sprite Scene_HeadIcon;
    [LabelText("介绍_1")]
    public string HeroInfo_1;
    [LabelText("介绍_2")]
    public string HeroInfo_2;

    [LabelText("连续攻击段数")]
    public int MaxAttackNum;
    [LabelText("跳跃力量")]
    public float JumpForce;
    [LabelText("最大跳跃次数")]
    public int MaxJumpNum;
    [LabelText("暴击率")]
    public float Crit;
    [LabelText("冲刺技能数据")]
    public SkillData Skill_Dash;
    [LabelText("1号技能数据")]
    public SkillData Skill_1;
    [LabelText("2号技能数据")]
    public SkillData Skill_2;

    [HideInInspector]
    public int CurrentAttackNum;    //当前攻击段数
    [HideInInspector]
    public int CurrentJumpNum;      //当前跳跃段数
}
