using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNum : BaseVFX
{
    public Text damageText;
    public float lifeTimer;

    protected override void Awake()
    {
        animator = damageText.transform.GetComponent<Animator>();
    }

    public void ShowUIDamage(float _amount, bool isCrit)
    {
        damageText.text = _amount.ToString();

        if (isCrit)
            damageText.color = Color.red;
        else
            damageText.color = Color.white;
    }
}
