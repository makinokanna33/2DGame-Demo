using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EquipmentType
{
    Attack,
    Defend,
    Hp,
}

[CreateAssetMenu(fileName = "EquipmentData", menuName = "MyCreate/EquipmentData")]
public class EquipmentData:ScriptableObject
{
    [LabelText("锟斤拷锟斤拷")]
    public string name;
    [LabelText("锟斤拷锟斤拷")]
    public string info;
    [LabelText("装锟斤拷锟斤拷锟斤拷")]
    public EquipmentType type;
    [LabelText("锟斤拷锟斤拷锟斤拷图锟斤拷")]
    public Sprite itemIcon;
    [LabelText("锟斤拷锟斤拷图锟斤拷")]
    public Sprite detailIcon;
    [LabelText("锟斤拷值锟斤拷锟斤拷")]
    public float point;
    [LabelText("锟桔硷拷")]
    public int SellMoney;
}
