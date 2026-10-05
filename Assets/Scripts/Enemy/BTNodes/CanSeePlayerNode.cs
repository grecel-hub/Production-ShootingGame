using BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSeePlayerNode : Leaf
{
    public CanSeePlayerNode(string _name, Enemy _enemy) : base(_name, _enemy)
    {
    }

    public override NodeState Process()
    {
        return base.Process();
    }

    protected override void Enter()
    {
        base.Enter();
    }

    protected override NodeState Update()
    {
        return enemy.perception.canSeePlayer ? NodeState.Success : NodeState.Failure;
    }

    protected override void Exit()
    {
        base.Exit();
    }

}
