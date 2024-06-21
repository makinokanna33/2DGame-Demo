using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpHub : MonoBehaviour
{
    protected Image currentHp;
    protected Image damage;

    protected float initHp;
    protected EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = transform.parent.parent.GetComponent<EnemyController>();
        initHp = enemyController.CurrentHp;

        currentHp = transform.Find("CurrentHp").GetComponent<Image>();
        damage = transform.Find("Damage").GetComponent<Image>();
    }

    // Update is called once per frame
    protected void Update()
    {
        transform.rotation = Quaternion.identity;
        if (damage.fillAmount > currentHp.fillAmount)
        {
            damage.fillAmount -= Time.deltaTime;
        }

        RefreshHp(enemyController.CurrentHp);
    }


    public void RefreshHp(float Hp)
    {
        currentHp.fillAmount = Hp / initHp;
    }
}
