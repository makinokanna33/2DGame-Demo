using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float flySpeed;

    private Transform enemy;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * flySpeed);
    }

    public void InitArrow(Transform  enemy)
    {
        this.enemy = enemy;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.EmitEvent(EventName.EnmeyAttackPlayer, enemy);
            PoolManager.Instance.Push(gameObject.name, gameObject);
        }
    }

}
