using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : BaseEnemyAction
{
    private Rigidbody2D rigidbody2D;
    public override void OnAwake()
    {
        base.OnAwake();
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
    }

    public override void OnStart()
    {
        base.OnStart();
        enemyData.Value.LastActTime = Time.time;
        if(Random.Range(0,100) < 50)
            transform.rotation = new Quaternion(0, 180, 0, 0);
        else
            transform.rotation = new Quaternion(0, 0, 0, 0);
        CurrentState.Value = EnemyState.WALK;
    }

    public override TaskStatus OnUpdate()
    {
        if(Time.time - enemyData.Value.LastActTime < enemyData.Value.ActResetTime)
        {
            if (transform.rotation.y == 0)
                transform.Translate(Vector2.right * new Vector2(enemyData.Value.MoveSpeed * Time.deltaTime, 0), Space.World);
            else
                transform.Translate(Vector2.right * new Vector2(-enemyData.Value.MoveSpeed * Time.deltaTime, 0), Space.World);

            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}
