using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyAction : Action
{
    protected Animator animator;
    public SharedEnemyData enemyData;
    public SharedEnemyState CurrentState;
    public override void OnAwake()
    {
        base.OnAwake();
        animator = transform.GetComponent<Animator>();
    }
}
