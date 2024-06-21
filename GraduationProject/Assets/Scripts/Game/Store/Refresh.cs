using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Refresh : MonoBehaviour
{
    public SceneButton sceneButton;
    public TextMeshProUGUI tips;

    private void Awake()
    {
        sceneButton.gameObject.SetActive(false);
    }
    public void InitText(string text)
    {
        tips.text = text;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
