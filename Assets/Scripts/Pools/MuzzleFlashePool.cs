using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//泛型对象池挂载脚本
public class MuzzleFlashePool : ObjectPool<MuzzleFlashe>
{
    public static MuzzleFlashePool instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    protected override void Start()
    {
        base.Start();
    }
}
