using System;
using System.Collections.Generic;
using System.Text;

namespace KagaIDE.Enuming
{
    /// <summary>
    /// 枚举类型：代码树节点类型
    /// </summary>
    public enum NodeType
    {
        // 空操作
        NOP,
        // 注释
        NOTE,
        // 开关定义
        USING_SWITCHES,
        // 变量定义
        DEFINE_VARIABLE,
        // 表达式
        EXPRESSION,
        // 中断循环
        BREAK,
        // 返回
        RETURN,
        // 函数调用
        CALL,
        // 单次函数调用
        RUNONCE,
        // 代码片段
        CODEBLOCK,
        // 条件真分支
        BLOCK__IF_TRUE,
        // 条件假分支
        BLOCK__IF_FALSE,
        // 条件循环
        BLOCK__WHILE,
        // 后置条件循环
        BLOCK__DO_WHILE,
        // 边界循环
        BLOCK__FOR,
        // 编译控制：条件语句
        PILE__IF,
        // 编译控制：条件边界
        PILE__ENDIF,
        // 编译控制：枚举边界
        PILE__PROCESS_BORDER_DO_NOT_USE,
        // 编译控制：根节点
        PILE__BLOCK__ROOT,
        // 编译控制：函数签名
        PILE__BLOCK__FUNCTION,
        // 编译控制：代码块右边界
        PILE__BRIGHT_BRUCKET,
        // 编译控制：待插入光标
        PILE__PADDING_CURSOR
    }

    /// <summary>
    /// 枚举类型：基本变量类型
    /// </summary>
    public enum VarType
    {
        VOID,
        INT,
        CHAR,
        LONG,
        FLOAT,
        DOUBLE,
        UNSIGNED_INT,
        UNSIGNED_CHAR
    }

    /// <summary>
    /// 枚举类型：变量操作类型
    /// </summary>
    public enum VarOperateType
    {
        赋值为,
        加上,
        减去,
        乘以,
        除以,
        取余
    }

    /// <summary>
    /// 枚举类型：变量操作数类型
    /// </summary>
    public enum OperandType
    {
        VO_Constant,
        VO_GlobalVar,
        VO_DefVar,
        VO_Random,
        VO_Switch
    }

    /// <summary>
    /// 枚举类型：条件语句操作符
    /// </summary>
    public enum CondOperatorType
    {
        等于,
        不等于,
        大于,
        小于,
        大于等于,
        小于等于
    }
}
