using DG.Tweening;
using GameFrameWork;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DarkModal : MonoBehaviour, IPointerClickHandler
{
    private Image darkImage;
    private BaseWindow currentWindow;

    private void Awake()
    {
        darkImage = transform.GetComponent<Image>();
    }

    public void SetDarkModal(BaseWindow window, bool isShow)
    {
        if (window != null && window.IsModel)
        {
            currentWindow = window;
            RectTransform rectTrans = (RectTransform)transform;
            if (rectTrans.transform.parent != window.transform.parent)
            {
                rectTrans.SetParent(window.transform.parent);
            }
            rectTrans.SetAsLastSibling();
            rectTrans.SetSiblingIndex(window.transform.GetSiblingIndex());
            rectTrans.offsetMin = Vector2.zero;
            rectTrans.offsetMax = Vector2.zero;
        }

        if (isShow)
        {
            gameObject.SetActive(true);
            Color color = darkImage.color;
            color.a = 0;
            darkImage.color = color;
            darkImage.DOFade(currentWindow.ModalAlpha, 0.5f);
        }
        else
        {
            darkImage.DOFade(0, 0.5f).onComplete += () =>
            {
                gameObject.SetActive(false);
            };
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentWindow.IsModel)
        {
            currentWindow.OnDarkModalClicked();
        }
    }
}