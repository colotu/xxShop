/**
* Show.cs
*
* 功 能： [N/A]
* 类 名： Show.cs
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
using YSWL.Common;
using System.Data;
using System.Text;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class ProductCategory : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BindData()
        {
            BLL.Shop.Products.ProductInfo productInfoBll = new BLL.Shop.Products.ProductInfo();
            DataSet ds = productInfoBll.GetCategoriesCount();
            BindJson(ds);
            this.gridView.DataSetSource = ds;
        }

        private void BindJson(DataSet ds)
        {
            List<YSWL.MALL.ViewModel.Order.CategoryCount> list = new List<YSWL.MALL.ViewModel.Order.CategoryCount>();
            //把dataset转换成list
            YSWL.MALL.ViewModel.Order.CategoryCount model = null;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                model = new ViewModel.Order.CategoryCount();
                model.CategoryName = dr.Field<string>("CategoryName");
                model.Count = Common.Globals.SafeInt(dr["count1"], 0);
                model.OffCount = Common.Globals.SafeInt(dr["count0"], 0);
                list.Add(model);
            }
            this.hfCategory.Value = String.Join(",", list.Select(c => c.CategoryName));
            this.hfCount.Value = String.Join(",", list.Select(c => c.Count));
            this.hfOffCount.Value = String.Join(",", list.Select(c => c.OffCount));
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}
