using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Component
{
    private T prefab;

    private Queue<T> pool = new Queue<T>();

    public string abName = "prefab";
    public string resName;
    public int poolSize = 20;


    protected virtual void Start()
    {
        InitAsync(abName, resName, poolSize).Forget();
    }

    //初始化对象池
    /*public void Init(T _prefab, int _poolSize = 20)
    {
        prefab = _prefab;
        poolSize = _poolSize;

        for (int i = 0; i < poolSize; i++)
        {
            T item = Instantiate(prefab, transform);
            item.gameObject.SetActive(false);
            pool.Enqueue(item);
        }
    }*/

    //异步初始化对象池
    public async UniTask InitAsync(string _abName, string _resName, int _poolSize)
    {
        abName = _abName;
        resName = _resName;
        poolSize = _poolSize;

        GameObject gamePrefab = await AssetBundleManager.instance.LoadResAsync<GameObject>(abName, resName);

        if (gamePrefab == null)
        {
            Debug.Log(resName + "预制体加载失败");
            return;
        }

        prefab = gamePrefab.GetComponent<T>();

        for (int i = 0; i < poolSize; i++)
        {
            T item = Instantiate(prefab, transform);
            item.gameObject.SetActive(false);
            pool.Enqueue(item);
        }
    }

    //取出对象
    public T Get()
    {
        if (pool.Count == 0)
        {
            T item = Instantiate(prefab, transform);
            item.gameObject.SetActive(false);
            pool.Enqueue(item);
        }

        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void Return(T obj)
    {
        if (obj == null) return;

        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
