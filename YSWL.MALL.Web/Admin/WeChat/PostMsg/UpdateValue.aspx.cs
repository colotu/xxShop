using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.WeChat.PostMsg
{
    public partial class UpdateValue : PageBaseAdmin
    {
        private YSWL.WeChat.BLL.Core.KeyValue valueBll = new YSWL.WeChat.BLL.Core.KeyValue();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                YSWL.WeChat.Model.Core.KeyValue postMsgModel = valueBll.GetModel(ValueId);
                if (postMsgModel != null)
                {
                    this.tName.Text = postMsgModel.Value;
                }
            }
        }

        #region 编号

        /// <summary>
        /// 编号
        /// </summary>
        protected int ValueId
        {
            get
            {
                int id = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["valueId"]))
                {
                    id = Globals.SafeInt(Request.Params["valueId"], 0);
                }
                return id;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.WeChat.Model.Core.KeyValue postMsgModel = valueBll.GetModel(ValueId);
            postMsgModel.Value = this.tName.Text;
            if (valueBll.Add(postMsgModel) > 0)
            {
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "$('#txtResetLoad', window.parent.document).click();");
                //Response.Redirect("GroupList.aspx");
            }
            else
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, "操作失败！");
            }
        }
        #endregion
    }
}