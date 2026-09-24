using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//子弹
public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private float lifeTime = 0f;
    private float maxLifeTime = 3f;

    private float speed;

    [SerializeField] private GameObject bulletMesh;
    [SerializeField] private GameObject bulletImpact;

    private ShootController shootController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        shootController = new ShootController();
    }

    private void Start()
    {
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxLifeTime)
            StartCoroutine(Return());
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.GetComponentInParent<Entity>();

        if (entity)
        {
            Debug.Log("目标layer:" + other.gameObject.layer);
            shootController.DoDamage(entity, other.gameObject.layer);
            
        }

        rb.isKinematic = true;
        bulletMesh.SetActive(false);
        bulletImpact.SetActive(true);

        StartCoroutine(Return());
    }

    //初始化子弹
    public void Init(Vector3 direction)
    {
        speed = LuaManager.instance.bulletTab.Get<float>("speed");

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = direction * speed;

        lifeTime = 0f;
    }

    public IEnumerator Return()
    {
        yield return new WaitForSeconds(3);

        Debug.Log("bulletImpact == null: " + (bulletImpact == null));

        Debug.Log("触发子弹回收");
        rb.isKinematic = false;
        bulletMesh.SetActive(true);
        bulletImpact.SetActive(false);


        BulletPool.instance.Return(this);
    }
    
}
