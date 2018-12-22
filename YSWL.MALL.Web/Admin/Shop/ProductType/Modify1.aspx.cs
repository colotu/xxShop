/**
* Add.cs
*
* 功 能： [N/A]
* 类 名： Add.cs
*
* Ver        变更日期                           负责人          变更内容
* ───────────────────────────────────
* V0.01   2012年6月13日 20:46:27    Rock            创建
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using YSWL.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace YSWL.MALL.Web.Admin.Shop.ProductType
{
    public partial class Modify1 : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 494; } } //Shop_商品类型管理_编辑页
        YSWL.MALL.BLL.Shop.Products.ProductType bll = new YSWL.MALL.BLL.Shop.Products.ProductType();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ProductTypeId == 0)
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "参数错误，正在返回商品类型列表页...", "list.aspx");
                    return;
                }
                this.chkBrandsCheckBox.DataBind();
                ShowInfo();
            }
        }


        private int ProductTypeId
        {
            get
            {
                int producrTypeId = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["tid"]))
                {
                    producrTypeId = YSWL.Common.Globals.SafeInt(Request.Params["tid"], 0);
                }
                return producrTypeId;
            }
        }

        private void ShowInfo()
        {
            Model.Shop.Products.ProductType model = bll.GetModel(ProductTypeId);
            if (model != null)
            {
                this.txtTypeName.Text = model.TypeName;
                this.txtRemark.Text = model.Remark;
                BLL.Shop.Products.BrandInfo brandsBll = new BLL.Shop.Products.BrandInfo();
                YSWL.MALL.Model.Shop.Products.BrandInfo modelList = brandsBll.GetRelatedProduct(null, ProductTypeId);
                foreach (System.Web.UI.WebControls.ListItem item in this.chkBrandsCheckBox.Items)
                {
                    if (modelList.ProductTypeIdOrBrandsId.Contains(int.Parse(item.Value)))
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtTypeName.Text.Trim()))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "商品类型名称不能为空，长度限制在1-50个字符之间！");
                return;
            }
            string TypeName = this.txtTypeName.Text;
            string Remark = this.txtRemark.Text;

            IList<int> list = new List<int>();
            foreach (ListItem item in this.chkBrandsCheckBox.Items)
            {
                if (item.Selected)
                {
                    list.Add(int.Parse(item.Value));
                }
            }

            YSWL.MALL.Model.Shop.Products.ProductType model = new YSWL.MALL.Model.Shop.Products.ProductType();
            model.TypeName = TypeName;
            model.Remark = Remark;
            model.BrandsTypes = list;
            model.TypeId = ProductTypeId;
            int typeid = 0;
            if (bll.ProductTypeManage(model, Model.Shop.Products.DataProviderAction.Update, out typeid))
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, "基本设置信息保存成功！");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "保存失败，正在返回商品类别列表页！", "list.aspx");
            }
        }


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
