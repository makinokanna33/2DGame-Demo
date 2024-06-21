using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected BehaviorTree behaviorTree;

    protected SharedEnemyState currentState;
    protected SharedEnemyData enemyData;
    protected Animator animator;
    protected Rigidbody2D rb;

    [Header("受伤时shader的改变时间")]
    public float hurtLength;
    private float hurtCounter;
    private SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        behaviorTree = transform.GetComponent<BehaviorTree>();
        currentState = behaviorTree.GetVariable("CurrentState") as SharedEnemyState;
        animator = transform.GetComponent<Animator>();
        enemyData = (behaviorTree.GetVariable("Data") as SharedEnemyData).Value;
        enemyData = Instantiate(enemyData.Value);
        enemyData.Value.AttackRange = enemyData.Value.AttackRange + Random.Range(-0.1f, 0.1f);
        behaviorTree.SetVariableValue("Data", enemyData);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnEnable()
    {
        enemyData.Value.CurrentHp = enemyData.Value.MaxHp;
        LevelManager.Instance.RegisterEnemy(this);
    }

    protected virtual void OnDisable()
    {
        LevelManager.Instance.RemoveEnemy(this);
    }

    protected virtual void Update()
    {
        enemyData.Value.RemainAttackCD -= Time.deltaTime;
        if(enemyData.Value.AttackCD < 0)
        {
            enemyData.Value.AttackCD = 0;
        }

        if (hurtCounter <= 0)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
        }
        else
            hurtCounter -= Time.deltaTime;
    }

    public float CurrentHp { get => enemyData.Value.CurrentHp; }

    #region 外界接口
    public virtual void HurtShader()
    {
        spriteRenderer.material.SetFloat("_FlashAmount", 1);
        hurtCounter = hurtLength;
    }

    public float GetRandomAttackDamage()
    {
        return Random.Range(enemyData.Value.MinAttack, enemyData.Value.MaxAttack);
    }

    public virtual void ControlEnemy(float time)
    {
        StartCoroutine(ReallyControlEnemy(time));
    }

    private IEnumerator ReallyControlEnemy(float time)
    {
        behaviorTree.enabled = false;
        animator.speed = 0f;
        yield return new WaitForSeconds(time);
        behaviorTree.enabled = true;
        animator.speed = 1f;
    }

    public virtual void TakeDamage(float damage, bool isCrit, bool isNormalAttack = true)
    {
        currentState.Value = EnemyState.BEHIT;
        enemyData.Value.BeHitNum += 1;
        if (enemyData.Value.BeHitNum > enemyData.Value.MaxBeHitNum)
            enemyData.Value.BeHitNum = 1;
      
        //血量
        enemyData.Value.CurrentHp -= damage;

        if (isNormalAttack)
        {
            HurtShader();
            GameObject obj;
            if (isCrit)
            {
                obj = GameUtility.LoadGameObject("Prefabs/VFX/EnemyBeHit_Critical");
                obj.transform.SetParent(null);
                obj.transform.position = transform.position;
            }
            else
            {
                obj = GameUtility.LoadGameObject("Prefabs/VFX/EnemyBeHit");
                obj.transform.SetParent(null);
                obj.transform.position = transform.position;
            }
            obj.transform.localScale = new Vector3(PlayerController.Instance.GetCurrentDirection(), 1, 1);
        }
        DamageNum damageNum = GameUtility.LoadGameObject("Prefabs/VFX/DamageNum").GetComponent<DamageNum>();
        damageNum.transform.position = transform.position;
        damageNum.ShowUIDamage(Mathf.RoundToInt(damage), isCrit);


        if (enemyData.Value.CurrentHp <= 0)
        {
            DeathVFX();
            DeathReward();
        }
    }

    protected virtual void DeathVFX()
    {
        //死亡特效
        GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/Die");
        obj.transform.SetParent(null);
        obj.transform.position = transform.position;
    }

    protected virtual void DeathReward()
    {
        //金币掉落
        int num = enemyData.Value.DeathReward_Gold / 100;
        for (int i = 0; i < num; i++)
        {
            Gold gold = GameUtility.LoadGameObject("Prefabs/DropItem/Gold_Drop").GetComponent<Gold>();
            gold.transform.position = transform.GetComponent<SpriteRenderer>().bounds.center;
            gold.InitGold(100);
        }

        GameObject.Destroy(gameObject);
    }

    public float GetCurrentHp()
    {
        return enemyData.Value.CurrentHp;
    }
    #endregion
}
