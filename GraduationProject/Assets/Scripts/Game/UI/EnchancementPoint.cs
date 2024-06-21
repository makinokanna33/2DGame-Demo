using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnchancementPoint : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public Image Icon;
    public TextMeshProUGUI LevelText;

    public void Show(int num, Sprite sprite, bool useTween)
    {
        LevelText.text = num.ToString();
        Icon.sprite = sprite;

        if(useTween)
        {
            CanvasGroup.DOFade(1f, 0.5f);
        }
        else
            CanvasGroup.alpha = 1f;
    }
}
