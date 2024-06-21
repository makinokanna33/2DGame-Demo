using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulHead : MonoBehaviour
{
    private Rigidbody2D rb;

    private SkillData skillData;

    [SerializeField, LabelText("锟斤拷锟斤拷锟劫讹拷")]
    private float flySpeed = 10f;
    [SerializeField, LabelText("锟斤拷转锟劫讹拷")]
    private float rotateSpeed = 1f;
    private Transform groundRayPoint;
    private CircleCollider2D circleCollider;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        groundRayPoint = transform.Find("GroundRayPoint");
    }

    //锟斤拷锟斤拷
    public void UseSkill(SkillData skillData)
    {
        this.skillData = skillData;
        Physics2D.IgnoreCollision(PlayerController.Instance.currentCharacter.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>(), true);
        rb.gravityScale = 0;
        transform.gameObject.layer = LayerMask.NameToLayer("AttackCollider");
        rb.AddForce(new Vector2(flySpeed * PlayerController.Instance.GetCurrentDirection(), 0), ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (rb.velocity.x > 0)
            transform.Rotate(new Vector3(0, 0, 1) * rotateSpeed * rb.velocity.x, Space.World);
        else if (rb.velocity.x < 0)
            transform.Rotate(new Vector3(0, 0, -1) * rotateSpeed * Mathf.Abs(rb.velocity.x), Space.World);
        groundRayPoint.position = circleCollider.bounds.center - new Vector3(0, circleCollider.bounds.extents.y);
        if (GameUtility.RayCheck(groundRayPoint.position, 0.2f, Vector2.down, LayerMask.GetMask("Ground")))
        {
            Physics2D.IgnoreCollision(PlayerController.Instance.currentCharacter.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>(), false);
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            skillData.RemainCD = 0;
            skillData.LastUseSkillTime = -10f;
            PoolManager.Instance.Push(gameObject.name, gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.layer = LayerMask.NameToLayer("Drop");
            rb.gravityScale = 1;
            if(rb.velocity.x > 0)
                rb.velocity = new Vector2(-3, 2);
            else
                rb.velocity = new Vector2(3, 2);
            float Damage = PlayerController.Instance.currentCharacter.GetRandomAttackDamage();
            bool isCrit = PlayerController.Instance.currentCharacter.GetRandomCrit();
            Damage *= isCrit ? 1.5f : 1;
            Damage *= 5;
            collision.GetComponent<EnemyController>().TakeDamage(Damage, isCrit);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            skillData.RemainCD = 0;
            skillData.LastUseSkillTime = -10f;
            PoolManager.Instance.Push(gameObject.name, gameObject);
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.layer = LayerMask.NameToLayer("Drop");

            rb.gravityScale = 1;
            rb.velocity = new Vector2(0, rb.velocity.y);
            Rigidbody2D enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();
            enemyRB.velocity = new Vector2(0, enemyRB.velocity.y);
            EventManager.SetData(EventName.PlayerAttackEnmey, collision.transform);
            EventManager.EmitEvent(EventName.PlayerAttackEnmey, transform.parent);
        }
        else if(collision.gameObject.CompareTag("Ground"))
        {
            gameObject.layer = LayerMask.NameToLayer("Drop");

            rb.gravityScale = 1;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}
