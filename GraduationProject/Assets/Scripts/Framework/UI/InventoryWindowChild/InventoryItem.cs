using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum InventoryItemType
{
    Hero,
    Equipment
}

public class InventoryItem : MonoBehaviour,IPointerEnterHandler
{
    public Image Background;
    public Image Icon;
    public Image Selected;

    public Action<InventoryItem> OnMouseEnter;

    [HideInInspector]
    public InventoryItemType ItemType;
    [HideInInspector]
    public HeroData heroData;
    [HideInInspector]
    public EquipmentData equipmentData;
    private void Awake()
    {
        Background.gameObject.SetActive(true);
        Selected.gameObject.SetActive(false);
    }

    public void Init(InventoryItemType type, ScriptableObject data)
    {
        ItemType = type;
        heroData = data as HeroData;
        equipmentData = data as EquipmentData;
        Selected.gameObject.SetActive(false);
        if (heroData == null && equipmentData == null)
        {
            Background.gameObject.SetActive(true);
            Icon.gameObject.SetActive(false);
        }
        else
        {
            Background.gameObject.SetActive(false);
            if(type == InventoryItemType.Hero)
                Icon.sprite = heroData.Invenotry_SlotIcon;
            else if(type == InventoryItemType.Equipment)
                Icon.sprite = equipmentData.itemIcon;
            Icon.gameObject.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter.Invoke(this);
        Selected.gameObject.SetActive(true);
    }
}
