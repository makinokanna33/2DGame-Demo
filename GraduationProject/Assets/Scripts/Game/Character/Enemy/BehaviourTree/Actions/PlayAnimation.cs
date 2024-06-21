using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : Action
{
    protected Animator animator;
    public SharedString clipName;
    int num = 0;

    public override void OnAwake()
    {
        base.OnAwake();
        animator = transform.GetComponent<Animator>();
    }
    public override void OnStart()
    {
        base.OnStart();
        animator.Play(clipName.Value);
    }

    public override TaskStatus OnUpdate()
    {
        if (num > 1)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        num += 1;
    }
}
