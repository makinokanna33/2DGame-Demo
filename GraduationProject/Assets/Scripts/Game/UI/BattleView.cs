using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BattleView : BaseView
{
    public TextMeshProUGUI HpText;
    public Image FillImageBG;
    public Image FillImage;
    public Image HeroIcon;

    public Image Skill1;
    public Image Skill2;
    public Image skill1_Dark;
    public Image skill2_Dark;

    public TextMeshProUGUI GoldNumText;

    bool needSkill1Vfx;
    bool needSkill2Vfx;
    private void Awake()
    {
        EventManager.StartListening(EventName.PlayerSwitchCharacter, ResetPlayer);
        EventManager.StartListening(EventName.PlayerHpChanged, OnPlayerHpChanged);
        EventManager.StartListening(EventName.AddEquip, RefreshHp);
        EventManager.StartListening(EventName.RemoveEquip, RefreshHp);
        ResetPlayer();
    }

    protected override void OnShowIng()
    {
        base.OnShowIng();
        needSkill1Vfx = false;
        needSkill2Vfx = false;
        Target.GetComponent<CanvasGroup>().alpha = 1;

        RefreshHp();
    }

    private void RefreshHp()
    {
        HeroController hero = PlayerController.Instance.currentCharacter;
        FillImageBG.fillAmount = FillImage.fillAmount = hero.GetCurrentHp() / hero.GetMaxHp();
        HpText.text = hero.GetCurrentHp().ToString("#0") + "/" + hero.GetMaxHp().ToString("#0");
    }

    private void Update()
    {
        if (PlayerController.Instance.currentCharacter.GetSkill1RemainCDRatio() > 0f && !needSkill1Vfx)
        {
            needSkill1Vfx = true;
        }
        else if (PlayerController.Instance.currentCharacter.GetSkill1RemainCDRatio() == 0f && needSkill1Vfx)
        {
            needSkill1Vfx = false;
            GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/ColdDown_UI");
            obj.transform.SetParent(skill1_Dark.transform);
            obj.transform.localPosition = Vector3.zero;
        }

        if (PlayerController.Instance.currentCharacter.GetSkill2RemainCDRatio() > 0f && !needSkill2Vfx)
        {
            needSkill2Vfx = true;
        }
        else if (PlayerController.Instance.currentCharacter.GetSkill2RemainCDRatio() == 0f && needSkill2Vfx)
        {
            needSkill2Vfx = false;
            GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/ColdDown_UI");
            obj.transform.SetParent(skill1_Dark.transform);
            obj.transform.localPosition = Vector3.zero;
        }
        skill1_Dark.fillAmount = PlayerController.Instance.currentCharacter.GetSkill1RemainCDRatio();
        skill2_Dark.fillAmount = PlayerController.Instance.currentCharacter.GetSkill2RemainCDRatio();

        GoldNumText.text = GameManager.Instance.GetGold().ToString();
    }

    private void OnPlayerHpChanged()
    {
        HeroController hero = PlayerController.Instance.currentCharacter;
        FillImage.fillAmount = hero.GetCurrentHp() / hero.GetMaxHp();
        FillImageBG.DOFillAmount(FillImage.fillAmount, 0.2f);
        HpText.text = hero.GetCurrentHp().ToString("#0") + "/" + hero.GetMaxHp().ToString("#0");
    }

    private void ResetPlayer()
    {
        HeroController hero = EventManager.GetData<HeroController>(EventName.PlayerSwitchCharacter);
        if(hero == null)
        {
            hero = PlayerController.Instance.currentCharacter;
        }
        needSkill1Vfx = false;
        needSkill2Vfx = false;
        HeroIcon.sprite = hero.GetHeroIcon();
        Skill1.sprite = hero.GetSkill1Icon();
        Skill2.sprite = hero.GetSkill2Icon();
        Skill1.enabled = Skill1.sprite != null;
        Skill2.enabled = Skill2.sprite != null;
    }
}
