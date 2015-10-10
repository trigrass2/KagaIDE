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
            return this.symbolMana.addFunction(new FunctionCell(fcname, signArgs, signRetType));
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
            np.ForeColor = Color.Blue;
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到代码管理器
            KagaNode codeParentNode = codeMana.getSubTree((x) =>
                x.index == curTree.SelectedNode.Parent.Index &&
                x.depth == curTree.SelectedNode.Parent.Level + 1);
            KagaNode nkn = new KagaNode(
                codeParentNode.nodeName + "__" + NodeType.DEFINE_VARIABLE.ToString(),
                NodeType.DEFINE_VARIABLE,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
            // 为父节点的符号表增加符号
            codeParentNode.symbolTable.symbols.Add(new KagaVar(splitItem[0], Consta.parseCTypeToVarType(splitItem[1])));
        }

        // 检查一个节点是否可以插入变量
        public bool isAbleInsertDefineVar()
        {
            TreeView curTree = this.getActiveTreeView();
            if (curTree.SelectedNode.Parent == null)
            {
                return false;
            }
            KagaNode codeParentNode = codeMana.getSubTree((x) =>
                x.index == curTree.SelectedNode.Parent.Index &&
                x.depth == curTree.SelectedNode.Parent.Level + 1);
            return codeParentNode.isNewBlock;    
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
                KagaNode funNode = codeMana.getSubTree(
                    (x) => x.type == NodeType.PILE__BLOCK__FUNCTION && x.nodeName == tabFunName);
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
            }
            // 展开所有节点
            treeViewPointer.ExpandAll();
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
                    currentParent.ForeColor = Color.Purple;
                    break;
                // 编译控制：右边界
                case NodeType.PILE__BRIGHT_BRUCKET:
                    currentParent = currentParent.Parent;
                    break;
                // 编译控制：插入节点
                case NodeType.PILE__PADDING_CURSOR:
                    currentParent.Nodes.Add(Consta.prefix_frontend + " ");
                    break;
                default:
                    throw new Exception("匹配树类型错误");
            }
            return parseNode;
        }
        private TreeNode currentParent = null;
        private TreeView treeViewPointer = null;
        #endregion



        #region 符号管理器界面相关函数
        // 获取所有函数名字
        public List<string> getAllFunction()
        {
            return this.symbolMana.getFunctionNameList();
        }
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
