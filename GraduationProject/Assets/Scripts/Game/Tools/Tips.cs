using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Tips : MonoBehaviour
{
    public TextMeshProUGUI text;
    private CanvasGroup canvasGroup;
    public static List<Tips> tipsSequence = new List<Tips>();
    Sequence TipSequence;
    private void Awake()
    {
        canvasGroup = transform.GetComponent<CanvasGroup>();
    }
    public static void ShowTips(string info)
    {
        Tips tmp = GameUtility.LoadGameObject("Prefabs/Tools/Tips").GetComponent<Tips>();
        tmp.transform.SetParent(UIManager.UICanvas.transform);
        (tmp.transform as RectTransform).SetAsLastSibling();
        tmp.canvasGroup.alpha = 0;
        tmp.text.text = info;
        tmp.transform.position = new Vector3(Screen.width/2, 0);
        tmp.canvasGroup.DOFade(1, 0.5f).onComplete += () =>
        {
            tmp.canvasGroup.DOFade(0, 0.25f).SetDelay(0.25f).onComplete += () =>
            {
                PoolManager.Instance.Push(tmp.name, tmp.gameObject);
            };
        };
        tmp.transform.DOMoveY((tmp.transform as RectTransform).sizeDelta.y * 4, 1f);
    }
}
