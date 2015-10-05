using System;
using System.Collections.Generic;
using System.Text;
using KageIDE.Enuming;

namespace KageIDE
{
    public class KagaNode
    {
        // 构造器
        public KagaNode() { }



        // 节点深度
        int depth = 0;
        // 节点类型
        NodeType type = NodeType.NOP;
        // 特殊尾部节点（括号）标记
        bool isBrucketNode = false;
        // 空节点标记位
        bool isNilNode = false;
        // 多路子树
        public List<KagaNode> subNode = null;
    }
}
