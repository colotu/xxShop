using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.MALL.BLL.Shop.Products;

namespace YSWL.MALL.Web.Admin.Shop.Depot
{
    public partial class SelectProList : PageBaseAdmin
    {
        //protected override int Act_PageLoad { get { return  ; } } //_列表页面   
        //protected new int Act_AddData =  ； //_新增数据 
        //protected new int Act_DelData =  ;    //_删除数据

        private BLL.Shop.Products.SKUInfo manage = new BLL.Shop.Products.SKUInfo();
        BLL.Shop.DisDepot.DepotProSKUs depotSkusBll = new BLL.Shop.DisDepot.DepotProSKUs();

        protected void Page_Load(object sender, EventArgs e)
        {
 
            if (!Page.IsPostBack)
            {
                if (DepotId <=0) {
                    MessageBox.Show(this, "请选择对应的仓库");
                    return;
                }
                hiddepotId.Value = DepotId.ToString();
                BindData();
            }
          
        }
        
        public int  DepotId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Request.Params["did"]))
                {
                    return  Globals.SafeInt(Request.Params["did"], 0);
                }
                return  0;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        #region BindData

        public void BindData()
        {
            //加载未新增的数据
            BindSearchProduct();

            //加载已新增的数据
            BindAddProduct();
        }

        private void BindAddProduct()
        {
            //获取已选择数据
            int categoryId = YSWL.Common.Globals.SafeInt(drpProductCategory.SelectedValue, 0);
            string pName = txtProductName.Text.Trim();
            if (anpAddedProducts.RecordCount == 0)
            {
                anpAddedProducts.RecordCount = anpAddedProducts.PageSize;
            }
            int recordCount = depotSkusBll.GetRecordCount(DepotId, categoryId, pName);
 
            int pageIndex = anpAddedProducts.CurrentPageIndex;
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * anpAddedProducts.PageSize + 1 : 1;
            int endIndex = pageIndex > 1 ? startIndex + anpAddedProducts.PageSize - 1 : anpAddedProducts.PageSize;
       
            anpAddedProducts.RecordCount = recordCount;
            dlstAddedProducts.DataSource = depotSkusBll.GetListByPage(DepotId, categoryId, pName, "", startIndex, endIndex);
            dlstAddedProducts.DataBind();
        }

        private void BindSearchProduct()
        {
            if (anpSearchProducts.RecordCount == 0)
            {
                anpSearchProducts.RecordCount = anpSearchProducts.PageSize;
            }
            int pageIndex = anpSearchProducts.CurrentPageIndex;
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * anpSearchProducts.PageSize + 1 : 1;
            int endIndex = pageIndex > 1 ? startIndex + anpSearchProducts.PageSize - 1 : anpSearchProducts.PageSize;
            int categoryId = YSWL.Common.Globals.SafeInt(drpProductCategory.SelectedValue, 0);
            string pName = txtProductName.Text.Trim();
            int totalCount = depotSkusBll.GetNoAddSKURecordCount(DepotId, categoryId, pName);   
            anpSearchProducts.RecordCount = totalCount;
            dlstSearchProducts.DataSource = depotSkusBll.GetNoAddSKUList(DepotId, categoryId, pName, startIndex, endIndex); 
            dlstSearchProducts.DataBind();
        }

        protected void dlstAddedProducts_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)) && GetPermidByActID(Act_AddData) != -1)
            {
                HtmlGenericControl lbtnDel = (HtmlGenericControl)e.Item.FindControl("lbtnDel");
                lbtnDel.Visible = false;
            }
        }

        protected void dlstSearchProducts_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)) && GetPermidByActID(Act_AddData) != -1)
            {
                HtmlGenericControl lbtnAdd = (HtmlGenericControl)e.Item.FindControl("lbtnAdd");
                lbtnAdd.Visible = false;
            }

        }
        #endregion BindData

        protected void AspNetPager_PageChanged(object sender, EventArgs e)
        {
            BindData();
        }

      
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected string GetSKUStr(object target)
        {
            string sku = target.ToString();
            List<YSWL.MALL.Model.Shop.Products.SKUItem> itemList = manage.GetSKUItemsBySku(sku);
            if (itemList == null || itemList.Count == 0)
            {
                return "";
            }
            string str = "规格(";
            foreach (var item in itemList)
            {
                str += item.AttributeName + "：" + item.AV_ValueStr + "  ";
            }
             str +=")";
            return str;
        }
    }
}