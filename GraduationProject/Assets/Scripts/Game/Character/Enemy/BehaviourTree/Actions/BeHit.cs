using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeHit : BaseEnemyAction
{
    private EnemyController enemyController;
    private bool isRepelling;
    public override void OnAwake()
    {
        base.OnAwake();
        enemyController = transform.GetComponent<EnemyController>();
        //EventManager.StartListening(EventName.PlayerAttackEnmey, OnBeHit);
    }
    public override void OnStart()
    {
        base.OnStart();
        isRepelling = false;
    }

    public override TaskStatus OnUpdate()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("BeHit_" + enemyData.Value.BeHitNum))
        {
            animator.Play("BeHit_" + enemyData.Value.BeHitNum);
            isRepelling = false;
        }
        ;
        if (!isRepelling)
        {
            isRepelling = true;
            float repelledDistance = UnityEngine.Random.Range(0, enemyData.Value.RepelledDistance);
            if (PlayerController.Instance.currentCharacter.transform.position.x - transform.position.x > 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                transform.DOMoveX(transform.position.x - repelledDistance,
                    repelledDistance / enemyData.Value.RepelledSpeed);
            }
            else
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);

                transform.DOMoveX(transform.position.x + repelledDistance,
                   repelledDistance / enemyData.Value.RepelledSpeed);
            }
        }
        if (stateInfo.IsTag("BeHit") && stateInfo.normalizedTime >= 1.0f)
        {
            CurrentState.Value = EnemyState.STAND;
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Running;
    }

    //private void OnBeHit()
    //{
    //    if (EventManager.GetData<Transform>(EventName.PlayerAttackEnmey) == transform)
    //    {
    //        enemyData.Value.BeHitNum += 1;
    //        if (enemyData.Value.BeHitNum > enemyData.Value.MaxBeHitNum)
    //            enemyData.Value.BeHitNum = 1;
    //        animator.Play("BeHit_" + enemyData.Value.BeHitNum);

    //        //获取玩家攻击力
    //        float damage = PlayerController.Instance.currentCharacter.GetRandomAttackDamage();
    //        bool isCrit =PlayerController.Instance.currentCharacter.GetRandomCrit();
    //        damage = isCrit ? damage * 1.5f : damage;

    //        //扣除血量
    //        enemyData.Value.CurrentHp -= damage;

    //        DamageNum damageNum = GameUtility.LoadGameObject("Prefabs/VFX/DamageNum").GetComponent<DamageNum>();
    //        damageNum.transform.position = transform.position;
    //        damageNum.ShowUIDamage(Mathf.RoundToInt(damage), isCrit);

    //        enemyController.HurtShader();

    //        if (enemyData.Value.CurrentHp<=0)
    //        {
    //            //死亡特效
    //            GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/Die");
    //            obj.transform.SetParent(null);
    //            obj.transform.position = transform.position;

    //            //金币掉落
    //            Gold gold = GameUtility.LoadGameObject("Prefabs/DropItem/Gold_Drop").GetComponent<Gold>();
    //            gold.transform.position = transform.GetComponent<SpriteRenderer>().bounds.center;
    //            gold.InitGold(enemyData.Value.DeathReward_Gold);

    //            GameObject.Destroy(gameObject);
    //        }
    //        else
    //        {
    //            if (isCrit)
    //            {
    //                GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/EnemyBeHit_Critical");
    //                obj.transform.SetParent(null);
    //                obj.transform.position = transform.position;
    //            }
    //            else
    //            {
    //                GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/EnemyBeHit");
    //                obj.transform.SetParent(null);
    //                obj.transform.position = transform.position;
    //            }
    //        }
    //    }
    //}
}
