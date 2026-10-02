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
        maxMagazineSize = LuaManager.instance.handgunTab.Get<int>("maxMagazineSize");
        duration = LuaManager.instance.handgunTab.Get<float>("duration");
        maxAngleDeg = LuaManager.instance.handgunTab.Get<float>("maxAngleDeg");
        aimOffset = LuaManager.instance.handgunTab.Get<Vector3>("aimOffset");
    }


    //开火
    public override bool Fire(Entity entity, float moveSpeed, int fireCount)
    {
        if (currentMagazineSize > 0 && fireTime > LuaManager.instance.handgunTab.Get<float>("fireInterval"))
        {
            shootController.Fire(entity, moveSpeed, fireCount, muzzleTransform, cam, maxAngleDeg);

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

    //获取枪支后坐力
    public override float GetGunRecoil(float elapsed)
    {
        return LuaManager.instance.getHandgunRecoil(elapsed);
    }

}
