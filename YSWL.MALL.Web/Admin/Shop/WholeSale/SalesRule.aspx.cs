﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.Json;

namespace YSWL.MALL.Web.Admin.Shop.WholeSale
{
    public partial class SalesRule : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 553; } } //Shop_批发规则管理_列表页
        protected new int Act_AddData = 554;    //Shop_批发规则管理_新增数据
        protected new int Act_UpdateData = 555;    //Shop_批发规则管理_编辑数据
        protected new int Act_DelData = 556;    //Shop_批发规则管理_删除数据
        YSWL.MALL.BLL.Shop.Sales.SalesRule ruleBll = new BLL.Shop.Sales.SalesRule();

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
                    liDel.Visible = false;
                    btnDelete.Visible = false;
                }
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)) && GetPermidByActID(Act_AddData) != -1)
                {
                    liAdd.Visible = false;
                }
            
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        public int Type
        {
            get
            {
                return   Globals.SafeInt(Request.QueryString["id"], 0);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0) return;
            ruleBll.DeleteListEx(idlist);
            YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
            gridView.OnBind();
        }

        #region gridView

        public void BindData()
        {
            DataSet ds = new DataSet();
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat("Type={0}  ", Type);
            if (txtKeyword.Text.Trim() != "")
            {
                strWhere.AppendFormat(" and  RuleName like '%{0}%'", Common.InjectionFilter.SqlFilter(txtKeyword.Text.Trim()));
            }
            ds = ruleBll.GetList(-1, strWhere.ToString(), " CreatedDate desc");
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
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
                {
                    LinkButton delbtn = (LinkButton)e.Row.FindControl("linkDel");
                    delbtn.Visible = false;
                }
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateData)) && GetPermidByActID(Act_UpdateData) != -1)
                {
                    HtmlGenericControl updatebtn = (HtmlGenericControl)e.Row.FindControl("lbtnModify");
                    updatebtn.Visible = false;
                }

                if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#F4F4F4");
                }
                else
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFFFFF");
                }
            }
        }

        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ID = (int)gridView.DataKeys[e.RowIndex].Value;
            ruleBll.DeleteEx(ID);
            gridView.OnBind();
            Common.MessageBox.ShowSuccessTip(this, "删除成功", "SalesRule.aspx");
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
                idlist = idlist.Substring(0, idlist.LastIndexOf(","));
            }
            return idlist;
        }

        #endregion


        #region 应用方式

        public string GetRuleMode(object obj)
        {
            if (obj == null)
                return string.Empty;
            int RuleMode = Common.Globals.SafeInt(obj.ToString(), -1);
            if (RuleMode < 0) return string.Empty;
            switch (RuleMode)
            {
                //0:即将进行 1:进行中(立即申请) 2:已结束
                case 0:
                    return "单个商品";
                case 1:
                    return "商品总计";
                default:
                    return "单个商品";
            }
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

        public string GetCreatedName(object obj)
        {
            if (obj == null)
                return string.Empty;
            int userId = Common.Globals.SafeInt(obj.ToString(), -1);
            if (userId < 0) return string.Empty;
            YSWL.Accounts.Bus.User userModel = new YSWL.Accounts.Bus.User(userId);

            return userModel.UserName;
        }

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
            int RuleId = Common.Globals.SafeInt(this.Request.Form["RuleId"], 0);
            int Status = Common.Globals.SafeInt(this.Request.Params["Status"], 0);
            if (ruleBll.UpdateStatus(RuleId, Status))
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
