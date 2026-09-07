using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected CapsuleCollider collider;

    protected int health = 100;

    protected virtual void Start()
    {
        collider = GetComponent<CapsuleCollider>();
    }

    protected virtual void Update()
    {
        if (health <= 0)
        {
            collider.enabled = false;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
