using System;
using System.Collections.Generic;
using System.Text;

namespace KagaIDE.Module
{
    // 解释器
    public class PileParser
    {
        // 工厂方法
        public static PileParser getInstance()
        {
            return synObject == null ? synObject = new PileParser() : synObject;
        }

        // 私有构造器
        private PileParser() { }

        // 唯一实例
        private static PileParser synObject = null;
    }
}
