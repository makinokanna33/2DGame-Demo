using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai : HeroController
{
    public Samural_Skill1Collider Skill1collider;
    protected override void Skill1()
    {
        UseSkill(heroData.Skill_1, RealySkill1());
    }
    protected override void Skill2()
    {
        UseSkill(heroData.Skill_2, RealySkill2());
    }

    protected override void NormalAttack()
    {
        if (!isOnGround && !isAirAttacking)
        {
            MusicManager.Instance.PlaySound("Music/Atk_Sword_Small1");
        }
        base.NormalAttack();
        if ((isOnGround && isAllowAttack))
        {
            switch (heroData.CurrentAttackNum)
            {
                case 1:
                    MusicManager.Instance.PlaySound("Music/Atk_Sword_Small1");
                    break;
                case 2:
                    MusicManager.Instance.PlaySound("Music/Atk_Sword_Small2");
                    break;
                case 3:
                    MusicManager.Instance.PlaySound("Music/Atk_Sword_Small3");
                    break;
            }
            MusicManager.Instance.PlaySound("Music/Skul_Atk 1");
        }
    }
    protected virtual IEnumerator RealySkill1()
    {
        MusicManager.Instance.PlaySound("Music/Samurai_2_FullMoonilseom_Ready");
        isUseSkill1 = true;
        InputManager.Instance.CharacterControlLock = true;
        yield return new WaitForSeconds(0.3f);
        //获取方向
        int direction = transform.rotation.y == 0 ? 1 : -1;
        rb.gravityScale = 0;
        //设置正在冲刺中
        rb.velocity = new Vector2(heroData.Skill_1.Speed * direction, 0);
        float time = heroData.Skill_1.DashSkillDuration;
        //开始冲刺计时
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        //冲刺时间结束
        rb.velocity = new Vector2(0, rb.velocity.y);
        if (Skill1collider.HasEnemy())
        {
            anima.updateMode = AnimatorUpdateMode.UnscaledTime;
            Time.timeScale = 0.2f;
        }
        else
            anima.speed = 1.5f;

        rb.gravityScale = 6;

        yield return new WaitUntil(() => !isUseSkill1);
        if (Skill1collider.HasEnemy())
        {
            anima.updateMode = AnimatorUpdateMode.Normal;
            Time.timeScale = 1f;
        }
        else
            anima.speed = 1f;
        InputManager.Instance.CharacterControlLock = false;
    }
    protected virtual IEnumerator RealySkill2()
    {
        isUseSkill2 = true;
        InputManager.Instance.CharacterControlLock = true;
        yield return new WaitUntil(() => !isUseSkill2);
        InputManager.Instance.CharacterControlLock = false;
    }

    #region Animation
    public void UseSkill2()
    {
        GameObject obj = GameUtility.LoadGameObject("Prefabs/Skill/QuickSlash");
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(0, 1);
        obj.transform.localRotation = Quaternion.Euler(0.0f, 0, Random.Range(-5, 6));
    }
    public void SetUseSkill1End()
    {
        isUseSkill1 = false;
    }

    public void SetUseSkill2End()
    {
        isUseSkill2 = false;
    }
    #endregion
}
