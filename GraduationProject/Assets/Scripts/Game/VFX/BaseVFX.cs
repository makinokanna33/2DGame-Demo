using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVFX : MonoBehaviour
{
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        animator.Play(animator.runtimeAnimatorController.animationClips[0].name);
    }

    protected void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            PoolManager.Instance.Push(gameObject.name, gameObject);
            gameObject.SetActive(false);
        }
    }
}
