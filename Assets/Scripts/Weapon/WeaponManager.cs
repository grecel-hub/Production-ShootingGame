using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance {  get; private set; }

    private Gun gun;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    
    public void SwitchGun(Gun newGun)
    {
        if (gun == null)
            gun = newGun;

        else
        {
            gun.GetComponent<ParentConstraint>().enabled = false;
            gun = newGun;
        }
    }

    public void WeaponFire()
    {
        gun.Fire();
    }
}
