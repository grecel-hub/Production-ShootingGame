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
        Debug.Log("初始化manager脚本");
        AudioManager.instance.InitAsync().Forget();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        LuaManager.instance.OnDestroy();
        AssetBundleManager.instance.ClearAll();

        Cursor.visible = true;
    }
}
