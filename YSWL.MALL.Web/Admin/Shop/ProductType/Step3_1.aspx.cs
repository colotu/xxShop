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
using System.Collections.Generic;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.ProductType
{
    public partial class Step3_1 : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Shop.Products.AttributeInfo bll = new YSWL.MALL.BLL.Shop.Products.AttributeInfo();

        protected override int Act_PageLoad { get { return 507; } } //Shop_规格_新增页
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ProductTypeId == 0)
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "参数错误，正在返回商品类型列表页...", "list.aspx");
                    return;
                }
                if (bll.IsExistDefinedAttribute(ProductTypeId, null))
                {
                    this.chbDefinePic.Enabled = false;
                }
            }
        }

        private int ProductTypeId
        {
            get
            {
                int producrTypeId = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["tid"]))
                {
                    producrTypeId = Globals.SafeInt(Request.Params["tid"], 0);
                }
                return producrTypeId;
            }
        }

        private int Action
        {
            get
            {
                int action = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["ed"]))
                {
                    action = Globals.SafeInt(Request.Params["ed"], 0);
                }
                return action;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtAttributeName.Text.Trim()))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "扩展属性名称不能为空，长度限制在1-15个字符之间！");
                return;
            }

            //if (string.IsNullOrWhiteSpace(this.txtAttributeValue.Text.Trim()))
            //{
            //    YSWL.Common.MessageBox.ShowFailTip(this, "扩展属性的值，多个属性值可用“，”号隔开，每个值最多15个字符！");
            //    return;
            //}

            string AttributeName = this.txtAttributeName.Text;
            string AttributeValue = this.txtAttributeValue.Text.Trim();

            YSWL.MALL.Model.Shop.Products.AttributeInfo model = new Model.Shop.Products.AttributeInfo();
            model.UsageMode = (int)YSWL.MALL.Model.Shop.Products.ProductAttributeModel.Specification;

            if (this.rbPic.Checked)
            {
                model.UseAttributeImage = true;
            }
            else
            {
                model.UseAttributeImage = false;
            }
            //ToDO: 获取属性值
            List<string> list = new List<string>();
            model.ValueStr = list;

            model.AttributeName = AttributeName;
            model.TypeId = ProductTypeId;
            model.UserDefinedPic = chbDefinePic.Checked;

            if (bll.AttributeManage(model, Model.Shop.Products.DataProviderAction.Create))
            {
                if (Action == 0)
                {
                    YSWL.Common.MessageBox.ResponseScript(this, "parent.location.href='Step3.aspx?tid=" + ProductTypeId + "'");
                }
                else
                {
                    YSWL.Common.MessageBox.ResponseScript(this, "parent.location.href='Modify3.aspx?tid=" + ProductTypeId + "&ed=" + Action + "'");
                }
            }
            else
            {
                this.btnSave.Enabled = false;
                YSWL.Common.MessageBox.ShowFailTip(this, "保存失败！", "Step2.aspx");
            }
        }
    }
}