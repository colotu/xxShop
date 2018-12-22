using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using YSWL.Common;
namespace YSWL.MALL.Web.Shop.Products.ActivityRule
{
    public partial class Modify : PageBaseAdmin
    {
        BLL.Members.Users usersBll = new BLL.Members.Users();
        YSWL.MALL.BLL.Shop.Activity.ActivityRule bll = new YSWL.MALL.BLL.Shop.Activity.ActivityRule();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (RuleId <= 0)
                {
                    MessageBox.ShowAndRedirect(this, "该信息不存在或已被删除！", "list.aspx");
                    return;
                }
                ShowInfo();
            }
        }
        private int RuleId
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
        private void ShowInfo()
        {
            YSWL.MALL.Model.Shop.Activity.ActivityRule model = bll.GetModel(RuleId);
            if (model != null)
            {
                this.lblRuleId.Text = model.RuleId.ToString();
                this.txtRuleName.Text = model.RuleName;
                this.txtPriority.Text = model.Priority.ToString();
                this.checkboxStatus.Checked = model.Status == 1 ? true : false;
                this.lblCreatedUserId.Text = usersBll.GetUserName(model.CreatedUserId);
                this.lblCreatedDate.Text = model.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                MessageBox.ShowAndRedirect(this, "该信息不存在或已被删除！", "list.aspx");
                return;
            }
        }

		public void btnSave_Click(object sender, EventArgs e)
		{
            string ruleName = this.txtRuleName.Text.Trim();
            if (ruleName.Trim().Length == 0)
            {
                MessageBox.ShowFailTip(this, "规则名称不能为空");
                return;
            }
            if (ruleName.Length > 200)
            {
                MessageBox.ShowFailTip(this, "规则名称过长");
                return;
            }
            if (txtPriority.Text.Trim().Length <= 0)
            {
                MessageBox.ShowFailTip(this, "优先级不能为空");
                return;
            }
            int priority = Common.Globals.SafeInt(txtPriority.Text, 0);
            if (priority <= 0)
            {
                MessageBox.ShowFailTip(this, "优先级格式错误");
                return;
            }
            int Status = checkboxStatus.Checked ? 1 : 0;
	 
			string RuleName=this.txtRuleName.Text;
			int Priority=int.Parse(this.txtPriority.Text);

            YSWL.MALL.Model.Shop.Activity.ActivityRule model = bll.GetModelByCache(RuleId);
            if (model != null)
            {
                model.RuleName = RuleName;
                model.Priority = Priority;
                model.Status = Status;
                if (bll.Update(model)) {
                    DataCache.DeleteCache("GetAvailable_RuleList");         
                    DataCache.DeleteCache("ActivityRuleModel-" + RuleId);
                    YSWL.Common.MessageBox.ShowSuccessTip(this, "保存成功！", "list.aspx");
                } else{
                    YSWL.Common.MessageBox.ShowFailTip(this, "保存失败！");
                }
            }
            else
            {
                MessageBox.ShowAndRedirect(this, "该信息不存在或已被删除！", "list.aspx");
                return;
            }
		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
