using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.WholeSale
{
    public partial class SalesProductList :PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 551; } } //Shop_批发商品管理_列表页
        protected new int Act_DelData = 552;    //Shop_批发商品管理_删除数据
        private YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct salesRuleProductBll = new BLL.Shop.Sales.SalesRuleProduct();
        private  YSWL.MALL.BLL.Shop.Sales.SalesRule ruleBll=new BLL.Shop.Sales.SalesRule();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnDelete.Attributes.Add("onclick", "return confirm(\"" + Resources.Site.TooltipDelConfirm + "\")");
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
                {
                    btnDelete.Visible = false;
                }
                if (Type == 1)
                {
                    this.lblTitle.Text = "会员价格商品管理";
                }
                //加载批发规则
                ddlRule.DataSource = ruleBll.GetAllList(Type);
                ddlRule.DataTextField = "RuleName";
                ddlRule.DataValueField = "RuleId";
                ddlRule.DataBind();
                this.ddlRule.Items.Insert(0, new ListItem("请选择", "0"));
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
            if (idlist.Trim().Length == 0) return;
            salesRuleProductBll.DeleteList(idlist);
            YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
            gridView.OnBind();
        }

        public int Type
        {
            get
            {
                return Globals.SafeInt(Request.QueryString["id"], 0);
            }
        }

        #region gridView

        public void BindData()
        {
            if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
            {
                gridView.Columns[3].Visible = false;
                btnDelete.Visible = false;
            }
            DataSet ds = new DataSet();
            StringBuilder strWhere = new StringBuilder();
            int ruleId = Common.Globals.SafeInt(ddlRule.SelectedValue, 0);
            strWhere.AppendFormat(" EXISTS(SELECT *  FROM Shop_SalesRule WHERE Type={0} and  Shop_SalesRuleProduct.RuleId=RuleId)", Type);
            if (ruleId > 0)
            {
                strWhere.AppendFormat(" and  RuleId={0}", ruleId);
            }
            if (txtKeyword.Text.Trim() != "")
            {
                strWhere.AppendFormat(" and ProductName like '%{0}%'", Common.InjectionFilter.SqlFilter(txtKeyword.Text.Trim()));
            }
            ds = salesRuleProductBll.GetList(-1, strWhere.ToString(), "ProductId ");
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
            }
        }

        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int RuleId = (int)gridView.DataKeys[e.RowIndex][0];
            int ProductId = Common.Globals.SafeInt(gridView.DataKeys[e.RowIndex][1].ToString(),0);
            salesRuleProductBll.Delete(RuleId, ProductId);
            gridView.OnBind();
            Common.MessageBox.ShowSuccessTip(this, "删除成功");
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
                        int RuleId = (int)gridView.DataKeys[i][0];
                        int ProductId = Common.Globals.SafeInt(gridView.DataKeys[i][1].ToString(), 0);
                        idlist += (RuleId+"|" +ProductId)+ ",";
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


        #region 获取规则名称

        public string GetRuleName(object obj)
        {
            if (obj == null) 
                return string.Empty;
            int ruleId = Common.Globals.SafeInt(obj.ToString(), 0);
            YSWL.MALL.Model.Shop.Sales.SalesRule RuleModel = ruleBll.GetModel(ruleId);
            if (RuleModel != null)
            {
                return RuleModel.RuleName;
            }
            return "未知规则名称";
        }

        #endregion

        
   
        
    }
}

