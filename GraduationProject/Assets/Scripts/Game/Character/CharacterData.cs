using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterData:ScriptableObject
{
    [LabelText("最大生命值")]
    public float MaxHp;
    [HideInInspector]
    public float CurrentHp;

    [LabelText("最大攻击力")]
    public float MaxAttack;
    [LabelText("最小攻击力")]
    public float MinAttack;

    [LabelText("移动速度")]
    public float MoveSpeed;
}
