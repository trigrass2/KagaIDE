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
        /// <returns>操作成功与否</returns>
        public bool save(string filePath)
        {
            bool sflag = true;
            try
            {
                this.serialization(CodeManager.getInstance(), filePath);
            }
            catch
            {
                sflag = false;
            }
            return sflag;
        }

        /// <summary>
        /// 加载一个项目
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>读取的代码管理器</returns>
        public CodeManager load(string filePath)
        {
            return (CodeManager)this.unserialization(filePath);
        }

        /// <summary>
        /// 把一个实例序列化
        /// </summary>
        /// <param name="instance">类的实例</param>
        /// <param name="savePath">保存路径</param>
        /// <returns>操作成功与否</returns>
        private bool serialization(object instance, string savePath)
        {
            try
            {
                Stream myStream = File.Open(savePath, FileMode.Create);
                if (myStream == null)
                {
                    throw new IOException();
                }
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(myStream, instance);
                myStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// 把二进制文件反序列化
        /// </summary>
        /// <param name="loadPath">二进制文件路径</param>
        /// <returns>类的实例</returns>
        private object unserialization(string loadPath)
        {
            try
            {
                Stream s = File.Open(loadPath, FileMode.Open);
                if (s == null)
                {
                    throw new IOException();
                }
                BinaryFormatter bf = new BinaryFormatter();
                object ob = bf.Deserialize(s);
                s.Close();
                return ob;
            }
            catch
            {
                return null;
            }
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
