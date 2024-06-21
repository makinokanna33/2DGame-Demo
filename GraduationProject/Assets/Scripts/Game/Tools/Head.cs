using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public SceneButton sceneButton;
    public SpriteRenderer icon;
    private HeroController hero;
    private string heroName;
    private void Awake()
    {
        sceneButton.gameObject.SetActive(false);
        sceneButton.OnPresedSomeTime.AddListener(() =>
        {
            hero = GameUtility.LoadGameObject("Prefabs/Character/Player/" + heroName).GetComponent<HeroController>();
            PlayerController.Instance.GetNewHero(hero);
            hero.transform.SetParent(PlayerController.Instance.transform);
            Destroy(gameObject);
        });
    }

    public void InitHead(string name, Sprite sp)
    {
        heroName = name;
        icon.sprite = sp;
        icon.transform.DOMoveY(transform.position.y - 0.5f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
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
