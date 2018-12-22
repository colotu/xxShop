using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using System.Drawing;
using System.Data;
using System.Text;
using YSWL.Json;
using YSWL.MALL.Model.Members.Enum;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class UserSource : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Members.UsersExp userExBll = new BLL.Members.UsersExp();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_PageLoad)) && GetPermidByActID(Act_PageLoad) != -1)
                {
                    MessageBox.ShowAndBack(this, "您没有权限查看此页面");
                    return;
                }
                this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtStartDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
           
            }
        }

        protected void btnReStatistic_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }


        #region gridView

        public void BindData()
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" UserType ='UU' ");
            if (!String.IsNullOrWhiteSpace(this.txtStartDate.Text) && Common.PageValidate.IsDateTime(this.txtStartDate.Text))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat(" User_dateCreate >='" +Common.InjectionFilter.SqlFilter( this.txtStartDate.Text )+ "' ");
            }
            //时间段
            if (!String.IsNullOrWhiteSpace(this.txtEndDate.Text) && Common.PageValidate.IsDateTime(this.txtEndDate.Text))
            {
                string endTime = Common.Globals.SafeDateTime(this.txtEndDate.Text, DateTime.Now).AddDays(1).ToString("yyyy-MM-dd");
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat(" User_dateCreate < '" + endTime + "' ");
            }
            DataSet ds = userExBll.SourceCount(strWhere.ToString());
            gridView.DataSetSource = ds;
            JsonArray newsArry = new JsonArray();
            JsonObject itemObj = null;
            this.hfData.Value = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    itemObj = new JsonObject();
                    int count = Common.Globals.SafeInt(ds.Tables[0].Rows[i]["Count"].ToString(), 0);
                    int type = Common.Globals.SafeInt(ds.Tables[0].Rows[i]["SourceType"].ToString(), 0);
                    string name = GetSourceType(type);
                    itemObj.Accumulate("name", name);
                    itemObj.Accumulate("y", count);
                    newsArry.Add(itemObj);
                }
                this.hfData.Value = newsArry.ToString();
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



        #endregion gridView

        public string GetSourceType(object target)
        {
            int type = Common.Globals.SafeInt(target, 0);
            SourceType sourceType = (YSWL.MALL.Model.Members.Enum.SourceType)type;
            string str = "未知来源";
            switch (sourceType)
            {
                case SourceType.PC:
                    str = "PC注册";
                    break;
                case SourceType.WeChat:
                    str = "微信注册";
                    break;
                case SourceType.Ding:
                    str = "订货注册";
                    break;
                case SourceType.SalesMan:
                    str = "业务员代替注册";
                    break;
                case SourceType.Cust:
                    str = "客服代注册";
                    break;
                default:
                    str = "未知来源";
                    break;
            }
            return str;
        }
    }
}