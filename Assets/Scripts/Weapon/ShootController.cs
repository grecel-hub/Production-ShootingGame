using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//开火控制
public class ShootController
{
    //开枪逻辑
    public void Shooting(Transform gunTransform, Camera cam, Vector3 targetPoint, float maxAngleDeg = 0)
    {
        Vector3 shootDirection = GetFireDirection((targetPoint - gunTransform.position).normalized, maxAngleDeg); //得到偏差后的方向

        Bullet bullet = BulletPool.instance.Get();
        bullet.transform.position = gunTransform.position;
        bullet.transform.rotation = Quaternion.LookRotation(shootDirection);

        bullet.Init(shootDirection);

        AudioManager.instance.PlayEvent("Play_Handgun_Fire", gunTransform.gameObject);
    }

    //开火时玩家移动或持续开火大于10发弹道出现偏移，两个条件同时触发偏移*2
    public void Fire(Player player, float moveSpeed, int fireCount, Transform muzzleTransform, Camera cam, float maxAngleDeg)
    {
        if ((moveSpeed >= 0.3 || moveSpeed <= -0.3) && fireCount > 10)
            Shooting(muzzleTransform, cam, player.aimTarget.position, maxAngleDeg * 2);
        else if (moveSpeed >= 0.3 || moveSpeed <= -0.3)
            Shooting(muzzleTransform, cam, player.aimTarget.position, maxAngleDeg);
        else if (fireCount > 10)
            Shooting(muzzleTransform, cam, player.aimTarget.position, maxAngleDeg);
        else
            Shooting(muzzleTransform, cam, player.aimTarget.position);

        MuzzleFlashe muzzleFlashe = MuzzleFlashePool.instance.Get();
        muzzleFlashe.transform.position = muzzleTransform.position;
        muzzleFlashe.transform.rotation = muzzleTransform.rotation * Quaternion.Euler(0, 180, 0);
    }

    //弹道偏差
    public Vector3 GetFireDirection(Vector3 baseDir, float maxAngleDeg)
    {
        float maxRad = Mathf.Deg2Rad * maxAngleDeg;

        Debug.Log(LuaManager.instance);

        Vector3 localDir = LuaManager.instance.getFireDirection(maxRad);

        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, baseDir);
        
        return (rot * localDir).normalized;
    }

    //造成伤害
    public void DoDamage(Entity entity, int layerMask)
    {
        int damage = LuaManager.instance.getDamage(layerMask);

        entity.TakeDamage(damage);

        Debug.Log($"命中 {entity.gameObject.name} 造成 {damage} 伤害");
    }


}
