/**
* Modify.cs
*
* 功 能： [N/A]
* 类 名： Modify.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.Brands
{
    public partial class Modify : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 399; } } //Shop_品牌管理_编辑页
        private YSWL.MALL.BLL.Shop.Products.BrandInfo bll = new YSWL.MALL.BLL.Shop.Products.BrandInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.chkProductTpyes.DataBind();
                ShowInfo(BrandId);
            }
        }

        private int BrandId
        {
            get
            {
                int id = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["id"]))
                {
                    id = Globals.SafeInt(Request.Params["id"], 0);
                }
                return id;
            }
        }

        private void ShowInfo(int BrandId)
        {
            YSWL.MALL.BLL.Shop.Products.BrandInfo bll = new YSWL.MALL.BLL.Shop.Products.BrandInfo();
            YSWL.MALL.Model.Shop.Products.BrandInfo model = bll.GetModel(BrandId);
            this.txtBrandName.Text = model.BrandName;
            this.txtBrandSpell.Text = model.BrandSpell;
            this.txtMeta_Description.Text = model.Meta_Description;
            this.txtMeta_Keywords.Text = model.Meta_Keywords;
            this.txtCompanyUrl.Text = model.CompanyUrl;
            this.txtDescription.Text = model.Description;
            this.imgLogo.ImageUrl = model.Logo;
            this.hfLogoUrl.Value = model.Logo;
            this.HiddenField_OldImage.Value = model.Logo;
            this.txtDisplaySequence.Text = model.DisplaySequence.ToString();
            YSWL.MALL.Model.Shop.Products.BrandInfo modelList = bll.GetRelatedProduct(BrandId, null);
            foreach (ListItem item in this.chkProductTpyes.Items)
            {
                if (modelList.ProductTypeIdOrBrandsId.Contains(int.Parse(item.Value)))
                {
                    item.Selected = true;
                }
            }
        }

        public void btnSave_Click(object sender, EventArgs e)
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
            model.BrandId = BrandId;
            model.ProductTypes = list;
            model.BrandName = BrandName;
            model.BrandSpell = BrandSpell;
            model.Meta_Description = Meta_Description;
            model.Meta_Keywords = Meta_Keywords;
            model.CompanyUrl = CompanyUrl;

            //待上传的图片名称
            string tempFile = string.Format("/Upload/Temp/{0}", DateTime.Now.ToString("yyyyMMdd"));
            string ImageFile = "/Upload/Shop/Brands";
            ArrayList imageList = new ArrayList();
            if (!string.IsNullOrWhiteSpace(HiddenField_ISModifyImage.Value))
            {
                string imageUrl = string.Format(hfLogoUrl.Value, "");
                imageList.Add(imageUrl.Replace(tempFile, ""));
                model.Logo = imageUrl.Replace(tempFile, ImageFile);
            }
            else
            {
                model.Logo = this.hfLogoUrl.Value;
            }
            model.Description = Description;
            model.DisplaySequence = DisplaySequence;

            if (bll.CreateBrandsAndTypes(model, Model.Shop.Products.DataProviderAction.Update))
            {
                this.btnCancle.Enabled = false;
                this.btnSave.Enabled = false;

                if (!string.IsNullOrWhiteSpace(HiddenField_ISModifyImage.Value))
                {
                    //将图片从临时文件夹移动到正式的文件夹下
                    Common.FileManage.MoveFile(Server.MapPath(tempFile), Server.MapPath(ImageFile), imageList);
                    if (!string.IsNullOrWhiteSpace(this.HiddenField_OldImage.Value))
                    {
                        FileManage.DeleteFile(Server.MapPath(this.HiddenField_OldImage.Value));
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(this.HiddenField_IsDeleteAttachment.Value))
                    {
                        //删除文件
                        Common.FileManage.DeleteFile(Server.MapPath(this.HiddenField_OldImage.Value));
                    }
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