using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skul : HeroController
{

    protected override void NormalAttack()
    {
        if (!isOnGround && !isAirAttacking)
        {
            MusicManager.Instance.PlaySound("Music/Skul_Atk 1");
        }
        base.NormalAttack();
        if ((isOnGround && isAllowAttack && (heroData.CurrentAttackNum == 1 || heroData.CurrentAttackNum == heroData.MaxAttackNum)))
        {
            MusicManager.Instance.PlaySound("Music/Skul_Atk 1");
        }
    }

    protected override void Skill1()
    {
        UseSkill(heroData.Skill_1, ReallyUseSkiil1(), true);
    }

    protected SkulHead skulHead;
    private IEnumerator ReallyUseSkiil1()
    {
        if (heroData.Skill_1.CurrentComboNum == 1)
        {
            anima.SetLayerWeight(anima.GetLayerIndex("NoHead"), 1);

            skulHead = GameUtility.LoadGameObject("Prefabs/Skill/SkulHead").GetComponent<SkulHead>();
            skulHead.transform.position = PlayerController.Instance.currentCharacter.transform.position + new Vector3(0.5f, 1f);
            skulHead.UseSkill(heroData.Skill_1);

            while (heroData.Skill_1.RemainCD > 0)
            {
                yield return null;
            }
            PoolManager.Instance.Push(skulHead.gameObject.name, skulHead.gameObject);
            anima.SetLayerWeight(anima.GetLayerIndex("NoHead"), 0);
        }
        else if (heroData.Skill_1.CurrentComboNum == 2)
        {
            if (skulHead == null)
                yield break;
            PoolManager.Instance.Push(skulHead.gameObject.name, skulHead.gameObject);
            PlayerController.Instance.currentCharacter.transform.position = skulHead.transform.position;
            anima.SetLayerWeight(anima.GetLayerIndex("NoHead"), 0);
            skulHead = null;
        }
       
    }
}
