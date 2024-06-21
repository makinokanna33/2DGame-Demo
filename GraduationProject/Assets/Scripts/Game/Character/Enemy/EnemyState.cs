using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    ALARM,      //警戒状态，
    STAND,      //站立状态              
    WALK,       //移动状态
    CHASE,      //追击状态
    ATTACK,     //攻击状态
    BEHIT,        //被攻击状态
}

public class SharedEnemyState : SharedVariable<EnemyState>
{
    public static implicit operator SharedEnemyState(EnemyState value)
    {
        return new SharedEnemyState() { mValue = value};
    }
}
