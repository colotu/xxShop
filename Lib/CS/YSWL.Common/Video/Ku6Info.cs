/**
* Ku6Info.cs
*
* 功 能： [N/A]
* 类 名： Ku6Info
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/24 15:41:15  蒋海滨    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Common.Video
{
    public class Ku6Info
    {
        public Ku6Info()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 视频ID
        /// </summary>
        public String vid
        {
            get;
            set;
        }

        /// <summary>
        /// 缩略图地址
        /// </summary>
        public String coverurl
        {
            get;
            set;
        }

        /// <summary>
        /// 播放器地址
        /// </summary>
        public String flash
        {
            get;
            set;
        }

        /// <summary>
        /// 视频标题
        /// </summary>
        public String title
        {
            get;
            set;
        }

        /// <summary>
        /// 视频描述信息
        /// </summary>
        public String desc
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public int type
        {
            get;
            set;
        }
    }
}
