using System;
using System.Collections.Generic;
using System.Text;

namespace KagaIDE.Module
{
    /// <summary>
    /// 代码转换翻译器
    /// </summary>
    public class PileParser
    {
        /// <summary>
        /// 工厂方法：获得唯一实例
        /// </summary>
        /// <returns>返回翻译器的唯一实例</returns>
        public static PileParser getInstance()
        {
            return synObject == null ? synObject = new PileParser() : synObject;
        }

        /// <summary>
        /// 私有构造器
        /// </summary>
        private PileParser() { }

        // 唯一实例
        private static PileParser synObject = null;
    }
}
