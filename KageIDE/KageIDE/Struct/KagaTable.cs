using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Enuming;
using KagaIDE.Module;

namespace KagaIDE.Struct
{
    /// <summary>
    /// 基础数据结构：层次符号表
    /// </summary>
    [Serializable]
    public class KagaTable
    {
        /// <summary>
        /// 层次符号表的构造器
        /// </summary>
        /// <param name="tableDepth">符号表深度</param>
        /// <param name="belonging">符号表的属节点</param>
        public KagaTable(int tableDepth, KagaNode belonging)
        {
            this.depth = tableDepth;
            this.belong = belonging;
            this.symbols = new List<KagaVar>();
            this.prefix = String.Format("{0}_{1}", Consta.prefix_var, this.depth);
            // 把自己追加到符号表里
            this.symbolMana = SymbolManager.getInstance();
            this.symbolMana.addSymbolTable(this);
        }

        /// <summary>
        /// 取得编译模式时所有符号的签名
        /// </summary>
        /// <returns>编译模式时符号签名字符串向量</returns>
        public List<string> getParseTable()
        {
            List<string> parList = new List<string>();
            foreach (KagaVar s in symbols)
            {
                parList.Add(String.Format("{0}_{1}@{2}", this.prefix, s.varname, s.vartype));
            }
            return parList;
        }

        /// <summary>
        /// 获得符号表的数量
        /// </summary>
        /// <returns>符号表的尺寸</returns>
        public int size()
        {
            return this.symbols.Count;
        }

        /// <summary>
        /// 符号表深度
        /// </summary>
        public int depth = 0;

        /// <summary>
        /// 属于哪个代码树节点
        /// </summary>
        public KagaNode belong = null;

        /// <summary>
        /// 符号表前缀
        /// </summary>
        public string prefix = Consta.prefix_var;

        /// <summary>
        /// 符号列表
        /// </summary>
        public List<KagaVar> symbols = null;

        /// <summary>
        /// 符号管理器指针
        /// </summary>
        private SymbolManager symbolMana = null;
    }
}
