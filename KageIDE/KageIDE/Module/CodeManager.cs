using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Enuming;

namespace KagaIDE.Module
{
    // 控制整棵代码树的类
    [Serializable]
    public class CodeManager
    {
        // 初始化
        public void initCodeTree()
        {
            // 建立一个根节点
            this.parseTree = new KagaNode("TreeRoot", NodeType.PILE__BLOCK__ROOT, 0, 0, null);
            // 为根节点追加一个main函数节点
            KagaNode mainFunNode = new KagaNode("main", NodeType.PILE__BLOCK__FUNCTION, 1, 0, this.parseTree);
            this.parseTree.children.Add(mainFunNode);
            // 为main函数节点追加代码块左边界、光标节点、代码块右边界
            mainFunNode.children.Add(new KagaNode("main__BLEFT_BRUCKET", NodeType.PILE__BLEFT_BRUCKET, 2, 0, mainFunNode));
            mainFunNode.children.Add(new KagaNode("main__PADDING_CURSOR", NodeType.PILE__PADDING_CURSOR, 2, 1, mainFunNode));
            mainFunNode.children.Add(new KagaNode("main__BRIGHT_BRUCKET", NodeType.PILE__BRIGHT_BRUCKET, 2, 2, mainFunNode));
        }

        // 直接返回根节点
        public KagaNode getRoot()
        {
            return this.parseTree;
        }

        // 返回满足指定条件的广度优先遍历得到的第一个根节点的子树
        public KagaNode getSubTree(Predicate<KagaNode> match)
        {
            List<KagaNode> res = this.BFS(match, null, true);
            return res != null ? res[0] : null;
        }        

        // 在指定的深度和广度处插入一个节点
        public bool insertNode(int dep, int bre, KagaNode obj)
        {
            // 插入深度必须大于0
            if (dep < 1)
            {
                return false;
            }
            // 首先找到父亲节点
            KagaNode father = this.getSubTree((t) => t.depth == dep - 1);
            if (father == null || father.children.Count < bre)
            {
                return false;
            }
            // 更新自己的信息
            obj.depth = dep;
            obj.index = bre;
            // 接下来找姐妹中自己的排位并插入
            father.children.Insert(bre, obj);
            // 更新子树信息
            this.update(father, false);
            return true;
        }

        // 把指定的深度和广度处的节点移除掉，并销毁掉它的符号表
        public bool deleteNode(int dep, int bre)
        {
            // 删除深度必须大于0
            if (dep < 1)
            {
                return false;
            }
            // 首先找到父亲节点
            KagaNode father = this.getSubTree((t) => t.depth == dep - 1);
            if (father == null || father.children.Count < bre)
            {
                return false;
            }
            // 移除自己
            father.children.RemoveAt(bre);
            // 更新姐妹和姐妹后代的信息
            this.update(father, true);
            return true;
        }

        // 更新某个节点的子树的信息
        private void update(KagaNode subTreeRoot, bool disposeFlag)
        {
            this.DFS(
                match:
                (x) => 
                {
                    // 删除子树时
                    if (disposeFlag == true)
                    {
                        return x.isNewBlock == true;
                    }
                    // 更新信息时
                    else
                    {
                        return x != null;
                    }
                },
                func:
                (x) => 
                {
                    // 消除符号表
                    if (disposeFlag == true)
                    {
                        return this.disposeChildrenSymbolTable(x);
                    }
                    // 更新信息
                    else
                    {
                        return this.updateChildrenInfo(x);
                    }
                },
                unique: false
            );
        }

        // 更新某节点所有直接孩子的深度和广度信息
        private KagaNode updateChildrenInfo(KagaNode father)
        {
            for (int i = 0; i < father.children.Count; i++)
            {
                father.children[i].index = i;
                father.children[i].depth = father.depth + 1;
            }
            return father;
        }

        // 更新某节点所有直接孩子的符号表
        private KagaNode disposeChildrenSymbolTable(KagaNode father)
        {
            for (int i = 0; i < father.children.Count; i++)
            {
                if (father.children[i].symbolTable != null)
                {
                    symbolMana.deleteSymbolTable(father.children[i]);
                    father.children[i].symbolTable = null;
                }
            }
            return father;
        }

        // 广度优先遍历语法树，对满足条件的节点进行func，并返回他们
        private List<KagaNode> BFS(Predicate<KagaNode> match, Func<KagaNode, KagaNode> func = null, bool unique = true)
        {
            List<KagaNode> resultContainer = new List<KagaNode>();
            Queue<KagaNode> pendingList = new Queue<KagaNode>();
            pendingList.Enqueue(this.parseTree);
            while (pendingList.Count > 0)
            {
                KagaNode currentNode = pendingList.Dequeue();
                // 判断当前节点的条件
                if (match(currentNode) == true)
                {
                    // 如果有func，就进行
                    if (func != null)
                    {
                        currentNode = func(currentNode);
                    }
                    // 添加到输出缓冲
                    resultContainer.Add(currentNode);
                    // 如果条件保证唯一就返回
                    if (unique == true)
                    {
                        break;
                    }
                }
                // 节点没有后继结点就迭代
                if (currentNode.children != null)
                {
                    // 把后继节点放到队列里
                    foreach (KagaNode kn in currentNode.children)
                    {
                        pendingList.Enqueue(kn);
                    }
                }
            }
            // 如果没有满足条件的节点就返回null
            return resultContainer.Count > 0 ? resultContainer : null;
        }

        // 深度优先遍历语法树，对满足条件的节点进行func，并返回他们
        private List<KagaNode> DFS(Predicate<KagaNode> match, Func<KagaNode, KagaNode> func = null, bool unique = true)
        {
            List<KagaNode> resultContainer = new List<KagaNode>();
            Stack<KagaNode> iStack = new Stack<KagaNode>();
            iStack.Push(this.parseTree);
            KagaNode currentNode;
            while (iStack.Count > 0)
            {
                currentNode = iStack.Pop();
                // 测试当前节点
                if (match(currentNode) == true)
                {
                    // 如果有func，就进行
                    if (func != null)
                    {
                        currentNode = func(currentNode);
                    }
                    // 添加到输出缓冲
                    resultContainer.Add(currentNode);
                    // 如果保证唯一就退出
                    if (unique == true)
                    {
                        break;
                    }
                }
                // 追加孩子节点到栈里
                if (currentNode.children != null)
                {
                    foreach (KagaNode kn in currentNode.children)
                    {
                        iStack.Push(kn);
                    }
                }
            }
            // 没有匹配项就返回null
            return resultContainer.Count > 0 ? resultContainer : null; 
        }

        // 工厂方法
        public static CodeManager getInstance()
        {
            return synObject == null ? synObject = new CodeManager() : synObject;
        }
        // 私有构造器
        private CodeManager()
        {
            symbolMana = SymbolManager.getInstance();
            this.initCodeTree();
        }
        // 唯一实例
        private static CodeManager synObject = null;
        // 符号管理器
        private SymbolManager symbolMana = null;
        // 语法树根节点
        private KagaNode parseTree = null;
    }
}
