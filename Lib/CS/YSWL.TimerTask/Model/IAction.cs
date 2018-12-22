/**
* IAction.cs
*
* 功 能： [N/A]
* 类 名： IAction
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/8/21 22:08:09  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

namespace YSWL.TimerTask.Model
{
    [System.Obsolete]
    public interface IAction
    {
        void Run(string[] args);
    }
}
