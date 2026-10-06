using BehaviorTree;
using TMPro;
using UnityEngine;

//丢失玩家视野后追踪玩家
public class ChaseLeaf : Leaf
{
    private EnemyPerception perception;

    public ChaseLeaf(string _name, Enemy _enemy) : base(_name, _enemy)
    {
        perception = enemy.perception;
    }

    protected override void Enter()
    {
        base.Enter();

        if (perception.playerPosition != Vector3.zero)
            MoveToPlayer();
    }

    protected override NodeState Update()
    {
        if (perception.playerPosition == Vector3.zero || enemy.ReachedDesination()) //到达追踪地点后前往下一个子节点Search
        {
            enemy.StopMove();

            return NodeState.Failure;
        }
            
        if (perception.canSeePlayer == true) //再次看见玩家停止追踪，重新思考
        {
            enemy.StopMove();

            return NodeState.Success;
        }

        return NodeState.Running;
    }

    protected override void Exit()
    {
        base.Exit();
    }

    //移动至上一次看见的玩家位置
    private void MoveToPlayer()
    {
        Vector3 playerPosition = perception.playerPosition;

        enemy.MoveTo(playerPosition, true);
    }
}
