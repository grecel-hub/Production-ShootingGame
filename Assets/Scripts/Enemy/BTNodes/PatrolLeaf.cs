using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrolLeaf : Leaf
{
    private EnemyPatrolManager patrolManager;

    private int currenPatrolPoint;

    public PatrolLeaf(string _name, Enemy _enemy) : base(_name, _enemy)
    {
        patrolManager = enemy.patrolManager;
    }

    public override NodeState Process()
    {
        return base.Process();
    }

    protected override void Enter()
    {
        base.Enter();

        MoveToPatrolPoint();
    }

    protected override NodeState Update()
    {
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
