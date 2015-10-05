using System;
using System.Collections.Generic;
using System.Text;

namespace KagaIDE.Enuming
{
    public enum NodeType
    {
        // 空操作
        NOP,
        // 注释
        NOTE,
        // 开关定义
        DEFINE_SWITCH,
        // 变量定义
        DEFINE_VARIABLE,
        // 表达式
        EXPRESSION,
        // 条件语句
        IF,
        // 分支语句
        IF_ELSE,
        // 条件循环
        WHILE,
        // 后置条件循环
        DO_WHILE,
        // 边界循环
        FOR,
        // 遍历循环
        FOREACH,
        // 中断循环
        BREAK,
        // 返回
        RETURN,
        // 函数调用
        CALL,
        // 单次函数调用
        RUNONCE,
        // 代码片段
        CODEBLOCK
    }
}
