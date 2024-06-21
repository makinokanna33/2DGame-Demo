using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameSetting : ScriptableObject
{
#if UNITY_EDITOR
    [FolderPath()]
    public string SkillDataPath;
#endif
}
