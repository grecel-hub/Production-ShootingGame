using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun

{
    protected override void Start()
    {
        base.Start();

        gunType = GunType.Long;

        maxMagazineSize = LuaManager.instance.rifleTab.Get<int>("maxMagazineSize");
        duration = LuaManager.instance.rifleTab.Get<float>("duration");
    }

    protected override void Update()
    {
        base.Update();

        
    }

    public override bool Fire(Player player)
    {
        if (currentMagazineSize > 0 && fireTime > LuaManager.instance.rifleTab.Get<float>("fireInterval"))
        {
            shootController.Shooting(muzzleTransform, cam, player.aimTarget.position);
            currentMagazineSize--;

            MuzzleFlashe muzzleFlashe = MuzzleFlashePool.instance.Get();
            muzzleFlashe.transform.position = muzzleTransform.position;
            muzzleFlashe.transform.rotation = muzzleTransform.rotation * Quaternion.Euler(0, 180, 0);

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
