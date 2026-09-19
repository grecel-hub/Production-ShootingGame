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

    protected Animator anim;


    [SerializeField] protected Transform LHandle;
    [SerializeField] protected Transform muzzleTransform;
    [SerializeField] protected Transform magazine;
    [SerializeField] protected int maxMagazineSize = 7;

    protected int currentMagazineSize;

    protected ShootController shootController;
    protected Camera cam;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();

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
    public virtual void Reload(bool isReloading)
    {
        Rigidbody rb = magazine.GetComponent<Rigidbody>();

        anim.SetBool("Reload", isReloading);

        rb.isKinematic = !isReloading;

        currentMagazineSize = maxMagazineSize;
    }

    //获取左手握把位置
    public virtual Transform GetLHandle()
    {
        return LHandle;
    }

    //获取弹匣容量
    public virtual (int x, int y) GetMagazineSize()
    {
        return (currentMagazineSize, maxMagazineSize);
    }

    //获取枪支类型
    public GunType GetGunType()
    {
        return gunType;
    }
}
