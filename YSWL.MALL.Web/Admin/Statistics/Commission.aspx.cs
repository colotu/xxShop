/**
* Show.cs
*
* 功 能： [N/A]
* 类 名： Show.cs
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
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Order;
using System.Linq;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class Commission : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.txtTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            }
        }
        protected void btnReStatistic_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        protected void BindData()
        {
            DateTime startDate = Common.Globals.SafeDateTime(this.txtFrom.Text, DateTime.Now);
            DateTime endDate = Common.Globals.SafeDateTime(this.txtTo.Text, DateTime.Now).AddDays(1);
            YSWL.MALL.BLL.Shop.Commission.CommissionDetail detailBll = new YSWL.MALL.BLL.Shop.Commission.CommissionDetail();
            DataSet ds = detailBll.StatUserFee(startDate, endDate);
            int top = Common.Globals.SafeInt(dropTop.SelectedValue, 10);
            BindJson(ds,top);
            this.gridView.DataSetSource = ds;
        }

        private void BindJson(DataSet ds,int top)
        {
            List<YSWL.MALL.ViewModel.Shop.CommissionStat> list = new List<YSWL.MALL.ViewModel.Shop.CommissionStat>();
            //把dataset转换成list
            YSWL.MALL.ViewModel.Shop.CommissionStat model = null;
            DataTable dt = ds.Tables[0];
            //只展示20条
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (i >= top)
                {
                    break;
                }
                model = new ViewModel.Shop.CommissionStat();
                model.UserId = Common.Globals.SafeInt(dr["UserId"], 0);
                model.NickName = dr.Field<string>("NickName");
                model.OrderCount = Common.Globals.SafeInt(dr["OrderCount"], 0);
                model.TotalFee = Common.Globals.SafeDecimal(dr["TotalFee"], 0);
                list.Add(model);
                i++;
            }
            this.hfCategory.Value = String.Join(",", list.Select(c => c.NickName));
            this.hfTotalFee.Value = String.Join(",", list.Select(c => c.TotalFee));
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("style", "background:#FFF");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#F4F4F4");
                }
                else
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFFFFF");
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
    }
}
