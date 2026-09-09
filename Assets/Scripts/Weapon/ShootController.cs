using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//开火控制
public class ShootController
{
    //开枪逻辑
    public void Shooting(Transform gunTransform, Camera cam, Vector3 targetPoint)
    {
        Vector3 shootDirection = (targetPoint - gunTransform.position).normalized;

        Bullet bullet = BulletPool.instance.Get();
        bullet.transform.position = gunTransform.position;
        bullet.transform.rotation = Quaternion.LookRotation(shootDirection);

        bullet.Init(shootDirection);

        AudioManager.instance.PlayEvent("Play_Handgun_Fire", gunTransform.gameObject);
    }

    public void DoDamage(Entity entity, int layerMask)
    {
        int damage = LuaManager.instance.getDamage(layerMask);

        entity.TakeDamage(damage);

        Debug.Log($"命中 {entity.gameObject.name} 造成 {damage} 伤害");
    }

}
