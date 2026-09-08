using System;
using System.Collections.Generic;
using XLua;

public static class LuaConfig
{
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(Func<float, float>),
        typeof(Func<int, int>)
    };
}
