using GameFrameWork;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : BaseWindow
{
    public InventoryItem Hero1;
    public InventoryItem Hero2;
    public Transform Containes;
    private List<InventoryItem> Equipments;
    private InventoryItem current;

    public RoleDetail Role;
    public EquipmentDeatil Equipment;
    public SceneButton Resolve;

    private new void Awake()
    {
        base.Awake();
        Equipments = new List<InventoryItem>(Containes.GetComponentsInChildren<InventoryItem>());


        Resolve.OnPresedSomeTime.AddListener(() =>
        {
            if(current != null && current.ItemType == InventoryItemType.Equipment)
            {
                PlayerController.Instance.RemoveEquip(current.equipmentData);
                Tips.ShowTips("分解成功");
                current.Init(InventoryItemType.Equipment, null);
                Resolve.gameObject.SetActive(false);
            }
        });
        Action<InventoryItem> action = (item) =>
        {
            if (current != null)
            {
                current.Selected.gameObject.SetActive(false);
            }
            current = item;

            Role.gameObject.SetActive(false);
            Equipment.gameObject.SetActive(false);
            Resolve.gameObject.SetActive(false);
            switch (item.ItemType)
            {
                case InventoryItemType.Hero:
                    Role.gameObject.SetActive(true);
                    Role.Init(item.heroData);
                    break;
                case InventoryItemType.Equipment:
                    Equipment.gameObject.SetActive(true);
                    Resolve.gameObject.SetActive(item.equipmentData != null);
                    Equipment.Init(item.equipmentData);
                    break;
            }
        };
        
        foreach (var item in Equipments)
        {
            item.OnMouseEnter += action;
        }
        Hero1.OnMouseEnter += action;
        Hero2.OnMouseEnter += action;
    }

    protected override void OnShowIng()
    {
        base.OnShowIng();
        current = null;
        Role.Init(null);
        Role.gameObject.SetActive(false);
        Resolve.gameObject.SetActive(false);
        Equipment.Init(null);
        Equipment.gameObject.SetActive(false);

        InitHero(PlayerController.Instance.character1, Hero1);
        InitHero(PlayerController.Instance.character2, Hero2);
        InitEquipment();
        InputManager.Instance.CharacterControlLock = true;
    }

    protected override void OnClosed()
    {
        base.OnClosed();
        InputManager.Instance.CharacterControlLock = false;
    }


    private void Update()
    {
        if(UIManager.CurrentWindow == ScreenName.InventoryWindow && Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.CloseWindow(ScreenName.InventoryWindow);
        }
    }

    private void InitHero(HeroController controller, InventoryItem item)
    {
        if (controller == null)
            item.Init(InventoryItemType.Hero, null);
        else
            item.Init(InventoryItemType.Hero, controller.GetHeroData());
    }

    private void InitEquipment()
    {
        for (int i = 0; i < Equipments.Count; i++) 
        {
            if (i < PlayerController.Instance.equipmentDatas.Count) 
            {
                Equipments[i].Init(InventoryItemType.Equipment, PlayerController.Instance.equipmentDatas[i]);
            }
            else
            {
                Equipments[i].Init(InventoryItemType.Equipment, null);
            }
        }
    }

}
