using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackColliderType
{
    Player,
    Enemy
}

[RequireComponent(typeof(Collider2D))]
public class AttackCollider : MonoBehaviour
{
    [LabelText("攻击碰撞器类型"), Tooltip("Player表示玩家拥有的攻击碰撞器，Enemy表示怪物拥有的攻击碰撞器")]
    public AttackColliderType colliderType;

    public Transform Attacker;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(colliderType == AttackColliderType.Player)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                float damage = PlayerController.Instance.currentCharacter.GetRandomAttackDamage();
                bool isCrit = PlayerController.Instance.currentCharacter.GetRandomCrit();
                damage = isCrit ? damage * 1.5f : damage;
                collision.GetComponent<EnemyController>().TakeDamage(damage, isCrit);
                //EventManager.SetData(EventName.PlayerAttackEnmey, collision.transform);
                //EventManager.EmitEvent(EventName.PlayerAttackEnmey, transform.parent);
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player")
            {
                if (Attacker == null)
                    Attacker = transform.parent;
                EventManager.EmitEvent(EventName.EnmeyAttackPlayer, Attacker);
            }
        }
    }
}
