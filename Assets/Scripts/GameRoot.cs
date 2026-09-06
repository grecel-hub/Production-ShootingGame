using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        LuaManager.CreateInstance();
        AssetBundleManager.CreateInstance();
        AudioManager.CreateInstance();
    }

    private void Start()
    {
        LuaManager.instance.Start();
        AudioManager.instance.InitAsync().Forget();
    }

    private void OnDestroy()
    {
        LuaManager.instance.OnDestroy();
        AssetBundleManager.instance.ClearAll();
    }
}
