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

        DataInit();

    }

    protected override void Update()
    {
        base.Update();

        //Debug.Log("手枪开火间隔时长：" + fireTime);
    }

    //从Lua脚本获取数据
    protected override void DataInit()
    {
        maxMagazineSize = LuaManager.instance.handGunTab.Get<int>("maxMagazineSize");
        duration = LuaManager.instance.handGunTab.Get<float>("duration");
        maxAngleDeg = LuaManager.instance.handGunTab.Get<float>("maxAngleDeg");
    }


    //开火
    public override bool Fire(Player player, float moveSpeed, int fireCount)
    {
        if (currentMagazineSize > 0 && fireTime > LuaManager.instance.handGunTab.Get<float>("fireInterval"))
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
