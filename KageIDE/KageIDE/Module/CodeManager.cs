using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Enuming;

namespace KagaIDE.Module
{
    /// <summary>
    /// 控制整棵代码树的类
    /// </summary>
    [Serializable]
    public class CodeManager
    {
        /// <summary>
        /// 初始化代码管理器，获得一棵带有root和main函数块的树
        /// </summary>
        public void initCodeTree()
        {
            // 建立一个根节点
            this.parseTree = new KagaNode("TreeRoot", NodeType.PILE__BLOCK__ROOT, 0, 0, null);
            // 为根节点追加一个main函数节点
            KagaNode mainFunNode = new KagaNode("main", NodeType.PILE__BLOCK__FUNCTION, 1, 0, this.parseTree);
            FunctionCell mainFunCell = new FunctionCell("main", null, VarType.VOID);
            mainFunNode.funBinding = mainFunCell;
            this.parseTree.children.Add(mainFunNode);
            // 为main函数节点追加代码块光标节点、代码块右边界
            mainFunNode.children.Add(new KagaNode("main___PADDING_CURSOR", NodeType.PILE__PADDING_CURSOR, 2, 0, mainFunNode));
            mainFunNode.children.Add(new KagaNode("main___BRIGHT_BRUCKET", NodeType.PILE__BRIGHT_BRUCKET, 2, 1, mainFunNode));
            // 追加main函数到符号管理器
            symbolMana.addFunction(mainFunCell);
        }
        
        /// <summary>
        /// 获得代码树的根节点
        /// </summary>
        /// <returns>语法树根节点</returns>
        public KagaNode getRoot()
        {
            return this.parseTree;
        }

        /// <summary>
        /// 获得指定函数的子树根节点
        /// </summary>
        /// <param name="callname">函数名称</param>
        /// <returns>函数子树的根节点</returns>
        public KagaNode getFunRoot(string callname)
        {
            KagaNode rootNode = this.parseTree;
            foreach (KagaNode kn in rootNode.children)
            {
                if (kn.anodeName == callname)
                {
                    return kn;
                }
            }
            return null;
        }

        /// <summary>
        /// 获得满足指定条件的广度优先遍历得到的第一个根节点的子树
        /// </summary>
        /// <param name="match">匹配条件</param>
        /// <returns>满足条件的子树根节点</returns>
        public KagaNode getSubTree(Predicate<KagaNode> match, KagaNode startNode)
        {
            List<KagaNode> res = this.BFS(match, startNode, null, true);
            return res != null ? res[0] : null;
        }        

        /// <summary>
        /// 在指定的深度和广度处插入一个节点
        /// </summary>
        /// <param name="dep">插入深度</param>
        /// <param name="bre">插入广度</param>
        /// <param name="obj">待插入节点</param>
        /// <returns>插入是否成功</returns>
        public bool insertNode(int dep, int bre, KagaNode obj)
        {
            // 插入深度必须大于0
            if (dep < 1)
            {
                return false;
            }
            if (obj.parent == null || obj.parent.children.Count < bre)
            {
                return false;
            }
            // 更新自己的信息
            obj.depth = dep;
            obj.index = bre;
            // 接下来找姐妹中自己的排位并插入
            obj.parent.children.Insert(bre, obj);
            // 更新子树信息
            this.update(obj.parent, false);
            return true;
        }

        /// <summary>
        /// 在指定的深度和广度处插入一个节点，并生成一个语句块
        /// </summary>
        /// <param name="dep">插入深度</param>
        /// <param name="bre">插入广度</param>
        /// <param name="obj">待插入节点</param>
        /// <returns>插入是否成功</returns>
        public bool insertBlockNode(int dep, int bre, KagaNode obj)
        {
            // 生成它的孩子节点，追加代码块光标节点、代码块右边界
            obj.children.Add(new KagaNode(obj.anodeName + "___PADDING_CURSOR", NodeType.PILE__PADDING_CURSOR, dep + 1, 0, obj));
            obj.children.Add(new KagaNode(obj.anodeName + "___BRIGHT_BRUCKET", NodeType.PILE__BRIGHT_BRUCKET, dep + 1, 1, obj));
            // 插入到代码树
            return this.insertNode(dep, bre, obj);
        }

