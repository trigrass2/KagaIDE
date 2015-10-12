using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using KagaIDE.Enuming;
using KagaIDE.Module;
using KagaIDE.Struct;

namespace KagaIDE.Module
{
    /// <summary>
    /// 程序前后台交互总控制器
    /// </summary>
    public class KagaController
    {
        #region 函数管理器函数
        // 增加一个函数
        public bool addFunction(string fcname, List<string> args, string retType)
        {
            VarType signRetType = Consta.parseCTypeToVarType(retType);
            List<KagaVar> signArgs = new List<KagaVar>();
            foreach (string s in args)
            {
                string[] splitItem = s.Split('@');
                signArgs.Add(new KagaVar(splitItem[0], Consta.parseCTypeToVarType(splitItem[1])));
            }
            // 提交给符号管理器
            FunctionCell nfc = new FunctionCell(fcname, signArgs, signRetType);
            bool flag = this.symbolMana.addFunction(nfc);
            if (flag == false)
            {
                return false;
            }
            // 提交给代码管理器
            KagaNode rootNode = codeMana.getRoot();
            // 为根节点追加一个fun函数节点
            KagaNode funNode = new KagaNode(fcname, NodeType.PILE__BLOCK__FUNCTION, 1, rootNode.children.Count, rootNode);
            funNode.funBinding = nfc;
            rootNode.children.Add(funNode);
            // 为fun函数节点追加代码块光标节点、代码块右边界
            funNode.children.Add(new KagaNode(fcname + "__PADDING_CURSOR", NodeType.PILE__PADDING_CURSOR, 2, 0, funNode));
            funNode.children.Add(new KagaNode(fcname + "__BRIGHT_BRUCKET", NodeType.PILE__BRIGHT_BRUCKET, 2, 1, funNode));
            return true;
        }

        // 删除一个函数
        public void deleteFunction(string fcname)
        {
            this.symbolMana.deleteFunction(fcname);
        }

        // 获取一个函数的信息
        public void getFunction(string fcname, out List<string> args, out string retType)
        {
            FunctionCell fc = symbolMana.getFunction(fcname);
            if (fc == null)
            {
                args = null;
                retType = null;
                return;
            }
            // 处理返回类型
            retType = Consta.parseVarTypeToCType(fc.returnType);
            // 处理参数列表
            args = new List<string>();
            if (fc.paraList != null)
            {
                foreach (KagaVar v in fc.paraList)
                {
                    args.Add(String.Format("{0}@{1}", v.varname, Consta.parseVarTypeToCType(v.vartype)));
                }
            }
        }

        // 编辑一个函数
        public bool editFunction(string fcname, string newname, List<string> args, string retType)
        {
            List<KagaVar> argsList = new List<KagaVar>();
            foreach (string s in args)
            {
                string[] splitItem = s.Split('@');
                argsList.Add(new KagaVar(splitItem[0], Consta.parseCTypeToVarType(splitItem[1])));
            }
            FunctionCell nfc = new FunctionCell(newname, argsList, Consta.parseCTypeToVarType(retType));
            return symbolMana.editFunction(fcname, nfc);
        }
        #endregion


        #region 全局变量操作函数
        // 增加一个全局变量
        public bool addGlobalVar(string varname, string cvartype)
        {
            // 获取全局符号表，并查找符号是否已存在
            KagaTable globalTable = symbolMana.getGlobalTable();
            KagaVar xvar = globalTable.symbols.Find((x) => x.varname == varname);
            if (xvar != null)
            {
                return false;
            }
            globalTable.symbols.Add(new KagaVar(varname, Consta.parseCTypeToVarType(cvartype)));
            return true;
        }

        // 删除一个全局变量
        public void deleteGlobalVar(string varname)
        {
            // 获取全局符号表
            KagaTable globalTable = symbolMana.getGlobalTable();
            // 移除符号
            globalTable.symbols.RemoveAll((x) => x.varname == varname);
        }

        // 取得全局变量表
        public List<string> getGlobalVar()
        {
            // 获取全局符号表
            KagaTable globalTable = symbolMana.getGlobalTable();
            // 处理符号表
            List<string> globalVector = new List<string>();
            foreach (KagaVar kv in globalTable.symbols)
            {
                globalVector.Add(String.Format("{0}@{1}", kv.varname, Consta.parseVarTypeToCType(kv.vartype)));
            }
            return globalVector;
        }

