using BehaviorTree;
using UnityEngine;


public class PatrolLeaf : Leaf
{
    private EnemyPatrolManager patrolManager;
    private EnemyPerception perception;

    private int currenPatrolPoint;

    public PatrolLeaf(string _name, Enemy _enemy) : base(_name, _enemy)
    {
        patrolManager = enemy.patrolManager;
        perception = enemy.perception;
    }

    protected override void Enter()
    {
        base.Enter();

        MoveToPatrolPoint();
    }

    protected override NodeState Update()
    {
        if (perception.canSeePlayer || enemy.CheckPlayerFire())
            return NodeState.Failure;

        if (enemy.ReachedDesination()) //到达巡逻点
        {
            currenPatrolPoint++;

            if (currenPatrolPoint >= patrolManager.PointCount)
                currenPatrolPoint = 0;

            return NodeState.Success;
        }

        return NodeState.Running;
    }

    protected override void Exit()
    {
        base.Exit();

        enemy.StopMove();
    }

    //前往巡逻点
    private void MoveToPatrolPoint()
    {
        Vector3 point = patrolManager.GetPatrolPoint(currenPatrolPoint);

        enemy.MoveTo(point, false);
    }

}
