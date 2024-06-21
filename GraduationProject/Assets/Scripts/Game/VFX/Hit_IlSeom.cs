using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_IlSeom : BaseVFX
{
    public float DelaytTime;
    protected override void OnEnable()
    {
        base.OnEnable();
        animator.speed = 0;
        transform.Rotate(Vector3.forward, Random.Range(0, 360));
        Invoke("PlayeAnimation", DelaytTime);
    }

    private void PlayeAnimation()
    {
        animator.speed = 1;
    }

}
