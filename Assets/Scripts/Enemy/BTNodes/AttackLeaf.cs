using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLeaf : Leaf
{
    public AttackLeaf(string _name, Enemy _enemy) : base(_name, _enemy)
    {
    }

    private WeaponManager weaponManager;

    public override NodeState Process()
    {
        return base.Process();
    }

    protected override void Enter()
    {
        base.Enter();

        weaponManager = enemy.weaponManager;

        enemy.anim.SetBool("Aim", true);
        weaponManager.WeaponAim(true);
    }

    protected override NodeState Update()
    {
        if (enemy.weaponManager.currentHaveGun.currentMagazineSize <= 0)
            return NodeState.Failure;

        enemy.aimTarget.position = enemy.perception.firePosition;

        enemy.weaponManager.WeaponFire(0);

        return NodeState.Running;
    }

    protected override void Exit()
    {
        base.Exit();

        enemy.aimTarget = enemy.aimStart;

        enemy.anim.SetBool("Aim", false);
        weaponManager.WeaponAim(false);
    }

}
