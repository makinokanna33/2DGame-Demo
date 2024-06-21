using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshAttackCD : BaseEnemyAction
{
    public override TaskStatus OnUpdate()
    {
        if(enemyData.Value.RemainAttackCD > 0)
        {
            return TaskStatus.Running;
        }

        enemyData.Value.RemainAttackCD = 0;
        return TaskStatus.Success;
    }
}
