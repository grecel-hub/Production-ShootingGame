//using System;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AssetBundleManager
{
    public static AssetBundleManager instance { get; private set; }
    private AssetBundleManager() { }

    private AssetBundle mainAB = null;
    private AssetBundleManifest manifest = null;

    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    private string MainABName
    {
        get
        {
#if UNITY_IOS
            retrun "IOS";
#elif UNITY_ANDROID
            retrun "Android";
#else
            return "StandaloneWindows";
#endif
        }
    }

    private string PathUrl
    {
        get
        {
            string baseDir;
#if UNITY_EDITOR
            // 编辑器下：项目根目录
            baseDir = System.IO.Path.GetDirectoryName(Application.dataPath);
#else
            // 发布后：exe所在目录
            baseDir = System.IO.Path.GetDirectoryName(Application.dataPath);
#endif
            // 拼接 AssetBundles/{平台名}/
            string platformDir = System.IO.Path.Combine(baseDir, "AssetBundles", MainABName);
            return platformDir + "/";  // 末尾加斜杠
        }
    }



    public static void CreateInstance()
    {
        if (instance != null) return;

        instance = new AssetBundleManager();
    }

    //加载AB包
    private void LoadAB(string abName)
    {
        //加载主包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        //获取依赖包
        AssetBundle ab = null;
        string[] strs = manifest.GetAllDependencies(abName);

        //加载依赖包
        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }

        //加载资源包
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    //同步加载-不指定类型
    public Object LoadRes(string abName, string resName)
    {
        LoadAB(abName);

        return abDic[abName].LoadAsset(resName);
    }

    //同步加载-type决定类型
    public Object LoadRes(string abName, string resName, System.Type type)
    {
        LoadAB(abName);

        return abDic[abName].LoadAsset(resName, type);
    }

    //同步加载-泛型决定类型
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        LoadAB(abName);

        return abDic[abName].LoadAsset<T>(resName);
    }

    //异步加载-不指定类型
    public async UniTask<Object> LoadResAsync(string abName, string resName)
    {
        LoadAB(abName);

        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        await abr;

        return abr.asset;
    }

    //异步加载-Type决定类型
    public async UniTask<Object> LoadResAsync(string abName, string resName, System.Type type)
    {
        LoadAB(abName);

        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        await abr;

        return abr.asset;
    }

    //异步加载-泛型决定类型
    public async UniTask<T> LoadResAsync<T>(string abName, string resName) where T : Object
    {
        LoadAB(abName);

        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        await abr;

        return abr.asset as T;
    }

    //卸载单个包
    public void UnLoad(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }

    //卸载所有包
    public void ClearAll()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}
