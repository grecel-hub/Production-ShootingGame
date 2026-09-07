using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

public class AudioManager
{
    public static AudioManager instance { get; private set; }

    private AudioManager() { }

    public static void CreateInstance()
    {
        if (instance != null) return;

        instance = new AudioManager();
    }

    public async UniTask InitAsync()
    {
        bool initLoaded = await LoadBankForAssetBundle("banks", "Init.bnk");
        bool footstepLoaded = await LoadBankForAssetBundle("banks", "Footstep_Bank.bnk");
    }

    public async UniTask<bool> LoadBankForAssetBundle(string _abName, string _resName)
    {
        try
        {

            TextAsset bankAsset = await AssetBundleManager.instance.LoadResAsync<TextAsset>(_abName, _resName);

            if (bankAsset == null)
            {
                Debug.Log(_resName + " bank文件加载失败");
                return false;
            }

            byte[] bankData = bankAsset.bytes;
            GCHandle handle = GCHandle.Alloc(bankData, GCHandleType.Pinned);
            IntPtr bankPtr = handle.AddrOfPinnedObject();

            uint bankID;
            AKRESULT result = AkSoundEngine.LoadBankMemoryCopy(
                bankPtr,                          // 内存指针
                (uint)bankData.Length,            // 数据大小
                out bankID                        // 输出 Bank ID
            );

            handle.Free();

            if (result != AKRESULT.AK_Success)
            {
                Debug.Log(_resName + " bank文件加载失败");
                return false;
            }
            else
                return true;
            }
        catch (Exception ex)
        {
            Debug.LogError($"加载{_resName}异常：{ex.Message}");
            return false;
        }

    }

    public void PlayEvent(string eventName, GameObject emitter)
    {
        AkSoundEngine.PostEvent(eventName, emitter);
    }
}
