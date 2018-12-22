/**
* List.cs
*
* 功 能： [N/A]
* 类 名： List.cs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using YSWL.Common;
using System.Drawing;
using YSWL.Accounts.Bus;
using YSWL.Json;

namespace YSWL.MALL.Web.Admin.Members.UserInvite
{
    public partial class List : PageBaseAdmin
    {
        //int Act_ShowInvalid = -1; //查看失效数据行为

        protected override int Act_PageLoad { get { return 687; } }//邀请管理_列表页
        protected new int Act_DelData = 688;    //邀请管理_删除数据 
		YSWL.MALL.BLL.Members.UserInvite bll = new YSWL.MALL.BLL.Members.UserInvite();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))
            {
                this.Controls.Clear();
                this.DoCallback();
            }
            if (!Page.IsPostBack)
            {
                btnDelete.Attributes.Add("onclick", "return confirm(\"" + Resources.Site.TooltipDelConfirm + "\")");
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
                {
                    btnDelete.Visible = false;
                }
                

              

                gridView.OnBind();
            }
        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }
        
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if(idlist.Trim().Length == 0) return;
            if (bll.DeleteList(idlist))
            {
                MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
                gridView.OnBind();
            }
            else
            {
                MessageBox.ShowFailTip(this, Resources.Site.TooltipDelError);
            }
        }
        
        #region gridView
                        
        public void BindData()
        {
           
            DataSet ds = new DataSet();
            StringBuilder strWhere = new StringBuilder();
            if (txtKeyword.Text.Trim() != "")
            {
                strWhere.AppendFormat(" InviteNick like '%{0}%'", Common.InjectionFilter.SqlFilter(txtKeyword.Text.Trim()));
            }            
            ds = bll.GetList(0,strWhere.ToString()," InviteId desc");
            gridView.DataSetSource = ds;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("style", "background:#FFF");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
         
            }
        }
       
        private string GetSelIDlist()
        {
            string idlist = "";
            bool BxsChkd = false;
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                CheckBox ChkBxItem = (CheckBox)gridView.Rows[i].FindControl(gridView.CheckBoxID);
                if (ChkBxItem != null && ChkBxItem.Checked)
                {
                    BxsChkd = true;
                    if (gridView.DataKeys[i].Value != null)
                    {
                        idlist += gridView.DataKeys[i].Value.ToString() + ",";
                    }
                }
            }
            if (BxsChkd)
            {
                idlist = idlist.TrimEnd(',');
            }
            return idlist;
        }

        #endregion

        #region 状态

        public string GetStatus(object obj)
        {
            if (obj == null)
                return string.Empty;
            int Status = Common.Globals.SafeInt(obj.ToString(), -1);
            if (Status < 0) return string.Empty;
            switch (Status)
            {
                //0:即将进行 1:进行中(立即申请) 2:已结束
                case 0:
                    return "不启用";
                case 1:
                    return "启用";
                default:
                    return "不启用";
            }
        }

        #endregion

        #region AjaxCallback

        private void DoCallback()
        {
            string action = this.Request.Form["Action"];
            this.Response.Clear();
            this.Response.ContentType = "application/json";
            string writeText = string.Empty;

            switch (action)
            {
                case "UpdateStatus":
                    writeText = UpdateStatus();
                    break;
                default:
                    writeText = UpdateStatus();
                    break;
            }
            this.Response.Write(writeText);
            this.Response.End();
        }

        private string UpdateStatus()
        {
            JsonObject json = new JsonObject();
            int InviteId = Common.Globals.SafeInt(this.Request.Form["InviteId"], 0);
            int Status = Common.Globals.SafeInt(this.Request.Params["Status"], 0);
            if (bll.UpdateStatus(InviteId, Status))
            {
                json.Put("STATUS", "SUCCESS");
            }
            else
            {
                json.Put("STATUS", "FAILED");
            }
            return json.ToString();
        }
        #endregion AjaxCallback



    }
}
