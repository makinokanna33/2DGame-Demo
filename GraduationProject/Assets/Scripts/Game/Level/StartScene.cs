using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using GameFrameWork;

public class StartScene : MonoBehaviour
{
    public Texture2D cursor;

    private void Awake()
    {
        //Cursor.SetCursor(cursor, new Vector2(16, 16), CursorMode.Auto);
        MusicManager.Instance.PlayBgMusic("Music/MainTitle");
    }

    void Start()
    {
        UIManager.CloseView(ScreenName.BattleView, false);
        UIManager.OpenView(ScreenName.StartView);
    }
}
