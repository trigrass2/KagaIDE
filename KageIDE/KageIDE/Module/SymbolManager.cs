using System;
using System.Collections.Generic;
using System.Text;
using KagaIDE.Struct;
using KagaIDE.Enuming;

namespace KagaIDE.Module
{
    /// <summary>
    /// 符号管理器
    /// </summary>
    [Serializable]
    public class SymbolManager
    {
        /// <summary>
        /// 工厂方法：获得唯一实例
        /// </summary>
        /// <returns>返回符号管理器的唯一实例</returns>
        public static SymbolManager getInstance()
        {
            return synObject == null ? synObject = new SymbolManager() : synObject;
        }

        /// <summary>
        /// 添加一个函数到符号表
        /// </summary>
        /// <param name="fc">待添加函数单元</param>
        /// <returns>操作成功与否</returns>
        public bool addFunction(FunctionCell fc)
        {
            if (fc == null || this.isExistFunction(fc.callname))
            {
                return false;
            }
            this.callfunContainer.Add(fc);
            return true;
        }

        /// <summary>
        /// 查找一个函数是否存在
        /// </summary>
        /// <param name="fname">查找函数的名称</param>
        /// <returns>操作成功与否</returns>
        public bool isExistFunction(string fname)
        {
            return this.callfunContainer.Find((x) => x.callname == fname) != null;
        }

        /// <summary>
        /// 删除一个函数
        /// </summary>
        /// <param name="fname">待删除函数的名称</param>
        /// <returns>操作成功与否</returns>
        public bool deleteFunction(string fname)
        {
            return this.callfunContainer.RemoveAll((x) => x.callname == fname) != 0;
        }

        /// <summary>
        /// 获取一个函数的引用
        /// </summary>
        /// <param name="fname">查找函数的名称</param>
        /// <returns>该函数的函数单元</returns>
        public FunctionCell getFunction(string fname)
        {
            return this.callfunContainer.Find((x) => x.callname == fname);
        }

        /// <summary>
        /// 修改一个函数的签名
        /// </summary>
        /// <param name="fname">查找函数的名称</param>
        /// <param name="nfc">待复制的新函数签名的函数单元</param>
        /// <returns>操作成功与否</returns>
        public bool editFunction(string fname, FunctionCell nfc)
        {
            // 更新
            FunctionCell ofc = this.getFunction(fname);
            if (ofc == null)
            {
                return false;
            }
            // 复制一个备份等待需要的回滚
            FunctionCell backupOfc = new FunctionCell("_BACKUP_NODE");
            backupOfc.editSign(ofc);
            ofc.editSign(nfc);
            // 检查是否有重复
            bool rollbackFlag = false;
            for (int i = 0; i < this.callfunContainer.Count; i++)
            {
                for (int j = i + 1; j < this.callfunContainer.Count; j++)
                {
                    if (this.callfunContainer[i].callname == this.callfunContainer[j].callname)
                    {
                        rollbackFlag = true;
                        // 弹出两层循环
                        i = j = this.callfunContainer.Count + 1;
                    }
                }
            }
            // 如果重名就要回滚操作
            if (rollbackFlag == true)
            {
                ofc.editSign(backupOfc);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获得所有函数名列表
        /// </summary>
        /// <returns>所有函数名字符串的列表</returns>
        public List<string> getFunctionNameList()
        {
            List<string> fnList = new List<string>();
            foreach (FunctionCell fc in this.callfunContainer)
            {
                fnList.Add(fc.callname);
            }
            return fnList;
        }

        /// <summary>
        /// 获得编译时函数签名列表
        /// </summary>
        /// <returns>所有函数编译时签名符串的列表</returns>
        public List<string> getPiledFunctionSignList()
        {
            List<string> pfList = new List<string>();
            foreach (FunctionCell fc in this.callfunContainer)
            {
                pfList.Add(fc.getSign());
            }
            return pfList;
        }

        /// <summary>
        /// 获得宏定义
        /// </summary>
        /// <returns>所有宏定义字符串的列表</returns>
        public string getMarcoContainer()
        {
            return this.marcoContainer;
        }

        /// <summary>
        /// 修改宏定义
        /// </summary>
        /// <param name="newMarcoContainer">待替换掉目前宏定义容器的新宏定义字符串</param>
        public void setMarcoContainer(string newMarcoContainer)
        {
            this.marcoContainer = newMarcoContainer;
        }

        /// <summary>
        /// 修改一个开关的名称
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="nname"></param>
        public void editSwitchName(int sid, string nname)
        {
            this.switchContainer[sid] = nname;
        }

        /// <summary>
        /// 添加一张符号表
        /// </summary>
        /// <param name="kt">待添加符号表</param>
        public void addSymbolTable(KagaTable kt)
        {
            this.tableContainer.Add(kt);
        }

        /// <summary>
        /// 获取所有符号表
        /// </summary>
        /// <returns>包含所有符号表引用的列表</returns>
        public List<KagaTable> getSymbolTableList()
        {
            return this.tableContainer;
        }
        
        /// <summary>
        /// 删除一张符号表
        /// </summary>
        /// <param name="belonging">待删除符号表的属节点</param>
        /// <returns>操作成功与否</returns>
        public bool deleteSymbolTable(KagaNode belonging)
        {
            KagaTable kt;
            if ((kt = this.tableContainer.Find((x) => x.belong == belonging)) != null)
            {
                this.tableContainer.Remove(kt);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取全局变量符号表
        /// </summary>
        /// <returns>全局变量符号表的引用</returns>
        public KagaTable getGlobalTable()
        {
            return this.tableContainer.Count > 0 ? this.tableContainer[0] : null;
        }

        /// <summary>
        /// 获取开关描述向量
        /// </summary>
        /// <returns>开关描述向量</returns>
        public List<string> getSwitchVector()
        {
            return this.switchContainer;
        }

        /// <summary>
        /// 更新整个开关描述向量
        /// </summary>
        public void setSwitchVector(List<string> nsv)
        {
            this.switchContainer = nsv;
        }

        /// <summary>
        /// 清空指定的符号管理器单元
        /// </summary>
        /// <param name="flag">待清除的单元：0 - 所有符号；1 - 符号表容器；2 - 函数容器；3 - 宏定义容器</param>
        public void clear(int flag = 0)
        {
            switch (flag)
            {
                case 1:
                    this.tableContainer.Clear();
                    break;
                case 2:
                    this.callfunContainer.Clear();
                    break;
                case 3:
                    this.marcoContainer = "";
                    break;
                default:
                    this.callfunContainer.Clear();
                    this.tableContainer.Clear();
                    this.marcoContainer = "";
                    break;
            }
        }

        // 私有的构造器
        private SymbolManager()
        {
            this.marcoContainer = "";
            this.tableContainer = new List<KagaTable>();
            this.callfunContainer = new List<FunctionCell>();
            this.switchContainer = new List<string>();
            for (int i = 0; i < Consta.switch_max; i++)
            {
                this.switchContainer.Add("");
            }
        }
        // 宏定义语句块
        private string marcoContainer = null;
        // 开关组向量
        private List<string> switchContainer = null;
        // 符号表向量
        private List<KagaTable> tableContainer = null;
        // 函数名向量
        private List<FunctionCell> callfunContainer = null;
        // 唯一实例
        private static SymbolManager synObject = null;
    }
}
