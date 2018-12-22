/**  版本信息模板在安装目录下，可自行修改。
* ActivityRule.cs
*
* 功 能： N/A
* 类 名： ActivityRule
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/6/10 22:26:33   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.Activity
{
	/// <summary>
	/// ActivityRule:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ActivityRule
	{
		public ActivityRule()
		{}
        #region Model
        private int _ruleid;
        private string _rulename;
        private int _priority=0;
        private int _status;
        private int _createduserid;
        private DateTime _createddate;
        /// <summary>
        /// 规则Id
        /// </summary>
        public int RuleId
        {
            set { _ruleid = value; }
            get { return _ruleid; }
        }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName
        {
            set { _rulename = value; }
            get { return _rulename; }
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            set { _priority = value; }
            get { return _priority; }
        }
        /// <summary>
        /// 活动状态 0：不启用，1：启用
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedUserId
        {
            set { _createduserid = value; }
            get { return _createduserid; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate
        {
            set { _createddate = value; }
            get { return _createddate; }
        }
        #endregion Model

	}
}

