using BehaviorDesigner.Runtime;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData",menuName = "MyCreate/Character/EnemyData")]
public class EnemyData : CharacterData
{
    [LabelText("攻击CD")]
    public float AttackCD;
    [LabelText("击退距离")]
    public float RepelledDistance;  
    [LabelText("击退速度")]
    public float RepelledSpeed;  
    [LabelText("追击范围")]
    public float ChaseRange;
    [LabelText("攻击范围")]
    public float AttackRange;
    [LabelText("巡逻状态指令更新间隔")]
    public float ActResetTime;
    [LabelText("最大连续被攻击次数")]
    public float MaxBeHitNum;
    [LabelText("死亡奖励_金币价值")]
    public int DeathReward_Gold;
    [HideInInspector]
    public int BeHitNum;
    [HideInInspector]
    public float RemainAttackCD;
    [HideInInspector]
    public float LastActTime;
}

public class SharedEnemyData : SharedVariable<EnemyData>
{
    public static implicit operator SharedEnemyData(EnemyData value)
    {
        return new SharedEnemyData() { mValue = value };
    }
}
