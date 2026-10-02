using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : BTNode
{
    public override NodeState Process()
    {
        while (currentChild < children.Count)
        {
            NodeState state = children[currentChild].Process();

            if (state == NodeState.Success)
            {
                return NodeState.Success;
            }

            if (state == NodeState.Running)
            {
                return NodeState.Running;
            }

            currentChild++;
        }

        return NodeState.Failure;
    }
}
