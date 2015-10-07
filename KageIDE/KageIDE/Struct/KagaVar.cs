using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Enuming;

namespace KagaIDE.Struct
{
    // 表达一个符号的基本类
    public class KagaVar
    {
        public KagaVar(string syname, VarType sytype)
        {
            this.varname = syname;
            this.vartype = sytype;
        }
        // 符号名字
        public string varname = Consta.const_none;
        // 符号类型
        public VarType vartype = VarType.VOID;
    }
}
