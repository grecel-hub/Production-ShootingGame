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

    [SerializeField] protected Transform lHandle;
    [SerializeField] protected Transform muzzleTransform;
    [SerializeField] protected Transform magazineTransform;
    protected Magazine magazine;

    //Data
    public int maxMagazineSize { get; protected set; } //最大弹容量
    public int currentMagazineSize { get; protected set;  } //当前弹容量
    public float duration { get; protected set; } //枪口复位时间
    public float maxAngleDeg { get; protected set; } //最大散射角度
    public Vector3 aimOffset { get; protected set; } //瞄准控制肩膀偏差

    protected float fireTime;
    protected bool canSkipChamber;


    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        magazine = GetComponentInChildren<Magazine>();

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
    public void StartReload()
    {
        canSkipChamber = false;

        magazine.UnloadMagazine();

        if (currentMagazineSize > 0) //判断是否需要上膛
            anim.SetBool("Chamber", false);

        else
            anim.SetBool("Chamber", true);

        anim.SetBool("Reload", true);
    }

    #region 换弹生命周期

    //获取弹匣
    public void GetNewMagazine(Transform takeMagazine)
    {
        if (gunType == GunType.Long)
            magazine = RifleMagazinePool.instance.Get();
        else if (gunType == GunType.Short)
            magazine = HandGunMagazinePool.instance.Get();

        magazine.UpdateTransform(takeMagazine);

    }

    //完成弹匣安装，并返回一个bool判断是否执行PlayerAnimator的后续上膛动画
    public void InstallMagazineAndNeedsChamber()
    {
        magazine.UpdateTransform(null);
        magazine.InstallMagazine(magazineTransform);

        currentMagazineSize = maxMagazineSize;
    }

    //结束换弹
    public void DoneReload()
    {
        anim.SetBool("Reload", false);
        anim.SetBool("Chamber", false);
    }

    #endregion

    //获取左手握把位置
    public Transform GetLHandle()
    {
        return lHandle;
    }

    //获取弹匣容量
    public (int x, int y) GetMagazineSize()
    {
        return (currentMagazineSize, maxMagazineSize);
    }

    public bool GetCanSkipChamber()
    {
        if (canSkipChamber)
            return true;
        else
            return false;
    }

    #region 子类拓展方法
    //初始化数据
    protected abstract void DataInit();
    //开火
    public abstract bool Fire(Player player, float moveSpeed, int fireCount);
    //获取后坐力计算
    public abstract float GetGunRecoil(float elapsed);
    #endregion

    #region 动画触发事件
    //完成弹匣更换，无后续上膛动画
    public void OnReloadFinishMagazineInstall()
    {
        canSkipChamber = true;
    }
    #endregion
}
