using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedToTurn : Conditional
{
    public SharedEnemyState currentState;

    private BoxCollider2D collider2D;
    public override void OnAwake()
    {
        base.OnAwake();
        collider2D = transform.GetComponent<BoxCollider2D>();
    }
    public override TaskStatus OnUpdate()
    {
        Vector3 leftPoint_ground = collider2D.bounds.min;
        Vector3 rightPoint_ground = leftPoint_ground + new Vector3(collider2D.bounds.size.x, 0, 0);
        Vector3 leftPoint_wall = collider2D.bounds.center - new Vector3(collider2D.bounds.extents.x, 0, 0);
        Vector3 rightPoint_wall = collider2D.bounds.center + new Vector3(collider2D.bounds.extents.x, 0, 0);

        if(currentState.Value == EnemyState.WALK || currentState.Value == EnemyState.CHASE)
        {
            if (!GameUtility.RayCheck(leftPoint_ground, 0.3f, Vector2.down, LayerMask.GetMask("Ground"))
            || GameUtility.RayCheck(leftPoint_wall, 0.3f, Vector2.left, LayerMask.GetMask("Ground"))
             || !GameUtility.RayCheck(rightPoint_ground, 0.3f, Vector2.down, LayerMask.GetMask("Ground"))
            || GameUtility.RayCheck(rightPoint_wall, 0.3f, Vector2.right, LayerMask.GetMask("Ground")))
            {
                return TaskStatus.Success;
            }
        }

        return TaskStatus.Failure;
    }
}
