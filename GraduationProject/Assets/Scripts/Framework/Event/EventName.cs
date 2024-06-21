public static class EventName
{
    // Example
    public static string ExampleEvent = "ExampleEvent";

    #region UI
    public static string OnScreenShowingHandle = "OnScreenShowingHandle";
    public static string OnScreenShowedHandle = "OnScreenShowedHandle";
    public static string OnScreenClosingHandle = "OnScreenClosingHandle";
    public static string OnScreenClosedHandle = "OnScreenClosedHandle";
    #endregion

    #region 输入
    public static string PlayerInput_Attack = "PlayerInput_Attack";
    public static string PlayerInput_Move = "PlayerInput_Move";
    public static string PlayerInput_Jump = "PlayerInput_Jump";
    public static string PlayerInput_Dash = "PlayerInput_Dash";
    public static string PlayerInput_Skill1 = "PlayerInput_Skill1";
    public static string PlayerInput_Skill2 = "PlayerInput_Skill2";
    public static string PlayerInput_Fall = "PlayerInput_Fall";
    public static string PlayerInput_Switch = "PlayerInput_Switch";
    #endregion

    #region 场景
    public static string OnLevelLoaded = "OnLevelLoaded";
    #endregion

    #region 战斗
    public static string PlayerSwitchCharacter = "PlayerSwitchCharacter";
    public static string PlayerAttackEnmey = "PlayerAttackEnemy";
    public static string EnmeyAttackPlayer = "EnemyAttackPlayer";
    public static string PlayerHpChanged = "PlayerHpChanged";
    public static string PlayerDie = "PlayerDie";
    #endregion

    #region 强化
    public static string PlayerEnhancement = "PlayerEnhancement";
    public static string AddEquip = "AddEquip";
    public static string RemoveEquip = "RemoveEquip";
    #endregion
}