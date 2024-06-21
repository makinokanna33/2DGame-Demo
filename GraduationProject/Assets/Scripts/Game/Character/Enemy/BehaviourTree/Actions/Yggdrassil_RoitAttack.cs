using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yggdrassil_RoitAttack : BaseEnemyAction
{
    public Transform LeftHand;
    private Transform LeftGroundCheck;
    public Transform RightHand;
    private Transform LeftSmogPoint;
    private Transform RightSmogPoint;
    private Transform RightGroundCheck;
    private Vector2 AttackDirection;
    private Transform target;
    private bool isAttacking;
    private bool RoitOnGround = false;

    public static int AttackNum = 0;
    public override void OnStart()
    {
        base.OnStart();
        //LeftSmogPoint = LeftHand.Find("SmogBuild");
        //RightSmogPoint = RightHand.Find("SmogBuild");
        //LeftGroundCheck = LeftHand.Find("GroundCheck");
        //RightGroundCheck = RightHand.Find("GroundCheck");
        //target = PlayerController.Instance.currentCharacter.transform;
        //if(PlayerPositionCheck() == 1)
        //{
        //    AttackDirection = new Vector2(target.position.x - RightHand.transform.position.x, target.position.y - RightHand.transform.position.y);
        //}
        //else
        //{
        //    AttackDirection = new Vector2(target.position.x - LeftHand.transform.position.x, target.position.y - LeftHand.transform.position.y);
        //}
        //AttackDirection.Normalize();
        //isAttacking = true;
        //RoitOnGround = false;

        //animator.SetTrigger("isAttack"); 

        Debug.Log("???");
        animator.SetTrigger("startRoitAttack");
        animator.SetBool("attackAgain", true);
        transform.GetComponent<Yggdrassil>().RoitAttackNum = 0;
        timer = Time.time;
    }
    float timer = 0;
    RaycastHit2D hit2D;
    public override TaskStatus OnUpdate()
    {
        if(Time.time  - timer > 5 && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            enemyData.Value.RemainAttackCD = enemyData.Value.AttackCD;
            return TaskStatus.Success;
        }
        //AttackPlayer(); 
        return TaskStatus.Running;
    }


    void AttackPlayer()
    {
        if (RoitOnGround)
            return;
        //player在右边,boss右拳锤他
        if (target.position.x - transform.position.x >= 0)
        {
            RightHand.transform.parent.Translate(AttackDirection * Time.deltaTime * 20);
            if (GameUtility.RayCheck(RightGroundCheck.position, 1f, Vector3.down, LayerMask.GetMask("Ground")))
            {
                RoitOnGround = true;
                //StartCoroutine(UpRoit(false));
            }
        }
        //player在左边,boss左拳锤他
        else
        {
            LeftHand.transform.parent.Translate(AttackDirection * Time.deltaTime * 20);
            if (GameUtility.RayCheck(LeftGroundCheck.position, 1f, Vector3.down, LayerMask.GetMask("Ground")))
            {
                RoitOnGround = true;
                //StartCoroutine(UpRoit(true));
            }
        }
    }
    private int PlayerPositionCheck()
    {
        if (target.transform.position.x - transform.position.x >= 0)
        {
            return 1;
        }

        return 2;
    }
    IEnumerator UpRoit(bool isLeft)
    {
        AttackNum++;
        isAttacking = false;

        GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/Smog_RoitAttack");
        if (isLeft)
            obj.transform.position = LeftSmogPoint.position;
        else
            obj.transform.position = RightSmogPoint.position;
        CameraController.Instance.ShakeCamera(0.3f, 0);
        yield return new WaitForSeconds(5f);
        if (isLeft)
            LeftHand.transform.parent.DOLocalMove(new Vector2(0, 0), 2f);
        else
            RightHand.transform.parent.DOLocalMove(new Vector2(0, 0), 2f);
        animator.SetTrigger("upRoit");

        //enemyData.Value.RemainAttackCD = enemyData.Value.AttackCD;
        //yield return new WaitForSeconds(enemyData.Value.AttackCD);
        //isAttacking = false;

        //if(AttackNum == 2)
        //{
        //    enemyData.Value.RemainAttackCD = enemyData.Value.AttackCD;
        //    isAttacking = false;
        //    AttackNum = 0;
        //}
        //else
        //{
        //    animator.SetTrigger("attackAgain");
        //    isAttacking = false;
        //}
    }

}
