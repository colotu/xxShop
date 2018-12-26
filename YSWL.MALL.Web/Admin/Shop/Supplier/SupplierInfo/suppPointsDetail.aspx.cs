/**
* PointsDetail.cs
*
* 功 能： [N/A]
* 类 名： PointsDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/8/20 14:44:45  Administrator    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Accounts.Bus;
using System.Drawing;
using System.Data;

namespace YSWL.MALL.Web.Admin.Shop.Supplier.SupplierInfo
{
    public partial class suppPointsDetail : PageBaseAdmin
    {
        private User currentUser;
        protected override int Act_PageLoad { get { return 301; } } //用户管理_积分详情页


        YSWL.MALL.BLL.Shop.Order.Orders orderbll = new BLL.Shop.Order.Orders();
        YSWL.MALL.BLL.Shop.Supplier.SupplierInfo suppbll = new BLL.Shop.Supplier.SupplierInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ((Request.Params["Empid"] != null) && (Request.Params["Empid"].ToString() != ""))
                {

                    if ((Request.Params["strstrdate"] != null) && (Request.Params["strstrdate"].ToString() != ""))
                    {
                        this.txtCreatedDateStart.Text = Request.Params["strstrdate"].ToString().Trim();
                    }
                    if ((Request.Params["strenddate"] != null) && (Request.Params["strenddate"].ToString() != ""))
                    {
                        this.txtCreatedDateEnd.Text = Request.Params["strenddate"].ToString().Trim();
                    }

                    if (Common.PageValidate.IsNumber(Request.Params["Empid"]))
                    {
                        this.lbsuppid.Text = Request.Params["Empid"].ToString().Trim();

                        txtUserName.Text = suppbll.GetSuppNameBywhere(" SupplierId='"+lbsuppid.Text+"'");

                        BindData();
                    }
                }
            }
        }

        #region gridView

        public void BindData()
        {
            if ((Request.Params["Empid"] != null) && (Request.Params["Empid"].ToString() != ""))
            {
                DataSet ds = new DataSet();

                //获取积分明细
                ds = orderbll.GetList(" Wdbh='" + lbsuppid.Text + "' and  CreatedDate>'" + txtCreatedDateStart.Text + "' and CreatedDate<'" + txtCreatedDateEnd.Text + "' ");

                if (ds != null)
                {
                    gridView.DataSetSource = ds;
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("style", "background:#FFF");
            if (e.Row.RowIndex>-1)
            {
                if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#F4F4F4");
                }
                else
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFFFFF");
                }
                //e.Row.Cells[3].Text = "购买商品";
            }
        }

        //返回处理
        public void btnReturn_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("/Admin/Shop/Supplier/SupplierInfo/List.aspx");
        }

        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetRuleName(object rule)
        {
            int ruleId = Common.Globals.SafeInt(rule, 0);
            if (ruleId ==(int)YSWL.MALL.Model.Members.Enum.PointRule.Order)
            {
                return "完成订单";
            }
            YSWL.MALL.BLL.Members.PointsRule RuleBll = new BLL.Members.PointsRule();
            return RuleBll.GetRuleName(ruleId);
        }

       
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }
    }
}