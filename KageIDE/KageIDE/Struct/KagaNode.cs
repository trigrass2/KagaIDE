﻿using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Enuming;
using KagaIDE.Struct;

namespace KagaIDE
{
    /// <summary>
    /// 基础数据结构：代码树节点
    /// </summary>
    [Serializable]
    public class KagaNode
    {
        /// <summary>
        /// 代码树节点构造器
        /// </summary>
        /// <param name="nname">节点名字</param>
        /// <param name="nt">节点类型</param>
        /// <param name="nodeDepth">节点深度</param>
        /// <param name="nodeIndex">节点广度</param>
        /// <param name="paraParent">节点双亲</param>
        public KagaNode(string nname, NodeType nt, int nodeDepth, int nodeIndex, KagaNode paraParent) 
        {
            // 初始化节点信息
            this.atype = nt;
            this.anodeName = nname;
            this.depth = nodeDepth;
            this.parent = paraParent;
            this.children = new List<KagaNode>();
            // 如果是代码块，那就要生成符号表
            if (nt.ToString().Contains(Consta.prefix_block))
            {
                this.isNewBlock = true;
                this.symbolTable = new KagaTable(this.depth, this);
            }
        }

        // 姐妹中的排位
        public int index = 0;
        // 节点深度
        public int depth = 0;

        // 错误位
        public bool errorBit = false;
        // 错误码
        public string errorCode = "___KAGA__NO__ERC___";

        // 节点名字
        public string anodeName = "___KAGA__NO__NAME___";
        // 节点类型
        public NodeType atype = NodeType.NOP;

        // 节点条件表达式标记
        public bool isConditionEx = false;
        // 节点含有分歧语句标记
        public bool isContainElse = false;
        // 节点条件表达式
        public string conditionEx = Consta.const_none;
        // 节点条件循环类型
        public CondLoopType condLoopType = CondLoopType.CLT_FOREVER;
        // 节点循环条件是否后置
        public bool isCondPostCheck = false;
        // 节点定义变量名
        public string defineVarName = Consta.const_none;
        // 节点定义变量类型
        public VarType defineVarType = VarType.VOID;
        // 节点For指令开始位置
        public string forBeginIter = "0";
        // 节点For指令终止位置
        public string forEndIter = "-1";
        // 节点For指令步长
        public string forStep = "1";
        // 节点For指令开始类型
        public ForLoopType forBeginType = ForLoopType.FLT_CONSTANT;
        // 节点For指令终止类型
        public ForLoopType forEndType = ForLoopType.FLT_CONSTANT;
        // 节点For指令步长类型
        public ForLoopType forStepType = ForLoopType.FLT_CONSTANT;
        // 节点For简单循环标记
        public bool isSimpleFor = true;
        // 节点开关序号
        public int switchId = -1;
        // 节点开关状态
        public bool switchFlag = false;
        // 节点条件判断类型
        public CondOperatorType condOperateType = CondOperatorType.等于;
        // 节点变量操作类型
        public VarOperateType varOperateType = VarOperateType.赋值为;
        // 节点左操作数
        public string operand1 = Consta.const_none;
        // 节点右操作数1
        public string operand2 = Consta.const_none;
        // 节点右操作数2
        public string operand3 = Consta.const_none;
        // 节点左操作数类型
        public OperandType LopType = OperandType.VO_GlobalVar;
        // 节点右操作数类型
        public OperandType RopType = OperandType.VO_Constant;
        // 注释内容
        public string notation = Consta.const_none;
        // 代码块
        public string myCode = Consta.const_none;
        // 节点函数调用名称
        public string calling = Consta.const_none;
        // 节点函数调用参数列表
        public string callingParams = Consta.const_none;
        // 节点函数返回选择类型
        public OperandType funretType = OperandType.VO_VOID;
        // 节点函数返回引用值
        public string funretOperand = Consta.const_none;
        // 节点附加值
        public string tag = Consta.const_none;

        // 函数绑定
        public FunctionCell funBinding = null;
        // 是否产生一个新的编译语句块
        public bool isNewBlock = false;
        // 符号表指针
        public KagaTable symbolTable = null;
        // 双亲指针
        public KagaNode parent = null;
        // 多路子树
        public List<KagaNode> children = null;
    }
}
