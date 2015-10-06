using System;
using System.Collections.Generic;
using System.Text;

namespace KagaIDE.Module
{
    // 程序前后台交互总控制器
    public class KagaController
    {
        // 工厂方法
        public static KagaController getInstance()
        {
            return synObject == null ? synObject = new KagaController() : synObject;
        }
        


        // 增加一个函数
        public bool addFunction(string fcname, List<string> args, string retType)
        {

            return true;
        }

        
        
        // 私有构造器
        private KagaController()
        {
            sematicer = Interpreter.getInstance();
            symbolMana = SymbolManager.getInstance();
        }
        // 唯一实例
        private static KagaController synObject = null;
        // 语法匹配器
        private Interpreter sematicer = null;
        // 符号管理器
        private SymbolManager symbolMana = null;
    }
}
