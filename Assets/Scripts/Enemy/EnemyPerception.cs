using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerception : MonoBehaviour
{
    public float viewDistance = 15f;
    public float viewAngle = 120f;
    public LayerMask targetMask;

    [SerializeField] private Player player;

    public bool canSeePlayer {  get; private set; }
    public Vector3 firePosition { get; private set;  }
    public Vector3 playerPosition { get; private set;  }

    private void Start()
    {
        playerPosition = Vector3.zero;
    }

    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        canSeePlayer = false;

        //距离检测
        float distance = (player.transform.position - transform.position).magnitude;

        if (distance > viewDistance)
            return;

        //角度检测
        float angle = Vector3.Angle(transform.forward, (player.transform.position - transform.position));

        if (angle > viewAngle * 0.5f)
            return;

        //障碍物检测
        for (float height = 1.7f; height > 0; height -= 0.3f)
        {
            Vector3 origin = transform.position + Vector3.up * height; 
            Vector3 direction = (player.transform.position + Vector3.up * height - origin).normalized;

            if  (Physics.Raycast(origin, direction, out RaycastHit hit, viewDistance, targetMask))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    canSeePlayer = true;
                    firePosition = hit.point;
                    playerPosition = hit.transform.position;

                    break;
                }
            }
        }
    }
}
