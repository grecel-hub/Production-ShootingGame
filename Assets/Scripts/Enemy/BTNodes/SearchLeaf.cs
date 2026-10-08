using BehaviorTree;
using UnityEngine;

public class SearchLeaf : Leaf
{
    private EnemyPerception perception;
    
    private Vector3 searchTarget;
    
    private float searchAngle;
    private float rotateSpeed;

    public SearchLeaf(string _name, Enemy _enemy, float _searchAngle, float _retateSpeed) : base(_name, _enemy)
    {
        perception = enemy.perception;
        searchAngle = _searchAngle;
        rotateSpeed = _retateSpeed;
    }

    protected override void Enter()
    {
        base.Enter();

        Vector3 forward = enemy.transform.forward;
        forward.y = 0f;

        searchTarget = enemy.transform.position + Quaternion.Euler(0, searchAngle, 0) * forward * 3f;
    }

    protected override NodeState Update()
    {
        if (perception.playerPosition == Vector3.zero || enemy.CheckPlayerFire())
            return NodeState.Failure;

        if (perception.canSeePlayer == true)
            return NodeState.Success;

        enemy.RotateToTarget(searchTarget, rotateSpeed);

        if (IsReachedSearchTarget())
        {
            return NodeState.Failure;
        }

        return NodeState.Running;
    }

    protected override void Exit()
    {
        base.Exit();
    }

    //检测扫视是否完成
    private bool IsReachedSearchTarget()
    {
        Vector3 direction = searchTarget - enemy.transform.position;

        return Vector3.Angle(enemy.transform.forward, direction) < 2f;
    }

}
