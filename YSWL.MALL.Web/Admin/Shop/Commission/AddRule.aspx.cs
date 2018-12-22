using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.Commission
{
    public partial class AddRule : PageBaseAdmin
    {
        YSWL.MALL.BLL.Shop.Commission.CommissionRule ruleBll = new BLL.Shop.Commission.CommissionRule();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.Commission.CommissionRule ruleModel = new Model.Shop.Commission.CommissionRule();
            ruleModel.RuleName = this.txtRuleName.Text;
            ruleModel.RuleMode = Common.Globals.SafeInt(this.radMode.SelectedValue, 0);
            ruleModel.Status = chkStatus.Checked ? 1 : 0;
            ruleModel.CreatedDate = DateTime.Now;
            ruleModel.CreatedUserID = CurrentUser.UserID;
            ruleModel.FirstValue = Common.Globals.SafeDecimal(this.txtFirst.Text, 0);
            ruleModel.SecondValue = Common.Globals.SafeDecimal(this.txtSecond.Text, 0);
            ruleModel.ThirdValue = Common.Globals.SafeDecimal(this.txtThird.Text, 0);
            ruleModel.FourthValue = Common.Globals.SafeDecimal(this.txtFourth.Text, 0);
            ruleModel.FifthValue = Common.Globals.SafeDecimal(this.txtFifth.Text, 0);
            ruleModel.IsAll = chkIsAll.Checked;
            //新增批发规则
            if (ruleBll.Add(ruleModel)>0)
            {
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
            }
            else
            {
                MessageBox.ShowFailTipScript(this, "操作失败，请稍候再试！", "window.parent.location.reload();");
            }
        }
    }
}