using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samural_Skill1Collider : MonoBehaviour
{
    private List<EnemyController> enemies = new List<EnemyController>();

    private void OnEnable()
    {
        enemies.Clear();
    }
    private void OnDisable()
    {
        ControlAndAttackEnemy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemey = collision.GetComponent<EnemyController>();
            if (!enemies.Contains(enemey))
            {
                enemies.Add(enemey);
            }
        }
    }

    public bool HasEnemy()
    {
        return enemies.Count != 0;
    }
    private void ControlAndAttackEnemy()
    {
        PlayerController.Instance.StartCoroutine(ReallyUseSkill1());
    }

    private IEnumerator ReallyUseSkill1()
    {
        foreach (var enemy in enemies)
        {
            SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
            sr.color = Color.black;
            enemy.ControlEnemy(1f);
            GameObject vfx = GameUtility.LoadGameObject("Prefabs/VFX/Hit_IlSeom");
            vfx.transform.position = sr.bounds.center;
        }
  
        yield return new WaitForSeconds(1f);
        float Damage;
        bool isCrit;
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.white;
            Damage = PlayerController.Instance.currentCharacter.GetRandomAttackDamage();
            isCrit = PlayerController.Instance.currentCharacter.GetRandomCrit();
            Damage *= isCrit? 1.5f : 1;
            Damage *= 10;
            enemy.TakeDamage(Damage, isCrit, false);
        }
    }
}
