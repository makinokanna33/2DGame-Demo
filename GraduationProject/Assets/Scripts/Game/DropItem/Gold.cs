using System;
using UnityEngine;

public class Gold : BaseVFX
{
    private Rigidbody2D rb;
    private CircleCollider2D cc;

    private bool isOnGround;
    private int gold;
    private Vector3 LastPostion;

    private new void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        EventManager.StartListening(EventName.OnLevelLoaded, OnLevelLoaded);
    }

    private void OnLevelLoaded()
    {
        PoolManager.Instance.Push(gameObject.name, gameObject);
    }

    public void InitGold(int gold)
    {
        this.gold = gold;
        animator.SetBool("GetGold", false);
        rb.AddForce(new Vector2(UnityEngine.Random.Range(-5,6), 7f), ForceMode2D.Impulse);
        rb.gravityScale = 6;
        isOnGround = false;
        LastPostion = transform.position;
    }

    private new void Update()
    {
        if (!isOnGround && GameUtility.RayCheck(cc.bounds.min, 0.2f, Vector2.down, LayerMask.GetMask("Ground")))
        {
            isOnGround = true;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            GameManager.Instance.AddGold(gold);
            animator.SetBool("GetGold", true);
        }

    }
    private void LateUpdate()
    {
        if (!isOnGround && GameUtility.RayCheck(LastPostion, 
            Vector3.Distance(transform.position, LastPostion), 
            Vector3.Normalize(transform.position - LastPostion), 
            LayerMask.GetMask("Ground")))
        {
            isOnGround = true;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            GameManager.Instance.AddGold(gold);
            animator.SetBool("GetGold", true);
        }

        LastPostion = transform.position;
    }

    public void OnGetGoldEnd()
    {
        PoolManager.Instance.Push(gameObject.name, gameObject);
    }
}
