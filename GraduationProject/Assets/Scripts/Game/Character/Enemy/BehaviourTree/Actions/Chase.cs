using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : BaseEnemyAction
{
    private Transform target;
    private Rigidbody2D rigidbody2D;

    public override void OnAwake()
    {
        base.OnAwake();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void OnStart()
    {
        base.OnStart();
        target = PlayerController.Instance.currentCharacter.transform;
        CurrentState.Value = EnemyState.CHASE;
    }

    public override TaskStatus OnUpdate()
    {
        //玩家在怪物左边
        if (target.position.x - transform.position.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            transform.Translate(new Vector2(-enemyData.Value.MoveSpeed * Time.deltaTime, 0), Space.World);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.Translate(new Vector2(enemyData.Value.MoveSpeed * Time.deltaTime, 0), Space.World);
        }

        return TaskStatus.Running;
    }
    public override void OnConditionalAbort()
    {
        rigidbody2D.velocity = new Vector2(0, 0);
    }
}
