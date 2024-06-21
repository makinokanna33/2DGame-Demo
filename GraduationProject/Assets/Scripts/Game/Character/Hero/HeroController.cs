using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GameFrameWork;
using UnityEngine.Events;

public class HeroController : MonoBehaviour
{
    [SerializeField]
    protected HeroData heroData;

    protected bool isOnGround = true;      //是否在地面
    protected bool isDashing = false;       //是否在冲刺
    protected bool isAttacking = false;     //是否正在攻击
    protected bool isJumping = false;       //是否正在跳跃
    protected bool isAirAttacking = false;  //是否正在空中攻击
    protected bool isUseSkill1 = false;  //正在使用技能1
    protected bool isUseSkill2 = false;  //正在使用技能2

    protected Rigidbody2D rb;
    protected Animator anima;

    protected const float GravityScale = 6f;
    [HideInInspector]
    public bool isAllowWalk = true;
    [HideInInspector]
    public bool isAllowAttack = true;

    protected Transform leftRayPoint;
    protected Transform rightRayPoint;


    protected virtual void Awake()
    {
        //HeroData仅为函数模板，需要根据模板创建人物数据避免修改原模版信息
        heroData = Instantiate(heroData);
        heroData.Skill_1 = heroData.Skill_1 != null ? Instantiate(heroData.Skill_1):null;
        heroData.Skill_2 = heroData.Skill_2 != null ? Instantiate(heroData.Skill_2):null;
        heroData.Skill_Dash = heroData.Skill_Dash != null ? Instantiate(heroData.Skill_Dash) :null;
        heroData.CurrentHp = GetMaxHp();
        rb = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
        if (heroData.Skill_1 != null)
        {
            heroData.Skill_1.RemainCD = 0;
            heroData.Skill_1.LastUseSkillTime = -10;
            heroData.Skill_1.CurrentComboNum = 0;
        }
        if (heroData.Skill_2 != null)
        {
            heroData.Skill_2.RemainCD = 0;
            heroData.Skill_2.LastUseSkillTime = -10;
            heroData.Skill_2.CurrentComboNum = 0;
        }
        if (heroData.Skill_Dash != null)
        {
            heroData.Skill_Dash.RemainCD = 0;
            heroData.Skill_Dash.LastUseSkillTime = -10;
            heroData.Skill_Dash.CurrentComboNum = 0;
        }
        leftRayPoint = transform.Find("GroundCheckPoint/LeftRayPoint");
        rightRayPoint = transform.Find("GroundCheckPoint/RightRayPoint");
    }


    protected virtual void OnEnable()
    {
        EventManager.StartListening(EventName.PlayerInput_Move, Move);
        EventManager.StartListening(EventName.PlayerInput_Jump, Jump);
        EventManager.StartListening(EventName.PlayerInput_Dash, Dash);
        EventManager.StartListening(EventName.PlayerInput_Attack, NormalAttack);
        EventManager.StartListening(EventName.PlayerInput_Skill1, Skill1);
        EventManager.StartListening(EventName.PlayerInput_Skill2, Skill2);
        EventManager.StartListening(EventName.PlayerInput_Fall, Fall);
        EventManager.StartListening(EventName.PlayerEnhancement, OnPlayerEnhancement);
    }

    protected virtual void OnDisable()
    {
        EventManager.StopListening(EventName.PlayerInput_Move, Move);
        EventManager.StopListening(EventName.PlayerInput_Jump, Jump);
        EventManager.StopListening(EventName.PlayerInput_Dash, Dash);
        EventManager.StopListening(EventName.PlayerInput_Attack, NormalAttack);
        EventManager.StopListening(EventName.PlayerInput_Skill1, Skill1);
        EventManager.StopListening(EventName.PlayerInput_Skill2, Skill2);
        EventManager.StopListening(EventName.PlayerEnhancement, OnPlayerEnhancement);
    }

