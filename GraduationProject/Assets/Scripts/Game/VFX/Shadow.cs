using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color color;

    [Header("时间控制参数")]
    public float activeTime;    // 显示时间
    private float activeStart;   // 开始显示的时间

    [Header("不透明度控制")]
    public float alphaSet;  // 初始值
    public float alphaMultiplier;
    private float alpha;

    private void OnEnable()
    {
        
        thisSprite = GetComponent<SpriteRenderer>();
        alpha = alphaSet;
        activeStart = Time.time;

        player = PlayerController.Instance.currentCharacter.transform;
        playerSprite = player.GetComponent<SpriteRenderer>();
        thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;

        color = new Color(0, 0, 0, alpha);

        thisSprite.color = color;

        if (Time.time >= activeStart + activeTime)
        {
            // 返回对象池
            PoolManager.Instance.Push(gameObject.name, gameObject);
        }
    }
}
