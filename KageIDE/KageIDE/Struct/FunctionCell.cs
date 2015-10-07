using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Struct;

namespace KagaIDE.Enuming
{
    public class FunctionCell
    {
        // 构造器
        public FunctionCell(
            string fname,
            List<KagaVar> args = null,
            VarType rt = VarType.VOID)
        {
            this.callname = fname;
            this.returnType = rt;
            this.paraList = args;
        }

        // 以另一个函数的形式修改自己
        public FunctionCell editSign(FunctionCell other)
        {
            this.callname = other.callname;
            this.returnType = other.returnType;
            this.paraList = new List<KagaVar>(other.paraList.ToArray());
            return this;
        }

        // 获得编译时函数签名
        public string getSign(bool containLeftBrucket = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.returnType.ToString().ToLower().Replace('_', ' '));
            sb.Append(" ");
            sb.Append(this.callname);
            sb.Append("(");
            for (int i = 0; i < this.paraList.Count; i++)
            {
                sb.Append(String.Format(
                    "{0} {1}", this.paraList[i].vartype, this.paraList[i].varname));
                if (i != this.paraList.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(")");
            if (containLeftBrucket == true)
            {
                sb.Append(" {");
            }
            return sb.ToString();
        }

        // 函数头
        public string callname = "___KAGA_";
        // 返回类型
        public VarType returnType = VarType.VOID;
        // 参数列表
        public List<KagaVar> paraList = new List<KagaVar>();

    }
}
