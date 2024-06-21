using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BaseEnemyAction
{
    private AnimatorStateInfo stateInfo;

    //private bool isSleeping;
    private BehaviorTree behaviorTree;

    public override void OnAwake()
    {
        base.OnAwake();
        behaviorTree = transform.GetComponent<BehaviorTree>();
    }

    public override void OnStart()
    {
        base.OnStart();
        //isSleeping = false;

        if (PlayerController.Instance.currentCharacter.transform.position.x - transform.position.x < 0)
            transform.rotation = new Quaternion(0, 180, 0, 0);
        else
            transform.rotation = new Quaternion(0, 0, 0, 0);
        CurrentState.Value = EnemyState.ATTACK;
        enemyData.Value.RemainAttackCD = enemyData.Value.AttackCD;
    }
    public override TaskStatus OnUpdate()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if ((stateInfo.IsName("Attack") || stateInfo.IsTag("Attack")) && stateInfo.normalizedTime >= 1f)
        {
            return TaskStatus.Success;
        }
       
        return TaskStatus.Running;
    }
    
    //IEnumerator Sleep()
    //{
    //    isSleeping = true;
    //    animator.Play("Stand");
    //    behaviorTree.enabled = false;
    //    yield return new WaitForSeconds(Random.Range(0.0f, 1f));
    //    behaviorTree.enabled = true;
    //    isSleeping = false;
    //}
}
