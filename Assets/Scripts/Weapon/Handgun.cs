using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//手枪
public class Handgun : Gun
{
    protected override void Start()
    {
        base.Start();

        gunType = GunType.Short;
    }

    protected override void Update()
    {
        base.Update();
    }


    //开火
    public override bool Fire(Player player)
    {
        return base.Fire(player);
    }

    //换弹
    public override void Reload(bool isReloading)
    {
        base.Reload(isReloading);
    }

}
