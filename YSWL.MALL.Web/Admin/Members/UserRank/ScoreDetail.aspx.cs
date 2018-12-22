using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Accounts.Bus;

namespace YSWL.MALL.Web.Admin.Members.UserRank
{
    public partial class ScoreDetail : PageBaseAdmin
    {
        private User currentUser;
        protected override int Act_PageLoad { get { return 301; } } //用户管理_积分详情页
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ((Request.Params["userid"] != null) && (Request.Params["userid"].ToString() != ""))
                {
                    if (Common.PageValidate.IsNumber(Request.Params["userid"]))
                    {
                        int userid = int.Parse(Request.Params["userid"]);
                        currentUser = new User(userid);
                        if (currentUser == null)
                        {
                            Response.Write("<script language=javascript>window.alert('" + Resources.Site.TooltipUserExist + "\\');history.back();</script>");
                            return;
                        }
                        this.txtUserName.Text = currentUser.NickName + "的成长值明细";

                   
                    }
                }
            }
        }

        #region gridView

        public void BindData()
        {
            if ((Request.Params["userid"] != null) && (Request.Params["userid"].ToString() != ""))
            {
                if (Common.PageValidate.IsNumber(Request.Params["userid"]))
                {
                    int userid = int.Parse(Request.Params["userid"]);
                    int type = Common.Globals.SafeInt(DropPointsType.SelectedValue,0);
                    DataSet ds = new DataSet();
                    YSWL.MALL.BLL.Members.RankDetail points = new BLL.Members.RankDetail();
                    //获取成长值明细
                    if (type != -1)
                    {
                        ds = points.GetList(" UserId=" + userid + " and type=" + type);
                    }
                    else
                    {
                        ds = points.GetList(" UserId=" + userid);
                    }
                    if (ds != null)
                    {
                        gridView.DataSetSource = ds;
                    }
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

        //返回处理
        public void btnReturn_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("/Admin/Accounts/Admin/UserAdmin.aspx");
        }
        /// <summary>
        /// 根据积分类型获取积分名字
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetRuleName(object rule)
        {
            int ruleId = Common.Globals.SafeInt(rule, 0);
            if (ruleId == (int)YSWL.MALL.Model.Members.Enum.PointRule.Order)
            {
                return "完成订单";
            }
            YSWL.MALL.BLL.Members.RankRule RuleBll = new BLL.Members.RankRule();
            return RuleBll.GetRuleName(ruleId);
        }

        public string GetTypeName(int type)
        {
            return type == 0 ? "成长值获取" : "成长值扣除";
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }
    }
}