        // 重置一份新的全局变量表
        public void setNewGlobalVar(List<string> newlist)
        {
            KagaTable gt = symbolMana.getGlobalTable();
            gt.symbols.Clear();
            foreach (string s in newlist)
            {
                string[] splitItem = s.Split('@');
                gt.symbols.Add(new KagaVar(splitItem[0], Consta.parseCTypeToVarType(splitItem[1])));
            }
        }

        // 获得开关描述向量
        public List<string> getSwitchDescriptionVector()
        {
            return symbolMana.getSwitchVector();
        }

        // 用新开关描述向量更新开关表
        public void updateSwitchDescriptionVector(List<string> nlist)
        {
            if (nlist != null)
            {
                symbolMana.setSwitchVector(nlist);
            }
        }
        #endregion


        #region 主窗口指令操作函数
        // 获得操作节点
        public KagaNode getOpNode()
        {
            TreeView curTree = this.getActiveTreeView();
            KagaNode codeFunNode = codeMana.getFunRoot(this.mainFormPointer.tabControl1.SelectedTab.Text);
            KagaNode codeParentNode = codeMana.getSubTree((x) =>
                x.index == curTree.SelectedNode.Parent.Index &&
                x.depth == curTree.SelectedNode.Parent.Level + 1, codeFunNode);
            // 函数根节点
            if (curTree.SelectedNode.Parent.Level == 0)
            {
                return codeFunNode;
            }
            return codeParentNode;
        }
        
        // 获得宏定义
        public string getMarcos()
        {
            return symbolMana.getMarcoContainer();
        }

        // 设置宏定义
        public void setMarcos(string newMarcos)
        {
            symbolMana.setMarcoContainer(newMarcos);
        }

