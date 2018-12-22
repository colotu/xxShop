using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web.Admin.Shop.Commission
{
    public partial class ProFeeStat : PageBaseAdmin
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
            DataSet ds = detailBll.StatProFee(startDate, endDate);
            int top = Common.Globals.SafeInt(dropTop.SelectedValue, 10);
            BindJson(ds, top);
            this.gridView.DataSetSource = ds;
        }

        private void BindJson(DataSet ds, int top)
        {
            List<YSWL.MALL.ViewModel.Shop.CommissionProStat> list = new List<YSWL.MALL.ViewModel.Shop.CommissionProStat>();
            //把dataset转换成list
            YSWL.MALL.ViewModel.Shop.CommissionProStat model = null;
            DataTable dt = ds.Tables[0];
            //只展示20条
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (i >= top)
                {
                    break;
                }
                model = new ViewModel.Shop.CommissionProStat();
                model.ProductId = Common.Globals.SafeInt(dr["ProductId"], 0);
                model.ProName = dr.Field<string>("Name");
                model.OrderCount = Common.Globals.SafeInt(dr["OrderCount"], 0);
                model.TotalFee = Common.Globals.SafeDecimal(dr["TotalFee"], 0);
                model.UserCount = Common.Globals.SafeInt(dr["UserCount"], 0);
                list.Add(model);
                i++;
            }
            this.hfCategory.Value = String.Join(",", list.Select(c => c.ProName));
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
