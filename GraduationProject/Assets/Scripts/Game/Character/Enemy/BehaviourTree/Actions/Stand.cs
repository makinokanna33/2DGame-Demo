using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : BaseEnemyAction
{
    public override void OnStart()
    {
        base.OnStart();
        enemyData.Value.LastActTime = Time.time;
        CurrentState.Value = EnemyState.STAND;
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time - enemyData.Value.LastActTime < enemyData.Value.ActResetTime)
        {
            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}
