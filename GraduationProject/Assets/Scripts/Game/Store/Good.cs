using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Good : MonoBehaviour
{
    public SceneButton sceneButton;
    public Transform point;
    [HideInInspector]
    public Equipment equipment;
    public TMPro.TextMeshProUGUI goldText;
    public bool buySuccess;
    private void Awake()
    {
        sceneButton.OnPresedSomeTime.AddListener(() =>
        {
            if(equipment != null)
            {
                if(PlayerController.Instance.equipmentDatas.Count >= 9)
                {
                    Tips.ShowTips("装备已满");
                }
                else
                {
                    if (GameManager.Instance.UseGold(equipment.equipmentData.SellMoney))
                    {
                        equipment.BuyTheEquipment();
                        sceneButton.gameObject.SetActive(false);
                        GetComponent<Collider2D>().enabled = false;
                        goldText.text = "-----";
                        Tips.ShowTips("购买成功");
                        buySuccess = true;
                    }
                    else
                    {
                        Tips.ShowTips("金币不足");
                    }
                }
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneButton.gameObject.SetActive(false);
    }

    public void Init(Equipment equip)
    {
        GetComponent<Collider2D>().enabled = true;
        buySuccess = false;
        equipment = equip;
        equipment.transform.position = point.transform.position;
        goldText.text = equip.equipmentData.SellMoney.ToString();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && equipment != null)
        {
            sceneButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            sceneButton.gameObject.SetActive(false);
        }
    }
}
