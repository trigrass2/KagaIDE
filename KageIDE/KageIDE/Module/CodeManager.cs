using System;
using System.Collections.Generic;
using System.Text;


namespace KagaIDE.Module
{
    // 控制整棵代码树的类
    public class CodeManager
    {














        // 工厂方法
        public static CodeManager getInstance()
        {
            return synObject == null ? synObject = new CodeManager() : synObject;
        }
        // 私有构造器
        private CodeManager()
        {
            symbolMana = SymbolManager.getInstance();
        }
        // 唯一实例
        private static CodeManager synObject = null;
        // 符号管理器
        private SymbolManager symbolMana = null;
    }
}
