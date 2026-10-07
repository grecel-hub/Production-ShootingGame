using BehaviorTree;

//攻击玩家行为
public class AttackLeaf : Leaf
{
    private WeaponManager weaponManager;
    private EnemyPerception perception;

    private bool isFirstAttack;

    public AttackLeaf(string _name, Enemy _enemy) : base(_name, _enemy)
    {
        weaponManager = enemy.weaponManager;
        perception = enemy.perception;
    }

    protected override void Enter()
    {
        base.Enter();

        isFirstAttack = true;

        enemy.anim.SetBool("Aim", true);
        weaponManager.WeaponAim(true);
    }

    protected override NodeState Update()
    {
        if (enemy.weaponManager.currentHaveGun.currentMagazineSize <= 0 || perception.canSeePlayer == false) //没子弹或看不见玩家返回失败
            return NodeState.Failure;

        enemy.aimTarget.position = enemy.perception.firePosition;

        if (!isFirstAttack) //刚进入Attack先瞄准，第二次进入才能开枪
            enemy.weaponManager.WeaponFire(0);

        isFirstAttack = false;

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
