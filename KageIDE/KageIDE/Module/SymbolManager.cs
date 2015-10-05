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
        }

        // 符号表向量
        public List<KagaTable> tableContainer = null;

        // 函数名向量
        public List<string> callfunContainer = null;

        // 私有的构造器
        private SymbolManager()
        {
            tableContainer = new List<KagaTable>();
            callfunContainer = new List<string>();
        }

        // 唯一实例
        private static SymbolManager synObject = null;
    }
}
