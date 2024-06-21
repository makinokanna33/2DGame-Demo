using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPause : BaseEnemyAction
{
    public override TaskStatus OnUpdate()
    {
        animator.speed = 0;
        return TaskStatus.Running;
    }
    public override void OnConditionalAbort()
    {
        base.OnConditionalAbort();
        animator.speed = 1;
    }
}
