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
    public partial class AddValue : PageBaseAdmin
    {
        private YSWL.WeChat.BLL.Core.KeyValue valueBll=new KeyValue();
        protected void Page_Load(object sender, EventArgs e)
        {

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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.WeChat.Model.Core.KeyValue valueModel = new YSWL.WeChat.Model.Core.KeyValue();
            valueModel.Value = this.tName.Text;
            valueModel.MatchType = 0;
            valueModel.RuleId = RuleId;
            if (valueBll.Add(valueModel)>0)
            {
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "$('#txtResetLoad', window.parent.document).click();");
                //Response.Redirect("GroupList.aspx");
            }
            else
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, "操作失败！");
            }
        }
    }
}