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
            codeBuilder.Append("char " + Consta.switch_name + "[" + Consta.switch_max + "];" + Environment.NewLine);
            codeBuilder.Append("// ____KAGA_SWITCHES_DEAL_ABOVE" + Environment.NewLine + Environment.NewLine);
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
            // 处理缩进
            for (int i = 0; i < this.indentCount; i++)
            {
                pileBuilder.Append(" ");
            }
            // 翻译
            switch (dashNode.atype)
            {
                // 语句块：函数
                case NodeType.PILE__BLOCK__FUNCTION:
                    // 头部
                    pileBuilder.Append(dashNode.funBinding.getSign(containLeftBrucket: true));
                    pileBuilder.Append(Environment.NewLine);
                    // 作用域局部变量
                    foreach (string fvar in dashNode.symbolTable.getParseTable())
                    {
                        pileBuilder.Append(fvar + Consta.pile_statend + Environment.NewLine);
                    }
                    pileBuilder.Append("// __KgFuncSignOver" + Environment.NewLine);
                    // 进
                    indWait();
                    break;
                // 语句块：条件真分支
                case NodeType.BLOCK__IF_TRUE:
                    pileBuilder.Append("if (");
                    if (dashNode.isConditionEx == true)
                    {
                        pileBuilder.Append(dashNode.conditionEx);
                    }
                    else
                    {
                        switch (dashNode.LopType)
                        {
                            case OperandType.VO_Switch:
                                pileBuilder.Append(Consta.switch_name + "[" + dashNode.operand1 + "]");
                                break;
                            default:
                                pileBuilder.Append(dashNode.operand1);
                                break;
                        }
                        switch (dashNode.condOperateType)
                        {
                            case CondOperatorType.等于:
                                pileBuilder.Append(" == ");
                                break;
                            case CondOperatorType.不等于:
                                pileBuilder.Append(" != ");
                                break;
                            case CondOperatorType.大于:
                                pileBuilder.Append(" > ");
                                break;
                            case CondOperatorType.大于等于:
                                pileBuilder.Append(" >= ");
                                break;
                            case CondOperatorType.小于:
                                pileBuilder.Append(" < ");
                                break;
                            case CondOperatorType.小于等于:
                                pileBuilder.Append(" <= ");
                                break;
                            default:
                                pileBuilder.Append(" == ");
                                break;
                        }
                        switch (dashNode.RopType)
                        {
                            case OperandType.VO_Switch:
                                pileBuilder.Append(dashNode.operand2 == "关闭" ? "0" : "1");
                                break;
                            default:
                                pileBuilder.Append(dashNode.operand1);
                                break;
                        }
                        pileBuilder.Append(dashNode.operand2);
                    }
                    pileBuilder.Append(") {" + Environment.NewLine);
                    // 进
                    indWait();
                    break;
                // 语句块：条件假分支
                case NodeType.BLOCK__IF_FALSE:
                    pileBuilder.Append("else {");
                    // 进
                    indWait();
                    break;
                // 语句块：条件循环
                case NodeType.BLOCK__WHILE:
                    if (dashNode.isCondPostCheck == true)
                    {
                        pileBuilder.Append("do {");
                    }
                    else
                    {
                        pileBuilder.Append("while (");
                        switch (dashNode.condLoopType)
                        {
                            case CondLoopType.CLT_EXPRESSION:
                                pileBuilder.Append(dashNode.conditionEx);
                                break;
                            case CondLoopType.CLT_SWITCH:
                                pileBuilder.Append(Consta.switch_name + "[");
                                pileBuilder.Append(dashNode.conditionEx + "]");
                                break;
                            case CondLoopType.CLT_FOREVER:
                            default:
                                pileBuilder.Append("true");
                                break;
                        }
                        pileBuilder.Append(") {");
                    }
                    // 进
                    indWait();
                    break;
                // 语句块：次数循环
                case NodeType.BLOCK__FOR:
                    pileBuilder.Append("for (int __KAGA_FOR_ITER = ");
                    pileBuilder.Append(dashNode.forBeginIter);
                    pileBuilder.Append("; __KAGA_FOR_ITER < ");
                    pileBuilder.Append(dashNode.forEndIter);
                    pileBuilder.Append("; __KAGA_FOR_ITER += ");
                    pileBuilder.Append(dashNode.forStep);
                    pileBuilder.Append(") {");
                    // 进
                    indWait();
                    break;
                // 指令：注释
                case NodeType.NOTE:
                    pileBuilder.Append("/*" + Environment.NewLine);
                    pileBuilder.Append(dashNode.notation);
                    pileBuilder.Append("*/" + Environment.NewLine);
                    break;
                // 指令：代码片段
                case NodeType.CODEBLOCK:
                    pileBuilder.Append("// __CODEBLOCK_BEGIN_" + Environment.NewLine);
                    pileBuilder.Append(dashNode.myCode);
                    pileBuilder.Append("// __CODEBLOCK_END_" + Environment.NewLine);
                    break;
                // 指令：中断循环
                case NodeType.BREAK:
                    pileBuilder.Append("break;" + Environment.NewLine);
                    break;
                // 指令：函数退出
                case NodeType.RETURN:
                    switch (dashNode.funretType)
                    {
                        case OperandType.VO_Constant:
                        case OperandType.VO_DefVar:
                        case OperandType.VO_GlobalVar:
                            pileBuilder.Append("return " + dashNode.funretOperand + Consta.pile_statend + Environment.NewLine);
                            break;
                        default:
                            pileBuilder.Append("return;" + Environment.NewLine);
                            break;
                    }
                    break;
                // 指令：变量定义
                case NodeType.DEFINE_VARIABLE:
                    pileBuilder.Append(Consta.parseVarTypeToCType(dashNode.defineVarType));
                    pileBuilder.Append(" " + dashNode.defineVarName + Consta.pile_statend + Environment.NewLine);
                    break;
                // 指令：变量操作
                case NodeType.EXPRESSION:
                    pileBuilder.Append(dashNode.operand1);
                    switch (dashNode.varOperateType)
                    {
                        case VarOperateType.赋值为:
                            pileBuilder.Append(" = ");
                            break;
                        case VarOperateType.加上:
                            pileBuilder.Append(" += ");
                            break;
                        case VarOperateType.减去:
                            pileBuilder.Append(" -= ");
                            break;
                        case VarOperateType.乘以:
                            pileBuilder.Append(" *= ");
                            break;
                        case VarOperateType.除以:
                            pileBuilder.Append(" /= ");
                            break;
                        case VarOperateType.取余:
                            pileBuilder.Append(" %= ");
                            break;
                        default:
                            pileBuilder.Append(" = ");
                            break;
                    }
                    switch (dashNode.RopType)
                    {
                        case OperandType.VO_Random:
                            break;
                        default:
                            pileBuilder.Append(dashNode.operand2);
                            break;
                    }
                    pileBuilder.Append(Consta.pile_statend + Environment.NewLine);
                    break;
                // 指令：开关操作
                case NodeType.USING_SWITCHES:
                    pileBuilder.Append(Consta.switch_name + "[" + dashNode.switchId + "]");
                    pileBuilder.Append(" = ");
                    pileBuilder.Append(dashNode.switchFlag == true ? "1" : "0");
                    pileBuilder.Append(";" + Environment.NewLine);
                    break;
                // 编译控制：右括弧
                case NodeType.PILE__BRIGHT_BRUCKET:
                    // 缩
                    indSignal();
                    // 右括
                    pileBuilder.Append("}");
                    break;
                // 编译控制：插入节点
                case NodeType.PILE__PADDING_CURSOR:
                    break;
                // 编译控制：条件语句体
                case NodeType.PILE__IF:
                case NodeType.PILE__ENDIF:
                    break;
                default:
                    break;
            }
            return dashNode;
        }

        private void indSignal(int offset = 1)
        {
            this.indentCount -= offset * Consta.pile_indent;
        }

        private void indWait(int offset = 1)
        {
            this.indentCount += offset * Consta.pile_indent;
        }

        private StringBuilder pileBuilder = null;
        private int indentCount = 0;

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
