using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : BaseWindow
{
    public Button SettingButon;
    public Button MainMenuButton;
    public Button HomeButton;
    public Button ExitGameButton;
    public Button EscapeButton;

    private new void Awake()
    {
        base.Awake();
        SettingButon.onClick.AddListener(() =>
        {
            UIManager.OpenWindow(ScreenName.SettingWindow);
        });
        MainMenuButton.onClick.AddListener(() =>
        {
            UIManager.CloseWindow(ScreenName.PauseWindow);
            LevelManager.Instance.LoadLevel("StartScene");
        });
        HomeButton.onClick.AddListener(() =>
        {
            UIManager.CloseWindow(ScreenName.PauseWindow);
            LevelManager.Instance.LoadLevel("HomeScene");
        });

        ExitGameButton.onClick.AddListener(() =>
        {
            UIManager.CloseWindow(ScreenName.PauseWindow);
        });

        EscapeButton.onClick.AddListener(() =>
        {
            UIManager.CloseWindow(ScreenName.PauseWindow);
        });
    }
    protected override void OnShowIng()
    {
        base.OnShowIng();
        Time.timeScale = 0;
    }

    protected override void OnClosed()
    {
        base.OnClosed();
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && UIManager.CurrentWindow == ScreenName.PauseWindow)
        {
            UIManager.CloseWindow(ScreenName.PauseWindow);
        }
    }
}
