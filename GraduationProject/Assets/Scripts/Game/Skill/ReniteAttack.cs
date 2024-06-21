using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReniteAttack : MonoBehaviour
{
    public Vector2 direction;
    public float speed;

    private Transform enemy;

    private void Start()
    {
        Invoke("PushIntoPool", 5f);
    }

    public void Init(Transform enemy, Vector2 direction)
    {
        this.enemy = enemy;
        this.direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(direction.x, direction.y) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.EmitEvent(EventName.EnmeyAttackPlayer, enemy);
            PoolManager.Instance.Push(gameObject.name, gameObject);
        }
    }
    public void PushIntoPool()
    {
        PoolManager.Instance.Push(gameObject.name, gameObject);
    }
}
