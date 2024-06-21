using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyConditional : Conditional
{
    public SharedEnemyData enemyData;
    protected Animator animator;

    public override void OnAwake()
    {
        base.OnAwake();
        animator = transform.GetComponent<Animator>();
    }
}
