using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yggdrassil_Skill : MonoBehaviour
{
    public void AttackPlayer()
    {
        Yggdrassil_SkillCollider SkillCollider = GameUtility.LoadGameObject("Prefabs/Skill/Yggdrassil_Skill").GetComponent<Yggdrassil_SkillCollider>();
        SkillCollider.Attacker = transform.parent;
        SkillCollider.transform.position = transform.position;
        SkillCollider.Init();
    }
    public void OnEnd()
    {
        gameObject.SetActive(false);
        transform.parent.GetComponent<Animator>().SetBool("SkillAttackEnd", true);
    }
}
