using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Accounts.Bus;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Ms.EmailRole
{
    public partial class EmailRoleAction : PageBaseAdmin
    {
        private Role currentRole;
        public string RoleID = "";
        protected override int Act_PageLoad { get { return 199; } }  //系统管理_角色管理_编辑页面 

        protected int Act_ShowReservedPerm = 14; //查看系统保留权限

        protected new int Act_UpdateData = 33;    //系统管理_角色管理_修改角色
        protected new int Act_DelData = 34;    //系统管理_用户管理_删除角色
        protected int Act_DelUserData = 35;    //系统管理_用户管理_从角色中移除用户
        protected int Act_UpdateUserData = 36;    //系统管理_用户管理_保存角色所拥有的权限

        private List<string> ReservedRoleIDs = YSWL.Common.StringPlus.GetStrArray(BLL.SysManage.ConfigSystem.GetValueByCache("ReservedRoleIDs"), ',', true);
        private string ReservedPermIDs = BLL.SysManage.ConfigSystem.GetValueByCache("ReservedPermIDs");
        private User bllUser = new YSWL.Accounts.Bus.User();
        private  YSWL.Accounts.Bus.Role role=new Role();
        private BLL.Ms.EmailRole.EmailRoleAction emailRoleBLL=new BLL.Ms.EmailRole.EmailRoleAction();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               // gridView.OnBind();
                DoInitialDataBind();
                FillCategoryList();
                 lblRoleID.Text =CategoryDownList.Items[0].Value;
                //CategoryDownList_SelectedIndexChanged(sender, e);
            }
        }

        #region 权限设置

        //绑定角色列表
        private void DoInitialDataBind()
        {
            DataSet dataSet = role.GetRoleList();
            if (null != dataSet)
            {
                CategoryDownList.DataSource = dataSet.Tables[0];
                CategoryDownList.DataTextField = "Description";
                CategoryDownList.DataValueField = "RoleID";
                CategoryDownList.DataBind();
            }
        }
        

        //选择类别，填充2个listbox
        public void CategoryDownList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BindData();
        }

        //填充邮件类型列表
        public void FillCategoryList()
        {
            foreach (int enumValue in Enum.GetValues(typeof(YSWL.MALL.Model.Ms.EmailRole.EnumEmailType.EmailType)))
            {
                string strName = Enum.GetName(typeof(Model.Ms.EmailRole.EnumEmailType.EmailType), enumValue);//获取名称
                chkPermissions.Items.Add(new ListItem(GetEnumValue(strName), enumValue.ToString()));
            }
        }
        public string GetEnumValue(string enumtype)
        {
            switch (enumtype)
            {
                case "OrderCount":
                    return "日/月 订单数";
                case "FavCount":
                    return "关注人数";
                case "ActivityUserCount":
                    return "当月活跃客户数";
                case "UserActivityFrequency":
                    return "客户当月活跃频次";
                case "SalePerDayAndMonthAct":
                    return "业务员每日业绩统计报表与月活跃数";
                case "SalePerDay":
                    return "业务员每日业绩统计报表";
                default:
                    return "默认邮件";
            }
        }


        public void chkPermissions_DataBinding(object sender, EventArgs e)
        {
            //foreach (ListItem item in ((CheckBoxList)sender).Items)
            //{
            //    if (rolePermissionlist.Contains(Convert.ToInt32(item.Value)))
            //    {
            //        item.Selected = true;
            //    }
            //}
        }

        public void btnSave_Click(object sender, System.EventArgs e)
        {
            
            if (CategoryDownList.SelectedIndex < 0)
            {
                MessageBox.ShowFailTip(this,"请先选择角色");
                return ;
            }
            int roleId = Globals.SafeInt(CategoryDownList.SelectedValue, -1);
            emailRoleBLL.Delete(roleId);
            //if (!emailRoleBLL.Delete(roleId))//删除失败
            //{
            //    MessageBox.ShowFailTip(this, "保存失败，请重试！");
            //    return;
            //}
            List<int> list = new List<int>();
            foreach (ListItem item in chkPermissions.Items)
            {
                if (item.Selected)
                {
                    list.Add(Convert.ToInt32(item.Value));
                }
            }
            if (list.Count < 1)
            {
                MessageBox.ShowFailTip(this, "请先选择邮件类型");
                return;
            }
            foreach (var itemId in list)
            {
                if (emailRoleBLL.Add(roleId, itemId) < 1)
                {
                    MessageBox.ShowFailTip(this, "保存失败，请重新操作");
                    return;
                }
            }
            MessageBox.ShowSuccessTip(this,"保存成功");

        }

        #region 角色编辑

        public void RemoveRoleButton_Click(object sender, System.EventArgs e)
        {
            if (ReservedRoleIDs.Contains(lblRoleID.Text))
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.ErrorCannotDeleteRole);
                return;
            }
            int currentRole = Convert.ToInt32(lblRoleID.Text);
            Role bizRole = new Role(currentRole);
            bizRole.Delete();
            LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, string.Format("删除角色：【{0}】", this.TxtNewname.Text.Trim()), this);
            Server.Transfer("RoleAdmin.aspx");
        }

        public void BtnUpName_Click(object sender, EventArgs e)
        {
            string newname = this.TxtNewname.Text.Trim();
            currentRole = new Role(Convert.ToInt32(lblRoleID.Text));
            currentRole.Description = newname;
            currentRole.Update();
            DoInitialDataBind();
            LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, string.Format("编辑角色：【{0}】", newname), this);
            lblTiptool.Text = Resources.Site.TooltipUpdateOK;// "修改成功！";
        }

        public void btnBach_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect("RoleAdmin.aspx");
        }

        #endregion 角色编辑

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                foreach (ListItem item in chkPermissions.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                foreach (ListItem item in chkPermissions.Items)
                {
                    item.Selected = false;
                }
            }
        }

        #endregion 权限设置

        #region gridView

        public void BindData()
        {
            if (CategoryDownList.SelectedIndex < 1)
            {
                gridView.DataSource = bllUser.GetUsersByRole(1); //默认加载管理员用户
                lblRoleID.Text = "1";
                gridView.DataBind();
            }
            else
            {
                int roleId = Globals.SafeInt(CategoryDownList.SelectedValue, 1);//默认加载管理员用户
                gridView.DataSource = bllUser.GetUsersByRole(roleId);
                lblRoleID.Text = CategoryDownList.SelectedValue;
                gridView.DataBind();
            }
           
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

                object obj2 = DataBinder.Eval(e.Row.DataItem, "UserID");
                if (obj2 != null)
                {
                    List<string> UserIDlist = YSWL.Common.StringPlus.GetStrArray(BLL.SysManage.ConfigSystem.GetValueByCache("AdminUserID"), ',', true);
                    if (UserIDlist.Contains(obj2.ToString()))
                    {
                        CheckBox ChkBxItem = (CheckBox)e.Row.FindControl(gridView.CheckBoxID);
                        if (ChkBxItem != null && ChkBxItem.Checked)
                        {
                            ChkBxItem.Visible = false;
                        }
                        }

                }
            }
        }

        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string ID = gridView.DataKeys[e.RowIndex].Value.ToString();
            List<string> UserIDlist = YSWL.Common.StringPlus.GetStrArray(BLL.SysManage.ConfigSystem.GetValueByCache("AdminUserID"), ',', true);
            if (UserIDlist.Contains(ID))
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.ErrorCannotDeleteID);
                return;
            }
            try
            {
                User User2 = new User(int.Parse(ID));
                User2.Delete();
                new BLL.Members.UsersExp().Delete(User2.UserID);
                gridView.OnBind();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.ErrorCannotDeleteUser);
                }
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                CheckBox ChkBxItem = (CheckBox)gridView.Rows[i].FindControl(gridView.CheckBoxID);
                if (ChkBxItem != null && ChkBxItem.Checked)
                {
                    if (gridView.DataKeys[i].Value != null)
                    {
                        int userid = Convert.ToInt32(gridView.DataKeys[i].Value);
                        bllUser.RemoveRole(userid, int.Parse(lblRoleID.Text));
                        User currentUser = new User(userid);
                        LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, string.Format("从角色中移除用户：【{0}】", currentUser.UserName), this);
                    }
                }
            }
            Common.MessageBox.ShowSuccessTip(this, "移除成功！");
            gridView.OnBind();
        }

        #endregion gridView
    }
}