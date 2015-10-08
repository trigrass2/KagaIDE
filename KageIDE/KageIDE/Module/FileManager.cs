using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using KagaIDE.Struct;
using KagaIDE.Module;

namespace KagaIDE.Module
{
    /// <summary>
    /// 文件管理器
    /// </summary>
    public class FileManager
    {
        /// <summary>
        /// 工厂方法：获得唯一实例
        /// </summary>
        /// <returns>返回文件管理器的唯一实例</returns>
        public static FileManager getInstance()
        {
            return synObject == null ? synObject = new FileManager() : synObject;
        }

        /// <summary>
        /// 保存一个项目
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void save(string filePath)
        {

        }

        /// <summary>
        /// 加载一个项目
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void load(string filePath)
        {
            
        }

        /// <summary>
        /// 把一个实例序列化
        /// </summary>
        /// <param name="instance">类的实例</param>
        /// <param name="savePath">保存路径</param>
        private void serialization(object instance, string savePath)
        {
            Stream s = File.Open(savePath, FileMode.Create);
            if (s == null)
            {
                throw new IOException();
            }
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, instance);
            s.Close();
        }

        /// <summary>
        /// 把二进制文件反序列化
        /// </summary>
        /// <param name="loadPath">二进制文件路径</param>
        /// <returns>类的实例</returns>
        private object serialization(string loadPath)
        {
            Stream s = File.Open(loadPath, FileMode.Open);
            if (s == null)
            {
                throw new IOException();
            }
            BinaryFormatter bf = new BinaryFormatter();
            s.Close();
            return bf.Deserialize(s);
        }

        /// <summary>
        /// 私有构造器
        /// </summary>
        private FileManager() { }

        /// <summary>
        /// 唯一实例
        /// </summary>
        private static FileManager synObject = null;
    }
}