        /// <summary>
        /// 把指定的深度和广度处的节点移除掉，并销毁掉它的符号表
        /// </summary>
        /// <param name="dep">待删除节点的深度</param>
        /// <param name="bre">待删除节点的广度</param>
        /// <returns>删除是否成功</returns>
        public bool deleteNode(int dep, int bre)
        {
            // 删除深度必须大于0
            if (dep < 1)
            {
                return false;
            }
            // 首先找到自己和双亲
            KagaNode selfNode = this.getSubTree((t) => t.depth == dep && t.index == bre, this.parseTree);
            KagaNode father = selfNode.parent;
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
        
        /// <summary>
        /// 更新某个节点的子树的信息
        /// </summary>
        /// <param name="subTreeRoot">子树根节点</param>
        /// <param name="disposeFlag">操作标记：true - 删除操作; false - 更新操作</param>
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
                unique: false,
                startNode: this.parseTree
            );
        }

        /// <summary>
        /// 更新某节点所有直接孩子的深度和广度信息
        /// </summary>
        /// <param name="father">待处理节点</param>
        /// <returns>返回参数自身</returns>
        private KagaNode updateChildrenInfo(KagaNode father)
        {
            for (int i = 0; i < father.children.Count; i++)
            {
                father.children[i].index = i;
                father.children[i].depth = father.depth + 1;
                if (father.children[i].isNewBlock == true)
                {
                    father.children[i].symbolTable.depth = father.depth + 1;
                }
            }
            return father;
        }

        /// <summary>
        /// 更新某节点所有直接孩子的符号表
        /// </summary>
        /// <param name="father">待处理节点</param>
        /// <returns>返回参数自身</returns>
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

        /// <summary>
        /// 广度优先遍历语法树，对满足条件的节点执行委托func，并返回他们
        /// </summary>
        /// <param name="match">节点匹配条件</param>
        /// <param name="startNode">搜索开始的节点</param>
        /// <param name="func">满足条件的节点处理函数</param>
        /// <param name="unique">是否命中一个节点就结束</param>
        /// <returns>符合要求的节点向量，若没有符合的节点就返回null</returns>
        public List<KagaNode> BFS(
            Predicate<KagaNode> match,
            KagaNode startNode,
            Func<KagaNode, KagaNode> func = null,
            bool unique = true)
        {
            // 如果节点为空就直接退出
            if (startNode == null)
            {
                return null;
            }
            List<KagaNode> resultContainer = new List<KagaNode>();
            Queue<KagaNode> pendingList = new Queue<KagaNode>();
            pendingList.Enqueue(startNode);
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

        /// <summary>
        /// 深度优先遍历语法树，对满足条件的节点执行委托func，并返回他们
        /// </summary>
        /// <param name="match">节点匹配条件</param>
        /// <param name="startNode">搜索开始的节点</param>
        /// <param name="func">满足条件的节点处理函数</param>
        /// <param name="unique">是否命中一个节点就结束</param>
        /// <returns>符合要求的节点向量，若没有符合的节点就返回null</returns>
        public List<KagaNode> DFS(
            Predicate<KagaNode> match,
            KagaNode startNode,
            Func<KagaNode, KagaNode> func = null,
            bool unique = true)
        {
            // 如果节点为空就直接退出
            if (startNode == null)
            {
                return null;
            }
            List<KagaNode> resultContainer = new List<KagaNode>();
            Stack<KagaNode> iStack = new Stack<KagaNode>();
            iStack.Push(startNode);
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
                // 反向追加孩子节点到栈里
                if (currentNode.children != null)
                {
                    for (int i = currentNode.children.Count - 1; i >= 0; i--)
                    {
                        iStack.Push(currentNode.children[i]);
                    }
                }
            }
            // 没有匹配项就返回null
            return resultContainer.Count > 0 ? resultContainer : null; 
        }

        /// <summary>
        /// 工厂方法：获得唯一实例
        /// </summary>
        /// <returns>返回代码管理器的唯一实例</returns>
        public static CodeManager getInstance()
        {
            return synObject == null ? synObject = new CodeManager() : synObject;
        }

        /// <summary>
        /// 私有构造器
        /// </summary>
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
