using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
{
    public bool NeedLongPress = true;
    public bool isUI = false;
    public Image FillImage;
    public KeyCode BindKey;

    public UnityEvent OnPresedSomeTime  = new UnityEvent();

    private float CoolDown = 1f;
    private float LastCDTime = -10f;

    private void OnEnable()
    {
        if (NeedLongPress)
            FillImage.fillAmount = 0;
        else
            FillImage.fillAmount = 1;
    }
    private void Update()
    {
        if(UIManager.CurrentWindow != ScreenName.None && !isUI)
        {
            return;
        }
        
        if(NeedLongPress)
        {
            if (Input.GetKey(BindKey))
            {
                if (Time.time - LastCDTime > CoolDown)
                {
                    FillImage.fillAmount += Time.deltaTime;
                    if (FillImage.fillAmount == 1)
                    {
                        OnPresedSomeTime?.Invoke();
                        FillImage.fillAmount = 0;
                        LastCDTime = Time.time;
                    }
                }
            }
            else if (Input.GetKeyUp(BindKey))
            {
                FillImage.fillAmount = 0;
            }
        }
        else
        {
            if(Input.GetKeyDown(BindKey))
            {
                OnPresedSomeTime?.Invoke();
            }
        }
    }
}
