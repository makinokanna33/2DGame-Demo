using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public static readonly string GameData_SaveKey = "GameData";
    public void SaveGame(IGameData gameData)
    {
        PlayerPrefs.SetString(GameData_SaveKey, JsonUtility.ToJson(gameData));
        PlayerPrefs.Save();
    }

    public IGameData LoadGame<T>() where T:IGameData
    {
        if (IsHaveSave())
        {
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(GameData_SaveKey));
        }
        return null;
    }

    public bool IsHaveSave()
    {
        if (PlayerPrefs.HasKey(GameData_SaveKey))
        {
            return true;
        }
        return false;
    }
}
