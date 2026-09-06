using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class LuaManager
{
    public static LuaManager instance { get; private set; }

    public LuaEnv luaEnv {  get; private set; }


    //手枪Lua脚本使用
    public LuaTable handGunTab { get; private set;  }
    public Func<float, float> getHandGunRecoil {  get; private set; }


    private LuaManager() { }

    public static void CreateInstance()
    {
        if (instance != null) return;

        instance = new LuaManager();
        instance.Init();
    }

    private void Init()
    {
        luaEnv = new LuaEnv();

        luaEnv.AddLoader((ref string filepath) =>
        {
            string path = Path.Combine(Application.dataPath, "Lua", filepath + ".lua");
            if (File.Exists(path))
            {
                Debug.Log(filepath + "读取成功");

                return File.ReadAllBytes(path);
            }
            Debug.Log(filepath + "读取失败");
            return null;
        });

        luaEnv.DoString("HandGun = require('HandGun')");
    }

    public void Start()
    {
        handGunTab = luaEnv.Global.Get<LuaTable>("HandGun");
        getHandGunRecoil = handGunTab.Get<Func<float, float>>("GetRecoil");

        Debug.Log(handGunTab.Get<float>("duration"));
    }

    public void OnDestroy()
    {

        getHandGunRecoil = null;

        luaEnv?.Dispose();

        instance = null;
    }
}
