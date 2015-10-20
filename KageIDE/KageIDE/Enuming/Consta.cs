using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

namespace KagaIDE.Enuming
{
    /// <summary>
    /// 管理全局方法和变量的类
    /// </summary>
    public static class Consta
    {
        /// <summary>
        /// 把一个C类型匹配成VarType枚举
        /// </summary>
        /// <param name="parStr">待匹配字符串</param>
        /// <returns>该字符串对应等价的VarType</returns>
        public static VarType parseCTypeToVarType(string parStr)
        {
            parStr = parStr.ToUpper().Replace(' ', '_');
            return (VarType)Enum.Parse(typeof(VarType), parStr, true);
        }

        /// <summary>
        /// 把一个VarType枚举匹配成C类型
        /// </summary>
        /// <param name="parEnum">待匹配VarType</param>
        /// <returns>该VarType对应等价的C类型字符串</returns>
        public static string parseVarTypeToCType(VarType parEnum)
        {
            return parEnum.ToString().ToLower().Replace('_', ' ');
        }

        /// <summary>
        /// 测试一个字符串是否可以作为一个C语言符号
        /// </summary>
        /// <param name="parStr">待匹配字符串</param>
        /// <returns>是否可以作为C符号</returns>
        public static bool IsStdCSymbol(string parStr)
        {
            return Consta.IsMatchRegEx(parStr, @"^[a-zA-Z_][a-zA-Z0-9_]*$") && (!Consta.IsStdCKeyword(parStr));
        }

        /// <summary>
        /// 测试一个字符串是否满足一个正则式
        /// </summary>
        /// <param name="parStr">待校验字符串</param>
        /// <param name="regEx">正则表达式</param>
        /// <returns>正则式真值</returns>
        public static bool IsMatchRegEx(string parStr, string regEx)
        {
            Regex myRegex = new Regex(regEx);
            return myRegex.IsMatch(parStr);
        }

        /// <summary>
        /// 测试一个字符串是否是C语言关键字
        /// </summary>
        /// <param name="parStr">待匹配字符串</param>
        /// <returns>是否是C的关键字</returns>
        public static bool IsStdCKeyword(string parStr)
        {
            return ckeyword.Find((x) => x == parStr) != null;
        }

        /// <summary>
        /// 获取NodeType的前端色彩
        /// </summary>
        /// <param name="tp">NodeType类型</param>
        /// <returns>前端颜色</returns>
        public static Color getColoring(NodeType tp)
        {
            switch (tp)
            {
                case NodeType.PILE__BLOCK__FUNCTION:
                    return Color.Purple;
                case NodeType.PILE__IF:
                    return Color.Blue;
                case NodeType.BLOCK__IF_TRUE:
                    return Color.Gray;
                case NodeType.BLOCK__IF_FALSE:
                    return Color.Gray;
                case NodeType.BLOCK__WHILE:
                    return Color.Blue;
                case NodeType.BLOCK__FOR:
                    return Color.Blue;
                case NodeType.BREAK:
                    return Color.Blue;
                case NodeType.CALL:
                    return Color.Orange;
                case NodeType.EXPRESSION:
                    return Color.Red;
                case NodeType.USING_SWITCHES:
                    return Color.Red;
                case NodeType.RETURN:
                    return Color.Violet;
                case NodeType.NOTE:
                    return Color.Green;
                case NodeType.CODEBLOCK:
                    return Color.Gray;
                case NodeType.DEFINE_VARIABLE:
                    return Color.Red;
                case NodeType.RUNONCE:
                    return Color.OrangeRed;
                case NodeType.PILE__PADDING_CURSOR:
                    return Color.Black;
                default:
                    return Color.Black;
            }
        }


        /// <summary>
        /// 控制前台内容刷新是否被阻塞
        /// </summary>
        public static bool refreshMutex = false;

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

        // 代码块枚举型前缀
        public static readonly string prefix_block = "BLOCK__";
        // 自定义函数前缀
        public static readonly string prefix_fun = "";
        // 自定义变量前缀
        public static readonly string prefix_var = "___KAGA_VAR";
        // 前台语句前缀
        public static readonly string prefix_frontend = "◆";
        // 前台条件语句前缀
        public static readonly string prefix_frontCond = ":";
        // 自定义开关数组名
        public static readonly string switch_name = "___KAGA_SWITCHES";
        // 待命名变量名
        public static readonly string const_none = "#UUZ#";
        // 翻译语句结尾符号
        public static readonly string pile_statend = ";";
        // 开关总量
        public static readonly int switch_max = 64;
        #endregion
    }
}
