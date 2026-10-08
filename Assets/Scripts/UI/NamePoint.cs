using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

//物品名字显示
public class NamePoint : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private TextMeshProUGUI textMeshPro;

    private void Update()
    {
        ShowNamePoint();
    }

    //显示可拾取武器
    private void ShowNamePoint()
    {
        if (player.weaponManager.GetPickGun() != null)
        {
            textMeshPro.enabled = true;

            if (player.weaponManager.GetPickGun().gunType == GunType.Short)
                textMeshPro.text = "Handgun";

            if (player.weaponManager.GetPickGun().gunType == GunType.Long)
                textMeshPro.text = "Rifle";

            transform.position = player.weaponManager.GetPickGun().transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            textMeshPro.enabled = false;
            transform.position = new Vector3(0, 0, 0);
        }
    }
}
