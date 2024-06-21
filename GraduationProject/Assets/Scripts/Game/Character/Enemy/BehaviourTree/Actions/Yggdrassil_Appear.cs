using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yggdrassil_Appear : BaseEnemyAction
{
    public SharedBool isFisrtAppear;
    public override void OnStart()
    {
        base.OnStart();
    }
    public override TaskStatus OnUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isFisrtAppear.Value = false;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

}
