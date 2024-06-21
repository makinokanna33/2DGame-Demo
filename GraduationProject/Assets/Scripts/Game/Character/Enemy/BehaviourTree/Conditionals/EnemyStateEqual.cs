using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateEqual : Conditional
{
    public SharedEnemyState enemyState1;
    public SharedEnemyState enemyState2;

    public override TaskStatus OnUpdate()
    {
        if(enemyState1.Value == enemyState2.Value)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
