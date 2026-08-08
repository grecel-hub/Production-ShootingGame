using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] protected Transform muzzleTransform;
    [SerializeField] protected int maxMagazineSize = 7;

    protected int currentMagazineSize;

    protected ShootManager shootManager;
    protected Camera cam;

    protected virtual void Start()
    {
        shootManager = new ShootManager();

        cam = Camera.main;
    }

    public virtual void Fire()
    {
        if (currentMagazineSize > 0)
        {
            shootManager.Shooting(muzzleTransform, cam);
            currentMagazineSize --;
        }
        else
        {
            Debug.Log("弹夹为空");
        }
    }

    public virtual void Reload()
    {
        currentMagazineSize = maxMagazineSize;
    }
}
