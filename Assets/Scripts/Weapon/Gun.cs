using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    Short,
    Long
}

//枪
public abstract class Gun : MonoBehaviour
{
    public GunType gunType { get; protected set; }

    protected Animator anim;
    protected ShootController shootController;
    protected Camera cam;

    [SerializeField] protected Transform LHandle;
    [SerializeField] protected Transform muzzleTransform;
    [SerializeField] protected Transform magazine;

    //Data
    public int maxMagazineSize { get; protected set; } //最大弹容量
    public int currentMagazineSize { get; protected set;  } //当前弹容量
    public float duration { get; protected set; } //枪口复位时间

    protected float fireTime;


    protected virtual void Start()
    {
        anim = GetComponent<Animator>();

        shootController = new ShootController();

        cam = Camera.main;

        currentMagazineSize = maxMagazineSize;

        fireTime = 10;
    }

    protected virtual void Update()
    {
        fireTime += Time.deltaTime;
    }

    public virtual void DrawRay()
    {
        Debug.DrawRay(muzzleTransform.position, muzzleTransform.right * -100, Color.red);
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
    public Transform GetLHandle()
    {
        return LHandle;
    }

    //获取弹匣容量
    public (int x, int y) GetMagazineSize()
    {
        return (currentMagazineSize, maxMagazineSize);
    }

    #region 子类拓展方法
    //开火
    public abstract bool Fire(Player player);
    //获取后坐力计算
    public abstract float GetGunRecoil(float elapsed);
    #endregion
}
