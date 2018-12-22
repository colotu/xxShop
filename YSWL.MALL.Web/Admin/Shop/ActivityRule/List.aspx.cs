using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using YSWL.Common;
using System.Drawing;
namespace YSWL.MALL.Web.Shop.Products.ActivityRule
{
    public partial class List : PageBaseAdmin
    {

        YSWL.MALL.BLL.Shop.Activity.ActivityRule bll = new YSWL.MALL.BLL.Shop.Activity.ActivityRule();
        BLL.Members.Users usersBll = new BLL.Members.Users();
        protected void Page_Load(object  sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               
                btnDelete.Attributes.Add("onclick", "return confirm(\"你确认要删除吗？\")");
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
            if (idlist.Trim().Length == 0) 
                return;
            if (bll.DeleteList(idlist)) {
                DataCache.DeleteCache("GetAvailable_RuleList");         
                MessageBox.ShowSuccessTip(this, "删除成功！");
                gridView.OnBind();
            }
            else
            {
                MessageBox.ShowFailTip(this, "删除失败！");
            }
        }
        
        #region gridView
                        
        public void BindData()
        {
            #region
            //if (!Context.User.Identity.IsAuthenticated)
            //{
            //    return;
            //}
            //AccountsPrincipal user = new AccountsPrincipal(Context.User.Identity.Name);
            //if (user.HasPermissionID(PermId_Modify))
            //{
            //    gridView.Columns[6].Visible = true;
            //}
            //if (user.HasPermissionID(PermId_Delete))
            //{
            //    gridView.Columns[7].Visible = true;
            //}
            #endregion

            DataSet ds = new DataSet();
            StringBuilder strWhere = new StringBuilder();
            if (txtKeyword.Text.Trim() != "")
            {
                strWhere.AppendFormat(" RuleName  like '%{0}%'",Common.InjectionFilter.SqlFilter(txtKeyword.Text.Trim()));
            }
            int status=Globals.SafeInt( ddlStatus.SelectedValue,-1);
            if (status>-1) {
                if (strWhere.Length > 0) {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" Status={0} ", status);
            }
            ds = bll.GetList(0, strWhere.ToString(), " RuleId ");
            gridView.DataSetSource = ds;
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
                LinkButton linkbtnDel = (LinkButton)e.Row.FindControl("LinkButton1");
                linkbtnDel.Attributes.Add("onclick", "return confirm(\"你确认要删除吗\")");

                object obj2 = DataBinder.Eval(e.Row.DataItem, "Status"); 
                if ((obj2 != null) && ((obj2.ToString() != "")))
                {
                    string states = obj2.ToString();
                    if (states == "0") {
                        e.Row.Cells[3].Text ="未启用";
                    }
                    else if (states == "1")
                    {
                        e.Row.Cells[3].Text = "已启用";
                    } 
                }

                object obj3 = DataBinder.Eval(e.Row.DataItem, "CreatedUserId");
                if ((obj3 != null) && ((obj3.ToString() != "")))
                {
                    int userId = Common.Globals.SafeInt(obj3.ToString(), 0);
                    if (userId > 0) {
                        e.Row.Cells[4].Text = usersBll.GetUserName(userId);
                    }
                }

                object obj4 = DataBinder.Eval(e.Row.DataItem, "CreatedDate");
                if ((obj4 != null) && ((obj4.ToString() != "")))
                {
                    e.Row.Cells[5].Text = Convert.ToDateTime(obj4.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }
        
        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ID = (int)gridView.DataKeys[e.RowIndex].Value;
            if (bll.Delete(ID))
            {
                DataCache.DeleteCache("GetAvailable_RuleList");         
                MessageBox.ShowSuccessTip(this, "删除成功！");
                gridView.OnBind();
            }
            else {
                MessageBox.ShowFailTip(this, "删除失败！");
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
                idlist = idlist.Substring(0, idlist.LastIndexOf(","));
            }
            return idlist;
        }

        #endregion





    }
}
