using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTreeMan : EnemyController
{
    public Transform AttackPoint;
    public void RemoteAttack()
    {
        GameObject obj = GameUtility.LoadGameObject("Prefabs/Skill/RemoteAttack");
        obj.GetComponent<ReniteAttack>().Init(transform, new Vector2(0, 1));
        obj.transform.position = AttackPoint.position;
        
        obj = GameUtility.LoadGameObject("Prefabs/Skill/RemoteAttack");
        obj.GetComponent<ReniteAttack>().Init(transform, new Vector2(0, -1));
        obj.transform.position = AttackPoint.position;


        obj = GameUtility.LoadGameObject("Prefabs/Skill/RemoteAttack");
        obj.GetComponent<ReniteAttack>().Init(transform, new Vector2(1, 0.5f));
        obj.transform.position = AttackPoint.position;

        obj = GameUtility.LoadGameObject("Prefabs/Skill/RemoteAttack");
        obj.GetComponent<ReniteAttack>().Init(transform, new Vector2(1, -0.5f));
        obj.transform.position = AttackPoint.position;

        obj = GameUtility.LoadGameObject("Prefabs/Skill/RemoteAttack");
        obj.GetComponent<ReniteAttack>().Init(transform, new Vector2(-1, 0.5f));
        obj.transform.position = AttackPoint.position;

        obj = GameUtility.LoadGameObject("Prefabs/Skill/RemoteAttack");
        obj.GetComponent<ReniteAttack>().Init(transform, new Vector2(-1, -0.5f));
        obj.transform.position = AttackPoint.position;
    }
}
