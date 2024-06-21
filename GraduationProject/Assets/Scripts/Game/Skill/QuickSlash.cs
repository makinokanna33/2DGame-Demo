using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlash : AttackCollider
{
    private Animator animator;

    private void Start()
    {
        animator = transform.GetComponent<Animator>();
    }
    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            gameObject.SetActive(false);
            PoolManager.Instance.Push(gameObject.name, gameObject);
        }
    }
}
