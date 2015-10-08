using System;
using System.Collections.Generic;
using System.Text;

namespace KagaIDE
{
    /// <summary>
    /// 可以使用BasicInputForm的窗体接口
    /// </summary>
    public interface IBasicInputForm
    {
        /// <summary>
        /// 接收BasicInputForm传递参数的父窗体的公共方法
        /// </summary>
        /// <param name="passer">接收变量</param>
        void passByBasicSubForm(string passer);
    }
}
