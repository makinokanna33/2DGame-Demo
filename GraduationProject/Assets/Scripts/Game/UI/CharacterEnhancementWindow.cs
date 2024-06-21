using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEnhancementWindow : BaseWindow
{
    public Transform AttackGrid;
    public Transform HpGrid;
    public Transform CDGrid;

    public Button AttackButton; 
    public Button HpButton; 
    public Button CDButton;

    public GameObject EnPoint;
    public Sprite[] Icons;

    public TextMeshProUGUI title;
    public TextMeshProUGUI info;
    public TextMeshProUGUI leveltext;

    [HideInInspector]
    public Transform currentGrid;
    [HideInInspector]
    public Button currentButton;
    [HideInInspector]
    public Sprite currentSprite;
    [HideInInspector]
    public int currentLevel;
    private new void Awake()
    {
        base.Awake();
        AttackButton.onClick.AddListener(() =>
        {
            int level = GameManager.Instance.GetEnhancementLevel().x;
            title.text = "锋利的武器";
            info.text = "增加" + level * 10 + "%的攻击力。";
            if(level < 4)
                info.text += "升级需要消耗" + 1000 * Mathf.Pow(2, level) + "个金币。";
            leveltext.text = "Level " + level + "/4";
            currentGrid = AttackGrid;
            currentButton = AttackButton;
            currentSprite = Icons[0];
            currentLevel = level;
        });
        HpButton.onClick.AddListener(() =>
        {
            int level = GameManager.Instance.GetEnhancementLevel().y;
            title.text = "强壮的体型";
            info.text = "增加" + level * 10 + "%的血量。";
            if (level < 4)
                info.text += "升级需要消耗" + 1000 * Mathf.Pow(2, level) + "个金币。";
            leveltext.text = "Level " + level + "/4";
            currentGrid = HpGrid;
            currentButton = HpButton;
            currentSprite = Icons[1];
            currentLevel = level;
        });

        CDButton.onClick.AddListener(() =>
        {
            int level = GameManager.Instance.GetEnhancementLevel().z;
            title.text = "奇幻的魔力";
            info.text = "增加" + level * 10 + "%的技能CD。";
            if (level < 4)
                info.text += "升级需要消耗" + 1000 * Mathf.Pow(2, level) + "个金币。";
            leveltext.text = "Level " + level + "/4";
            currentGrid = CDGrid;
            currentButton = CDButton;
            currentSprite = Icons[2];
            currentLevel = level;
        });
    }

    protected override void OnShowIng()
    {
        base.OnShowIng();
        Vector3Int level = GameManager.Instance.GetEnhancementLevel();
        InitGrid(AttackGrid, Icons[0], level.x);
        InitGrid(HpGrid, Icons[1], level.y);
        InitGrid(CDGrid, Icons[2], level.z);
        AttackButton.onClick.Invoke();

        InputManager.Instance.CharacterControlLock = true;
    }

    protected override void OnClosed()
    {
        base.OnClosing();
        InputManager.Instance.CharacterControlLock = false;
    }

    private void Update()
    {
        if(UIManager.CurrentWindow != ScreenName.CharacterEnhancementWindow)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(currentLevel < 4)
            {
                if (GameManager.Instance.UseGold(1000 * (int)Mathf.Pow(2, currentGrid.childCount)))
                {
                    Vector3Int level = GameManager.Instance.GetEnhancementLevel();
                    // 将等级设置为Vector3Int，x代表攻击、y代表血量、z代表技能CD
                    if (currentGrid == AttackGrid)
                        level.x = currentLevel + 1;
                    else if (currentGrid == HpGrid)
                        level.y = currentLevel + 1;
                    else
                        level.z = currentLevel + 1;
                    GameManager.Instance.SetEnhancementLevel(level);
                    currentLevel += 1;
                    InitGrid(currentGrid, currentSprite, currentLevel);
                    currentButton.onClick.Invoke();
                    Tips.ShowTips("升级成功");
                }
                else
                    Tips.ShowTips("金币不足");
            }
            else
            {
                Tips.ShowTips("已满级");
            }
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            UIManager.CloseWindow(ScreenName.CharacterEnhancementWindow);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && UIManager.CurrentWindow == ScreenName.CharacterEnhancementWindow)
        {
            UIManager.CloseWindow(ScreenName.CharacterEnhancementWindow);
        }
    }

    private void InitGrid(Transform grid, Sprite sprite, int level)
    {
        if (level == 0)
        {
            foreach(Transform tran in grid)
            {
                if(tran != grid)
                {
                    Destroy(tran.gameObject);
                }
            }
            return;
        }
        EnchancementPoint point = null;
        for (int i = grid.childCount + 1; i <= level; i++) 
        {
            point = Instantiate(EnPoint.gameObject, grid).GetComponent<EnchancementPoint>();
            point.Show(i, sprite, false);
        }
    }
}
