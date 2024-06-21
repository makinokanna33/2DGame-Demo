using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMan : EnemyController
{
    public Transform AttackBuild;
    public void AttackEffect()
    {
        GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/TreeMan_AttackEffect");
        obj.transform.SetParent(transform);
        obj.transform.position = AttackBuild.position;
        obj.transform.rotation = transform.rotation;
    }
}
