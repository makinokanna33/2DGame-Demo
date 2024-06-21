using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Equipment : MonoBehaviour
{
    public SceneButton sceneButton;

    public EquipmentData equipmentData;
    private Transform icon;
    private bool isOnGround;
    private Rigidbody2D rb;
    private Collider2D collider2D;
    public Tweener tweener;
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        icon = transform.Find("Icon");
        collider2D = transform.GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        sceneButton.OnPresedSomeTime.AddListener(() =>
        {
            PlayerController.Instance.AddEquip(equipmentData);
            PoolManager.Instance.Push(gameObject.name, gameObject);
        });
            
        tweener = icon.DOMoveY(transform.position.y - 0.5f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private bool ignorePlayerCollision;
    private void OnEnable()
    {
        sceneButton.gameObject.SetActive(false);
        rb.gravityScale = 0;
        ignorePlayerCollision = true;
        icon.GetComponent<SpriteRenderer>().sprite = equipmentData.itemIcon;
        tweener.Play();
    }

    
    public void BuyTheEquipment()
    {
        rb.gravityScale = 1;
        tweener.Pause();
    }

    private void Update()
    {
        Physics2D.IgnoreCollision(collider2D, PlayerController.Instance.currentCharacter.GetComponent<Collider2D>(), 
            ignorePlayerCollision);


        if (GameUtility.RayCheck(collider2D.bounds.min, 0.2f, Vector2.down, LayerMask.GetMask("Ground")) && !isOnGround)
        {
            icon.DOMoveY(transform.position.y - 0.5f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            isOnGround = true;
            icon.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            ignorePlayerCollision = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            sceneButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sceneButton.gameObject.SetActive(false);
        }
    }
}
