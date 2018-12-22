using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web.Admin.Members.UserRank
{
    public partial class AddRule : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 290; } }
        private YSWL.MALL.BLL.Members.RankRule RuleBll = new BLL.Members.RankRule();
        private YSWL.MALL.BLL.Members.RankLimit LimitBll = new BLL.Members.RankLimit();
        private YSWL.MALL.BLL.Members.RankAction actionBll = new BLL.Members.RankAction();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
            }
        }

        public void btnSave_Click(object sender, System.EventArgs e)
        {
            int actionId = Common.Globals.SafeInt(ddlAction.SelectedValue, 0);
            if (actionId == 0)
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "请选择该规则对应的操作！");
                return;
            }
            if (RuleBll.Exists(actionId,0,0))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "已存在该规则操作，请重新选择");
                return;
            }

            YSWL.MALL.Model.Members.RankRule RuleModel = new Model.Members.RankRule();
            RuleModel.ActionId = actionId;
            RuleModel.Name = this.tName.Text.Trim();
            RuleModel.LimitID = Convert.ToInt32(DropLimitName.SelectedValue);
            RuleModel.Description = this.tDesc.Text.Trim();
            RuleModel.TargetId = 0;
            RuleModel.TargetType = 0;
            if (Common.PageValidate.IsNumber(this.tScore.Text.Trim()))
            {
                RuleModel.Score = Common.Globals.SafeInt(this.tScore.Text.Trim(), 0);
            }
            if (RuleBll.Add(RuleModel) > 0)
            {
                Response.Redirect("RankRule.aspx");
            }
            else
            {
                this.lblMsg.ForeColor = Color.Red;
                this.lblMsg.Text = "新增规则出错！";
            }
        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("RankRule.aspx");
        }
    }
}