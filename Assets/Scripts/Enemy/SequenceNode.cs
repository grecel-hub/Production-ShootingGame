using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : BTNode
{
    public override NodeState Process()
    {
        while(currentChild < children.Count)
        {
            NodeState state = children[currentChild].Process();

            if (state == NodeState.Failure)
            {
                return NodeState.Failure;
            }

            if (state == NodeState.Running)
            {
                return NodeState.Running;
            }

            currentChild++;
        }

        return NodeState.Success;
    }
}
