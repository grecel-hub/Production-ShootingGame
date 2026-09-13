using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    Short,
    Long
}

//枪
public class Gun : MonoBehaviour
{
    protected GunType gunType;

    [SerializeField] protected Transform muzzleTransform;
    [SerializeField] protected int maxMagazineSize = 7;

    [SerializeField] protected Transform LHandle;

    protected int currentMagazineSize;

    protected ShootController shootController;
    protected Camera cam;

    protected virtual void Start()
    {
        shootController = new ShootController();

        cam = Camera.main;

        currentMagazineSize = maxMagazineSize;
    }

    protected virtual void Update()
    {
        
    }

    public virtual void DrawRay()
    {
        Debug.DrawRay(muzzleTransform.position, muzzleTransform.right * -100, Color.red);
    }

    //开火
    public virtual bool Fire(Player player)
    {
        if (currentMagazineSize > 0)
        {
            shootController.Shooting(muzzleTransform, cam, player.aimTarget.position);
            currentMagazineSize --;

            MuzzleFlashe muzzleFlashe = MuzzleFlashePool.instance.Get();
            muzzleFlashe.transform.position = muzzleTransform.position;
            muzzleFlashe.transform.rotation = muzzleTransform.rotation * Quaternion.Euler(0, 180, 0);

            return true;
        }
        else
        {
            Debug.Log("弹夹为空");

            return false;
        }
    }

    //换弹
    public virtual void Reload()
    {
        if (currentMagazineSize >= maxMagazineSize)
            return;

        currentMagazineSize = maxMagazineSize;
    }

    //获取左手握把位置
    public virtual Transform GetLHandle()
    {
        return LHandle;
    }

    public virtual (int x, int y) GetMagazineSize()
    {
        return (currentMagazineSize, maxMagazineSize);
    }

    public GunType GetGunType()
    {
        return gunType;
    }
}
