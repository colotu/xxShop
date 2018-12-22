using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.WeChat.Group
{
    public partial class UpdateGroup : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 637; } } //移动营销_用户小组管理_编辑页
        private YSWL.WeChat.BLL.Core.Group groupBll = new YSWL.WeChat.BLL.Core.Group();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                YSWL.WeChat.Model.Core.Group groupModel = groupBll.GetModel(GroupId);
                if (groupModel != null)
                {
                    this.tName.Text = groupModel.GroupName;
                    this.tDesc.Text = groupModel.Remark;
                }
            }
        }

        #region 编号

        /// <summary>
        /// 编号
        /// </summary>
        protected int GroupId
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string openId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", -1, CurrentUser.UserType);
            YSWL.WeChat.Model.Core.Group groupModel = groupBll.GetModel(GroupId);
            if (String.IsNullOrWhiteSpace(this.tName.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "请填写分组名称！");
                return;
            }
            groupModel.GroupName = this.tName.Text;
            groupModel.Remark = this.tDesc.Text;
            groupModel.OpenId = openId;
            if (groupBll.Update(groupModel))
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