/**
* EntryForm.cs
*
* 功 能： [N/A]
* 类 名： EntryForm
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/23 16:06:13  蒋海滨    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Web.UI;
using YSWL.Common;
namespace YSWL.MALL.Web.Ms.EntryForm
{
    public partial class Modify : PageBaseAdmin
    {
        YSWL.MALL.BLL.Ms.EntryForm bll = new YSWL.MALL.BLL.Ms.EntryForm();
        protected override int Act_PageLoad { get { return 277; } } //客服管理_报名用户管理_编辑页
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string id = Request.Params["id"];
                if (!string.IsNullOrWhiteSpace(id))
                {
                    ShowInfo(int.Parse(id));
                }
            }
        }

        protected void BindData()
        {
            for (int i = 0; i <= 119; i++)
            {
                this.dropAge.Items.Add(i.ToString());
            }
        }


        private void ShowInfo(int Id)
        {

            YSWL.MALL.Model.Ms.EntryForm model = bll.GetModel(Id);
            if (null != model)
            {
                this.lblId.Text = model.Id.ToString();
                this.txtUserName.Text = model.UserName;
                BindData();
                if (model.Age.HasValue)
                {
                    this.dropAge.SelectedValue = model.Age.ToString();
                }
                this.txtEmail.Text = model.Email;
                this.txtTelPhone.Text = model.TelPhone;
                this.txtPhone.Text = model.Phone;
                this.txtQQ.Text = model.QQ;
                this.txtMSN.Text = model.MSN;
                this.txtHouseAddress.Text = model.HouseAddress;
                this.txtCompanyAddress.Text = model.CompanyAddress;
                if (model.RegionId.HasValue)
                {
                    this.dropProvince.Area_iID = (int)model.RegionId;
                }
                this.dropSex.SelectedValue = model.Sex.Trim();
                this.txtDescription.Text = model.Description;
                this.txtRemark.Text = model.Remark;
                if (model.State.HasValue)
                {
                    this.dropState.SelectedValue = model.State.ToString();
                }
            }

        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            string username = this.txtUserName.Text.Trim();
            string remark = this.txtRemark.Text.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.ShowFailTip(this, Resources.MsEntryForm.ErrorNameNotNull);
                return;
            }
            if (remark.Length > 300)
            {
                MessageBox.ShowFailTip(this, Resources.MsEntryForm.ErrorRemarkoverlength);
                return;
            }
            int Id = int.Parse(this.lblId.Text);

            YSWL.MALL.Model.Ms.EntryForm model = bll.GetModel(Id);
            model.UserName = username;
            model.Age = int.Parse(this.dropAge.SelectedValue);
            model.Email = this.txtEmail.Text;
            model.TelPhone = this.txtTelPhone.Text;
            model.Phone = this.txtPhone.Text;
            model.QQ = this.txtQQ.Text;
            model.MSN = this.txtMSN.Text;
            model.HouseAddress = this.txtHouseAddress.Text;
            model.CompanyAddress = this.txtCompanyAddress.Text;
            model.RegionId = Convert.ToInt32(this.dropProvince.Area_iID);
            model.Sex = this.dropSex.SelectedValue;
            model.Description = this.txtDescription.Text;
            model.Remark = remark;
            model.State = int.Parse(this.dropState.SelectedValue);

            if (bll.Update(model))
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipUpdateOK, "list.aspx");
            }
        }


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
