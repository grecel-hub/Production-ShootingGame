using BehaviorTree;

public class ReloadLeaf : Leaf
{
    private WeaponManager weaponManager;

    public ReloadLeaf(string _name, Enemy _enemy) : base(_name, _enemy)
    {
        weaponManager = enemy.weaponManager;
    }

    protected override void Enter()
    {
        base.Enter();

        if (enemy.weaponManager.currentHaveGun.currentMagazineSize < enemy.weaponManager.currentHaveGun.maxMagazineSize)
            weaponManager.Reload();
    }

    protected override NodeState Update()
    {
        if (enemy.weaponManager.currentHaveGun.currentMagazineSize >= enemy.weaponManager.currentHaveGun.maxMagazineSize)
            return NodeState.Failure;

        if (!weaponManager.isReloading)
            return NodeState.Success;

        return NodeState.Running;
    }


    protected override void Exit()
    {
        base.Exit();
    }

}
