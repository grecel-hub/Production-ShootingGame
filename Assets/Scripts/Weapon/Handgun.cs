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

        maxMagazineSize = LuaManager.instance.handGunTab.Get<int>("maxMagazineSize");
        duration = LuaManager.instance.handGunTab.Get<float>("duration");

        Debug.Log("当前武器最大弹容量：" + maxMagazineSize);
    }

    protected override void Update()
    {
        base.Update();

        //Debug.Log("手枪开火间隔时长：" + fireTime);
    }


    //开火
    public override bool Fire(Player player)
    {
        if (currentMagazineSize > 0 && fireTime > LuaManager.instance.handGunTab.Get<float>("fireInterval"))
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

    //换弹
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
