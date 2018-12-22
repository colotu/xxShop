using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.WMS.Depot
{
    public partial class UpdateDepot : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Shop.DisDepot.Depot depotBll = new YSWL.MALL.BLL.Shop.DisDepot.Depot();
        protected override int Act_PageLoad
        {
            get { return 400; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                ShowInfo();
            }
        }
        public int DepotId
        {
            get
            {
                return Common.Globals.SafeInt(Request.Params["id"], 0);
            }
        }

        private void ShowInfo()
        {
            YSWL.MALL.Model.Shop.DisDepot.Depot infoModel = depotBll.GetModel(DepotId);
            if (infoModel != null)
            {
                txtName.Text = infoModel.Name;
                txtCode.Text = infoModel.Code;
                txtContactName.Text = infoModel.ContactName;
                RegionID.Region_iID = infoModel.RegionId;
                txtAddress.Text = infoModel.Address;
                txtPhone.Text = infoModel.Phone;
                txtRemark.Text = infoModel.Remark;
                txtEmail.Text = infoModel.Email;
                chkStatus.Checked = infoModel.Status == 1;
            }
            else {
                MessageBox.ShowFailTipScript(this, "该信息不存在或者已被删除！", "$(parent.document).find('[id$=btnSearch]').click()");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.DisDepot.Depot infoModel = depotBll.GetModel(DepotId);
            string name = txtName.Text;
            if (String.IsNullOrWhiteSpace(name))
            {
                Common.MessageBox.ShowFailTip(this, "请填写仓库名称");
                return;
            }
            string contact = txtContactName.Text;
            if (String.IsNullOrWhiteSpace(contact))
            {
                Common.MessageBox.ShowFailTip(this, "请填写联系人");
                return;
            }
            int regionId = RegionID.Region_iID;
            if (regionId <= 0)
            {
                Common.MessageBox.ShowFailTip(this, "请选择仓库区域");
                return;
            }

            string address = txtAddress.Text;
            if (String.IsNullOrWhiteSpace(address))
            {
                Common.MessageBox.ShowFailTip(this, "请填写仓库详细地址");
                return;
            }
            string code = txtCode.Text;
            if (!String.IsNullOrWhiteSpace(code) && depotBll.IsExistCode(code, DepotId))
            {
                Common.MessageBox.ShowFailTip(this, "该仓库编码已经存在");
                return;
            }
            infoModel.Name = name;
            infoModel.Code = code;
            infoModel.ContactName = contact;
            infoModel.Email = txtEmail.Text;
            infoModel.Phone = txtPhone.Text;
            infoModel.CreatedDate = DateTime.Now;
            infoModel.RegionId = regionId;
            infoModel.Address = txtAddress.Text;
            infoModel.Remark = txtRemark.Text;
            infoModel.Status = chkStatus.Checked ? 1 : 0;

            if (depotBll.Update(infoModel))
            {
                btnSave.Visible = false;
                Common.MessageBox.ShowSuccessTipScript(this, "编辑仓库成功", "$(parent.document).find('[id$=btnSearch]').click()");
            }
            else
            {
                this.btnSave.Visible = true;
                Common.MessageBox.ShowFailTip(this, "编辑仓库失败！");
            }
        }

    }
}