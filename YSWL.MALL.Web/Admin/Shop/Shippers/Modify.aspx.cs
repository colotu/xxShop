﻿/**
* Modify.cs
*
* 功 能： [N/A]
* 类 名： Modify.cs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using YSWL.Common;
using YSWL.Accounts.Bus;
namespace YSWL.MALL.Web.Admin.Shop.Shippers
{
    public partial class Modify : PageBaseAdmin
    {
        private readonly YSWL.MALL.BLL.Shop.Shippers bll = new YSWL.MALL.BLL.Shop.Shippers();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (ShipperId <= 0)
                {
                    MessageBox.ShowAndRedirect(this, "该信息不存在或者已被删除！", "list.aspx");
                }
                ShowInfo();
            }
        }

        public int ShipperId
        {
            get
            {
                if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
                {
                    return Globals.SafeInt(Request.Params["id"], 0);
                }
                return 0;
            }
        }

        private void ShowInfo()
        {
            YSWL.MALL.Model.Shop.Shippers model = bll.GetModel(ShipperId);
            if (model == null)
            {
                MessageBox.ShowAndRedirect(this, "该信息不存在或者已被删除！", "list.aspx");
                return;
            }
            this.txtShipperTag.Text = model.ShipperTag;
            this.txtShipperName.Text = model.ShipperName;
            this.RegionID.Region_iID = model.RegionId;
            this.txtAddress.Text = model.Address;
            this.txtCellPhone.Text = model.CellPhone;
            this.txtTelPhone.Text = model.TelPhone;
            this.txtZipcode.Text = model.Zipcode;
            this.txtRemark.Text = model.Remark;
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            if (this.txtShipperTag.Text.Trim().Length == 0)
            {
                MessageBox.ShowFailTip(this, "发货人标签不能为空！");
                return;
            }
            else
            {
                if (bll.Exists(this.txtShipperTag.Text.Trim(),ShipperId))
                {
                    MessageBox.ShowFailTip(this, "发货人标签已存在！");
                    return;
                }
            }
            if (this.txtShipperName.Text.Trim().Length == 0)
            {
                MessageBox.ShowFailTip(this, "发货人名称不能为空！");
                return;
            }
            int regionId = Globals.SafeInt(this.RegionID.Region_iID, -1);
            if (regionId <= 0)
            {
                MessageBox.ShowFailTip(this, "请选择地区！");
                return;
            }
            if (this.txtAddress.Text.Trim().Length == 0)
            {
                MessageBox.ShowFailTip(this, "地址不能为空！");
                return;
            }
            if (this.txtCellPhone.Text.Trim().Length == 0)
            {
                MessageBox.ShowFailTip(this, "手机不能为空！");
                return;
            }
            else
            {
                if (!PageValidate.IsPhone(this.txtCellPhone.Text.Trim()))
                {
                    MessageBox.ShowFailTip(this, "手机格式不正确！");
                    return;
                }
            }
            if (this.txtTelPhone.Text.Trim().Length > 0)
            {
                if (!PageValidate.IsPhone(this.txtTelPhone.Text.Trim()))
                {
                    MessageBox.ShowFailTip(this, "电话格式不正确！");
                    return;
                }
            }
            if (this.txtZipcode.Text.Trim().Length == 0)
            {
                MessageBox.ShowFailTip(this, "邮编不能为空！");
                return;
            }
            if (this.txtZipcode.Text.Trim().Length !=6)
            {
                MessageBox.ShowFailTip(this, "邮编不正确！");
                return;
            }
            string ShipperTag = this.txtShipperTag.Text;
            string ShipperName = this.txtShipperName.Text;
            string Address = this.txtAddress.Text;
            string CellPhone = this.txtCellPhone.Text;
            string TelPhone = this.txtTelPhone.Text;
            string Zipcode = this.txtZipcode.Text;
            string Remark = this.txtRemark.Text;
            YSWL.MALL.Model.Shop.Shippers model = bll.GetModel(ShipperId);
            if (model == null)
            {
                MessageBox.ShowAndRedirect(this, "该信息不存在或者已被删除！", "list.aspx");
                return;
            }
            model.ShipperTag = ShipperTag;
            model.ShipperName = ShipperName;
            model.RegionId = regionId;
            model.Address = Address;
            model.CellPhone = CellPhone;
            model.TelPhone = TelPhone;
            model.Zipcode = Zipcode;
            model.Remark = Remark;
            if (bll.Update(model))
            {
                DataCache.DeleteCache("ShippersModel-" + ShipperId);
                MessageBox.ShowSuccessTip(this, "保存成功！", "list.aspx");
            }
            else
            {
                MessageBox.ShowFailTip(this, "保存失败！", "list.aspx");
            }
        }
        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
