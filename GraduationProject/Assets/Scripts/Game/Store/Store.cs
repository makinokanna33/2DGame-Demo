using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    private List<Good> goods;
    private List<Equipment> allEquipments;
    private List<Equipment> current;

    public Refresh refresh;
    private int refreshGold = 500;

    private void Awake()
    {
        goods = new List<Good>(GetComponentsInChildren<Good>());
        allEquipments = new List<Equipment>(Resources.LoadAll<Equipment>("Prefabs/Equipment/"));

        refresh.InitText("重新进货(" + refreshGold + ")");
        refresh.sceneButton.OnPresedSomeTime.AddListener(() =>
        {
            if(GameManager.Instance.UseGold(refreshGold))
            {
                foreach(var eq in current)
                {
                    Destroy(eq.gameObject);
                }
                RefreshStore();
                refreshGold *= 2;
                refresh.InitText("重新进货(" + refreshGold + ")");
            }
            else
                Tips.ShowTips("金币不足");
        });

        foreach (var good in goods)
        {
            good.sceneButton.OnPresedSomeTime.AddListener(() => {
                if (good.buySuccess && good.equipment != null)
                {
                    current.Remove(good.equipment);
                }
            });
        }
    }

    private void OnEnable()
    {
        RefreshStore();
    }
    
    public void RefreshStore()
    {
        current = GetRandomEquipment(3);
        for(int i =0;i<3;i++)
        {
            goods[i].Init(current[i]);
        }
    }
    public  List<Equipment> GetRandomEquipment(int num)
    {
        int i = 0, index = 0;
        List<int> tmp = new List<int>();
        List<Equipment> tmp2 = new List<Equipment>();
        while (i < num)
        {
            index = UnityEngine.Random.Range(0, allEquipments.Count);
            if (!tmp.Contains(index))
            {
                tmp.Add(index);
                tmp2.Add(Instantiate(allEquipments[index]));
                i++;
            }
        }
        return tmp2;
    }
}
