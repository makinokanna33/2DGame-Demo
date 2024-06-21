using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SkillType
{
    [LabelText("无")]
    None,   // 无
    [LabelText("常规型")]
    Common, // 常规型
    [LabelText("冲刺型")]
    Dash    // 冲刺型
}

[CreateAssetMenu(fileName = "SkillData", menuName ="MyCreate/Character/SkillData")]
public class SkillData:ScriptableObject
{
    [BoxGroup("Skill"), LabelText("技能名称")]
    public string SkillName;
    [BoxGroup("Skill"), LabelText("技能类型")]
    public SkillType Type;
    [ShowIfGroup("Skill/Common", Condition = "@Type==SkillType.Dash||Type==SkillType.Common"), Header("Common")]
    [LabelText("技能图标")]
    public Sprite SkillIcon;

    [ShowIfGroup("Skill/Common", Condition = "@Type==SkillType.Dash||Type==SkillType.Common"), Header("Common")]
    [LabelText("技能CD")]
    public float CoolDown;          // 技能CD
    [ShowIfGroup("Skill/Common", Condition = "@Type==SkillType.Dash||Type==SkillType.Common")]
    [LabelText("技能次数")]
    public int MaxComboNum;         // 最大技能连击段数 
    [ShowIfGroup("Skill/Common", Condition = "@Type==SkillType.Dash||Type==SkillType.Common")]
    [LabelText("土狼时间")]
    public float GraceTime;         // 技能多段释放的土狼时间

    [ShowIfGroup("Skill/Dash", Condition = "@Type==SkillType.Dash"), Header("Dash")]
    [LabelText("冲刺技能持续时长")]
    public float DashSkillDuration; // 冲刺技能持续时间
    [ShowIfGroup("Skill/Dash", Condition = "@Type==SkillType.Dash")]
    [LabelText("冲刺技能速度")]
    public float Speed;             // 冲刺技能速度

    [HideInInspector] public float RemainCD;            //剩余CD时间
    [HideInInspector] public float LastUseSkillTime;    //上一次使用技能时间
    [HideInInspector] public int CurrentComboNum;       //当前技能段数
}
