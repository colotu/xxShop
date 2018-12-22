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
    public partial class Add : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                       
        }

        		protected void btnSave_Click(object sender, EventArgs e)
		{
            string ruleName = this.txtRuleName.Text.Trim();
            if (ruleName.Trim().Length == 0)
			{
                MessageBox.ShowFailTip(this, "规则名称不能为空");
                return;
			}
            if (ruleName.Length >200)
            {
                MessageBox.ShowFailTip(this, "规则名称过长");
                return;
            }
            if (txtPriority.Text.Trim().Length <= 0) {
                MessageBox.ShowFailTip(this, "优先级不能为空");
                return;
            }
            int priority = Common.Globals.SafeInt(txtPriority.Text, 0);
            if (priority<=0)
			{
                MessageBox.ShowFailTip(this, "优先级格式错误");
                return;
			}
            int Status = checkboxStatus.Checked?1:0;
            YSWL.MALL.Model.Shop.Activity.ActivityRule model = new YSWL.MALL.Model.Shop.Activity.ActivityRule();
			model.RuleName=ruleName;
			model.Priority=priority;
            model.Status = Status;
            model.CreatedUserId =CurrentUser.UserID;
            model.CreatedDate =DateTime.Now;

            YSWL.MALL.BLL.Shop.Activity.ActivityRule bll = new YSWL.MALL.BLL.Shop.Activity.ActivityRule();
            if (bll.Add(model)>0) {
                DataCache.DeleteCache("GetAvailable_RuleList");         
                YSWL.Common.MessageBox.ShowSuccessTip(this, "保存成功！", "list.aspx");
            }else{
                YSWL.Common.MessageBox.ShowFailTip(this, "保存失败！");
            }
		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
