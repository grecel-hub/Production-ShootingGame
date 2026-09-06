using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
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
        TextAsset bankAsset = await AssetBundleManager.instance.LoadResAsync<TextAsset>(_abName, _resName);

        if (bankAsset == null)
        {
            Debug.Log(_resName + " bank文件加载失败");
            return false;
        }

        byte[] bankData = bankAsset.bytes;
        uint bankID;
        AKRESULT result = AkSoundEngine.LoadBank(bankData, out bankID);

        if (result != AKRESULT.AK_Success)
        {
            Debug.Log(_resName + " bank文件加载失败");
            return false;
        }
        else
            return true;

    }

    public void PlayEvent(string eventName, GameObject emitter)
    {
        AkSoundEngine.PostEvent(eventName, emitter);
    }
}
