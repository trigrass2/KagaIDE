﻿using System;
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
        // 条件语句
        BLOCK__IF,
        // 分支语句
        BLOCK__IF_ELSE,
        // 条件循环
        BLOCK__WHILE,
        // 后置条件循环
        BLOCK__DO_WHILE,
        // 边界循环
        BLOCK__FOR,
        // 编译控制：枚举边界
        PILE__PROCESS_BORDER_DO_NOT_USE,
        // 编译控制：根节点
        PILE__ROOT,
        // 编译控制：函数签名
        PILE__FUNCTION,
        // 编译控制：代码块左边界
        PILE__BLOCK_LEFT_BRUCKET,
        // 编译控制：代码块右边界
        PILE__BLOCK_RIGHT_BRUCKET,
        // 编译控制：待插入光标
        PILE__PADDING_CURSOR
    }

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
}
