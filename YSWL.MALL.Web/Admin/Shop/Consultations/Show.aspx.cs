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
using System.Web.UI;

namespace YSWL.MALL.Web.Admin.Shop.Consultations
{
    public partial class Show : PageBaseAdmin
    {
        public string strid = "";
        protected override int Act_PageLoad { get { return 406; } } //Shop_商品咨询管理_详细页
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
                {
                    strid = Request.Params["id"];
                    int ConsultationId = (Convert.ToInt32(strid));
                    ShowInfo(ConsultationId);
                }
            }
        }

        private void ShowInfo(int ConsultationId)
        {
            YSWL.MALL.BLL.Shop.Products.ProductConsults bll = new YSWL.MALL.BLL.Shop.Products.ProductConsults();
            YSWL.MALL.Model.Shop.Products.ProductConsults model = bll.GetModel(ConsultationId);
            this.lblConsultationId.Text = model.ConsultationId.ToString();
            this.lblProductId.Text = model.ProductId.ToString();
            this.lblUserName.Text = model.UserName;
            this.lblUserEmail.Text = model.UserEmail;
            this.lblConsultationText.Text = model.ConsultationText;
            this.lblCreatedDate.Text = model.CreatedDate.ToString();
            this.lblReplyDate.Text = model.ReplyDate.ToString();
            this.chkIsReply.Checked = model.IsReply;
            this.lblReplyText.Text = model.ReplyText;
            this.lblReplyUserId.Text = model.ReplyUserId.ToString();
            this.chkIsStatus.Checked = model.Status == 1;
           // this.CheckBox2.Checked = model;

            YSWL.Accounts.Bus.User user = new YSWL.Accounts.Bus.User();
           // this.lblReplyUserId.Text = user.GetTrueNameByCache(model.ReplyUserId);
            BLL.Shop.Products.ProductInfo manage = new BLL.Shop.Products.ProductInfo();
            Model.Shop.Products.ProductInfo productModel = manage.GetModel(model.ProductId);
            this.lblProductId.Text =productModel==null?"": productModel.ProductName;
        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}