using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//子弹
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
            BulletPool.instance.Return(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        BulletPool.instance.Return(this);
    }

    //初始化子弹
    public void Init(Vector3 direction)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = direction * speed;

        lifeTime = 0f;
    }

    
}
