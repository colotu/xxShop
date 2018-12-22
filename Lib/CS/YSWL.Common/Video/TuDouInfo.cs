/**
* TuDouInfo.cs
*
* 功 能： [N/A]
* 类 名： TuDouInfo
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/24 15:41:50  蒋海滨    初版
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
    public class TuDouInfo
    {
        public TuDouInfo()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 说明：视频ID
        /// </summary>
        private string itemId;

        public string ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        /// <summary>
        /// 说明：视频编码
        /// 备注：11位字符型编码,视频唯一标识
        /// </summary>
        private string itemCode;

        public string ItemCode
        {
            get { return itemCode; }
            set { itemCode = value; }
        }

        /// <summary>
        /// 说明：视频标题
        /// </summary>
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 说明：视频标签
        /// 备注：字符串,多个标签之间用逗号','分隔
        /// </summary>
        private string tags;

        public string Tags
        {
            get { return tags; }
            set { tags = value; }
        }
        /// <summary>
        /// 说明：视频描述
        /// </summary>
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 视频截图
        /// </summary>
        private string picUrl;

        public string PicUrl
        {
            get { return picUrl; }
            set { picUrl = value; }
        }

        /// <summary>
        /// 说明：视频备选截图
        /// 备注：URL地址字符串列表
        /// </summary>
        private string[] picChoiceUrl;

        public string[] PicChoiceUrl
        {
            get { return picChoiceUrl; }
            set { picChoiceUrl = value; }
        }

        /// <summary>
        /// 说明：视频时长
        /// 备注：单位毫秒
        /// </summary>
        private string totalTime;

        public string TotalTime
        {
            get { return totalTime; }
            set { totalTime = value; }
        }

        /// <summary>
        /// 说明：频发布时间
        /// 备注：指视频审核通过的发布时间,格式(yyyy-mm-dd)
        /// </summary>
        private string pubDate;

        public string PubDate
        {
            get { return pubDate; }
            set { pubDate = value; }
        }

        /// <summary>
        /// 说明：播客id
        /// 备注：上传该视频的用户ID
        /// </summary>
        private string ownerId;

        public string OwnerId
        {
            get { return ownerId; }
            set { ownerId = value; }
        }

        /// <summary>
        ///  说明：播客名 
        /// </summary>
        private string ownerName;

        public string OwnerName
        {
            get { return ownerName; }
            set { ownerName = value; }
        }

        /// <summary>
        ///  说明：播客昵称
        /// </summary>
        private string ownerNickname;

        public string OwnerNickname
        {
            get { return ownerNickname; }
            set { ownerNickname = value; }
        }

        /// <summary>
        ///  说明：所属频道ID
        ///  备注：对应的频道名称可查看:频道信息说明
        ///  http://api.tudou.com/apidoc/index.php/%E9%A2%91%E9%81%93%E4%BF%A1%E6%81%AF%E8%AF%B4%E6%98%8E
        /// </summary>
        private string channelId;

        public string ChannelId
        {
            get { return channelId; }
            set { channelId = value; }
        }

        /// <summary>
        ///  说明：站外播放器URL
        ///  备注：该视频的独立播放地址,可嵌入非土豆网站内容中提供播放
        private string outerPlayerUrl;

        public string OuterPlayerUrl
        {
            get { return outerPlayerUrl; }
            set { outerPlayerUrl = value; }
        }

        /// <summary>
        ///  说明：播放页URL
        ///  备注：该视频的土豆网站播放地址
        /// </summary>
        private string itemUrl;

        public string ItemUrl
        {
            get { return itemUrl; }
            set { itemUrl = value; }
        }

        /// <summary>
        ///  说明：媒体类型
        ///  备注：视频"或"音频"
        /// </summary>
        private string mediaType;

        public string MediaType
        {
            get { return mediaType; }
            set { mediaType = value; }
        }

        /// <summary>
        ///  说明：视频是否加密
        ///  备注：该视频播放时,若被加密,需要密码才能播放 
        /// </summary>
        private string secret;

        public string Secret
        {
            get { return secret; }
            set { secret = value; }
        }

        /// <summary>
        ///  说明：视频清晰度
        ///  备注：0:256p,1:360p,2:480p,3:720p 
        /// </summary>
        private string definition;

        public string Definition
        {
            get { return definition; }
            set { definition = value; }
        }
    }
}
