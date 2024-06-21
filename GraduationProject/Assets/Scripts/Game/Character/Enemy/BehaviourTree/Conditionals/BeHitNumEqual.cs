using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeHitNumEqual : BaseEnemyConditional
{
    public SharedInt num;
    public override TaskStatus OnUpdate()
    {
        if(enemyData.Value.BeHitNum == num.Value)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
