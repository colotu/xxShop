﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Coupon;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.Coupon
{
    public partial class AddClass : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 410; } } //Shop_优惠券分类管理_新增页
        YSWL.MALL.BLL.Shop.Coupon.CouponClass classBll = new CouponClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtSequence.Text = (classBll.GetSequence() + 1).ToString();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.Coupon.CouponClass classModel=new Model.Shop.Coupon.CouponClass();
           
            string name = this.tName.Text;
            if (String.IsNullOrWhiteSpace(name))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "请输入分类名称");
                return;
            }
            classModel.Name = name;
            classModel.Status = chkStatus.Checked ? 1 : 0;
            classModel.Sequence = Common.Globals.SafeInt(this.txtSequence.Text, 0);

            if (classBll.Add(classModel) > 0)
            {
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
            }
            else
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, "操作失败！");
            }
        }

    }
}