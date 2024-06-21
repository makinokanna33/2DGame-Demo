using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : EnemyController
{
    public Transform ShootPoint;
    public void AttackEffect()
    {
        GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/Archer_Attack");
        obj.transform.SetParent(transform);
        obj.transform.position = ShootPoint.position;
        obj.transform.rotation = transform.rotation;
    }

    public void ShootArrow()
    {
        GameObject obj = GameUtility.LoadGameObject("Prefabs/Skill/Arrow");
        obj.transform.position = ShootPoint.position;
        obj.transform.rotation = ShootPoint.rotation;
        obj.GetComponent<Arrow>().InitArrow(transform);
    }
}
