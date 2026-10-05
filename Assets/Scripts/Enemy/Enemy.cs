using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public EnemyPerception perception {  get; private set; }
    public EnemyPatrolManager patrolManager { get; private set; }

    [SerializeField] private Gun equipingGun;
    public Transform aimStart;

    private BTNode root;
    private NavMeshAgent agent;
    private float currentSpeed;

    protected override void Start()
    {
        base.Start();

        perception = GetComponent<EnemyPerception>();
        patrolManager = GetComponent<EnemyPatrolManager>();

        agent = GetComponent<NavMeshAgent>();

        CreateBTNode();

        EquipGun();

        agent.updatePosition = false;

        aimTarget.position = aimStart.position;

    }

    //构建行为树
    private void CreateBTNode()
    {
        root = new BTNode("Root");

        var patrol = new PatrolLeaf("Patrol", this);

        var reload = new ReloadLeaf("Reload", this);
        var attack = new AttackLeaf("Attack", this);
        var canSeePlayer = new CanSeePlayerNode("CanSeePlayer?", this);

        var attackSequenceNode = new SequenceNode("Attack", 0);
        var selectorNode = new SelectorNode("Reload", 1f);

        attackSequenceNode.AddChild(canSeePlayer);
        attackSequenceNode.AddChild(attack);

        selectorNode.AddChild(attackSequenceNode);
        selectorNode.AddChild(reload);
        selectorNode.AddChild(patrol);

        root.AddChild(selectorNode);
    }

    protected override void Update()
    {
        base.Update();

        if (perception.canSeePlayer)
            RotateToTarget(perception.firePosition);

        if (equipingGun.gunType == GunType.Long)
            anim.SetBool("HoldLongGun", true);
        if (equipingGun.gunType == GunType.Short)
            anim.SetBool("HoldShortGun", true);

        root.Process();

        UpdateAnimation();
    }

    //敌人旋转
    public void RotateToTarget(Vector3 targetPosition, float rotateSpeed = 1000f)
    {
        Vector3 direction = targetPosition - transform.position;

        direction.y = 0;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    //设置移动地点
    public void MoveTo(Vector3 targetPosition, bool isRun)
    {
        if (isRun)
            agent.speed = 3;
        else
            agent.speed = 1.5f;

        agent.isStopped = false;

        agent.SetDestination(targetPosition);
    }

    //停止移动
    public void StopMove()
    {
        agent.isStopped = true;
    }

    //检测敌人是否到达目的地
    public bool ReachedDesination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    //更新根运动
    private void UpdateAnimation()
    {
        float targetSpeed = agent.desiredVelocity.magnitude;

        agent.nextPosition = transform.position;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 0.1f);
        anim.SetFloat("Speed", currentSpeed);
    }


    //装备武器
    private void EquipGun()
    {
        weaponManager.EquipGun(equipingGun);
    }
}
