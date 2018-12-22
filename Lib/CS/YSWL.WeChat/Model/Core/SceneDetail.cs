/**  版本信息模板在安装目录下，可自行修改。
* SceneDetail.cs
*
* 功 能： N/A
* 类 名： SceneDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/2/20 12:32:24   N/A    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.WeChat.Model.Core
{
	/// <summary>
	/// SceneDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SceneDetail
	{
		public SceneDetail()
		{}
        #region Model
        private int _detailid;
        private int _sceneid;
        private string _openid;
        private string _username;
        private string _nickname;
        private int _sex = 0;
        private string _city;
        private string _province;
        private string _country;
        private int _referuserid = 0;
        private string _language;
        private DateTime _createtime;
        /// <summary>
        /// 详情Id
        /// </summary>
        public int DetailId
        {
            set { _detailid = value; }
            get { return _detailid; }
        }
        /// <summary>
        /// 场景ID
        /// </summary>
        public int SceneId
        {
            set { _sceneid = value; }
            get { return _sceneid; }
        }
        /// <summary>
        /// 微信原始Id
        /// </summary>
        public string OpenId
        {
            set { _openid = value; }
            get { return _openid; }
        }
        /// <summary>
        /// 微信用户码
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string NickName
        {
            set { _nickname = value; }
            get { return _nickname; }
        }
        /// <summary>
        /// 0: 未知 1：男 2：女
        /// </summary>
        public int Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 城市
        /// </summary>
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// 省
        /// </summary>
        public string Province
        {
            set { _province = value; }
            get { return _province; }
        }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country
        {
            set { _country = value; }
            get { return _country; }
        }
        /// <summary>
        /// 推荐人
        /// </summary>
        public int ReferUserId
        {
            set { _referuserid = value; }
            get { return _referuserid; }
        }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language
        {
            set { _language = value; }
            get { return _language; }
        }
        /// <summary>
        /// 关注时间
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        #endregion Model

    }
}

