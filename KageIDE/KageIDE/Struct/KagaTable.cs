using System;
using System.Collections.Generic;
using System.Text;

namespace KageIDE.Struct
{
    public class KagaTable
    {
        // 构造器
        public KagaTable(int tableDepth)
        {
            this.depth = tableDepth;
            this.symbols = new List<string>();
            this.prefix = String.Format("___KAGA_VAR_{0}", this.depth);
        }

        // 编译模式取得所有符号
        public List<string> getParseTable()
        {
            List<string> parList = new List<string>();
            foreach (string s in symbols)
            {
                parList.Add(String.Format("{0}_{1}___", this.prefix, s));
            }
            return parList;
        }

        // 符号表数量
        public int size()
        {
            return this.symbols.Count;
        }

        // 符号表深度
        public int depth = 0;
        // 符号表前缀
        public string prefix = "___KAGA_";
        // 符号列表
        public List<string> symbols = null;
    }
}
