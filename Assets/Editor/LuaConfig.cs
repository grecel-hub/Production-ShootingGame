using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public static class LuaConfig
{
    //配置Lua函数
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(Func<float, float>),
        typeof(Func<int, int>),
        typeof(Func<float, Vector3>)
    };
}
