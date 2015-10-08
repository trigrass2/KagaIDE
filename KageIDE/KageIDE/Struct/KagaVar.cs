using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Enuming;

namespace KagaIDE.Struct
{
    /// <summary>
    /// 基础数据结构：符号单元
    /// </summary>
    [Serializable]
    public class KagaVar
    {
        /// <summary>
        /// 符号单元的构造器
        /// </summary>
        /// <param name="syname">符号名称</param>
        /// <param name="sytype">变量类型</param>
        public KagaVar(string syname, VarType sytype)
        {
            this.varname = syname;
            this.vartype = sytype;
        }

        /// <summary>
        /// 符号名字
        /// </summary>
        public string varname = Consta.const_none;

        /// <summary>
        /// 符号类型
        /// </summary>
        public VarType vartype = VarType.VOID;
    }
}
