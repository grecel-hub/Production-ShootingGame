using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Gun
{
    protected override void Start()
    {
        base.Start();
        WeaponManager.instance.SwitchGun(this);
    }

    public override void Fire()
    {
        base.Fire();
    }

    public override void Reload()
    {
        base.Reload();
    }
}
