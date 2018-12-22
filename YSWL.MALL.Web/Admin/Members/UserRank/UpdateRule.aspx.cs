using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Members.UserRank
{
    public partial class UpdateRule : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Members.RankRule RuleBll = new BLL.Members.RankRule();
        private YSWL.MALL.BLL.Members.RankLimit LimitBll = new BLL.Members.RankLimit();
        private YSWL.MALL.BLL.Members.RankAction actionBll = new BLL.Members.RankAction();
        protected override int Act_PageLoad { get { return 291; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                YSWL.MALL.Model.Members.RankRule RuleModel = RuleBll.GetModel(RuleId);
                if (RuleModel == null)
                {
                    Response.Redirect("RankRule.aspx");
                }

                DropLimitName.DataSource = LimitBll.GetAllList();
                DropLimitName.DataTextField = "Name";
                DropLimitName.DataValueField = "LimitID";
                DropLimitName.DataBind();
                DropLimitName.Items.Insert(0, new ListItem("无限制", "-1"));

                ddlAction.DataSource = actionBll.GetAllList();
                ddlAction.DataTextField = "Name";
                ddlAction.DataValueField = "ActionId";
                ddlAction.DataBind();
                ddlAction.Items.Insert(0, new ListItem("请选择", "0"));

                ddlAction.SelectedValue = RuleModel.ActionId.ToString();
                tName.Text = RuleModel.Name;
                tScore.Text = RuleModel.Score.ToString();
                tDesc.Text = RuleModel.Description;
                DropLimitName.SelectedValue = RuleModel.LimitID.ToString();

            }

        }


        #region 编号

        /// <summary>
        /// 编号
        /// </summary>
        protected int RuleId
        {
            get
            {
                int id = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["id"]))
                {
                    id = Globals.SafeInt(Request.Params["id"], 0);
                }
                return id;
            }
        }

        #endregion

        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, System.EventArgs e)
        {

            YSWL.MALL.Model.Members.RankRule RuleModel = RuleBll.GetModel(RuleId);
            RuleModel.Name = this.tName.Text.Trim();
            RuleModel.LimitID = Convert.ToInt32(DropLimitName.SelectedValue);
            RuleModel.Description = this.tDesc.Text.Trim();
            if (Common.PageValidate.IsNumber(this.tScore.Text.Trim()))
            {
                RuleModel.Score = Common.Globals.SafeInt(this.tScore.Text.Trim(), 0);
            }
            if (RuleBll.Update(RuleModel))
            {
                Response.Redirect("RankRule.aspx");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "修改规则失败！");
            }

        }

        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("RankRule.aspx");
        }

    }
}