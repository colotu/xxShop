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
    public partial class CommissionRule : PageBaseAdmin
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
            DataSet ruleFeeDs = detailBll.StatRuleFee(startDate, endDate);
            DataSet ruleProDs = detailBll.StatRulePro(startDate, endDate);
            
            int top = Common.Globals.SafeInt(dropTop.SelectedValue, 10);
     
            List<YSWL.MALL.ViewModel.Shop.CommissionRuleStat> jsonlist;
            DataSet ds = GetMerger(ruleFeeDs, ruleProDs, top, out jsonlist);
            BindJson(jsonlist);
             this.gridView.DataSetSource = ds;
        }
        private DataSet GetMerger(DataSet ruleFeeDs, DataSet ruleProDs, int top, out List<YSWL.MALL.ViewModel.Shop.CommissionRuleStat> jsonlist)
        {
            jsonlist = new List<YSWL.MALL.ViewModel.Shop.CommissionRuleStat>();
            List<YSWL.MALL.ViewModel.Shop.CommissionRuleStat> proList = GetProList(ruleProDs);
            YSWL.MALL.ViewModel.Shop.CommissionRuleStat proModel = new ViewModel.Shop.CommissionRuleStat();
            //把dataset转换成list
            YSWL.MALL.ViewModel.Shop.CommissionRuleStat model = null;
            DataTable dt = ruleFeeDs.Tables[0];
            dt.Columns.Add(new DataColumn("TotalProduct", typeof(int)));
            DataTable proDt = ruleProDs.Tables[0];
            //只展示20条
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                model = new ViewModel.Shop.CommissionRuleStat();
                model.RuleId = Common.Globals.SafeInt(dr["RuleId"], 0);
                model.RuleName = dr.Field<string>("RuleName");
                model.TotalFee = Common.Globals.SafeDecimal(dr["TotalFee"], 0);
                //按规则筛选出商品总数               
                proModel = proList.Where(o => o.RuleId == model.RuleId && o.RuleName == model.RuleName).FirstOrDefault();
                if (proModel != null)
                {
                    model.TotalProduct = proModel.TotalProduct;
                }
                dr["TotalProduct"] = model.TotalProduct;
                if (i < top)
                {
                    jsonlist.Add(model);
                }
                i++;
            }
            return ruleFeeDs;
        } 
        private void BindJson(List<YSWL.MALL.ViewModel.Shop.CommissionRuleStat> jsonlist)
        {
            this.hfCategory.Value = String.Join(",", jsonlist.Select(c => c.RuleName));
            this.hfTotalFee.Value = String.Join(",", jsonlist.Select(c => c.TotalFee));
            this.hfTotalProdcut.Value= String.Join(",", jsonlist.Select(c => c.TotalProduct));
        }
        private List<YSWL.MALL.ViewModel.Shop.CommissionRuleStat> GetProList(DataSet ds)
        {
            if (Common.DataSetTools.DataSetIsNull(ds)) {
                return null;
            }
            List<YSWL.MALL.ViewModel.Shop.CommissionRuleStat> list= new List<YSWL.MALL.ViewModel.Shop.CommissionRuleStat>();
            YSWL.MALL.ViewModel.Shop.CommissionRuleStat model = null;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                model = new ViewModel.Shop.CommissionRuleStat();
                model.RuleId = Common.Globals.SafeInt(dr["RuleId"], 0);
                model.RuleName = dr.Field<string>("RuleName");
                model.TotalProduct = Common.Globals.SafeInt(dr["TotalProduct"], 0);
                list.Add(model);
            }
            return list;
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
