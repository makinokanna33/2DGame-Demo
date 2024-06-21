using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
    public SceneButton sceneButton;

    private void Awake()
    {
        sceneButton.gameObject.SetActive(false);
        sceneButton.OnPresedSomeTime.AddListener(() =>
        {
            UIManager.OpenWindow(ScreenName.HPStoreWindow);
        });
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        sceneButton.gameObject.SetActive(true);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        sceneButton.gameObject.SetActive(false);
    }
}
