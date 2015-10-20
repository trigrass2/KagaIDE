using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Struct;
using KagaIDE.Module;
using KagaIDE.Enuming;

namespace KagaIDE.Module
{
    /// <summary>
    /// 代码转换翻译器
    /// </summary>
    public class PileParser
    {
        /// <summary>
        /// 翻译代码树到C语言
        /// </summary>
        public void startDash()
        {
            StringBuilder codeBuilder = new StringBuilder();
            // 第一阶段：处理标头和宏
            string stdHeadAndMacro = this.symbolMana.getMarcoContainer();
            codeBuilder.Append(stdHeadAndMacro);
            codeBuilder.Append("// ____KAGA_STDHEADANDMACRO_DEAL_ABOVE" + Environment.NewLine + Environment.NewLine);
            // 第二阶段：处理全局量和函数签名
            KagaTable globalKT = this.symbolMana.getGlobalTable();
            foreach (string gs in globalKT.getParseTable())
            {
                codeBuilder.Append(gs + Consta.pile_statend + Environment.NewLine);
            }
            codeBuilder.Append("// ____KAGA_GLOBALVAR_DEAL_ABOVE" + Environment.NewLine + Environment.NewLine);
            foreach(FunctionCell fc in this.symbolMana.getFuncCellVector())
            {
                codeBuilder.Append(fc.getSign(containLeftBrucket: false) + Consta.pile_statend);
            }
            codeBuilder.Append("// ____KAGA_FUNCDECLARATION_DEAL_ABOVE" + Environment.NewLine + Environment.NewLine);
            // 第三阶段：递归下降翻译代码树
            this.pileBuilder = new StringBuilder();
            codeMana.DFS(
                match: (x) => x != null,
                startNode: codeMana.getRoot(),
                func: (x) => pile(x),
                unique: false);
            codeBuilder.Append("// ____KAGA_PILE_DEAL_ABOVE" + Environment.NewLine + Environment.NewLine);
            // 第四阶段：收尾并固化
            codeBuilder.Append("// ____KAGA_EOF" + Environment.NewLine + Environment.NewLine);
            string test = codeBuilder.ToString();
        }

        // 代码翻译方法
        private KagaNode pile(KagaNode dashNode)
        {
            switch (dashNode.atype)
            {

            }


            return dashNode;
        }

        private StringBuilder pileBuilder = null;


        /// <summary>
        /// 工厂方法：获得唯一实例
        /// </summary>
        /// <returns>返回翻译器的唯一实例</returns>
        public static PileParser getInstance()
        {
            return synObject == null ? synObject = new PileParser() : synObject;
        }

        /// <summary>
        /// 私有构造器
        /// </summary>
        private PileParser()
        {
            this.symbolMana = SymbolManager.getInstance();
            this.codeMana = CodeManager.getInstance();
        }

        // 符号管理器
        private SymbolManager symbolMana = null;
        // 代码管理器
        private CodeManager codeMana = null;
        // 唯一实例
        private static PileParser synObject = null;
    }
}
