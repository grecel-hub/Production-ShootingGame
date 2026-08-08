using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private float lifeTime = 0f;
    private float maxLifeTime = 3f;

    [SerializeField] private float speed = 1;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxLifeTime)
            BulletsPool.instance.ReturnBullet(gameObject);
    }

    public void Init(Vector3 direction)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = direction * speed;

        lifeTime = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        BulletsPool.instance.ReturnBullet(gameObject);
    }
}
