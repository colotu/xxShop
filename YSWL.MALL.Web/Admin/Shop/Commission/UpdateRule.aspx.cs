using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.Commission
{
    public partial class UpdateRule : PageBaseAdmin
    {
        YSWL.MALL.BLL.Shop.Commission.CommissionRule ruleBll = new BLL.Shop.Commission.CommissionRule();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowInfo(RuleId);
            }
        }

        private void ShowInfo(int RuleId)
        {
            //规则信息
            YSWL.MALL.Model.Shop.Commission.CommissionRule ruleModel = ruleBll.GetModel(RuleId);
            this.txtRuleName.Text = ruleModel.RuleName;
            this.radMode.SelectedValue = ruleModel.RuleMode.ToString();
            this.chkStatus.Checked = ruleModel.Status == 1;
            this.txtFirst.Text = ruleModel.FirstValue.ToString("F");
            this.txtSecond.Text = ruleModel.SecondValue.ToString("F");
            this.txtThird.Text = ruleModel.ThirdValue.ToString("F");
            this.txtFourth.Text = ruleModel.FourthValue.ToString("F");
            this.txtFifth.Text = ruleModel.FifthValue.ToString("F");
            this.chkIsAll.Checked = ruleModel.IsAll; 
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.Commission.CommissionRule ruleModel = ruleBll.GetModel(RuleId);
            ruleModel.RuleName = this.txtRuleName.Text;
            ruleModel.RuleMode = Common.Globals.SafeInt(this.radMode.SelectedValue, 0);
            ruleModel.Status = this.chkStatus.Checked ? 1 : 0;
            ruleModel.CreatedDate = DateTime.Now;
            ruleModel.CreatedUserID = CurrentUser.UserID;
            ruleModel.FirstValue = Common.Globals.SafeDecimal(this.txtFirst.Text, 0);
            ruleModel.SecondValue = Common.Globals.SafeDecimal(this.txtSecond.Text, 0);
            ruleModel.ThirdValue = Common.Globals.SafeDecimal(this.txtThird.Text, 0);
            ruleModel.FourthValue = Common.Globals.SafeDecimal(this.txtFourth.Text, 0);
            ruleModel.FifthValue = Common.Globals.SafeDecimal(this.txtFifth.Text, 0);
            ruleModel.IsAll = chkIsAll.Checked;
            //新增批发规则
            if (ruleBll.Update(ruleModel))
            {
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
            }
            else
            {
                MessageBox.ShowFailTipScript(this, "操作失败，请稍候再试！", "window.parent.location.reload();");
            }

        }

        public int RuleId
        {
            get
            {
                int ruleId = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["id"]))
                {
                    ruleId = Globals.SafeInt(Request.Params["id"], 0);
                }
                return ruleId;
            }
        }
    }
}