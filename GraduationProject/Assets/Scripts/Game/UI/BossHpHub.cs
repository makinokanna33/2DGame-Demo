using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpHub : EnemyHpHub
{
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = enemy.GetComponent<EnemyController>();
        initHp = enemyController.CurrentHp;
        currentHp = transform.Find("CurrentHp").GetComponent<Image>();
        damage = transform.Find("Damage").GetComponent<Image>();
    }
}
