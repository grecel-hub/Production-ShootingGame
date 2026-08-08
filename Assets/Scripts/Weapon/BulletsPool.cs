using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//子弹对象池
public class BulletsPool : MonoBehaviour
{
    public static BulletsPool instance {  get; private set; }

    [SerializeField] private GameObject bulletPrefab;

    private int poolSize = 100;

    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform);
            newBullet.SetActive(false);
            poolQueue.Enqueue(newBullet);
            Debug.Log("初始化子弹对象池");
        }
    }

    //取出子弹
    public GameObject GetBullet()
    {
        GameObject bullet = poolQueue.Dequeue();

        bullet.SetActive(true);

        Debug.Log("取出子弹");

        return bullet;
    }

    //放回子弹
    public void ReturnBullet(GameObject bullet)
    {
        if (bullet == null) return;

        if (poolQueue.Contains(bullet)) return;

        bullet.SetActive(false);
        poolQueue.Enqueue(bullet);
    }

}
