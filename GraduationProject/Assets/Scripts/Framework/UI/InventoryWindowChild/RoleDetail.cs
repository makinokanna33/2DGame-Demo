using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleDetail : MonoBehaviour
{
    public Image HeroIcon;
    public TextMeshProUGUI HeroInfo_1;
    public TextMeshProUGUI HeroInfo_2;
    public Image skill1_Icon;
    public Image skill2_Icon;
    public TextMeshProUGUI Skill1Name;
    public TextMeshProUGUI Skill2Name;
    
    public void Init(HeroData heroData)
    {
        bool notNull = heroData != null;
        HeroIcon.gameObject.SetActive(notNull);
        HeroInfo_1.gameObject.SetActive(notNull);
        HeroInfo_2.gameObject.SetActive(notNull);
        skill1_Icon.gameObject.SetActive(notNull);
        skill2_Icon.gameObject.SetActive(notNull);
        Skill1Name.gameObject.SetActive(notNull);
        Skill2Name.gameObject.SetActive(notNull);

        if(notNull)
        {
            HeroIcon.sprite = heroData.Inventory_DetailIcon;
            HeroInfo_1.text = heroData.HeroInfo_1;
            HeroInfo_2.text = heroData.HeroInfo_2;
            if(heroData.Skill_1 != null)
            {
                skill1_Icon.sprite = heroData.Skill_1.SkillIcon;
                Skill1Name.text = heroData.Skill_1.SkillName;
            }
            else
            {
                skill1_Icon.gameObject.SetActive(false);
                Skill1Name.gameObject.SetActive(false);
            }
            if(heroData.Skill_2 != null)
            {
                skill2_Icon.sprite = heroData.Skill_2.SkillIcon;
                Skill2Name.text = heroData.Skill_2.SkillName;
            }
            else
            {
                skill2_Icon.gameObject.SetActive(false);
                Skill2Name.gameObject.SetActive(false);
            }
        }
    }
}
