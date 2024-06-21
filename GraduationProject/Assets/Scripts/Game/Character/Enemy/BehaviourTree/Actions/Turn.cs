using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : BaseEnemyAction
{
    float timer;
    public override void OnStart()
    {
        base.OnStart();
        if (transform.rotation.y == 0)
            transform.rotation = new Quaternion(0, 180, 0, 0);
        else
            transform.rotation = new Quaternion(0, 0, 0, 0);
        timer = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time - timer < 1f)
        {
            if (transform.rotation.y == 0)
                transform.Translate(Vector2.right * new Vector2(enemyData.Value.MoveSpeed * Time.deltaTime, 0), Space.World);
            else
                transform.Translate(Vector2.right * new Vector2(-enemyData.Value.MoveSpeed * Time.deltaTime, 0), Space.World);
            return TaskStatus.Running;
        }

        return TaskStatus.Success;
    }
}
