using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Struct;
using KagaIDE.Enuming;

namespace KagaIDE.Module
{
    // 符号管理器
    public class SymbolManager
    {
        // 工厂
        public static SymbolManager getInstance()
        {
            return synObject == null ? synObject = new SymbolManager() : synObject;
        }
        
        // 清空符号表
        public void clear()
        {
            tableContainer.Clear();
            callfunContainer.Clear();
            marcoContainer.Clear();
        }

        // 私有的构造器
        private SymbolManager()
        {
            marcoContainer = new List<string>();
            tableContainer = new List<KagaTable>();
            callfunContainer = new List<FunctionCell>();
        }

        // 添加一个函数到符号表
        public bool addFunction(FunctionCell fc)
        {
            if (fc == null || this.isExistFunction(fc.callname))
            {
                return false;
            }
            this.callfunContainer.Add(fc);
            return true;
        }

        // 查找一个函数是否存在
        public bool isExistFunction(string fname)
        {
            return this.callfunContainer.Find((x) => x.callname == fname) != null;
        }

        // 删除一个函数
        public bool deleteFunction(string fname)
        {
            return this.callfunContainer.RemoveAll((x) => x.callname == fname) != 0;
        }

        // 获取一个函数的引用
        public FunctionCell getFunction(string fname)
        {
            return this.callfunContainer.Find((x) => x.callname == fname);
        }

        // 修改一个函数的签名
        public bool editFunction(string fname, FunctionCell nfc)
        {
            FunctionCell ofc = this.getFunction(fname);
            if (ofc == null)
            {
                return false;
            }
            return ofc.editSign(nfc) != null;
        }

        // 获得函数名列表
        public List<string> getFunctionNameList()
        {
            List<string> fnList = new List<string>();
            foreach (FunctionCell fc in this.callfunContainer)
            {
                fnList.Add(fc.callname);
            }
            return fnList;
        }

        // 获得编译时函数签名列表
        public List<string> getPiledFunctionSignList()
        {
            List<string> pfList = new List<string>();
            foreach (FunctionCell fc in this.callfunContainer)
            {
                pfList.Add(fc.getSign());
            }
            return pfList;
        }

        // 添加一张符号表
        public void addSymbolTable(KagaTable kt)
        {
            this.tableContainer.Add(kt);
        }

        // 获取符号表
        public List<KagaTable> getSymbolTableList()
        {
            return this.tableContainer;
        }
        
        // 清空表
        public void clear(int flag = 0)
        {
            switch (flag)
            {
                case 1:
                    this.tableContainer.Clear();
                    break;
                case 2:
                    this.callfunContainer.Clear();
                    break;
                case 3:
                    this.marcoContainer.Clear();
                    break;
                default:
                    this.callfunContainer.Clear();
                    this.tableContainer.Clear();
                    this.marcoContainer.Clear();
                    break;
            }
        }

        // 宏定义向量
        private List<string> marcoContainer = null;
        // 符号表向量
        private List<KagaTable> tableContainer = null;
        // 函数名向量
        private List<FunctionCell> callfunContainer = null;
        // 唯一实例
        private static SymbolManager synObject = null;
    }
}
