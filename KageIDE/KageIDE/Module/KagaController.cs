using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using KagaIDE.Enuming;
using KagaIDE.Module;
using KagaIDE.Struct;

namespace KagaIDE.Module
{
    /// <summary>
    /// 程序前后台交互总控制器
    /// </summary>
    public class KagaController
    {
        #region 文件操作相关函数
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="savePath">带保存文件名的路径</param>
        /// <returns>操作成功与否</returns>
        public bool menuSave(string savePath)
        {
            return fileMana.save(savePath);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="loadPath">读取文件的路径</param>
        /// <returns>操作成功与否</returns>
        public bool menuLoad(string loadPath)
        {
            CodeManager ncmana = fileMana.load(loadPath);
            if (ncmana == null)
            {
                return false;
            }
            this.setMana(ncmana, ncmana.getSymbolRef());
            pileMana.refreshRef();
            return true;
        }
        #endregion

        #region 编译相关的函数
        public void dash()
        {
            pileMana.startDash();
        }

        #endregion

        #region 函数管理相关函数
        /// <summary>
        /// 增加一个函数
        /// </summary>
        /// <param name="fcname">函数名</param>
        /// <param name="args">参数列表，以"name@type"形式的字符串表示</param>
        /// <param name="retType">返回类型</param>
        /// <returns>操作成功与否</returns>
        public bool addFunction(string fcname, List<string> args, string retType)
        {
            VarType signRetType = Consta.parseCTypeToVarType(retType);
            List<KagaVar> signArgs = new List<KagaVar>();
            foreach (string s in args)
            {
                string[] splitItem = s.Split('@');
                signArgs.Add(new KagaVar(splitItem[0], Consta.parseCTypeToVarType(splitItem[1])));
            }
            // 提交给符号管理器
            FunctionCell nfc = new FunctionCell(fcname, signArgs, signRetType);
            bool flag = this.symbolMana.addFunction(nfc);
            if (flag == false)
            {
                return false;
            }
            // 提交给代码管理器
            KagaNode rootNode = codeMana.getRoot();
            // 为根节点追加一个fun函数节点
            KagaNode funNode = new KagaNode(fcname, NodeType.PILE__BLOCK__FUNCTION, 1, rootNode.children.Count, rootNode);
            funNode.funBinding = nfc;
            rootNode.children.Add(funNode);
            // 为fun函数节点追加代码块光标节点、代码块右边界
            funNode.children.Add(new KagaNode(fcname + "___PADDING_CURSOR", NodeType.PILE__PADDING_CURSOR, 2, 0, funNode));
            funNode.children.Add(new KagaNode(fcname + "___BRIGHT_BRUCKET", NodeType.PILE__BRIGHT_BRUCKET, 2, 1, funNode));
            return true;
        }

        /// <summary>
        /// 删除一个函数
        /// </summary>
        /// <param name="fcname">函数名</param>
        public void deleteFunction(string fcname)
        {
            this.symbolMana.deleteFunction(fcname);
        }

        /// <summary>
        /// 获取一个函数的信息
        /// </summary>
        /// <param name="fcname">函数名</param>
        /// <param name="args">（传出）参数列表，以"name@type"形式的字符串表示</param>
        /// <param name="retType">（传出）返回类型</param>
        public void getFunction(string fcname, out List<string> args, out string retType)
        {
            FunctionCell fc = symbolMana.getFunction(fcname);
            if (fc == null)
            {
                args = null;
                retType = null;
                return;
            }
            // 处理返回类型
            retType = Consta.parseVarTypeToCType(fc.returnType);
            // 处理参数列表
            args = new List<string>();
            if (fc.paraList != null)
            {
                foreach (KagaVar v in fc.paraList)
                {
                    args.Add(String.Format("{0}@{1}", v.varname, Consta.parseVarTypeToCType(v.vartype)));
                }
            }
        }

        /// <summary>
        /// 编辑一个函数
        /// </summary>
        /// <param name="fcname">函数名</param>
        /// <param name="newname">新函数名</param>
        /// <param name="args">参数列表，以"name@type"形式的字符串表示</param>
        /// <param name="retType">返回类型</param>
        /// <returns>操作成功与否</returns>
        public bool editFunction(string fcname, string newname, List<string> args, string retType)
        {
            List<KagaVar> argsList = new List<KagaVar>();
            foreach (string s in args)
            {
                string[] splitItem = s.Split('@');
                argsList.Add(new KagaVar(splitItem[0], Consta.parseCTypeToVarType(splitItem[1])));
            }
            FunctionCell nfc = new FunctionCell(newname, argsList, Consta.parseCTypeToVarType(retType));
            return symbolMana.editFunction(fcname, nfc);
        }

        /// <summary>
        /// 获取所有函数名字
        /// </summary>
        /// <returns></returns>
        public List<string> getAllFunction()
        {
            return this.symbolMana.getFunctionNameList();
        }
        #endregion

        #region 全局变量操作函数
        /// <summary>
        /// 增加一个全局变量
        /// </summary>
        /// <param name="varname">变量名</param>
        /// <param name="cvartype">变量类型</param>
        /// <returns>操作成功与否</returns>
        public bool addGlobalVar(string varname, string cvartype)
        {
            // 获取全局符号表，并查找符号是否已存在
            KagaTable globalTable = symbolMana.getGlobalTable();
            KagaVar xvar = globalTable.symbols.Find((x) => x.varname == varname);
            if (xvar != null)
            {
                return false;
            }
            globalTable.symbols.Add(new KagaVar(varname, Consta.parseCTypeToVarType(cvartype)));
            return true;
        }

        /// <summary>
        /// 删除一个全局变量
        /// </summary>
        /// <param name="varname">待删除的变量名</param>
        public void deleteGlobalVar(string varname)
        {
            // 获取全局符号表
            KagaTable globalTable = symbolMana.getGlobalTable();
            // 移除符号
            globalTable.symbols.RemoveAll((x) => x.varname == varname);
        }

        /// <summary>
        /// 取得全局变量表
        /// </summary>
        /// <returns>一个字符串向量，每个以"name@type"的命名</returns>
        public List<string> getGlobalVar()
        {
            // 获取全局符号表
            KagaTable globalTable = symbolMana.getGlobalTable();
            // 处理符号表
            List<string> globalVector = new List<string>();
            foreach (KagaVar kv in globalTable.symbols)
            {
                globalVector.Add(String.Format("{0}@{1}", kv.varname, Consta.parseVarTypeToCType(kv.vartype)));
            }
            return globalVector;
        }

        /// <summary>
        /// 重置一份新的全局变量表
        /// </summary>
        /// <param name="newlist">准备替换掉原全局变量表的字符串向量，每个元素以"name@type"格式命名</param>
        public void setNewGlobalVar(List<string> newlist)
        {
            KagaTable gt = symbolMana.getGlobalTable();
            gt.symbols.Clear();
            foreach (string s in newlist)
            {
                string[] splitItem = s.Split('@');
                gt.symbols.Add(new KagaVar(splitItem[0], Consta.parseCTypeToVarType(splitItem[1])));
            }
        }

        /// <summary>
        /// 获得开关描述向量
        /// </summary>
        /// <returns>一个向量，每个元素是对应下标的开关的描述文本</returns>
        public List<string> getSwitchDescriptionVector()
        {
            return symbolMana.getSwitchVector();
        }

        /// <summary>
        /// 用新开关描述向量更新开关表
        /// </summary>
        /// <param name="nlist">准备替换掉原开关描述向量的新向量</param>
        public void updateSwitchDescriptionVector(List<string> nlist)
        {
            if (nlist != null)
            {
                symbolMana.setSwitchVector(nlist);
            }
        }
        #endregion

        #region 主窗口指令操作函数
        /// <summary>
        /// 递归查找当前前端选中节点所对应的后台代码树节点
        /// </summary>
        /// <param name="offset">偏移量：指示后台代码树节点的index值应该减去多少</param>
        /// <returns>前台激活节点在后台代码树上的对应节点</returns>
        private KagaNode recursiveFindOpNode(int offset)
        {
            TreeView curTree = this.getActiveTreeView();
            TreeNode curNode = curTree.SelectedNode;
            KagaNode codefindNode = codeMana.getFunRoot(this.mainFormPointer.tabControl1.SelectedTab.Text);
            // 从内到外把前端节点相对位置压栈
            Stack<KeyValuePair<int, int>> findStack = new Stack<KeyValuePair<int, int>>();
            while (curNode.Parent != null)
            {
                findStack.Push(new KeyValuePair<int, int>(curNode.Index - offset, curNode.Level + 1));
                curNode = curNode.Parent;
                offset = 0;
            }
            // 从外到内出栈，搜索代码树上的节点
            while (findStack.Count != 0)
            {
                KeyValuePair<int, int> kvp = findStack.Pop();
                codefindNode = codeMana.getChild((x) => x.index == kvp.Key && x.depth == kvp.Value, codefindNode);
            }
            return codefindNode;
        }

        /// <summary>
        /// 获得当前前端操作节点所对应后台代码树上节点的双亲节点
        /// </summary>
        /// <param name="offset">偏移量：指示后台代码树节点的index值应该减去多少</param>
        /// <returns>前台激活节点在后台代码树上对应节点的双亲节点</returns>
        public KagaNode getOpNodeParent(int offset)
        {
            return this.recursiveFindOpNode(offset).parent;
        }

        /// <summary>
        /// 检查一个节点是否可以插入变量定义
        /// </summary>
        /// <returns>是否可以插入变量定义语句</returns>
        public bool isAbleInsertDefineVar()
        {
            TreeView curTree = this.getActiveTreeView();
            if (curTree.SelectedNode.Parent == null)
            {
                return false;
            }
            KagaNode codeParentNode = this.getOpNodeParent(0);
            return codeParentNode.isNewBlock;
        }

        /// <summary>
        /// 获得宏定义
        /// </summary>
        /// <returns>宏定义的字符串</returns>
        public string getMarcos()
        {
            return symbolMana.getMarcoContainer();
        }

        /// <summary>
        /// 设置宏定义
        /// </summary>
        /// <param name="newMarcos">待设置的包含新的宏定义的字符串</param>
        public void setMarcos(string newMarcos)
        {
            symbolMana.setMarcoContainer(newMarcos);
        }

        /// <summary>
        /// 删除节点操作
        /// </summary>
        /// <returns>操作成功与否</returns>
        public bool deleteCodeNode()
        {
            TreeView curTree = this.getActiveTreeView();
            TreeNode curNode = curTree.SelectedNode;
            if (curNode.Text[0] == ':')
            {
                return false;
            }
            KagaNode pendingNode = this.recursiveFindOpNode(0);
            if (pendingNode.atype == NodeType.PILE__PADDING_CURSOR)
            {
                return false;
            }
            codeMana.deleteNode(pendingNode);
            refreshAll();
            return true;
        }

        /// <summary>
        /// 操作：定义变量 
        /// </summary>
        /// <param name="arg">定义的变量：以"name@type"格式传递</param>
        public void dash_defineVariable(string arg)
        {
            string[] splitItem = arg.Split('@');
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np = new TreeNode(
                String.Format("{0} 变量定义：{1} ({2})", Consta.prefix_frontend, splitItem[0], splitItem[1]));
            np.ForeColor = Consta.getColoring(NodeType.DEFINE_VARIABLE);
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到代码管理器
            KagaNode codeParentNode = this.getOpNodeParent(1);
            KagaNode nkn = new KagaNode(
                codeParentNode.anodeName + "___" + NodeType.DEFINE_VARIABLE.ToString(),
                NodeType.DEFINE_VARIABLE,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.defineVarName = splitItem[0];
            nkn.defineVarType = Consta.parseCTypeToVarType(splitItem[1]);
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
            // 为父节点的符号表增加符号
            codeParentNode.symbolTable.symbols.Add(new KagaVar(splitItem[0], Consta.parseCTypeToVarType(splitItem[1])));
        }

        /// <summary>
        /// 操作：开关操作
        /// </summary>
        /// <param name="sid">开关号</param>
        /// <param name="state">开关要设置成的状态</param>
        public void dash_switchOperate(int sid, bool state)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np = new TreeNode(
                String.Format("{0} 开关操作：[{1}:{2}] 状态设置为 {3}", Consta.prefix_frontend,
                sid.ToString(), symbolMana.getSwitchVector()[sid], state ? "打开" : "关闭"));
            np.ForeColor = Consta.getColoring(NodeType.USING_SWITCHES);
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到代码管理器
            KagaNode codeParentNode = this.getOpNodeParent(1);
            KagaNode nkn = new KagaNode(
                codeParentNode.anodeName + "___" + NodeType.USING_SWITCHES.ToString(),
                NodeType.USING_SWITCHES,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.switchFlag = state;
            nkn.switchId = sid;
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }
        
        /// <summary>
        /// 指令：变量操作
        /// </summary>
        /// <param name="Lt">左操作数类型</param>
        /// <param name="lop">左操作数</param>
        /// <param name="vt">操作符类型</param>
        /// <param name="Rt">右操作数类型</param>
        /// <param name="rop1">右操作数1</param>
        /// <param name="rop2">右操作数2</param>
        public void dash_varOperator(OperandType Lt, string lop, VarOperateType vt, OperandType Rt, string rop1, string rop2 = null)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np;
            switch (Rt)
            {
                case OperandType.VO_Constant:
                    np = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 常数{3}", Consta.prefix_frontend,
                        lop, vt.ToString(), rop1));
                    break;
                case OperandType.VO_GlobalVar:
                    np = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 全局变量{3}的值", Consta.prefix_frontend,
                        lop, vt.ToString(), rop1));
                    break;
                case OperandType.VO_DefVar:
                    np = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 局部变量{3}的值", Consta.prefix_frontend,
                        lop, vt.ToString(), rop1));
                    break;
                case OperandType.VO_Random:
                    np = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 随机数范围[{3},{4}]", Consta.prefix_frontend,
                        lop, vt.ToString(), rop1, rop2));
                    break;
                default:
                    throw new Exception("操作类型为空错误");
            }
            np.ForeColor = Consta.getColoring(NodeType.EXPRESSION);
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到代码管理器
            KagaNode codeParentNode = this.getOpNodeParent(1);
            KagaNode nkn = new KagaNode(
                codeParentNode.anodeName + "___" + NodeType.EXPRESSION.ToString(),
                NodeType.EXPRESSION,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.varOperateType = vt;
            nkn.operand1 = lop;
            nkn.operand2 = rop1;
            nkn.operand3 = rop2;
            nkn.LopType = Lt;
            nkn.RopType = Rt;
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }

        /// <summary>
        /// 指令：条件分支
        /// </summary>
        /// <param name="isCondEx">是否自带表达式</param>
        /// <param name="condEx">表达式</param>
        /// <param name="containElse">是否有分支</param>
        /// <param name="Lt">左操作数类型</param>
        /// <param name="lop">左操作数</param>
        /// <param name="condt">比较符号类型</param>
        /// <param name="Rt">右操作数类型</param>
        /// <param name="rop">右操作数</param>
        public void dash_condition(
            bool isCondEx, string condEx, bool containElse,
            OperandType Lt, string lop, CondOperatorType condt, OperandType Rt, string rop)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            StringBuilder sb = new StringBuilder();
            sb.Append(Consta.prefix_frontCond + " 当");
            if (isCondEx)
            {
                sb.Append("表达式：" + condEx + " 为真");
            }
            else
            {
                switch (Lt)
                {
                    case OperandType.VO_GlobalVar:
                        sb.Append("全局变量");
                        break;
                    case OperandType.VO_DefVar:
                        sb.Append("局部变量");
                        break;
                    case OperandType.VO_Switch:
                        sb.Append("开关");
                        break;
                    default:
                        break;
                }
                sb.Append(lop);
                sb.Append(" " + condt.ToString() + " ");
                switch (Rt)
                {
                    case OperandType.VO_Constant:
                        sb.Append("常数");
                        break;
                    case OperandType.VO_GlobalVar:
                        sb.Append("全局变量");
                        break;
                    case OperandType.VO_DefVar:
                        sb.Append("局部变量");
                        break;
                    case OperandType.VO_Switch:
                        break;
                    default:
                        break;
                }
                sb.Append(rop);
            }
            sb.Append("时：");
            TreeNode fp = new TreeNode(String.Format("{0} 条件分支", Consta.prefix_frontend));
            fp.ForeColor = Consta.getColoring(NodeType.PILE__IF);
            TreeNode np = new TreeNode(sb.ToString());
            np.ForeColor = Consta.getColoring(NodeType.BLOCK__IF_TRUE);
            // padding节点要追加给np
            np.Nodes.Add(Consta.prefix_frontend +
                        "                                            ");
            fp.Nodes.Add(np);
            // 如果有分支
            if (containElse)
            {
                TreeNode ep = new TreeNode(Consta.prefix_frontCond + " 除此以外的情况：");
                ep.Nodes.Add(Consta.prefix_frontend +
                        "                                            ");
                ep.ForeColor = Consta.getColoring(NodeType.BLOCK__IF_TRUE);
                fp.Nodes.Add(ep);
            }
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, fp);
            curTree.ExpandAll();
            // 把修改提交到代码管理器
            KagaNode codeParentNode = this.getOpNodeParent(1);
            KagaNode nkn = new KagaNode(
                codeParentNode.anodeName + "___" + NodeType.PILE__IF.ToString(),
                NodeType.PILE__IF,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            // 判断是否需要插入分支语句
            KagaNode trueNode = new KagaNode(
                nkn.anodeName + "___" + NodeType.BLOCK__IF_TRUE.ToString(),
                NodeType.BLOCK__IF_TRUE, nkn.depth + 1, 0, nkn);
            trueNode.isConditionEx = isCondEx;
            trueNode.isContainElse = containElse;
            if (isCondEx == true)
            {
                trueNode.conditionEx = condEx;
            }
            else
            {
                trueNode.operand1 = lop;
                trueNode.operand2 = rop;
                trueNode.LopType = Lt;
                trueNode.RopType = Rt;
                trueNode.condOperateType = condt;
            }
            // 为true节点追加代码块光标节点、代码块右边界
            trueNode.children.Add(new KagaNode(trueNode.anodeName + "___PADDING_CURSOR",
                NodeType.PILE__PADDING_CURSOR, trueNode.depth, 0, trueNode));
            trueNode.children.Add(new KagaNode(trueNode.anodeName + "___BRIGHT_BRUCKET",
                NodeType.PILE__BRIGHT_BRUCKET, trueNode.depth, 1, trueNode));
            nkn.children.Add(trueNode);
            // 如果有分支
            if (containElse)
            {
                KagaNode falseNode = new KagaNode(
                    nkn.anodeName + "___" + NodeType.BLOCK__IF_FALSE.ToString(),
                    NodeType.BLOCK__IF_FALSE, nkn.depth + 1, 1, nkn);
                // 为false节点追加代码块光标节点、代码块右边界
                falseNode.children.Add(new KagaNode(falseNode.anodeName + "___PADDING_CURSOR",
                    NodeType.PILE__PADDING_CURSOR, falseNode.depth, 0, falseNode));
                falseNode.children.Add(new KagaNode(falseNode.anodeName + "___BRIGHT_BRUCKET",
                    NodeType.PILE__BRIGHT_BRUCKET, falseNode.depth, 1, falseNode));
                nkn.children.Add(falseNode);
            }
            KagaNode borderNode = new KagaNode(
                nkn.anodeName + "___" + NodeType.PILE__ENDIF.ToString(),
                NodeType.PILE__ENDIF, nkn.depth + 1, 2, nkn);
            nkn.children.Add(borderNode);
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }

        /// <summary>
        /// 指令：条件循环
        /// </summary>
        /// <param name="clt">条件循环的类型</param>
        /// <param name="operand">操作数</param>
        /// <param name="dowhileFlag">是否为do-while类型的循环</param>
        public void dash_condLoop(CondLoopType clt, string operand, bool dowhileFlag)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            StringBuilder sb = new StringBuilder();
            if (dowhileFlag == true)
            {
                sb.Append(Consta.prefix_frontend + " 循环后：");
            }
            else
            {
                sb.Append(Consta.prefix_frontend + " 循环：");
            }
            switch (clt)
            {
                case CondLoopType.CLT_EXPRESSION:
                    sb.Append("表达式 " + operand + " 为真时");
                    break;
                case CondLoopType.CLT_SWITCH:
                    sb.Append("开关 " + operand + " 状态为打开时");
                    break;
                default:
                    sb.Append("永远");
                    break;
            }
            TreeNode np = new TreeNode(sb.ToString());
            // padding节点要追加给np
            np.Nodes.Add(Consta.prefix_frontend +
                        "                                            ");
            np.ForeColor = Consta.getColoring(NodeType.BLOCK__WHILE);
            np.ExpandAll();
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交给后台
            KagaNode codeParentNode = this.getOpNodeParent(1);
            KagaNode nkn = new KagaNode(
                codeParentNode.anodeName + "___" + NodeType.BLOCK__WHILE.ToString(),
                NodeType.BLOCK__WHILE,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.condLoopType = clt;
            if (clt == CondLoopType.CLT_SWITCH)
            {
                nkn.conditionEx = operand.Split(':')[0];
            }
            else
            {
                nkn.conditionEx = operand;
            }
            nkn.isCondPostCheck = dowhileFlag;
            // 为循环节点追加代码块光标节点、代码块右边界
            nkn.children.Add(new KagaNode(nkn.anodeName + "___PADDING_CURSOR",
                NodeType.PILE__PADDING_CURSOR, nkn.depth, 0, nkn));
            nkn.children.Add(new KagaNode(nkn.anodeName + "___BRIGHT_BRUCKET",
                NodeType.PILE__BRIGHT_BRUCKET, nkn.depth, 1, nkn));
            // 把修改提交到代码管理器
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }

        /// <summary>
        /// 指令：次数循环
        /// </summary>
        /// <param name="simFlag">是否为简单次数循环</param>
        /// <param name="tbegin">开始边界类型</param>
        /// <param name="obegin">开始边界值</param>
        /// <param name="tend">结束边界类型</param>
        /// <param name="oend">结束边界值</param>
        /// <param name="tstep">步长类型</param>
        /// <param name="ostep">步长值</param>
        public void dash_forLoop(bool simFlag, ForLoopType tbegin, string obegin,
            ForLoopType tend, string oend, ForLoopType tstep, string ostep)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            StringBuilder sb = new StringBuilder();
            // 简单模式时
            if (simFlag == true)
            {
                sb.Append(Consta.prefix_frontend + " 循环：反复 " + oend + " 次");
            }
            // 高级模式时
            else
            {
                sb.Append(Consta.prefix_frontend + " 循环：从");
                switch (tbegin)
                {
                    case ForLoopType.FLT_CONSTANT:
                        sb.Append("常数" + obegin);
                        break;
                    case ForLoopType.FLT_GLOBAL:
                        sb.Append("全局变量" + obegin);
                        break;
                    default:
                        sb.Append("变量" + obegin);
                        break;
                }
                sb.Append(" 到");
                switch (tend)
                {
                    case ForLoopType.FLT_CONSTANT:
                        sb.Append("常数" + oend);
                        break;
                    case ForLoopType.FLT_GLOBAL:
                        sb.Append("全局变量" + oend);
                        break;
                    default:
                        sb.Append("变量" + oend);
                        break;
                }
                sb.Append("，每次递增 ");
                switch (tstep)
                {
                    case ForLoopType.FLT_CONSTANT:
                        sb.Append("常数" + ostep);
                        break;
                    case ForLoopType.FLT_GLOBAL:
                        sb.Append("全局变量" + ostep);
                        break;
                    default:
                        sb.Append("变量" + ostep);
                        break;
                }
            }
            TreeNode np = new TreeNode(sb.ToString());
            // padding节点要追加给np
            np.Nodes.Add(Consta.prefix_frontend +
                        "                                            ");
            np.ForeColor = Consta.getColoring(NodeType.BLOCK__WHILE);
            np.ExpandAll();
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交给后台
            KagaNode codeParentNode = this.getOpNodeParent(1);
            KagaNode nkn = new KagaNode(
                codeParentNode.anodeName + "___" + NodeType.BLOCK__FOR.ToString(),
                NodeType.BLOCK__FOR,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.forBeginType = tbegin;
            nkn.forEndType = tend;
            nkn.forStepType = tstep;
            nkn.forBeginIter = obegin;
            nkn.forEndIter = oend;
            nkn.forStep = ostep;
            nkn.isSimpleFor = simFlag;
            // 为循环节点追加代码块光标节点、代码块右边界
            nkn.children.Add(new KagaNode(nkn.anodeName + "___PADDING_CURSOR",
                NodeType.PILE__PADDING_CURSOR, nkn.depth, 0, nkn));
            nkn.children.Add(new KagaNode(nkn.anodeName + "___BRIGHT_BRUCKET",
                NodeType.PILE__BRIGHT_BRUCKET, nkn.depth, 1, nkn));
            // 把修改提交到代码管理器
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }

        /// <summary>
        /// 指令：函数退出
        /// </summary>
        /// <param name="opType">函数返回类型</param>
        /// <param name="operand">函数返回值</param>
        public void dash_return(OperandType opType, string operand)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            StringBuilder sbfr = new StringBuilder();
            switch (opType)
            {
                case OperandType.VO_Constant:
                    sbfr.Append(" 退出当前函数，返回值：常数" + operand);
                    break;
                case OperandType.VO_DefVar:
                    sbfr.Append(" 退出当前函数，返回值：局部变量" + operand);
                    break;
                case OperandType.VO_GlobalVar:
                    sbfr.Append(" 退出当前函数，返回值：全局变量" + operand);
                    break;
                case OperandType.VO_VOID:
                default:
                    sbfr.Append(" 退出当前函数");
                    break;
            }
            TreeNode np = new TreeNode(Consta.prefix_frontend + sbfr.ToString());
            np.ForeColor = Consta.getColoring(NodeType.RETURN);
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 提交修改到代码管理器
            KagaNode codeParentNode = this.getOpNodeParent(1);
            KagaNode nkn = new KagaNode(
                codeParentNode.anodeName + "___" + NodeType.RETURN.ToString(),
                NodeType.RETURN,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.funretOperand = operand;
            nkn.funretType = opType;
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }

        /// <summary>
        /// 指令：中断循环
        /// </summary>
        public void dash_break()
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np = new TreeNode(Consta.prefix_frontend + " 中断本次循环");
            np.ForeColor = Consta.getColoring(NodeType.BREAK);
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到后台
            KagaNode codeParentNode = this.getOpNodeParent(1);
            KagaNode nkn = new KagaNode(
                codeParentNode.anodeName + "___" + NodeType.BREAK.ToString(),
                NodeType.BREAK,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }

        /// <summary>
        /// 指令：插入注释
        /// </summary>
        /// <param name="nota">注释字符串</param>
        public void dash_notation(string nota)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np = new TreeNode(String.Format("{0} 注释：{1} {2}",
                Consta.prefix_frontend, nota.Split('\n')[0],
                nota.Split('\n').Length > 1 ? "..." : ""));
            np.ForeColor = Consta.getColoring(NodeType.NOTE);
            np.ToolTipText = nota;
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到代码管理器
            KagaNode codeParentNode = this.getOpNodeParent(1);
            KagaNode nkn = new KagaNode(
                codeParentNode.anodeName + "___" + NodeType.NOTE.ToString(),
                NodeType.NOTE,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.notation = nota;
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }

        /// <summary>
        /// 指令：函数调用
        /// </summary>
        /// <param name="fcname">被调用的函数名称</param>
        /// <param name="argPairs">参数列表字符串：以"arg1:operand1//arg2:operand2..."的格式传递</param>
        public void dash_funcall(string fcname, string argPairs)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np = new TreeNode(Consta.prefix_frontend + " 函数：" + fcname);
            np.ToolTipText = argPairs.Replace(":", ": ").Replace("//", Environment.NewLine);
            np.ForeColor = Consta.getColoring(NodeType.CALL);
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 提交给代码管理器
            KagaNode codeParentNode = this.getOpNodeParent(1);
            KagaNode nkn = new KagaNode(
                codeParentNode.anodeName + "___" + NodeType.CALL.ToString(),
                NodeType.CALL,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.calling = fcname;
            nkn.callingParams = argPairs;
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }

        /// <summary>
        /// 指令：插入代码片段
        /// </summary>
        /// <param name="myCode">代码片段字符串</param>
        public void dash_codeblock(string myCode)
        {
            // 刷新前台
            TreeView curTree = this.getActiveTreeView();
            int insertPoint = curTree.SelectedNode.Index;
            TreeNode np = new TreeNode(String.Format("{0} 代码片段：{1} {2}",
                Consta.prefix_frontend, myCode.Split('\n')[0], 
                myCode.Split('\n').Length > 1 ? "..." : ""));
            np.ToolTipText = myCode;
            np.ForeColor = Consta.getColoring(NodeType.CODEBLOCK);
            curTree.SelectedNode.Parent.Nodes.Insert(insertPoint, np);
            // 把修改提交到代码管理器
            KagaNode codeParentNode = this.getOpNodeParent(1);
            KagaNode nkn = new KagaNode(
                codeParentNode.anodeName + "___" + NodeType.CODEBLOCK.ToString(),
                NodeType.CODEBLOCK,
                codeParentNode.depth + 1,
                insertPoint,
                codeParentNode);
            nkn.myCode = myCode;
            codeMana.insertNode(codeParentNode.depth + 1, insertPoint, nkn);
        }
        #endregion

        #region 前台刷新相关函数 
        /// <summary>
        /// 设置MainForm的引用
        /// </summary>
        /// <param name="mainRef">主窗体实例</param>
        public void setMainForm(MainForm mainRef)
        {
            this.mainFormPointer = mainRef;
        }

        /// <summary>
        /// 获取当前活跃的TabPage里的编辑器
        /// </summary>
        /// <returns>活跃标签页里的TreeView实例</returns>
        public TreeView getActiveTreeView()
        {
            TabPage p = this.mainFormPointer.tabControl1.SelectedTab;
            Control[] controlItem = p.Controls.Find("codeTreeView", true);
            return controlItem != null ? (TreeView)controlItem[0] : null;
        }

        /// <summary>
        /// 把前端的信息更新为目前后台的状态
        /// </summary>
        public void refreshAll()
        {
            // 更新函数列表
            this.mainFormPointer.functionListBox.Items.Clear();
            foreach (string s in this.getAllFunction())
            {
                this.mainFormPointer.functionListBox.Items.Add(s);
            }
            // 更新全局变量
            this.mainFormPointer.globalvarListBox.Items.Clear();
            foreach (string s in this.getGlobalVar())
            {
                this.mainFormPointer.globalvarListBox.Items.Add(s.Replace("@", " @ "));
            }
            // 刷新Tab页
            foreach (string s in this.getAllFunction())
            {
                if (this.mainFormPointer.tabControl1.TabPages.ContainsKey(s) == false)
                {
                    this.mainFormPointer.addTabCard(s);
                }
            }
            foreach (TabPage tp in this.mainFormPointer.tabControl1.TabPages)
            {
                if (this.getAllFunction().Contains(tp.Name) == false)
                {
                    this.mainFormPointer.closeTabCard(tp.Name);
                }
            }
            // 遍历所有Tab
            foreach (TabPage p in this.mainFormPointer.tabControl1.TabPages)
            {
                // 取得函数在代码树上的根节点
                string tabFunName = p.Text;
                KagaNode funNode = codeMana.getFunRoot(tabFunName);
                // 清空当前tab页
                Control[] controls = p.Controls.Find("codeTreeView", true);
                if (controls.Length < 1)
                {
                    throw new Exception("Tab刷新时遇到空错误");
                }
                this.treeViewPointer = controls[0] as TreeView;
                this.treeViewPointer.Nodes.Clear();
                // 深度优先搜索代码树，绘制窗体
                this.codeMana.DFS(
                    match: (x) => x != null,
                    startNode: funNode, 
                    func: (x) => this.drawTreeContext(x), 
                    unique: false);
                // 展开所有节点
                treeViewPointer.ExpandAll();
            }
        }

        /// <summary>
        /// 可视化代码树的绘制函数
        /// </summary>
        /// <param name="parseNode">当前处理的节点</param>
        /// <returns></returns>
        private KagaNode drawTreeContext(KagaNode parseNode)
        {
            switch (parseNode.atype)
            {
                // 编译控制：右边界
                case NodeType.PILE__BRIGHT_BRUCKET:
                    currentParent = currentParent.Parent;
                    break;
                // 编译控制：插入节点
                case NodeType.PILE__PADDING_CURSOR:
                    currentParent.Nodes.Add(Consta.prefix_frontend +
                        "                                            ");
                    break;
                // 编译控制：条件分支
                case NodeType.PILE__IF:
                    currentParent = currentParent.Nodes.Add(
                        String.Format("{0} 条件分支", Consta.prefix_frontend));
                    currentParent.ForeColor = Consta.getColoring(parseNode.atype);
                    break;
                // 编译控制：条件分支结束
                case NodeType.PILE__ENDIF:
                    currentParent = currentParent.Parent;
                    break;
                // 代码块：函数签名
                case NodeType.PILE__BLOCK__FUNCTION:
                    currentParent = this.treeViewPointer.Nodes.Add(
                        String.Format("{0} {1}", Consta.prefix_frontCond, parseNode.funBinding.getSign()));
                    currentParent.ForeColor = Consta.getColoring(parseNode.atype);
                    break;
                // 代码块：条件真分支
                case NodeType.BLOCK__IF_TRUE:
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Consta.prefix_frontCond + " 当");
                    if (parseNode.isConditionEx)
                    {
                        sb.Append("表达式：" + parseNode.conditionEx + " 为真");
                    }
                    else
                    {
                        switch (parseNode.LopType)
                        {
                            case OperandType.VO_GlobalVar:
                                sb.Append("全局变量");
                                break;
                            case OperandType.VO_DefVar:
                                sb.Append("局部变量");
                                break;
                            case OperandType.VO_Switch:
                                sb.Append("开关");
                                break;
                            default:
                                break;
                        }
                        sb.Append(parseNode.operand1);
                        if (parseNode.LopType == OperandType.VO_Switch)
                        {
                            sb.Append(":" + this.getSwitchDescriptionVector()[Convert.ToInt32(parseNode.operand1)]);
                        }
                        sb.Append(" " + parseNode.condOperateType.ToString() + " ");
                        switch (parseNode.RopType)
                        {
                            case OperandType.VO_Constant:
                                sb.Append("常数");
                                break;
                            case OperandType.VO_GlobalVar:
                                sb.Append("全局变量");
                                break;
                            case OperandType.VO_DefVar:
                                sb.Append("局部变量");
                                break;
                            case OperandType.VO_Switch:
                                break;
                            default:
                                break;
                        }
                        sb.Append(parseNode.operand2);
                    }
                    sb.Append("时：");
                    currentParent = currentParent.Nodes.Add(sb.ToString());
                    currentParent.ForeColor = Consta.getColoring(parseNode.atype);
                    break;
                // 代码块：条件假分支
                case NodeType.BLOCK__IF_FALSE:
                    currentParent = currentParent.Nodes.Add(Consta.prefix_frontCond + " 除此以外的情况：");
                    currentParent.ForeColor = Consta.getColoring(parseNode.atype);
                    break;
                // 代码块：条件循环
                case NodeType.BLOCK__WHILE:
                    StringBuilder sbw = new StringBuilder();
                    if (parseNode.isCondPostCheck == true)
                    {
                        sbw.Append(Consta.prefix_frontend + " 循环后：");
                    }
                    else
                    {
                        sbw.Append(Consta.prefix_frontend + " 循环：");
                    }
                    switch (parseNode.condLoopType)
                    {
                        case CondLoopType.CLT_EXPRESSION:
                            sbw.Append("表达式 " + parseNode.conditionEx + " 为真时");
                            break;
                        case CondLoopType.CLT_SWITCH:
                            sbw.Append("开关 " + parseNode.conditionEx + ":" +
                                this.getSwitchDescriptionVector()[Convert.ToInt32(parseNode.conditionEx)] + " 状态为打开时");
                            break;
                        default:
                            sbw.Append("永远");
                            break;
                    }
                    // padding节点要追加给np
                    currentParent = currentParent.Nodes.Add(sbw.ToString());
                    currentParent.ForeColor = Consta.getColoring(parseNode.atype);
                    break;
                // 代码块：次数循环
                case NodeType.BLOCK__FOR:
                    StringBuilder sbf = new StringBuilder();
                    if (parseNode.isSimpleFor)
                    {
                        sbf.Append(Consta.prefix_frontend + " 循环：反复 " + parseNode.forEndIter + " 次");
                    }
                    else 
                    {
                        sbf.Append(Consta.prefix_frontend + " 循环：从");
                        switch (parseNode.forBeginType)
                        {
                            case ForLoopType.FLT_CONSTANT:
                                sbf.Append("常数" + parseNode.forBeginIter);
                                break;
                            case ForLoopType.FLT_GLOBAL:
                                sbf.Append("全局变量" + parseNode.forBeginIter);
                                break;
                            default:
                                sbf.Append("变量" + parseNode.forBeginIter);
                                break;
                        }
                        sbf.Append(" 到");
                        switch (parseNode.forEndType)
                        {
                            case ForLoopType.FLT_CONSTANT:
                                sbf.Append("常数" + parseNode.forEndIter);
                                break;
                            case ForLoopType.FLT_GLOBAL:
                                sbf.Append("全局变量" + parseNode.forEndIter);
                                break;
                            default:
                                sbf.Append("变量" + parseNode.forEndIter);
                                break;
                        }
                        sbf.Append("，每次递增 ");
                        switch (parseNode.forStepType)
                        {
                            case ForLoopType.FLT_CONSTANT:
                                sbf.Append("常数" + parseNode.forStep);
                                break;
                            case ForLoopType.FLT_GLOBAL:
                                sbf.Append("全局变量" + parseNode.forStep);
                                break;
                            default:
                                sbf.Append("变量" + parseNode.forStep);
                                break;
                        }
                    }
                    currentParent = currentParent.Nodes.Add(sbf.ToString());
                    currentParent.ForeColor = Consta.getColoring(parseNode.atype);
                    break;
                // 操作：变量定义
                case NodeType.DEFINE_VARIABLE:
                    TreeNode tdefvar = new TreeNode(String.Format("{0} 变量定义：{1} ({2})", Consta.prefix_frontend,
                        parseNode.defineVarName, Consta.parseVarTypeToCType(parseNode.defineVarType)));
                    tdefvar.ForeColor = Consta.getColoring(parseNode.atype);
                    currentParent.Nodes.Add(tdefvar);
                    break;
                // 操作：开关操作
                case NodeType.USING_SWITCHES:
                    TreeNode tuseswt = new TreeNode(
                        String.Format("{0} 开关操作：[{1}:{2}] 状态设置为 {3}", Consta.prefix_frontend,
                        parseNode.switchId.ToString(), symbolMana.getSwitchVector()[parseNode.switchId],
                        parseNode.switchFlag ? "打开" : "关闭"));
                    tuseswt.ForeColor = Consta.getColoring(parseNode.atype);
                    currentParent.Nodes.Add(tuseswt);
                    break;
                // 操作：变量操作
                case NodeType.EXPRESSION:
                    TreeNode vexpnode;
                    if (parseNode.RopType == OperandType.VO_GlobalVar)
                    {
                        vexpnode = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 全局变量{3}的值", Consta.prefix_frontend,
                        parseNode.operand1, parseNode.varOperateType.ToString(), parseNode.operand2));
                    }
                    else if (parseNode.RopType == OperandType.VO_DefVar)
                    {
                        vexpnode = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 局部变量{3}的值", Consta.prefix_frontend,
                        parseNode.operand1, parseNode.varOperateType.ToString(), parseNode.operand2));
                    }
                    else if (parseNode.RopType == OperandType.VO_Random)
                    {
                        vexpnode = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 随机数范围[{3},{4}]", Consta.prefix_frontend,
                        parseNode.operand1, parseNode.varOperateType.ToString(), parseNode.operand2, parseNode.operand3));
                    }
                    else
                    {
                        vexpnode = new TreeNode(
                        String.Format("{0} 变量操作：{1} {2} 常数{3}", Consta.prefix_frontend,
                        parseNode.operand1, parseNode.varOperateType.ToString(), parseNode.operand2));
                    }
                    vexpnode.ForeColor = Consta.getColoring(parseNode.atype);
                    currentParent.Nodes.Add(vexpnode);
                    break;
                // 操作：函数调用
                case NodeType.CALL:
                    TreeNode ncall = new TreeNode(Consta.prefix_frontend + " 函数：" + parseNode.calling);
                    ncall.ToolTipText = parseNode.callingParams.Replace(":", ": ").Replace("//", Environment.NewLine);
                    ncall.ForeColor = Consta.getColoring(parseNode.atype);
                    currentParent.Nodes.Add(ncall);
                    break;
                // 操作：函数退出
                case NodeType.RETURN:
                    StringBuilder sbfr = new StringBuilder();
                    switch (parseNode.funretType)
                    {
                        case OperandType.VO_Constant:
                            sbfr.Append(" 退出当前函数，返回值：常数" + parseNode.funretOperand);
                            break;
                        case OperandType.VO_DefVar:
                            sbfr.Append(" 退出当前函数，返回值：局部变量" + parseNode.funretOperand);
                            break;
                        case OperandType.VO_GlobalVar:
                            sbfr.Append(" 退出当前函数，返回值：全局变量" + parseNode.funretOperand);
                            break;
                        case OperandType.VO_VOID:
                        default:
                            sbfr.Append(" 退出当前函数");
                            break;
                    }
                    TreeNode returnNode = new TreeNode(Consta.prefix_frontend + sbfr.ToString());
                    returnNode.ForeColor = Consta.getColoring(parseNode.atype);
                    currentParent.Nodes.Add(returnNode);
                    break;
                // 操作：中断循环
                case NodeType.BREAK:
                    TreeNode breakNode = new TreeNode(Consta.prefix_frontend + " 中断本次循环");
                    breakNode.ForeColor = Consta.getColoring(parseNode.atype);
                    currentParent.Nodes.Add(breakNode);
                    break;
                // 操作：注释
                case NodeType.NOTE:
                    TreeNode notanode = new TreeNode(
                        String.Format("{0} 注释：{1} {2}", Consta.prefix_frontend,
                        parseNode.notation.Split('\n')[0], parseNode.notation.Split('\n').Length > 1 ? "..." : ""));
                    notanode.ToolTipText = parseNode.notation;
                    notanode.ForeColor = Consta.getColoring(parseNode.atype);
                    currentParent.Nodes.Add(notanode);
                    break;
                // 操作：代码片段
                case NodeType.CODEBLOCK:
                    TreeNode ncodeblock = new TreeNode(
                        String.Format("{0} 代码片段：{1} {2}", Consta.prefix_frontend, 
                        parseNode.myCode.Split('\n')[0], parseNode.myCode.Split('\n').Length > 1 ? "..." : ""));
                    ncodeblock.ToolTipText = parseNode.myCode;
                    ncodeblock.ForeColor = Consta.getColoring(parseNode.atype);
                    currentParent.Nodes.Add(ncodeblock);
                    break;
                default:
                    throw new Exception("匹配树类型错误");
            }
            return parseNode;
        }

        // 当前处理节点的双亲节点
        private TreeNode currentParent = null;
        // 活跃前端代码树指针
        private TreeView treeViewPointer = null;
        #endregion

        /// <summary>
        /// 初始化整个控制器，并重新取得符号管理器和代码管理器的引用
        /// </summary>
        public void Init()
        {
            this.symbolMana.clear();
            this.codeMana.clear();
            this.symbolMana = SymbolManager.getInstance();
            this.codeMana = CodeManager.getInstance();
        }
        
        /// <summary>
        /// 将代码管理器和符号管理器的引用重置
        /// </summary>
        /// <param name="cmana">代码管理器实例</param>
        /// <param name="smana">符号管理器实例</param>
        public void setMana(CodeManager cmana, SymbolManager smana)
        {
            // 重置单例
            SymbolManager.setSynObj(smana);
            CodeManager.setSynObj(cmana);
            // 刷新引用
            this.symbolMana = SymbolManager.getInstance();
            this.codeMana = CodeManager.getInstance();
        }

        /// <summary>
        /// 工厂方法：获得唯一实例
        /// </summary>
        /// <returns>返回控制器的唯一实例</returns>
        public static KagaController getInstance()
        {
            return synObject == null ? synObject = new KagaController() : synObject;
        }

        /// <summary>
        /// 私有的构造器
        /// </summary>
        private KagaController()
        {
            pileMana = PileParser.getInstance();
            symbolMana = SymbolManager.getInstance();
            codeMana = CodeManager.getInstance();
            fileMana = FileManager.getInstance();
        }
        // 唯一实例
        private static KagaController synObject = null;
        // 语法匹配器
        private PileParser pileMana = null;
        // 代码管理器
        private CodeManager codeMana = null;
        // 符号管理器
        private SymbolManager symbolMana = null;
        // 文件管理器
        private FileManager fileMana = null;
        // 主窗体指针
        private MainForm mainFormPointer = null;
    }
}
