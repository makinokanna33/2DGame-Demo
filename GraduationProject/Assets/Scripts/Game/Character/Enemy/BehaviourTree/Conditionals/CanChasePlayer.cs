using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanChasePlayer : BaseEnemyConditional
{
    public Transform leftRayPoint;
    public Transform rightRayPoint;
    public override TaskStatus OnUpdate()
    {
        if (GameUtility.RayCheck(leftRayPoint.position, enemyData.Value.ChaseRange, Vector2.left, LayerMask.GetMask("Player")) ||
            GameUtility.RayCheck(rightRayPoint.position, enemyData.Value.ChaseRange, Vector2.right, LayerMask.GetMask("Player")))
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
