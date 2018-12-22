using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using YSWL.Common;
using System.Drawing;
namespace  YSWL.MALL.Web.Admin.Shop.ActivityInfo
{
    public partial class List : PageBaseAdmin
    {
        YSWL.MALL.BLL.Shop.Activity.ActivityInfo bll = new YSWL.MALL.BLL.Shop.Activity.ActivityInfo();
        YSWL.MALL.BLL.Shop.Activity.ActivityRule activityRuleBll = new YSWL.MALL.BLL.Shop.Activity.ActivityRule();
        YSWL.MALL.BLL.Members.Users usersBll = new BLL.Members.Users();
        YSWL.MALL.BLL.Shop.Products.ProductInfo producBll = new BLL.Shop.Products.ProductInfo();
        YSWL.MALL.BLL.Shop.Coupon.CouponRule ruleBll = new BLL.Shop.Coupon.CouponRule();
        private BLL.Shop.Products.CategoryInfo manage = new BLL.Shop.Products.CategoryInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
              
                btnDelete.Attributes.Add("onclick", "return confirm(\"你确认要删除吗？\")");
                //规则
                this.ddlRule.DataSource = activityRuleBll.GetList(" Status=1");
                ddlRule.DataTextField = "RuleName";
                ddlRule.DataValueField = "RuleId";
                ddlRule.DataBind();
                ddlRule.Items.Insert(0, new ListItem("全   部", "0"));
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
            if (bll.DeleteList(idlist))
            {
                YSWL.Common.DataCache.DeleteCache("GetAvailable_ActivityInfo");
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
       
            DataSet ds = new DataSet();
            StringBuilder strWhere = new StringBuilder();
            int status = Common.Globals.SafeInt(ddlStatus.SelectedValue, -1);
            string endDate = txtEndDate.Text;
            string startDate = txtStartDate.Text;
            //状态
            if (status>-1)
            {
                strWhere.AppendFormat(" Status ={0}", status);
            }
            //开始时间
            if (!String.IsNullOrWhiteSpace(startDate))
            {
                if (!String.IsNullOrWhiteSpace(strWhere.ToString()))
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" startDate >='{0}'", startDate);
            }
            //结束时间
            if (!String.IsNullOrWhiteSpace(endDate))
            {
                if (!String.IsNullOrWhiteSpace(strWhere.ToString()))
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" EndDate <='{0}'", endDate);
            }
            //规则
            int ruleId = Globals.SafeInt(ddlRule.SelectedValue, 0);
            if (ruleId>0)
            {
                if (!String.IsNullOrWhiteSpace(strWhere.ToString()))
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" RuleId  ={0}", ruleId);
            }

            ds = bll.GetList(0, strWhere.ToString(), " ActivityId  desc");
            gridView.DataSetSource = ds;
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }
 
        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           // e.Row.Attributes.Add("style", "background:#FFF");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton linkbtnDel = (LinkButton)e.Row.FindControl("LinkButton1");
                linkbtnDel.Attributes.Add("onclick", "return confirm(\"你确认要删除吗\")");
                
                //object obj1 = DataBinder.Eval(e.Row.DataItem, "Levels");
                //if ((obj1 != null) && ((obj1.ToString() != "")))
                //{
                //    e.Row.Cells[1].Text = obj1.ToString();
                //}
               
            }
        }
        
        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ID = (int)gridView.DataKeys[e.RowIndex].Value;
            if (bll.Delete(ID))
            {
                YSWL.Common.DataCache.DeleteCache("GetAvailable_ActivityInfo");
                MessageBox.ShowSuccessTip(this, "删除成功！");
                gridView.OnBind();
            }
            else
            {
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

        #region 辅助方法
        public string GetRuleName(object o) 
        {
            if (o == null) {
                return "";
            }
            int ruleId = Globals.SafeInt(o, 0);
            if (ruleId <= 0) {
                return "";
            }
            return activityRuleBll.GetNameByCache(ruleId);
        }
        public string GetUserName(object o) 
        {
            if (o == null) {
                return "";
            }
            int userId = Globals.SafeInt(o, 0);
            if (userId <= 0)
            {
                return "";
            }
            return usersBll.GetUserName(userId);
        }
        public string GetCpRuleName(object o) 
        {
            if (o == null) {
                return "";
            }
            int ruleId = Globals.SafeInt(o, 0);
            if (ruleId <= 0)
            {
                return "";
            }
            return ruleBll.GetNameByCache(ruleId);
        }

        public string GetProductName(object o)
        {
            if (o == null)
            {
                return "";
            }
            int pId = Globals.SafeInt(o, 0);
            if (pId <= 0)
            {
                return "";
            }
            return producBll.GetNameByCache(pId);
        }
        public string GetCategoryName(object o)
        {
            if (o == null)
            {
                return "";
            }
            int cId = Globals.SafeInt(o, 0);
            if (cId <= 0)
            {
                return "";
            }
            return  manage.GetFullNameByCache(cId);
        }
        
        #endregion


    }
}
