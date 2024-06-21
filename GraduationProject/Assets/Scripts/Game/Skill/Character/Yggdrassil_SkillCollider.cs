using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yggdrassil_SkillCollider : AttackCollider
{
    private Vector2 Direction;
    private CircleCollider2D collider;
    bool isOnGround = false;
    private Animator animator;
    public float Speed = 10f;
    private void Awake()
    {
        collider = transform.GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void Init()
    {
        Direction = PlayerController.Instance.currentCharacter.transform.position - transform.position;
        Direction = Direction.normalized;
        isOnGround = false;
        animator.speed = 0;
        collider.enabled = true;
    }

    private void Update()
    {
        
        if(!isOnGround && GameUtility.RayCheck(collider.bounds.min + new Vector3(collider.bounds.extents.x, 0), 0.5f, Vector3.down, LayerMask.GetMask("Ground")))
        {
            isOnGround = true;
            animator.speed = 1;
        }

        if (!isOnGround)
        {
            transform.Translate(Direction * Speed * Time.deltaTime);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.speed = 1;
            isOnGround = true;
            collider.enabled = false;
            if (Attacker == null)
                Attacker = transform.parent;
            EventManager.EmitEvent(EventName.EnmeyAttackPlayer, Attacker);
        }
    }

    public void OnEnd()
    {
        PoolManager.Instance.Push(gameObject.name,gameObject);
    }
}
