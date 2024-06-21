using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAttackRange : BaseEnemyConditional
{
    private Transform player;
    public Transform leftRayPoint;
    public Transform rightRayPoint;

    public float attackRange = 0f;
    public bool isCircle;
    private BoxCollider2D collider2D;
    private AnimatorStateInfo stateInfo;
    public override void OnStart()
    {
        collider2D = transform.GetComponent<BoxCollider2D>();
        base.OnStart();
        if(attackRange == 0)
        {
            attackRange = enemyData.Value.AttackRange;
        }
    }

    public override TaskStatus OnUpdate()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        ////正在攻击时不用判定
        if ((stateInfo.IsName("Attack") || stateInfo.IsTag("Attack")) && stateInfo.normalizedTime <= 0.95f)
        {
            return TaskStatus.Success;
        }
        if(isCircle)
        {
            if (GameUtility.RayCheckCircle(collider2D.bounds.center, attackRange, LayerMask.GetMask("Player")))
            {
                return TaskStatus.Success;
            }
        }
        else
        {
            if (GameUtility.RayCheck(leftRayPoint.position, attackRange, Vector2.left, LayerMask.GetMask("Player")) ||
      GameUtility.RayCheck(rightRayPoint.position, attackRange, Vector2.right, LayerMask.GetMask("Player")))
            {
                return TaskStatus.Success;
            }
        }

      

        return TaskStatus.Failure;
    }
}
