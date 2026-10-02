using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNode
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }

    public readonly List<BTNode> children = new();
    protected int currentChild;

    public void AddChild(BTNode child) => children.Add(child);

    public virtual NodeState Process() => children[currentChild].Process();

    public virtual void Reset()
    {
        currentChild = 0;

        foreach(var child in children)
            child.Reset();
    }
}
