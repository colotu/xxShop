using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using System.Data;
using System.Text;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class BrandSales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
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
            DataSet ds = BLL.Shop.Order.OrderManage.BrandSaleInfo(startDate, endDate,10);

            BindJson(ds);
            this.gridView.DataSetSource = ds;
        }


        private void BindJson(DataSet ds)
        {
            List<YSWL.MALL.ViewModel.Order.BrandSaleInfo> shopSaleList = new List<YSWL.MALL.ViewModel.Order.BrandSaleInfo>();
            //把dataset转换成list
            YSWL.MALL.ViewModel.Order.BrandSaleInfo model = null;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                model = new ViewModel.Order.BrandSaleInfo();
                model.BrandName = dr.Field<string>("BrandName");
                model.Amount = Common.Globals.SafeDecimal(dr["AdjustedPrice"], 0);
                shopSaleList.Add(model);
            }
            this.hfCategory.Value = String.Join(",", shopSaleList.Select(c => c.BrandName));
            this.hfAmount.Value = String.Join(",", shopSaleList.Select(c => c.Amount));
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