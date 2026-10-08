using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        AssetBundleManager.CreateInstance();
        LuaManager.CreateInstance();
        AudioManager.CreateInstance();

        AudioManager.instance.InitAsync().Forget();
    }

    private void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        LuaManager.instance.OnDestroy();
        AssetBundleManager.instance.ClearAll();

        Cursor.visible = true;
    }
}
