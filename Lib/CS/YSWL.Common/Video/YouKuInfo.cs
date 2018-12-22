/**
* YouKuInfo.cs
*
* 功 能： [N/A]
* 类 名： YouKuInfo
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/24 15:42:11  蒋海滨    初版
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
    public class YouKuInfo
    {
        public YouKuInfo()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 视频ID
        /// </summary>
        private string videoid;

        public string VideoID
        {
            get { return videoid; }
            set { videoid = value; }
        }

        /// <summary>
        /// 视频ID
        /// </summary>
        private string vidEncoded;

        public string VidEncoded
        {
            get { return vidEncoded; }
            set { vidEncoded = value; }
        }

        /// <summary>
        /// 视频标题
        /// </summary>
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 截图地址
        /// </summary>
        private string logo;

        public string Logo
        {
            get { return logo; }
            set { logo = value; }
        }
    }
}
