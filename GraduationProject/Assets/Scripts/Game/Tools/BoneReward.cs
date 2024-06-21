using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneReward : MonoBehaviour
{

    public SceneButton sceneButton;
    private Animator animator;

    public HeroData heroData;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0;
        sceneButton.gameObject.SetActive(false);
        sceneButton.OnPresedSomeTime.AddListener(() =>
        {
            animator.speed = 1;
        });
    }
    public void OnAnimationEnd()
    {
        Head head = GameUtility.LoadGameObject("Prefabs/Tools/Head").GetComponent<Head>();
        head.transform.position = transform.position + new Vector3(0, 2, 0);
        head.InitHead(heroData.name, heroData.Scene_HeadIcon);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        sceneButton.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        sceneButton.gameObject.SetActive(false);
    }
}
