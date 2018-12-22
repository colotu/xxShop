/**
* Add.cs
*
* 功 能： [新增品牌]
* 类 名： Add.cs
*
* Ver    变更日期                              负责人  变更内容
* ───────────────────────────────────
* V0.01   2012年6月12日 13:36:16  孙鹏        修改
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.Brands
{
    public partial class Add : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 398; } } //Shop_品牌管理_新增页
        YSWL.MALL.BLL.Shop.Products.BrandInfo bll = new YSWL.MALL.BLL.Shop.Products.BrandInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)))
                {
                    MessageBox.ShowAndBack(this, "您没有权限");
                    return;
                }
                this.chkProductTpyes.DataBind();
                this.txtDisplaySequence.Text = bll.GetMaxDisplaySequence().ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtBrandName.Text.Trim()))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "请输入品牌名称！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.txtDisplaySequence.Text.Trim()) || !PageValidate.IsNumber(txtDisplaySequence.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "请输入正确的显示顺序！");
                return;
            }
            string BrandName = this.txtBrandName.Text;
            string BrandSpell = this.txtBrandSpell.Text;
            string Meta_Description = this.txtMeta_Description.Text;
            string Meta_Keywords = this.txtMeta_Keywords.Text;
            string CompanyUrl = this.txtCompanyUrl.Text;
            string Description = this.txtDescription.Text;
            int DisplaySequence = int.Parse(this.txtDisplaySequence.Text);
            IList<int> list = new List<int>();
            foreach (ListItem item in this.chkProductTpyes.Items)
            {
                if (item.Selected)
                {
                    list.Add(int.Parse(item.Value));
                }
            }
            YSWL.MALL.Model.Shop.Products.BrandInfo model = new YSWL.MALL.Model.Shop.Products.BrandInfo();
            model.ProductTypes = list;
            model.BrandName = BrandName;
            model.BrandSpell = BrandSpell;
            model.Meta_Description = Meta_Description;
            model.Meta_Keywords = Meta_Keywords;

            //待上传的图片名称
            string tempFile = string.Format("/Upload/Temp/{0}", DateTime.Now.ToString("yyyyMMdd"));
            string ImageFile = "/Upload/Shop/Brands";
            ArrayList imageList = new ArrayList();
            if (!string.IsNullOrWhiteSpace(this.hfLogoUrl.Value))
            {
                string imageUrl = string.Format(this.hfLogoUrl.Value, "");

                imageList.Add(imageUrl.Replace(tempFile, ""));

                model.Logo = imageUrl.Replace(tempFile, ImageFile);
            }
            model.CompanyUrl = CompanyUrl;
            model.Description = Description;
            model.DisplaySequence = DisplaySequence;
            model.Theme = Theme;

            if (bll.CreateBrandsAndTypes(model, Model.Shop.Products.DataProviderAction.Create))
            {
                this.btnCancle.Enabled = false;
                this.btnSave.Enabled = false;
                if (!string.IsNullOrWhiteSpace(this.hfLogoUrl.Value))
                {
                    //将图片从临时文件夹移动到正式的文件夹下
                    Common.FileManage.MoveFile(Server.MapPath(tempFile), Server.MapPath(ImageFile), imageList);
                }
                YSWL.Common.MessageBox.ShowSuccessTip(this, "保存成功！", "Alist.aspx");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "保存失败！", "Alist.aspx");
            }
        }


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("Alist.aspx");
        }
    }
}
