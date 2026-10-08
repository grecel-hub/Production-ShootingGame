using BehaviorTree;
using UnityEngine;

//到达巡逻点后等待，总停留时间=等待时间+思考时间
public class WaitLeaf : Leaf
{
    private EnemyPerception perception;

    private float minWaitTime;
    private float maxWaitTime;

    private float timer;
    private float waitTime;

    public WaitLeaf(string _name, Enemy _enemy, float _minWaitTime, float _maxWaitTime) : base(_name, _enemy)
    {
        perception = enemy.perception;

        minWaitTime = _minWaitTime;
        maxWaitTime = _maxWaitTime;
    }

    protected override void Enter()
    {
        base.Enter();

        timer = 0;
        waitTime = Random.Range(minWaitTime, maxWaitTime);

        enemy.StopMove();
    }

    protected override NodeState Update()
    {
        timer += Time.deltaTime;

        if (perception.canSeePlayer || enemy.CheckPlayerFire())
            return NodeState.Failure;

        if (timer >= waitTime)
            return NodeState.Success;

        return NodeState.Running;
    }

    protected override void Exit()
    {
        base.Exit();
    }
}
