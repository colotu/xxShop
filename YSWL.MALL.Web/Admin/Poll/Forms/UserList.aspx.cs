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
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Accounts.Bus;
using YSWL.Common;
using YSWL.Json;
using YSWL.Json.Conversion;

namespace YSWL.MALL.Web.Admin.Poll.Forms
{
    public partial class UserList : PageBaseAdmin
    {
        //int Act_ShowInvalid = -1; //查看失效数据行为
        protected override int Act_PageLoad { get { return 182; } } //用户管理_是否显示会员信息管理页面
 
        private YSWL.MALL.BLL.Members.Users user = new BLL.Members.Users();

        protected void Page_Load(object sender, EventArgs e)
        {
        
            if (!Page.IsPostBack)
            {
                
            }
        }

        #region 获取参数
        private int Fid
        {
            get
            {
                if (Request.QueryString["fid"] != null)
                {
                    return Globals.SafeInt(Request.QueryString["fid"], 0);
                }
                return 0;
            }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }
 
        #region 确定

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0)
            {
                MessageBox.ShowFailTip(this,"请选择一个用户");
                return;
            }
            if (idlist.Trim().Contains(","))
            {
                MessageBox.ShowFailTip(this, "只能选择一个用户");
                return;
            }
            BLL.Poll.PollUsers pollUsers = new BLL.Poll.PollUsers();
            BLL.Members.Users userBll = new BLL.Members.Users();
            int userid = Globals.SafeInt(idlist.Trim(), 0);
            if (!userBll.Exists(userid))
            {
                MessageBox.ShowFailTip(this, "请重新选择用户再试");
                return;
            }
            if (pollUsers.ExistsSysUser(userid))
            {
                MessageBox.ShowFailTip(this,"该用户已提交过问卷，请重新选择用户");
                return;
            }
            MessageBox.ShowSuccessTipScript(this, "操作成功，正在为您跳转。。。", string.Format("window.parent.location.href='/admin/Poll/Forms/submitpoll.aspx?fid={0}&uid={1}'", Fid, userid));
        }

        #endregion 
 
        #region gridView

        public void BindData()
        {
            #region

            if (!Page.IsPostBack)
            {
                return;
            }

            #endregion gridView

            DataSet ds = new DataSet();
            StringBuilder strWhere = new StringBuilder();
            if (txtKeyword.Text.Trim() != "")
            {
                strWhere.AppendFormat("( NickName like '%{0}%'  or  UserName  like '%{0}%'  or   Phone like '%{0}%'  )", Common.InjectionFilter.SqlFilter(txtKeyword.Text));
            }
            if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()))
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.Append("  convert(date,User_dateCreate)>='" + Common.InjectionFilter.SqlFilter(txtBeginTime.Text.Trim()) + "' ");
            }
            if (Common.Globals.SafeInt(dropType.SelectedValue, -1) > -1)
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.Append("  Activity=" + dropType.SelectedValue + " ");
            }
      
            if (!string.IsNullOrEmpty(txtEndTime.Text.Trim()))
            {
                string endTime = Common.Globals.SafeDateTime(this.txtEndTime.Text, DateTime.Now).AddDays(1).ToString("yyyy-MM-dd");
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.Append("  convert(date,User_dateCreate)<='" + Common.InjectionFilter.SqlFilter(endTime) + "' ");
            }
            int regionId = this.RegionID.Region_iID;
            if (regionId>0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" EXISTS (SELECT * FROM  Ms_Regions  AS regions  WHERE Accounts_UsersExp.RegionId= regions.RegionId and  ( Path like '%,{0},%' or RegionId={0}))" , regionId);
            }
            ds = user.GetSearchList("UU", strWhere.ToString());

            //ds = bll.GetList(strWhere.ToString(), UserPrincipal.PermissionsID, UserPrincipal.PermissionsID.Contains(GetPermidByActID(Act_ShowInvalid)));
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
                if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#F4F4F4");
                }
                else
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFFFFF");
                }
                //object obj1 = DataBinder.Eval(e.Row.DataItem, "Levels");
                //if ((obj1 != null) && ((obj1.ToString() != "")))
                //{
                //    e.Row.Cells[4].Text = obj1.ToString() == "0" ? "Private" : "Shared";
                //}
            }
        }

        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //int ID = (int)gridView.DataKeys[e.RowIndex].Value;
            //if (rankBll.Delete(ID))
            //{
            //    YSWL.Common.MessageBox.ShowSuccessTip(this, "删除成功！");
            //}
            gridView.OnBind();
        }
        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Status")
            {
                if (e.CommandArgument != null)
                {
                    int Id = 0;
                    string[] Args = e.CommandArgument.ToString().Split(new char[] { ',' });
                    Id = Common.Globals.SafeInt(Args[0], 0);
                    AccountsPrincipal user = new AccountsPrincipal(Id);
                    User currentUser = new YSWL.Accounts.Bus.User(user);
                    bool Status = Common.Globals.SafeBool(Args[1], false);
                    currentUser.Activity = Status ? false : true;
                    currentUser.Update();
                    gridView.OnBind();
                }
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
                        //idlist += gridView.Rows[i].Cells[1].Text + ",";
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


        protected string GetGravatar(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                string savePath = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValue("SNSGravatarPath");
                if (string.IsNullOrEmpty(savePath))
                {
                    savePath = "/Upload/User/Gravatar/";
                }
                int UserId = Common.Globals.SafeInt(target.ToString(), 0);
                return savePath + UserId + ".jpg";
            }
            return str;
        }

        BLL.Ms.Regions RegionBll = new BLL.Ms.Regions();
        public string GetRegion(object o)
        {
            if (o == null || String.IsNullOrWhiteSpace(o.ToString()))
            {
                return "";
            }
            return RegionBll.GetAddress(Globals.SafeInt(o.ToString(), null));
        }

        #endregion
 
    }
}