using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yggdrassi_SkillAttack : BaseEnemyAction
{
    public override void OnStart()
    {
        base.OnStart();
        animator.SetTrigger("startSkillAttack");
        animator.SetBool("SkillAttackEnd", false);
    }

    public override TaskStatus OnUpdate()
    {
        if(animator.GetBool("SkillAttackEnd"))
        {
            enemyData.Value.RemainAttackCD = enemyData.Value.AttackCD;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
