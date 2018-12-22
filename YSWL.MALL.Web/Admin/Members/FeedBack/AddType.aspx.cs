using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Members.FeedBack
{
    public partial class AddType : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 280; } } 
        YSWL.MALL.BLL.Members.FeedbackType typeBll = new BLL.Members.FeedbackType();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string Name = this.txtName.Text.Trim();
            if (Name.Length == 0)
            {
                MessageBox.ShowServerBusyTip(this, "标签的名称不能为空");
                return;
            }

            YSWL.MALL.Model.Members.FeedbackType typeModel = new Model.Members.FeedbackType();
            typeModel.TypeName = Name;
            typeModel.Description = this.txtDesc.Text;

            if (typeBll.Add(typeModel) > 0)
            {
                LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, "新增反馈类型成功", this);
                MessageBox.ShowSuccessTip(this, Resources.Site.TooltipSaveOK, "TypeList.aspx");
            }
            else
            {
                MessageBox.ShowFailTip(this, Resources.Site.TooltipSaveError);
            }
        }


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("TypeList.aspx");
        }
    }
}
