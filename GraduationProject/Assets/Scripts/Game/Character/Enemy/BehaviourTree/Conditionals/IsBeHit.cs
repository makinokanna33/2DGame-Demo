using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBeHit : BaseEnemyConditional
{
    public SharedEnemyState enemyState;
    protected AnimatorStateInfo stateInfo;
    public override void OnAwake()
    {
        base.OnAwake();
        //EventManager.StartListening(EventName.PlayerAttackEnmey, BeHitStateJudge);
    }


    public override TaskStatus OnUpdate()
    {
        if (enemyState.Value == EnemyState.BEHIT)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    private void BeHitStateJudge()
    {
        if(EventManager.GetData<Transform>(EventName.PlayerAttackEnmey) == transform)
        {
            enemyState.Value = EnemyState.BEHIT;
        }
    }
}
