using System;
using System.Collections.Generic;
using System.Text;


namespace KagaIDE.Module
{
    public class Interpreter
    {
        // 工厂方法
        public static Interpreter getInstance()
        {
            return synObject == null ? synObject = new Interpreter() : synObject;
        }
        
        // 增加一个函数




        
        
        // 私有构造器
        private Interpreter()
        {
            pileParser = PileParser.getInstance();
            symbolMana = SymbolManager.getInstance();
        }
        // 唯一实例
        private static Interpreter synObject = null;
        // 语法匹配器
        private PileParser pileParser = null;
        // 符号管理器
        private SymbolManager symbolMana = null;
    }
}
