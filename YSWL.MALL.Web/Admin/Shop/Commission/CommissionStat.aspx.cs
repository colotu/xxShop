using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Commission;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.Commission
{
    public partial class CommissionStat : PageBaseAdmin
    {
        private  YSWL.MALL.BLL.Shop.Commission.CommissionDetail detailBll=new CommissionDetail();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            }
        }

        #region gridView

        protected void btnReStatistic_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        public void BindData()
        {

            DateTime startDate = Common.Globals.SafeDateTime(this.txtFrom.Text, DateTime.Now.AddDays(-6)).Date;
            DateTime endDate = Common.Globals.SafeDateTime(this.txtTo.Text, DateTime.Now).Date.AddDays(1).AddSeconds(-1);
            Model.Shop.Order.StatisticMode mode = (Model.Shop.Order.StatisticMode)Globals.SafeInt(rdoMode.SelectedValue, 0);
            DataSet ds = detailBll.StatCommission(startDate, endDate, mode);
            BindJson(ds, mode);
            this.gridView.DataSetSource = ds;
        }


        private void BindJson(DataSet ds, Model.Shop.Order.StatisticMode mode)
        {
            this.hfCategory.Value = "";
            this.hfDayCount.Value = "";
            List<YSWL.MALL.ViewModel.Shop.StatCommissionFee> feeList = new List<YSWL.MALL.ViewModel.Shop.StatCommissionFee>();
            //把dataset转换成list
            YSWL.MALL.ViewModel.Shop.StatCommissionFee model = null;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["TradeDate"] == null)
                {
                    continue;
                }
                model = new YSWL.MALL.ViewModel.Shop.StatCommissionFee();
                model.DateStr = dr["TradeDate"].ToString();
                model.OrderCount = Common.Globals.SafeInt(dr["OrderCount"], 0);
                model.TotalFee = Common.Globals.SafeDecimal(dr["TotalFee"], 0);
                model.UserCount = Common.Globals.SafeInt(dr["UserCount"], 0);
                feeList.Add(model);
            }

            this.hfCategory.Value = String.Join(",", feeList.Select(c => c.DateStr));
            this.hfDayCount.Value = String.Join(",", feeList.Select(c => c.TotalFee));
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
        #endregion

    }
}