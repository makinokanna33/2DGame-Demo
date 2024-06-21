using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yggdrassil : EnemyController
{
    public Transform Hand_Left;
    public Transform Hand_Right;
    private Transform Platform_Left;
    private Transform Platform_Right;

    private bool RoitAttacking = false;

    public int RoitAttackNum = 0;
    private Vector3 AttackDirection;
    private Vector2 Target;

    public BossHpHub bossHpHub;
    public Door door;
    private int PlayerPositionCheck()
    {
        if (PlayerController.Instance.currentCharacter.transform.position.x - transform.position.x >= 0)
        {
            return 1;
        }

        return 2;
    }

    protected override void Awake()
    {
        base.Awake();
        Platform_Left = Hand_Left.Find("Platform");
        Platform_Left.gameObject.SetActive(false);
        Platform_Right = Hand_Right.Find("Platform");
        Platform_Right.gameObject.SetActive(false);
    }
    protected override void Update()
    {
        base.Update();
        Debug.Log(enemyData.Value.RemainAttackCD);
        if (RoitAttacking)
        {

            RaycastHit2D hit2D;

            //player在右边,boss右拳锤他
            if (Target.x - transform.position.x >= 0)
            {
                hit2D = GameUtility.RayCheck(Hand_Right.transform.Find("GroundCheck").position, 1, Vector3.down, LayerMask.GetMask("Ground"));

                Hand_Right.transform.parent.Translate(AttackDirection * Time.deltaTime * 20);

            }
            //player在左边,boss左拳锤他
            else
            {
                hit2D = GameUtility.RayCheck(Hand_Left.transform.Find("GroundCheck").position, 1, Vector3.down, LayerMask.GetMask("Ground"));
                Hand_Left.transform.parent.Translate(AttackDirection * Time.deltaTime * 20);
            }
            if (hit2D)
            {
                GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/Smog_RoitAttack");

                if (Target.x - transform.position.x >= 0)
                {
                    Hand_Right.transform.GetComponent<BoxCollider2D>().enabled = false;
                    obj.transform.position = Hand_Right.Find("SmogBuild").position;
                    Platform_Right.gameObject.SetActive(true);
                }

                else
                {
                    Hand_Left.transform.GetComponent<BoxCollider2D>().enabled = false;
                    obj.transform.position = Hand_Left.Find("SmogBuild").position;
                    Platform_Left.gameObject.SetActive(true);
                }
                CameraController.Instance.ShakeCamera(0.3f, 0);

                //MyCamera.Instance.DoCameraShake(1, 10);

                RoitAttacking = false;

                StartCoroutine(UpRoit());
            }

        }
    }

    public void RoitAttack()
    {
        RoitAttackNum++;
        if (RoitAttackNum < 3)
        {
            if (RoitAttackNum == 2)
            {
                animator.SetBool("attackAgain", false);
            }
            Target = PlayerController.Instance.currentCharacter.transform.position;
            if (PlayerPositionCheck() == 1)
            {
                AttackDirection = new Vector2(Target.x - Hand_Right.transform.position.x, Target.y - Hand_Left.transform.position.y);
                Hand_Right.transform.GetComponent<BoxCollider2D>().enabled = true;
            }

            else
            {
                AttackDirection = new Vector2(Target.x - Hand_Left.transform.position.x, Target.y - Hand_Left.transform.position.y);
                Hand_Left.transform.GetComponent<BoxCollider2D>().enabled = true;
            }

            AttackDirection = AttackDirection.normalized;

            RoitAttacking = true;
        }

    }
    IEnumerator UpRoit()
    {
        //等待两秒后开始抖动
        yield return new WaitForSeconds(5f);
        Platform_Left.gameObject.SetActive(false);
        Platform_Right.gameObject.SetActive(false);
        //拳头暂停完毕之后，抬升拳头
        if (Target.x - transform.position.x >= 0)
        {
            Hand_Right.transform.parent.DOLocalMove(new Vector2(0, 0), 2f);
        }
        else
        {
            Hand_Left.transform.parent.DOLocalMove(new Vector2(0, 0), 2f);
        }

        animator.SetTrigger("upRoit");
    }

    public void OnAppearStart()
    {
        InputManager.Instance.CharacterControlLock = true;
        bossHpHub.enemy = gameObject;
        bossHpHub.gameObject.SetActive(false);
        door.gameObject.SetActive(false);
    }

    public void Appear_1()
    {
        CameraController.Instance.ShakeCamera(0.5f);
        GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/Smog_Appear");
        obj.transform.position = Hand_Left.Find("SmogBuild").position;
        Hand_Left.GetComponent<SpriteRenderer>().sortingLayerName = "Enemy";
    }
    public void Appear_2()
    {
        CameraController.Instance.ShakeCamera(0.5f);
        GameObject obj = GameUtility.LoadGameObject("Prefabs/VFX/Smog_Appear");
        obj.transform.position = Hand_Right.Find("SmogBuild").position;
        Hand_Right.GetComponent<SpriteRenderer>().sortingLayerName = "Enemy";
    }
    public void Appear_3()
    {
        CameraController.Instance.ShakeCamera(3.5f, 1);
    }

    public void OnAppearEnd()
    {
        CanvasGroup canvasGroup = bossHpHub.GetComponent<CanvasGroup>();
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1f, 1f).onComplete+=()=> { InputManager.Instance.CharacterControlLock = false; };
    }

    public override void ControlEnemy(float time)
    {

    }

    protected override void DeathReward()
    {
        base.DeathReward();
        Destroy(bossHpHub);
        bossHpHub.gameObject.SetActive(false);
        door.gameObject.SetActive(true);
    }
}
