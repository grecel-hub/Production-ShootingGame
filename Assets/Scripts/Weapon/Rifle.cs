using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun

{
    protected override void Start()
    {
        base.Start();

        gunType = GunType.Long;

        DataInit();
    }

    protected override void Update()
    {
        base.Update();
    }

    //从Lua脚本获取数据
    protected override void DataInit()
    {
        maxMagazineSize = LuaManager.instance.rifleTab.Get<int>("maxMagazineSize");
        duration = LuaManager.instance.rifleTab.Get<float>("duration");
        maxAngleDeg = LuaManager.instance.rifleTab.Get<float>("maxAngleDeg");
    }

    public override bool Fire(Player player, float moveSpeed, int fireCount)
    {
        if (currentMagazineSize > 0 && fireTime > LuaManager.instance.rifleTab.Get<float>("fireInterval"))
        {
            shootController.Fire(player, moveSpeed, fireCount, muzzleTransform, cam, maxAngleDeg);

            currentMagazineSize--;
            fireTime = 0;

            return true;
        }
        else
        {
            Debug.Log("弹夹为空||开火间隔");

            return false;
        }
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
