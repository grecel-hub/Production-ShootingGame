using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolManager : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;

    public Vector3 GetPatrolPoint(int index)
    {
        return patrolPoints[index].position;
    }

    public int PointCount => patrolPoints.Length;
}
