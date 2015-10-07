using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace KagaIDE.Enuming
{
    // 管理全局方法和变量的类
    public static class Consta
    {
        // 把一个类型匹配成VarType枚举的类型
        public static VarType parseCTypeToVarType(string parStr)
        {
            parStr = parStr.ToUpper().Replace(' ', '_');
            return (VarType)Enum.Parse(typeof(VarType), parStr, true);
        }

        // 把一个类型匹配成VarType枚举的类型
        public static string parseVarTypeToCType(VarType parEnum)
        {
            return parEnum.ToString().ToLower().Replace('_', ' ');
        }

        // 测试一个字符串是否可以作为一个C语言符号
        public static bool IsStdCSymbol(string parStr)
        {
            Regex regex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*$");
            bool f1 = regex.IsMatch(parStr);
            bool f2 = (!IsStdCKeyword(parStr));
            return f1 && f2;
        }

        // 测试一个字符串是否是C语言关键字
        public static bool IsStdCKeyword(string parStr)
        {
            return ckeyword.Find((x) => x == parStr) != null;
        }

        #region 全局常量区
        // 基础数据类型
        public static readonly List<string> basicType = new List<string>()
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
        public static readonly List<string> returnType = new List<string>()
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

        // 关键字集合
        public static readonly List<string> ckeyword = new List<string>()
        {
            "auto",
            "break",
            "case",
            "char",
            "const",
            "continue",
            "default",
            "do",
            "double",
            "else",
            "enum",
            "extern",
            "float",
            "for",
            "goto",
            "if",
            "int",
            "long",
            "register",
            "return",
            "short",
            "signed",
            "sizoef",
            "static",
            "struct",
            "switch",
            "typedef",
            "union",
            "unsigned",
            "void",
            "volatile",
            "while"
        };
        #endregion
    }
}
