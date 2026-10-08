using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BehaviorTree
{
    #region 复合节点
    //Selector会在重新选择行为前等待
    public class SelectorNode : BTNode
    {
        private float timer = 0;
        private float thinkTime;
        private bool isRunning;

        public SelectorNode(string _name, float _thinkTime = 0f) : base(_name)
        {
            thinkTime = _thinkTime;
            timer = thinkTime;
        }

        public override NodeState Process()
        {
            // 当前子节点Running时每帧继续执行
            // 当前行为结束后，等待thinkTime再重新选择行为
            if (isRunning)
            {
                NodeState state = children[currentChild].Process();

                if (state == NodeState.Success)
                {
                    isRunning = false;
                    Reset();
                    return NodeState.Success;
                }

                if (state == NodeState.Running)
                    return NodeState.Running;

                currentChild++;
                isRunning = false;
            }

            timer += Time.deltaTime;

            if (timer < thinkTime)
                return NodeState.Running;

            timer = 0;

            while (currentChild < children.Count)
            {

                NodeState state = children[currentChild].Process();

                if (state == NodeState.Success)
                {
                    Reset();
                    return NodeState.Success;
                }

                if (state == NodeState.Running)
                {
                    isRunning = true;
                    return NodeState.Running;
                }

                currentChild++;
            }

            Reset();
            return NodeState.Failure;
        }

        public override void Reset()
        {
            base.Reset();

            isRunning = false;
        }
    }

    //Sequence会每次执行子节点前等待
    public class SequenceNode : BTNode
    {
        private float timer = 0;
        private float thinkTime;
        public SequenceNode(string _name, float _thinkTime = 0f) : base(_name)
        {
            thinkTime = _thinkTime;
            timer = thinkTime;
        }

        public override NodeState Process()
        {
            timer += Time.deltaTime;

            if (timer < thinkTime)
                return NodeState.Running;

            timer = 0;

            while (currentChild < children.Count)
            {
                NodeState state = children[currentChild].Process();

                if (state == NodeState.Failure)
                {
                    Reset();
                    return NodeState.Failure;
                }

                if (state == NodeState.Running)
                {
                    return NodeState.Running;
                }

                currentChild++;
            }

            Reset();
            return NodeState.Success;
        }
    }
    #endregion

    public abstract class Leaf : BTNode
    {
        protected bool isEnter = true;

        protected Enemy enemy;
        public Leaf(string _name, Enemy _enemy) : base(_name)
        {
            enemy = _enemy;
        }

        public override NodeState Process()
        {
            NodeState state = NodeState.Failure;

            if (enemy.isDead)
                return state;
            
            if (isEnter)
            {
                isEnter = false;
                Enter();
            }

            state = Update();

            if (state != NodeState.Running)
            {
                Exit();
                isEnter = true;
            }

            return state;
        }

        protected virtual void Enter()
        {
            Debug.Log($"进入{name}行为");
        }

        protected abstract NodeState Update();

        protected virtual void Exit()
        {

        }

        public override void Reset()
        {
            isEnter = true;

            base.Reset();
        }
    }

    public class BTNode
    {
        public enum NodeState
        {
            Running,
            Success,
            Failure
        }

        public readonly string name;

        public BTNode(string _name)
        {
            name = _name;
        }

        public readonly List<BTNode> children = new();
        protected int currentChild;

        public void AddChild(BTNode child) => children.Add(child);

        public virtual NodeState Process() => children[currentChild].Process();

        public virtual void Reset()
        {
            currentChild = 0;

            foreach (var child in children)
                child.Reset();
        }
    }
}