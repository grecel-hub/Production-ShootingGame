using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun

{
    protected override void Start()
    {
        base.Start();

        gunType = GunType.Long;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool Fire(Player player)
    {
        return base.Fire(player);
    }

    public override void Reload(bool isReloading)
    {
        base.Reload(isReloading);
    }

    //获取枪支后坐力
    public override float GetGunRecoil(float elapsed)
    {
        return LuaManager.instance.getHandGunRecoil(elapsed);
    }
}
