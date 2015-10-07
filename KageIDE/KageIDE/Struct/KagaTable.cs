﻿using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Enuming;

namespace KagaIDE.Struct
{
    [Serializable]
    public class KagaTable
    {
        // 构造器
        public KagaTable(int tableDepth, KagaNode belonging)
        {
            this.depth = tableDepth;
            this.belong = belonging;
            this.symbols = new List<KagaVar>();
            this.prefix = String.Format("{0}_{1}", Consta.prefix_var, this.depth);
        }

        // 编译模式取得所有符号
        public List<string> getParseTable()
        {
            List<string> parList = new List<string>();
            foreach (KagaVar s in symbols)
            {
                parList.Add(String.Format("{0}_{1}@{2}", this.prefix, s.varname, s.vartype));
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
        // 属于哪个节点
        public KagaNode belong = null;
        // 符号表前缀
        public string prefix = Consta.prefix_var;
        // 符号列表
        public List<KagaVar> symbols = null;
    }
}
