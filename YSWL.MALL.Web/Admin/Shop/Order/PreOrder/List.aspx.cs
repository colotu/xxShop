using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using YSWL.Common;
using System.Drawing;
namespace YSWL.MALL.Web.Shop.Order.PreOrder
{
    public partial class List : PageBaseAdmin
    {

        YSWL.MALL.BLL.Shop.PrePro.PreOrder bll = new YSWL.MALL.BLL.Shop.PrePro.PreOrder();
        YSWL.MALL.BLL.Members.Users userBll = new BLL.Members.Users();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindUser();
                txtCreatedDateStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
                dropStatus.SelectedValue = "1";
         

                gridView.OnBind();

            
            }
            
        }
        #region 绑定用户
        /// <summary>
        /// 用户
        /// </summary>
        private void BindUser()
        {
            DataSet ds = userBll.GetList("  UserType='UU' ");
            if (!DataSetTools.DataSetIsNull(ds))
            {
                this.ddlUserId.DataSource = ds;
                this.ddlUserId.DataTextField = "UserName";
                this.ddlUserId.DataValueField = "UserID";
                this.ddlUserId.DataBind();
            }
            this.ddlUserId.Items.Insert(0, new ListItem("全　部", string.Empty));
            this.ddlUserId.Items.Insert(0, new ListItem(string.Empty, string.Empty));
            ddlUserId.SelectedIndex = 0;
        }
        #endregion
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
        
  
        
        #region gridView
                        
        public void BindData()
        {
            DataSet ds = new DataSet();
            StringBuilder strWhere = new StringBuilder();
            if (dropStatus.SelectedValue != "0")
            {
                strWhere.AppendFormat(" Status= '{0}'", dropStatus.SelectedValue);
            }
            if (!String.IsNullOrWhiteSpace(this.txtCreatedDateStart.Text) && Common.PageValidate.IsDateTime(this.txtCreatedDateStart.Text))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat(" CreatedDate >='" +Common.InjectionFilter.SqlFilter(this.txtCreatedDateStart.Text) + "' ");
            }
            //时间段
            if (!String.IsNullOrWhiteSpace(this.txtCreatedDateEnd.Text) && Common.PageValidate.IsDateTime(this.txtCreatedDateEnd.Text))
            {
                string endTime = Common.Globals.SafeDateTime(this.txtCreatedDateEnd.Text, DateTime.Now).AddDays(1).ToString("yyyy-MM-dd");
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat(" CreatedDate <'" + endTime + "' ");
            }

            int userId = Common.Globals.SafeInt(this.ddlUserId.SelectedValue, 0);
            if (userId > 0)
            {
                if (strWhere.Length > 1) strWhere.Append(" and ");
                strWhere.AppendFormat(" UserId = {0}", userId);
            }
            ds = bll.GetList(0, strWhere.ToString(), "  PreOrderId desc ");            
            gridView.DataSource = ds;
            gridView.DataBind();
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex;
            BindData();
        }
        protected void gridView_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Cells[0].Text = "<input id='Checkbox2' type='checkbox' onclick='CheckAll()'/><label></label>";
            }
        }
        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            e.Row.Attributes.Add("style", "background:#FFF");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnHandle = (LinkButton)e.Row.FindControl("lbtnHandle");
                LinkButton btnNotHandle = (LinkButton)e.Row.FindControl("lbtnNotHandle");
                object obj1 = DataBinder.Eval(e.Row.DataItem, "Status");
                if ((obj1 != null) && ((obj1.ToString() != "")))
                {
                    if (obj1.ToString() == "1")
                    {
                        btnNotHandle.Visible = true;
                    }  else if(obj1.ToString() == "2")
                    {
                        btnHandle.Visible = true;
                    }
                }

                //object obj1 = DataBinder.Eval(e.Row.DataItem, "Levels");
                //if ((obj1 != null) && ((obj1.ToString() != "")))
                //{
                //    e.Row.Cells[1].Text = obj1.ToString();
                //}
               
            }
        }
        
        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //#warning 代码生成警告：请检查确认真实主键的名称和类型是否正确
            //int ID = (int)gridView.DataKeys[e.RowIndex].Value;
            //bll.Delete(ID);
            //gridView.OnBind();
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

        #endregion

        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Status")
            {
                if (e.CommandArgument != null)
                {
                    string[] Args = e.CommandArgument.ToString().Split(new char[] { ',' });
                    int PreOrderId = Globals.SafeInt(Args[0], -1);
                    int Status = Globals.SafeInt(Args[1], -1);
                    if (PreOrderId <= 0 || Status <= 0)
                    {
                        return;
                    }
                
                   // Model.Pay.BalanceDrawRequest model = bll.GetModelByCache(PreOrderId);
                    if (bll.UpdateStatus(PreOrderId, Status,CurrentUser.UserID))
                    {
                        DataCache.DeleteCache("PreOrderModel-" + PreOrderId);
                        MessageBox.ShowSuccessTip(this, "操作成功");
                        gridView.OnBind();
                    }
                    else
                    {
                        MessageBox.ShowFailTip(this, "操作失败");
                    }
                }
            }
        }
        public string GetUserName(object o) 
        {
            if (o == null)
            {
                return "";
            }
            int userId = Common.Globals.SafeInt(o.ToString(), 0);
            if (userId <= 0)
            {
                return "";
            }
            return userBll.GetUserName(userId);
        }
 
        protected void dropStatusList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (dropStatusList.SelectedValue == "0")
            {
                return;
            }
            string strErr = string.Empty;
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0) return;
            switch (this.dropStatusList.SelectedValue)
            {
                case "1":
                    if (bll.UpdateList(idlist, 1,CurrentUser.UserID))
                    {
                        strErr = Resources.Site.TooltipBatchUpdateOK;
                    }
                    break;
                case "2":
                    if (bll.UpdateList(idlist, 2, CurrentUser.UserID))
                    {
                        strErr = Resources.Site.TooltipBatchUpdateOK;
                    }
                    break;
                default:
                    return;
            }
            if (strErr != "")
            {
                //清理文章缓存
                string[] arrcontid = idlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arrcontid)
                {
                    DataCache.DeleteCache("PreOrderModel-" + item);
                }
                MessageBox.ShowSuccessTip(this, strErr);
                gridView.OnBind();
            }
        }


    }
}
