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
            this.type = nt;
            this.nodeName = nname;
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
        public string nodeName = "___KAGA__NO__NAME___";
        // 节点类型
        public NodeType type = NodeType.NOP;
        // 节点条件表达式
        public string condition = "___KAGA__NO__CONDITION___";
        // 节点定义变量名
        public string defineVarName = "___KAGA__NO__VARNAME___";
        // 节点For指令开始位置
        public string forBeginIter = "0";
        // 节点For指令终止位置
        public string forEndIter = "-1";
        // 节点For指令步长
        public string forStep = "1";
        // 节点函数调用名称
        public string calling = "___KAGA__NO__FUNCALL___";
        // 节点函数调用参数列表
        public string callingParams = "___KAGA__NO__PARAS____";

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