        // 操作：定义变量
        public void dash_defineVariable(string arg)
        {
            string[] splitItem = arg.Split('@');
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np = new TreeNode(
                String.Format("{0} 变量定义：{1} ({2})", Consta.prefix_frontend, splitItem[0], splitItem[1]));
            np.ForeColor = Consta.getColoring(NodeType.DEFINE_VARIABLE);
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到代码管理器
            KagaNode codeParentNode = this.getOpNode();
            KagaNode nkn = new KagaNode(
                codeParentNode.nodeName + "__" + NodeType.DEFINE_VARIABLE.ToString(),
                NodeType.DEFINE_VARIABLE,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.defineVarName = splitItem[0];
            nkn.defineVarType = Consta.parseCTypeToVarType(splitItem[1]);
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
            // 为父节点的符号表增加符号
            codeParentNode.symbolTable.symbols.Add(new KagaVar(splitItem[0], Consta.parseCTypeToVarType(splitItem[1])));
        }

        // 操作：开关操作
        public void dash_switchOperate(int sid, bool state)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np = new TreeNode(
                String.Format("{0} 开关操作：[{1}:{2}] 状态设置为 {3}", Consta.prefix_frontend,
                sid.ToString(), symbolMana.getSwitchVector()[sid], state ? "打开" : "关闭"));
            np.ForeColor = Consta.getColoring(NodeType.USING_SWITCHES);
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到代码管理器
            KagaNode codeParentNode = this.getOpNode();
            KagaNode nkn = new KagaNode(
                codeParentNode.nodeName + "__" + NodeType.USING_SWITCHES.ToString(),
                NodeType.USING_SWITCHES,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.switchFlag = state;
            nkn.switchId = sid;
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }

        // 检查一个节点是否可以插入变量
        public bool isAbleInsertDefineVar()
        {
            TreeView curTree = this.getActiveTreeView();
            if (curTree.SelectedNode.Parent == null)
            {
                return false;
            }
            KagaNode codeParentNode = this.getOpNode();
            return codeParentNode.isNewBlock;
        }

        // 指令：变量操作
        public void dash_varOperator(OperandType Lt, string lop, VarOperateType vt, OperandType Rt, string rop1, string rop2 = null)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np;
            switch (Rt)
            {
                case OperandType.VO_Constant:
                    np = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 常数{3}", Consta.prefix_frontend,
                        lop, vt.ToString(), rop1));
                    break;
                case OperandType.VO_GlobalVar:
                    np = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 全局变量{3}的值", Consta.prefix_frontend,
                        lop, vt.ToString(), rop1));
                    break;
                case OperandType.VO_DefVar:
                    np = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 局部变量{3}的值", Consta.prefix_frontend,
                        lop, vt.ToString(), rop1));
                    break;
                case OperandType.VO_Random:
                    np = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 随机数范围[{3},{4}]", Consta.prefix_frontend,
                        lop, vt.ToString(), rop1, rop2));
                    break;
                default:
                    throw new Exception("操作类型为空错误");
            }
            np.ForeColor = Consta.getColoring(NodeType.EXPRESSION);
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到代码管理器
            KagaNode codeParentNode = this.getOpNode();
            KagaNode nkn = new KagaNode(
                codeParentNode.nodeName + "__" + NodeType.EXPRESSION.ToString(),
                NodeType.EXPRESSION,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.varOperateType = vt;
            nkn.operand1 = lop;
            nkn.operand2 = rop1;
            nkn.operand3 = rop2;
            nkn.LopType = Lt;
            nkn.RopType = Rt;
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }

        // 指令：插入注释
        public void dash_notation(string nota)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np = new TreeNode(String.Format("{0} 注释：{1} {2}",
                Consta.prefix_frontend, nota.Split('\n')[0],
                nota.Split('\n').Length > 1 ? "..." : ""));
            np.ForeColor = Consta.getColoring(NodeType.NOTE);
            np.ToolTipText = nota;
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到代码管理器
            KagaNode codeParentNode = this.getOpNode();
            KagaNode nkn = new KagaNode(
                codeParentNode.nodeName + "__" + NodeType.NOTE.ToString(),
                NodeType.NOTE,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.notation = nota;
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }

        // 指令：插入代码片段
        public void dash_codeblock(string myCode)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np = new TreeNode(String.Format("{0} 代码片段：{1} {2}",
                Consta.prefix_frontend, myCode.Split('\n')[0], 
                myCode.Split('\n').Length > 1 ? "..." : ""));
            np.ToolTipText = myCode;
            np.ForeColor = Consta.getColoring(NodeType.CODEBLOCK);
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到代码管理器
            KagaNode codeParentNode = this.getOpNode();
            KagaNode nkn = new KagaNode(
                codeParentNode.nodeName + "__" + NodeType.CODEBLOCK.ToString(),
                NodeType.CODEBLOCK,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.myCode = myCode;
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }
        #endregion

        #region 符号管理器界面相关函数
        // 获取所有函数名字
        public List<string> getAllFunction()
        {
            return this.symbolMana.getFunctionNameList();
        }
        #endregion

        #region 前台刷新相关函数 
        // 设置MainForm的引用
        public void setMainForm(MainForm mainRef)
        {
            this.mainFormPointer = mainRef;
        }

        // 获取当前活跃的TabPage里的编辑器
        public TreeView getActiveTreeView()
        {
            TabPage p = this.mainFormPointer.tabControl1.SelectedTab;
            Control[] controlItem = p.Controls.Find("codeTreeView", true);
            return controlItem != null ? (TreeView)controlItem[0] : null;
        }

        // 更新编辑器内容
        public void refreshAll()
        {
            // 遍历所有Tab
            foreach (TabPage p in this.mainFormPointer.tabControl1.TabPages)
            {
                // 取得函数在代码树上的根节点
                string tabFunName = p.Text;
                KagaNode funNode = codeMana.getFunRoot(tabFunName);
                // 清空当前tab页
                Control[] controls = p.Controls.Find("codeTreeView", true);
                if (controls.Length < 1)
                {
                    throw new Exception("Tab刷新时遇到空错误");
                }
                this.treeViewPointer = controls[0] as TreeView;
                this.treeViewPointer.Nodes.Clear();
                // 深度优先搜索代码树，绘制窗体
                this.codeMana.DFS(
                    match: (x) => x != null,
                    startNode: funNode, 
                    func: (x) => this.drawTreeContext(x), 
                    unique: false);
                // 展开所有节点
                treeViewPointer.ExpandAll();
            }
        }

        // 窗体绘制函数
        private KagaNode drawTreeContext(KagaNode parseNode)
        {
            switch (parseNode.type)
            {
                // 代码块：函数签名
                case NodeType.PILE__BLOCK__FUNCTION:
                    currentParent = this.treeViewPointer.Nodes.Add(
                        String.Format("{0} {1}", Consta.prefix_frontend, parseNode.funBinding.getSign()));
                    currentParent.ForeColor = Consta.getColoring(parseNode.type);
                    break;
                // 编译控制：右边界
                case NodeType.PILE__BRIGHT_BRUCKET:
                    currentParent = currentParent.Parent;
                    break;
                // 编译控制：插入节点
                case NodeType.PILE__PADDING_CURSOR:
                    currentParent.Nodes.Add(Consta.prefix_frontend +
                        "                                                                                            ");
                    break;
                // 操作：变量定义
                case NodeType.DEFINE_VARIABLE:
                    TreeNode tdefvar = new TreeNode(String.Format("{0} 变量定义：{1} ({2})", Consta.prefix_frontend,
                        parseNode.defineVarName, Consta.parseVarTypeToCType(parseNode.defineVarType)));
                    tdefvar.ForeColor = Consta.getColoring(parseNode.type);
                    currentParent.Nodes.Add(tdefvar);
                    break;
                // 操作：开关操作
                case NodeType.USING_SWITCHES:
                    TreeNode tuseswt = new TreeNode(
                        String.Format("{0} 开关操作：[{1}:{2}] 状态设置为 {3}", Consta.prefix_frontend,
                        parseNode.switchId.ToString(), symbolMana.getSwitchVector()[parseNode.switchId],
                        parseNode.switchFlag ? "打开" : "关闭"));
                    tuseswt.ForeColor = Consta.getColoring(parseNode.type);
                    currentParent.Nodes.Add(tuseswt);
                    break;
                // 操作：变量操作
                case NodeType.EXPRESSION:
                    TreeNode vexpnode;
                    if (parseNode.RopType == OperandType.VO_GlobalVar)
                    {
                        vexpnode = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 全局变量{3}的值", Consta.prefix_frontend,
                        parseNode.operand1, parseNode.varOperateType.ToString(), parseNode.operand2));
                    }
                    else if (parseNode.RopType == OperandType.VO_DefVar)
                    {
                        vexpnode = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 局部变量{3}的值", Consta.prefix_frontend,
                        parseNode.operand1, parseNode.varOperateType.ToString(), parseNode.operand2));
                    }
                    else if (parseNode.RopType == OperandType.VO_Random)
                    {
                        vexpnode = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 随机数范围[{3},{4}]", Consta.prefix_frontend,
                        parseNode.operand1, parseNode.varOperateType.ToString(), parseNode.operand2, parseNode.operand3));
                    }
                    else
                    {
                        vexpnode = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 常数{3}", Consta.prefix_frontend,
                        parseNode.operand1, parseNode.varOperateType.ToString(), parseNode.operand2));
                    }
                    vexpnode.ForeColor = Consta.getColoring(parseNode.type);
                    currentParent.Nodes.Add(vexpnode);
                    break;
                // 操作：注释
                case NodeType.NOTE:
                    TreeNode notanode = new TreeNode(
                        String.Format("{0} 注释：{1} {2}", Consta.prefix_frontend,
                        parseNode.notation.Split('\n')[0], parseNode.notation.Split('\n').Length > 1 ? "..." : ""));
                    notanode.ToolTipText = parseNode.notation;
                    notanode.ForeColor = Consta.getColoring(parseNode.type);
                    currentParent.Nodes.Add(notanode);
                    break;
                // 操作：代码片段
                case NodeType.CODEBLOCK:
                    TreeNode ncodeblock = new TreeNode(
                        String.Format("{0} 代码片段：{1} {2}", Consta.prefix_frontend, 
                        parseNode.myCode.Split('\n')[0], parseNode.myCode.Split('\n').Length > 1 ? "..." : ""));
                    ncodeblock.ToolTipText = parseNode.myCode;
                    ncodeblock.ForeColor = Consta.getColoring(parseNode.type);
                    currentParent.Nodes.Add(ncodeblock);
                    break;
                default:
                    throw new Exception("匹配树类型错误");
            }
            return parseNode;
        }
        private TreeNode currentParent = null;
        private TreeView treeViewPointer = null;
        #endregion


        // 工厂方法
        public static KagaController getInstance()
        {
            return synObject == null ? synObject = new KagaController() : synObject;
        }
        // 私有构造器
        private KagaController()
        {
            sematicer = PileParser.getInstance();
            symbolMana = SymbolManager.getInstance();
            codeMana = CodeManager.getInstance();
            fileMana = FileManager.getInstance();
        }
        // 唯一实例
        private static KagaController synObject = null;
        // 语法匹配器
        private PileParser sematicer = null;
        // 代码管理器
        private CodeManager codeMana = null;
        // 符号管理器
        private SymbolManager symbolMana = null;
        // 文件管理器
        private FileManager fileMana = null;
        // 主窗体指针
        private MainForm mainFormPointer = null;
    }
}
