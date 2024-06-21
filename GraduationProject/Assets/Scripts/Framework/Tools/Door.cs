using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public GameObject F_Button;
    public GameObject Fire;

    public bool isActive;

    public SpriteRenderer spriteRenderer;

    public Sprite[] sprites;

    private void OnEnable()
    {
        F_Button.SetActive(false);

        if (!isActive)
        {
            spriteRenderer.sprite = sprites[0];
            Fire.SetActive(false);
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
        }
    }

    private void Update()
    {
        isActive = LevelManager.Instance.IsDefeatAllEnemys();

        if (!isActive)
        {
            spriteRenderer.sprite = sprites[0];
            Fire.SetActive(false);
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Fire.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isActive)
            return;

        if(collision.gameObject.tag == "Player")
        {
            F_Button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            F_Button.SetActive(false);
        }
    }

    public void LoadScene(string sceneName)
    {
        LevelManager.Instance.LoadLevel(sceneName);
    }

    public void SetActive(bool bl)
    {
        if (bl)
        {
            isActive = true;
            spriteRenderer.sprite = sprites[1];
            Fire.SetActive(true);
        }
        else
        {
            isActive = false;
            spriteRenderer.sprite = sprites[0];
            Fire.SetActive(false);
        }
    }

}
