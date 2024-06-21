using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMono<GameManager>
{
    private GameData gameData;

    private new void Awake()
    {
        base.Awake();
#if UNITY_EDITOR
        if (gameData == null)
            LoadGameData();
#endif
    }

    private void Update()
    {

        if (UIManager.CurrentView != ScreenName.BattleView && SceneManager.GetActiveScene().name != "StartScene")
        {
            UIManager.ChangeView(ScreenName.BattleView);
        }
    }

    #region 游戏存档相关
    public void LoadGameData()
    {
        if (!SaveManager.Instance.IsHaveSave())
        {
            gameData = new GameData();
            return;
        }
        gameData = SaveManager.Instance.LoadGame<GameData>() as GameData;
    }

    public bool HasGameData()
    {
        return SaveManager.Instance.IsHaveSave();
    }

    public void SaveGameData()
    {
        SaveManager.Instance.SaveGame(gameData);
    }

    public void NewGame()
    {
        gameData = new GameData();
        SaveGameData();
    }


    #endregion

    #region 金币相关
    public void AddGold(int gold)
    {
        gameData.Gold += gold;
        SaveGameData();
    }

    public bool UseGold(int gold)
    {
        if(gameData.Gold >= gold)
        {
            gameData.Gold -= gold;
            SaveGameData();
            return true;
        }
        return false;
    }

    public int GetGold()
    {
        return gameData.Gold;
    }
    #endregion

    #region 强化等级
    public Vector3Int GetEnhancementLevel()
    {
        return gameData.EnhancementLevel;
    }

    public void SetEnhancementLevel(Vector3Int level)
    {
        gameData.EnhancementLevel = level;
        SaveGameData();
        EventManager.SetData(EventName.PlayerEnhancement, level);
        EventManager.EmitEvent(EventName.PlayerEnhancement);
    }
    #endregion

#if UNITY_EDITOR
    [MenuItem("Tools/DevelopTools/AddGold", priority = 1)]
    public static void AddGold_10000()
    {
        GameManager.Instance.AddGold(10000);
    }
#endif
}
