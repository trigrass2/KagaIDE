using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Enuming;
using KagaIDE.Module;
using KagaIDE.Struct;

namespace KagaIDE.Module
{
    // 程序前后台交互总控制器
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
            foreach (KagaVar v in fc.paraList)
            {
                args.Add(String.Format("{0}@{1}", v.varname, Consta.parseVarTypeToCType(v.vartype)));
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
        }
        // 唯一实例
        private static KagaController synObject = null;
        // 语法匹配器
        private PileParser sematicer = null;
        // 代码管理器
        private CodeManager codeMana = null;
        // 符号管理器
        private SymbolManager symbolMana = null;
    }
}
