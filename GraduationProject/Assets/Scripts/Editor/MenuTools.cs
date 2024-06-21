using Sirenix.OdinInspector.Demos;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MenuTools : OdinMenuEditorWindow
{
    private const string GameSettingPath = "Assets/ScriptObjects/GameSetting.asset";

    [MenuItem("Tools/DevelopTools/MenuTools",priority = 10)]
    private static void OpenWindow()
    {
        var window = GetWindow<MenuTools>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }

    [SerializeField]
    private SomeData someData = new SomeData(); // Take a look at SomeData.cs to see how serialization works in Editor Windows.

    protected override OdinMenuTree BuildMenuTree()
    {
        GameSetting gameSetting = AssetDatabase.LoadAssetAtPath<GameSetting>(GameSettingPath);
        if(gameSetting is null)
        {
            AssetDatabase.CreateAsset(new GameSetting(), GameSettingPath);
            gameSetting = AssetDatabase.LoadAssetAtPath<GameSetting>(GameSettingPath);
        }
        OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                {"SkillData", null},
                {"GameSetting",  gameSetting},
                //{ "Home",                           this,                           EditorIcons.House                       }, // Draws the this.someData field in this case.
                //{ "Odin Settings",                  null,                           EditorIcons.SettingsCog                 },
                //{ "Odin Settings/Color Palettes",   ColorPaletteManager.Instance,   EditorIcons.EyeDropper                  },
                //{ "Odin Settings/AOT Generation",   AOTGenerationConfig.Instance,   EditorIcons.SmartPhone                  },
                //{ "Player Settings",                Resources.FindObjectsOfTypeAll<PlayerSettings>().FirstOrDefault()       },
                //{ "Some Class",                     this.someData                                                           }
            };

        //tree.AddAllAssetsAtPath("Odin Settings/More Odin Settings", "Plugins/Sirenix", typeof(ScriptableObject), true)
        //    .AddThumbnailIcons();

        //tree.AddAssetAtPath("Odin Getting Started", "Plugins/Sirenix/Getting Started With Odin.asset");

        tree.MenuItems.Insert(1, new OdinMenuItem(tree, "Menu Style", tree.DefaultMenuStyle));

        //tree.Add("Menu/Items/Are/Created/As/Needed", new GUIContent());
        //tree.Add("Menu/Items/Are/Created", new GUIContent("And can be overridden"));

        tree.SortMenuItemsByName();
        return tree;
    }
}
