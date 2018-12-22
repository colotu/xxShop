using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.WeChat.BLL.Core;

namespace YSWL.MALL.Web.Admin.WeChat.PostMsg
{
    public partial class SetPostMsg : PageBaseAdmin
    {
        private YSWL.WeChat.BLL.Core.KeyValue valueBll = new KeyValue();
        private YSWL.WeChat.BLL.Core.PostMsg postMsgBll = new YSWL.WeChat.BLL.Core.PostMsg();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
            }
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

        protected void btnAddValue_Click(object sender, EventArgs e)
        {
            YSWL.WeChat.Model.Core.KeyValue valueModel = new YSWL.WeChat.Model.Core.KeyValue();
            valueModel.Value = this.txtValue.Text;
            valueModel.MatchType = 0;
            valueModel.RuleId = RuleId;
            if (valueBll.Add(valueModel) > 0)
            {
                MessageBox.ShowSuccessTip(this, "操作成功！");
                //Response.Redirect("GroupList.aspx");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "操作失败！");
            }
        }

        protected void btnAddMsg_Click(object sender, EventArgs e)
        {
            YSWL.WeChat.Model.Core.PostMsg postMsgModel = new YSWL.WeChat.Model.Core.PostMsg();
            postMsgModel.Description = this.txtPostMsg.Text;
            postMsgModel.RuleId = RuleId;
            postMsgModel.MsgType = "text";
            postMsgModel.CreateTime = DateTime.Now;
            if (postMsgBll.Add(postMsgModel) > 0)
            {
                MessageBox.ShowSuccessTip(this,"操作成功！");
                //Response.Redirect("GroupList.aspx");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "操作失败！");
            }
        }
    
    }
}