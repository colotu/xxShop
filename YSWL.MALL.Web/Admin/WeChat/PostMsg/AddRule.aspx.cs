using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.WeChat.BLL.Core;

namespace YSWL.MALL.Web.Admin.WeChat.PostMsg
{
    public partial class AddRule : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 642; } } //移动营销_智能客服设置管理_新增页
        YSWL.WeChat.BLL.Core.KeyRule ruleBll = new KeyRule();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string openId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", -1, CurrentUser.UserType);
                if (String.IsNullOrWhiteSpace(openId))
                {
                    MessageBox.ShowFailTipScript(this, "您还未填写微信原始ID，请在公众号配置中填写！", "window.parent.location.href='/admin/WeChat/Setting/Config.aspx';");
                }
            }

        }

    
        //新增规则
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string openId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", -1, CurrentUser.UserType);
            if (String.IsNullOrWhiteSpace(this.tName.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "请填写规则名称！");
                return;
            }
            YSWL.WeChat.Model.Core.KeyRule ruleModel = new YSWL.WeChat.Model.Core.KeyRule();
            ruleModel.Name = this.tName.Text;
            ruleModel.Remark = this.tDesc.Text;
            ruleModel.OpenId = openId;
            if (ruleBll.Add(ruleModel)>0)
            {
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "操作失败！");
            }

        }
    }
}