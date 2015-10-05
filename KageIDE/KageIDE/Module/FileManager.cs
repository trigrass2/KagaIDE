using System;
using System.Collections.Generic;
using System.Text;

namespace KagaIDE.Module
{
    // 文件管理器
    public class FileManager
    {
        // 工厂方法
        public static FileManager getInstance()
        {
            return synObject == null ? synObject = new FileManager() : synObject;
        }

        // 保存一个项目
        public void save(Delegate t)
        {

        }

        // 打开一个项目
        public void load(Delegate t)
        {
            
        }

        // 私有构造器
        private FileManager() { }

        // 唯一实例
        private static FileManager synObject = null;
    }
}
