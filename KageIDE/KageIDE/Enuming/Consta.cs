using System;
using System.Collections.Generic;
using System.Text;

namespace KagaIDE.Enuming
{
    // 管理全局常量的类
    public static class Consta
    {
        // 基础数据类型
        public static List<string> basicType = new List<string>()
        {
            "int",
            "char",
            "long",
            "float",
            "double",
            "unsigned int",
            "unsigned char"
        };

        // 基础返回类型
        public static List<string> returnType = new List<string>()
        {
            "void",
            "int",
            "long",
            "char",
            "float",
            "double",
            "unsigned int",
            "unsigned char"
        };

        // 把一个类型匹配成VarType枚举的类型
        public static VarType parseToVarType(string parStr)
        {
            parStr = parStr.ToUpper().Replace(' ', '_');
            return (VarType)Enum.Parse(typeof(VarType), parStr, true);
        }

    }
}
