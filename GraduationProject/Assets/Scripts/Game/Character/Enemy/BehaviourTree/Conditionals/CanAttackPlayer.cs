using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAttackPlayer : BaseEnemyConditional
{
    private Transform player;

    public override void OnStart()
    {
        base.OnStart();
        player = PlayerController.Instance.currentCharacter.transform;
    }

    public override TaskStatus OnUpdate()
    {
        if (enemyData.Value.RemainAttackCD > 0)
        {
            return TaskStatus.Failure;
        }
        else
            return TaskStatus.Success;
    }

}
