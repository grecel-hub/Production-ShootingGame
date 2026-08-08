using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager
{
    //开枪逻辑
    public void Shooting(Transform gunTransform, Camera cam)
    {
        Vector3 targetPoint = GetShootTargetPoint(cam);

        Vector3 shootDirection = (targetPoint - gunTransform.position).normalized;

        GameObject bullet = BulletsPool.instance.GetBullet();
        bullet.transform.position = gunTransform.position;
        bullet.transform.rotation = Quaternion.LookRotation(shootDirection);

        bullet.GetComponent<Bullet>()?.Init(shootDirection);



    }

    //获取准星目标位置
    private Vector3 GetShootTargetPoint(Camera cam)
    {
        Vector3 targetPoint;

        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = cam.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        return targetPoint;
    }
}
