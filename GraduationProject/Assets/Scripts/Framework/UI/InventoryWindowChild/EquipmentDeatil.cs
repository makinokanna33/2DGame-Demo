using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDeatil : MonoBehaviour
{
    public Image EquipIcon;
    public TextMeshProUGUI Title_1;
    public TextMeshProUGUI Title_2;
    public TextMeshProUGUI Info;


    public void Init(EquipmentData data)
    {
        bool hasData = data != null;
        EquipIcon.gameObject.SetActive(hasData);
        Title_1.gameObject.SetActive(hasData);
        Title_2.gameObject.SetActive(hasData);
        Info.gameObject.SetActive(hasData);

        if(hasData)
        {
            EquipIcon.sprite = data.detailIcon;
            switch(data.type)
            {
                case EquipmentType.Attack:
                    Title_1.text = "攻击类";
                    break;
                case EquipmentType.Defend:
                    Title_1.text = "防御类";
                    break;
                case EquipmentType.Hp:
                    Title_1.text = "生命类";
                    break;
            }

            Title_2.text = data.name;
            Info.text = data.info;
        }
    }
}
