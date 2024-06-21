using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAir : BaseEnemyConditional
{
    public Transform leftRayPoint_Ground;
    public Transform rightRayPoint_Ground;
    private BoxCollider2D collider2D;
    public override void OnAwake()
    {
        base.OnAwake();
        collider2D = transform.GetComponent<BoxCollider2D>();
    }

    public override TaskStatus OnUpdate()
    {
        Vector3 leftPoint = collider2D.bounds.min;
        Vector3 rightPoint = leftPoint + new Vector3(collider2D.bounds.size.x, 0, 0);
        if (!GameUtility.RayCheck(leftPoint, 0.3f, Vector2.down, LayerMask.GetMask("Ground"))
            && !GameUtility.RayCheck(rightPoint, 0.3f, Vector2.down, LayerMask.GetMask("Ground")))
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
