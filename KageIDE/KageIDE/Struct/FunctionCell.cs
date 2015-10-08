using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Struct;

namespace KagaIDE.Enuming
{
    /// <summary>
    /// 基础数据结构：函数单元
    /// </summary>
    [Serializable]
    public class FunctionCell
    {
        /// <summary>
        /// 函数单元的构造器
        /// </summary>
        /// <param name="fname">函数名</param>
        /// <param name="args">参数列表</param>
        /// <param name="rt">返回类型</param>
        public FunctionCell(string fname, List<KagaVar> args = null, VarType rt = VarType.VOID)
        {
            this.callname = fname;
            this.returnType = rt;
            this.paraList = args;
        }

        /// <summary>
        /// 以另一个函数的形式修改自己
        /// </summary>
        /// <param name="other">待复制函数单元</param>
        /// <returns>返回更新后的自身</returns>
        public FunctionCell editSign(FunctionCell other)
        {
            this.callname = other.callname;
            this.returnType = other.returnType;
            this.paraList = new List<KagaVar>(other.paraList.ToArray());
            return this;
        }

        /// <summary>
        /// 获得编译时函数签名
        /// </summary>
        /// <param name="containLeftBrucket">是否包含右花括号</param>
        /// <returns>编译时函数签名字符串</returns>
        public string getSign(bool containLeftBrucket = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.returnType.ToString().ToLower().Replace('_', ' '));
            sb.Append(" ");
            sb.Append(String.Format("{0}_{1}", Consta.prefix_fun, this.callname));
            sb.Append("(");
            for (int i = 0; i < this.paraList.Count; i++)
            {
                sb.Append(String.Format("{0} {1}", this.paraList[i].vartype, this.paraList[i].varname));
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

        /// <summary>
        /// 函数名称
        /// </summary>
        public string callname = Consta.prefix_fun;

        /// <summary>
        /// 返回类型
        /// </summary>
        public VarType returnType = VarType.VOID;

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<KagaVar> paraList = new List<KagaVar>();

    }
}
