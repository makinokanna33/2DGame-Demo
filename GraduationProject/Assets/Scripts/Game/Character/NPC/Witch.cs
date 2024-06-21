using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    public SceneButton sceneButton;

    private void Awake()
    {
        sceneButton.OnPresedSomeTime.AddListener(() =>
        {
            UIManager.OpenWindow(ScreenName.CharacterEnhancementWindow);
        });
        sceneButton.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
