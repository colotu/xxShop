/**
* TaskTimers.cs
*
* 功 能： N/A
* 类 名： TaskTimers
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/8/21 21:28:54   Ben    初版
*
* Copyright (c) 2012-2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;

namespace YSWL.TimerTask.Model
{
    /// <summary>
    /// TaskTimers:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public partial class TaskTimer
    {
        public string[] Params
        {
            get
            {
                return new[]
                {
                    Param1, Param2, Param3, Param4
                    , Param5, Param6, Param7, Param8, Param9
                    , Param10
                };
            }
            set
            {
                if (value == null || value.Length < 1) return;
                if (value.Length > 0) Param1 = value[0];
                if (value.Length > 1) Param2 = value[1];
                if (value.Length > 2) Param3 = value[2];
                if (value.Length > 3) Param4 = value[3];
                if (value.Length > 4) Param5 = value[4];
                if (value.Length > 5) Param6 = value[5];
                if (value.Length > 6) Param7 = value[6];
                if (value.Length > 7) Param8 = value[7];
                if (value.Length > 8) Param9 = value[8];
                if (value.Length > 9) Param10 = value[9];
            }
        }
    }
}

