using System;
using System.IO;
using UnityEngine;
using XLua;

public class LuaManager
{
    public static LuaManager instance { get; private set; }

    public LuaEnv luaEnv {  get; private set; }


    //手枪Lua脚本使用
    public LuaTable handGunTab { get; private set; }
    public Func<float, float> getHandGunRecoil { get; private set; }

    //步枪Lua脚本使用后
    public LuaTable rifleTab { get; private set;  }
    public Func<float, float> getRifleRecoil { get; private set; }

    //子弹Lua脚本使用
    public LuaTable bulletTab {  get; private set; }
    public Func<int, int> getDamage { get; private set;  }
    public Func<float, Vector3> getFireDirection { get; private set; }

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

        LuaDoString();
        LuaRegister();

        
    }

    private void LuaDoString()
    {
        luaEnv.DoString("HandGun = require('HandGun')");
        luaEnv.DoString("Rifle = require('Rifle')");
        luaEnv.DoString("Bullet = require('Bullet')");
    }

    public void LuaRegister()
    {
        handGunTab = luaEnv.Global.Get<LuaTable>("HandGun");
        getHandGunRecoil = handGunTab.Get<Func<float, float>>("GetRecoil");

        rifleTab = luaEnv.Global.Get<LuaTable>("Rifle");
        getRifleRecoil = rifleTab.Get<Func<float, float>>("GetRecoil");

        bulletTab = luaEnv.Global.Get<LuaTable>("Bullet");
        getDamage = bulletTab.Get<Func<int, int>>("GetDamage");
        getFireDirection = bulletTab.Get<Func<float, Vector3>>("GetFireDirection");
    }

    public void OnDestroy()
    {
        getHandGunRecoil = null;
        getRifleRecoil = null;
        getDamage = null;
        getFireDirection = null;

        luaEnv?.Dispose();

        instance = null;
    }
}
