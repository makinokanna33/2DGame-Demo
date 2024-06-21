using DG.Tweening;
using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartView : BaseView
{
    public TextMeshProUGUI Tips;
    public Image Dark;
    private bool isDisplaying = false;

    public Button NewGame;
    public Button ContinueGame;
    public Button Setting;
    public Button ExitGame;

    // Start is called before the first frame update
    void Awake()
    {
        NewGame.transform.parent.gameObject.SetActive(false);
        
        NewGame.onClick.AddListener(() =>
        {
            GameManager.Instance.NewGame();
            LevelManager.Instance.LoadLevelAsync("HomeScene", () =>
            {
                UIManager.ChangeView(ScreenName.BattleView);
            });
        });

        ContinueGame.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadGameData();
            LevelManager.Instance.LoadLevelAsync("HomeScene", () =>
            {
                UIManager.ChangeView(ScreenName.BattleView);
            });
        });

        Setting.onClick.AddListener(() =>
        {
            UIManager.OpenWindow(ScreenName.SettingWindow);
        });

        ExitGame.onClick.AddListener(() => { Application.Quit(); });

        Tips.DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    protected override void OnShowIng()
    {
        base.OnShowIng();
        ContinueGame.gameObject.SetActive(GameManager.Instance.HasGameData());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !isDisplaying)
        {
            isDisplaying = true;
            Tips.gameObject.SetActive(false);
            NewGame.transform.parent.gameObject.SetActive(true);
            NewGame.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
            NewGame.transform.parent.GetComponent<CanvasGroup>().DOFade(1f, 1f);
        }
    }
}
