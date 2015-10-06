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
        // 工厂方法
        public static KagaController getInstance()
        {
            return synObject == null ? synObject = new KagaController() : synObject;
        }


        


        // 增加一个函数
        public bool addFunction(string fcname, List<string> args, string retType)
        {
            VarType signRetType = Consta.parseToVarType(retType);
            List<KagaVar> signArgs = new List<KagaVar>();
            foreach (string s in args)
            {
                string[] splitItem = s.Split('@');
                signArgs.Add(new KagaVar(splitItem[0], Consta.parseToVarType(splitItem[1])));
            }
            return this.symbolMana.addFunction(new FunctionCell(fcname, signArgs, signRetType));
        }

        // 删除一个函数
        public void deleteFunction(string fcname)
        {
            this.symbolMana.deleteFunction(fcname);
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
