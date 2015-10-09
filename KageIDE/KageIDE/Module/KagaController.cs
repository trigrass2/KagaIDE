using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
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

        #endregion




        #region 前台刷新相关函数 

        // 设置MainForm的引用
        public void setMainForm(MainForm mainRef)
        {
            this.mainFormPointer = mainRef;
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
                TreeView codeTreeView = controls[0] as TreeView;
                codeTreeView.Nodes.Clear();
                // 深度优先搜索代码树，绘制窗体
                this.codeMana.DFS(
                    match: (x) => x != null,
                    startNode: funNode, 
                    func: (x) => this.drawGraphTreeView(x), 
                    unique: false);
            }
        }

        // 窗体绘制函数
        private KagaNode drawGraphTreeView(KagaNode node)
        {
            return node;
        }

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
