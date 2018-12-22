using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Members.UserRank
{
    public partial class RankLimit : PageBaseAdmin
    {
        YSWL.MALL.BLL.Members.RankLimit LimitBll = new BLL.Members.RankLimit();
        protected override int Act_PageLoad { get { return 295; } } //用户管理_积分限制管理_列表页
        protected new int Act_AddData = 298;    //用户管理_积分限制管理_新增数据
        protected new int Act_UpdateData = 299;    //用户管理_积分限制管理_编辑数据
        protected new int Act_DelData = 300;    //用户管理_积分限制管理_删除数据
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               


                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)) && GetPermidByActID(Act_AddData) != -1)
                {
                    liadd.Visible = false;
                }
            }
        }

        #region gridView

        public void BindData()
        {
            if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateData)) && GetPermidByActID(Act_UpdateData) != -1)
            {
                gridView.Columns[5].Visible = false;
            }
            if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
            {
                gridView.Columns[6].Visible = false;
            }

            //获取积分限制
            string keyWord = this.txtKeyword.Text;
            DataSet ds = LimitBll.GetListByKeyWord(keyWord);
            if (ds != null)
            {
                gridView.DataSetSource = ds;
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
        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int limitId = (int)gridView.DataKeys[e.RowIndex].Value;
            if (LimitBll.ExistsLimit(limitId))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "该限制条件已经被使用，不能删除！");
                gridView.OnBind();
                return;
            }
            LimitBll.Delete(limitId);
            gridView.OnBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            gridView.OnBind();
        }

        protected string GetUnitName(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                string CycleUnit = target.ToString();
                switch (CycleUnit)
                {
                    case "day":
                        return "日";
                    case "month":
                        return "月";
                    case "year":
                        return "年";
                    default:
                        return "未知";
                }
            }
            return str;
        }
        #endregion
    }
}