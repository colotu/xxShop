﻿/**
* PhotoClass.cs
*
* 功 能： 图片分类下拉列表
* 类 名： PhotoClass
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/25 12:08:25  伍伟    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Web.UI.WebControls;
using System.Data;
using YSWL.MALL.BLL.CMS;

namespace YSWL.MALL.Web.Controls
{
    /// <summary>
    /// 图片分类下拉列表 基于DropDownList
    /// </summary>
    public class PhotoClassDropDownList : DropDownList
    {
        public PhotoClassDropDownList()
        {
            NullToDisplay = false;
            ParentId = null;
        }

        public override void DataBind()
        {
            this.Items.Clear();
            if (this.NullToDisplay)
            {
                this.Items.Add(new ListItem("", "0"));
            }

            PhotoClass classBll = new PhotoClass();
            DataSet dsParent = classBll.GetList(ParentId.HasValue ? "ParentId=" + this.ParentId : "0");

            if (dsParent != null && dsParent.Tables[0].Rows.Count > 0)
            {
                Common.TreeBind.SetMultiLevelDropDownList(this, "ParentId", dsParent.Tables[0]);
            }
            base.DataBind();
        }

        public bool NullToDisplay { get; set; }

        public int? ParentId { get; set; }
    }
}
