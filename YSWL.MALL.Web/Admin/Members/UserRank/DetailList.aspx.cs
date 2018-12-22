using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Members.UserRank
{
    public partial class DetailList : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Members.RankDetail detailBll = new YSWL.MALL.BLL.Members.RankDetail();
        private YSWL.MALL.BLL.Members.RankRule ruleBll = new BLL.Members.RankRule();
        private YSWL.MALL.BLL.Members.Users userBll = new BLL.Members.Users();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
              
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        #region gridView


        public void BindData()
        {
            StringBuilder strWhere = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(this.txtFrom.Text) && Common.PageValidate.IsDateTime(this.txtFrom.Text))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat(" CreatedDate >='" + Common.InjectionFilter.SqlFilter(this.txtFrom.Text) + "' ");
            }
            //时间段
            if (!String.IsNullOrWhiteSpace(this.txtTo.Text) && Common.PageValidate.IsDateTime(this.txtTo.Text))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                if (YSWL.DBUtility.PubConstant.IsSQLServer)
                {
                    strWhere.AppendFormat("  CreatedDate< dateadd(day,1,'{0}')", txtTo.Text.Trim());
                }
                else
                {
                    strWhere.AppendFormat("  CreatedDate< DATE_ADD('{0}',INTERVAL 1 DAY)", txtTo.Text.Trim());
                }
            }
            gridView.DataSetSource = detailBll.GetList(-1, strWhere.ToString(), " CreatedDate desc");
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



        protected string GetRuleName(object target)
        {
            //0:取消关注、1:关注、
            string str = "未知";
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int ruleId = Common.Globals.SafeInt(target, 0);
                str = ruleBll.GetRuleName(ruleId);
            }
            return str;
        }

        protected string GetUserName(object target)
        {
            //0:取消关注、1:关注、
            string str = "";
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int userId = Common.Globals.SafeInt(target, -1);
                YSWL.MALL.Model.Members.Users userModel = userBll.GetModelByCache(userId);
                str = userModel == null ? str : userModel.UserName;
            }
            return str;
        }

        #endregion

    }
}