    protected virtual void Update()
    {
        //玩家状态检测
        CharacterStateCheck();

        //更新动画
        RefreshAnimation();

        //技能CD
        SkillColdDown();

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer();
        }
    }

    #region Action
    private void Fall()
    {
        BoxCollider2D collider2D = transform.GetComponent<BoxCollider2D>();
        RaycastHit2D res = GameUtility.RayCheck(collider2D.bounds.min + new Vector3(collider2D.bounds.extents.x, 0, 0), 0.3f, Vector2.down, LayerMask.GetMask("Ground"));
        if (res && res.collider.CompareTag("Platform"))
        {
            collider2D.isTrigger = true;
        }
    }

    protected virtual void NormalAttack()
    {
        //地面攻击
        if (isOnGround && isAllowAttack)
        {
            isAttacking = true;
            if (heroData.CurrentAttackNum == heroData.MaxAttackNum)
            {
                heroData.CurrentAttackNum = 0;
            }
            heroData.CurrentAttackNum += 1;
        }
        //空中攻击
        else if (!isOnGround && !isAirAttacking)
        {
            isAirAttacking = true;
        }
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    protected virtual void Jump()
    {

        if (isJumping && heroData.CurrentJumpNum == heroData.MaxJumpNum)
        {
            return;
        }
        MusicManager.Instance.PlaySound("Music/Default_Jump_Air");
        //第一段跳跃重置跳跃次数
        if (isOnGround && isJumping == false)
        {
            heroData.CurrentJumpNum = 0;
        }

        isJumping = true;
        heroData.CurrentJumpNum++;

        if (heroData.CurrentJumpNum != 1)
        {
            //生成烟雾
            GameObject smoke = PoolManager.Instance.Get<GameObject>("DoubleJump_Smoke");
            if (smoke == null)
            {
                smoke = ResourcesManager.Instance.LoadAssetSync<GameObject>("Prefabs/VFX/DoubleJump_Smoke");
                smoke = Object.Instantiate(smoke);
                smoke.name = "DoubleJump_Smoke";
            }
            smoke.transform.SetParent(null);
            smoke.SetActive(true);
            smoke.transform.position = transform.position + new Vector3(0.5f, 0, 0);
        }
        if (isDashing)
        {
            rb.gravityScale = GravityScale;
            rb.velocity = new Vector2(0, heroData.JumpForce);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, heroData.JumpForce);
        }
    }

    /// <summary>
    /// 玩家移动逻辑
    /// </summary>
    protected virtual void Move()
    {
        //冲刺最高优先级，此时不能移动
        if (isDashing)
            return;

        float XVelocity = EventManager.GetData<float>(EventName.PlayerInput_Move);

        //攻击动作达到指定帧时无法移动
        if (isAttacking)
        {
            if (isAllowAttack)
                rb.velocity = new Vector2(XVelocity * heroData.MoveSpeed * 0.3f, rb.velocity.y);
            return;
        }


        rb.velocity = new Vector2(XVelocity * heroData.MoveSpeed, rb.velocity.y);
        //翻转方向
        if (XVelocity < 0)
            transform.rotation = new Quaternion(0, 180, 0, 0);
        if (XVelocity > 0)
            transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    #endregion

    #region Skill
    /// <summary>
    /// 玩家冲刺
    /// </summary>
    protected virtual void Dash()
    {
       UseSkill(heroData.Skill_Dash, RealyUseDashSkill(), true);
    }

    protected virtual IEnumerator RealyUseDashSkill()
    {
        MusicManager.Instance.PlaySound("Music/Dash");
        //获取方向
        int direction = transform.rotation.y == 0 ? 1 : -1;
        //设置正在冲刺中
        isDashing = true;
        rb.velocity = new Vector2(heroData.Skill_Dash.Speed * direction, 0);
        float time = heroData.Skill_Dash.DashSkillDuration;
        //生成并播放烟雾和人物残影特效
        GameObject vfx = PoolManager.Instance.Get<GameObject>("Dash_Smoke");
        if (vfx == null)
        {
            vfx = ResourcesManager.Instance.LoadAssetSync<GameObject>("Prefabs/VFX/Dash_Smoke");
            vfx = Object.Instantiate(vfx);
            vfx.name = "Dash_Smoke";
        }
        vfx.transform.SetParent(null);
        vfx.SetActive(true);
        vfx.transform.position = transform.position + new Vector3(0, 0.5f);

        //冲刺时不受重力影响
        rb.gravityScale = 0;
        int startFrame = 0;
        //开始冲刺计时
        while (time > 0)
        {
            time -= Time.deltaTime;
            if(startFrame == 0)
            {
                GameUtility.LoadGameObject("Prefabs/VFX/Shadow");
                startFrame = 5;
            }
           
            yield return null;
            startFrame -= 1;
        }
        //冲刺时间结束
        rb.velocity = new Vector2(0, rb.velocity.y);
        //空中悬停一段时间，即关闭重力影响
        yield return new WaitForSeconds(0.1f);
        rb.gravityScale = GravityScale;
        //设置不在冲刺中
        isDashing = false;
    }

    protected virtual void Skill1() { }
    protected virtual void Skill2() { }

    protected Dictionary<SkillData, Coroutine> skillCoroutineDict = new Dictionary<SkillData, Coroutine>();
    protected virtual void UseSkill(SkillData skillData, IEnumerator skillFunc, bool autoStopLast = true)
    {
        if (skillData == null)
            return;

        if (skillData.RemainCD > 0)
        {
            //如果技能在CD就判断是否可以多段连击
            if (skillData.CurrentComboNum >= skillData.MaxComboNum || Time.time > skillData.LastUseSkillTime + skillData.GraceTime)
            {
                return;
            }
            if (autoStopLast)
            {
                if (skillCoroutineDict.ContainsKey(skillData) && skillCoroutineDict[skillData] != null)
                {
                    StopCoroutine(skillCoroutineDict[skillData]);
                }
            }
            //技能段数计算+1
            skillData.CurrentComboNum += 1;
            //记录技能释放时间
            skillData.LastUseSkillTime = Time.time;
            Coroutine tmp = StartCoroutine(skillFunc);
            if (skillCoroutineDict.ContainsKey(skillData))
                skillCoroutineDict[skillData] = tmp;
            else
                skillCoroutineDict.Add(skillData, tmp);
        }
        else
        {
            //初始化Combo段数
            skillData.CurrentComboNum = 1;
            //记录技能释放时间
            skillData.LastUseSkillTime = Time.time;
            //第一段时技能进入CD
            skillData.RemainCD = skillData.CoolDown * (1 - GameManager.Instance.GetEnhancementLevel().z * 0.1f);
            Coroutine tmp = StartCoroutine(skillFunc);
            if (skillCoroutineDict.ContainsKey(skillData))
                skillCoroutineDict[skillData] = tmp;
            else
                skillCoroutineDict.Add(skillData, tmp);
        }
    }

    /// <summary>
    /// 技能CD更新
    /// </summary>
    protected void SkillColdDown()
    {
        if (heroData.Skill_Dash != null)
        {
            heroData.Skill_Dash.RemainCD = Mathf.Max(0, heroData.Skill_Dash.RemainCD - Time.deltaTime);
        }

        if (heroData.Skill_1 != null)
        {
            heroData.Skill_1.RemainCD = Mathf.Max(0, heroData.Skill_1.RemainCD - Time.deltaTime);
        }

        if (heroData.Skill_2 != null)
        {
            heroData.Skill_2.RemainCD = Mathf.Max(0, heroData.Skill_2.RemainCD - Time.deltaTime);
        }
    }
    #endregion

    #region Physic
    public void CharacterStateCheck()
    {
        isOnGround = GameUtility.RayCheck(leftRayPoint.position, 0.2f, Vector2.down, LayerMask.GetMask("Ground"))
            || GameUtility.RayCheck(rightRayPoint.position, 0.2f, Vector2.down, LayerMask.GetMask("Ground"));
        if (isOnGround && rb.velocity.y <= 0.1f)
            isJumping = false;
        else
            isJumping = true;
    }

    #endregion

    #region Animator
    protected virtual void RefreshAnimation()
    {
        anima.SetBool("IsOnGround", isOnGround);
        anima.SetFloat("XVelocity", Mathf.Abs(InputManager.Instance.XVelocity));
        anima.SetBool("IsJumping", isJumping);
        anima.SetFloat("YVelocity", rb.velocity.y);
        anima.SetBool("IsDashing", isDashing);
        anima.SetBool("IsAttacking", isAttacking);
        anima.SetInteger("AttackNum", heroData.CurrentAttackNum);
        anima.SetBool("IsAirAttacking", isAirAttacking);
        anima.SetBool("IsUseSkill1", isUseSkill1);
        anima.SetBool("IsUseSkill2", isUseSkill2);
            
    }

    public virtual void OnAnimationAttackStart()
    {
        isAttacking = true;
        isAllowAttack = false;
        isAllowWalk = true;
    }

    public virtual void OnAnimationAttacking()
    {
        isAllowAttack = true;
        isAllowWalk = false;
    }
    
    protected void OnAnimationAttackEnd()
    {
        isAttacking = false;
        isAllowWalk = true;
        isAllowAttack = true;
        heroData.CurrentAttackNum = 0;
    }

    public void SetAllowWalkTrue()
    {
        isAllowWalk = true;
    }
    public void SetAllowWalkFalse()
    {
        isAllowWalk = false;
    }
    public void SetIsAttackingTrue()
    {
        isAttacking = true;
    }
    public void SetIsAttackingFalse()
    {
        isAttacking = true;
    }
    public void SetAllowAttackTrue()
    {
        isAllowAttack = true;
    }

    public void SetAllowAttackFalse()
    {
        isAllowAttack = false;
    }

    public void SetAirAttackingTrue()
    {
        isAirAttacking = true;
    }
    
    public void SetAirAttackingFalse()
    {
        isAirAttacking = false;
    }



    #endregion

    #region 外部接口

    public void RefreshHp()
    {
        heroData.CurrentHp = GetMaxHp();
        EventManager.EmitEvent(EventName.PlayerHpChanged);
    }

    public virtual float GetRandomAttackDamage()
    {
        float mul = 1 + 0.1f * GameManager.Instance.GetEnhancementLevel().x + 
            PlayerController.Instance.GetEquipmentEffect(EquipmentType.Attack);
        return Random.Range(heroData.MinAttack * mul, heroData.MaxAttack * mul);
    }

    public virtual bool GetRandomCrit()
    {
        return Random.Range(0, 100) <= heroData.Crit * 100;
    }

    public HeroData GetHeroData()
    {
        return Instantiate(heroData);
    }

    public Sprite GetHeroIcon()
    {
        return heroData.BattleView_Icon;
    }

    public Sprite GetSkill1Icon()
    {
        return heroData.Skill_1 == null ? null : heroData.Skill_1.SkillIcon;
    }

    public Sprite GetSkill2Icon()
    {
        return heroData.Skill_2 == null ? null : heroData.Skill_2.SkillIcon;
    }

    public float GetBasicMaxHp()
    {
        return heroData.MaxHp;
    }

    public float GetMaxHp()
    {
        return heroData.MaxHp * (1 + 0.1f * GameManager.Instance.GetEnhancementLevel().y + 
            PlayerController.Instance.GetEquipmentEffect(EquipmentType.Hp));
    }

    public void AddCurrentHp(float Hp)
    {
        heroData.CurrentHp += Hp;
        if(heroData.CurrentHp > GetMaxHp())
        {
            heroData.CurrentHp = GetMaxHp();
        }
        EventManager.SetData(EventName.PlayerHpChanged, this);
        EventManager.EmitEvent(EventName.PlayerHpChanged);
    }

    public float GetCurrentHp()
    {
        return heroData.CurrentHp;
    }

    public float GetSkill1RemainCDRatio()
    {
        if (heroData.Skill_1 == null)
            return 0;

        return heroData.Skill_1.RemainCD / (heroData.Skill_1.CoolDown * 
            (1 - 0.1f * GameManager.Instance.GetEnhancementLevel().z));
    }

    public float GetSkill2RemainCDRatio()
    {
        if (heroData.Skill_2 == null)
            return 0;
        return heroData.Skill_2.RemainCD / (heroData.Skill_2.CoolDown * 
            (1 - 0.1f * GameManager.Instance.GetEnhancementLevel().z));
    }

    public void TakeDamage(float Damage)
    {
        Damage *= (1 - PlayerController.Instance.GetEquipmentEffect(EquipmentType.Defend));

        heroData.CurrentHp = Mathf.Max(heroData.CurrentHp - Damage, 0);
        EventManager.SetData(EventName.PlayerHpChanged, this);
        EventManager.EmitEvent(EventName.PlayerHpChanged);
        if (heroData.CurrentHp <= 0)
        {
            EventManager.EmitEvent(EventName.PlayerDie);
        }
    }
    #endregion

    private void OnPlayerEnhancement()
    {
        Vector3Int level = EventManager.GetData<Vector3Int>(EventName.PlayerEnhancement);
        heroData.CurrentHp = GetMaxHp();
        EventManager.SetData(EventName.PlayerHpChanged, this);
        EventManager.EmitEvent(EventName.PlayerHpChanged);
    }

    public void SetPlayerHp(float hp)
    {
        heroData.CurrentHp = hp;
    }

    protected virtual void ResetPlayer()
    {
        isOnGround = true;
        isDashing = false;
        isAttacking = false;
        isJumping = false;
        isAirAttacking = false;
        isUseSkill1 = false;
        isUseSkill2 = false;
        isAllowWalk = true;
        isAllowAttack = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Platform"))
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
