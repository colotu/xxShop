using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web.Admin.Shop.Depot
{
    public partial class AddDepot : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Shop.DisDepot.Depot depotBll = new YSWL.MALL.BLL.Shop.DisDepot.Depot();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.DisDepot.Depot infoModel = new Model.Shop.DisDepot.Depot();
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
            if (!String.IsNullOrWhiteSpace(code) && depotBll.IsExistCode(code))
            {
                Common.MessageBox.ShowFailTip(this, "该仓库编码已经存在");
                return;
            }


            infoModel.Name = name;
            infoModel.Code = txtCode.Text;
            infoModel.ContactName = contact;
            infoModel.Email = txtEmail.Text;
            infoModel.Phone = txtPhone.Text;
            infoModel.CreatedDate = DateTime.Now;
            infoModel.RegionId = regionId;
            infoModel.Address = txtAddress.Text;
            infoModel.Remark = txtRemark.Text;
            infoModel.Status = chkStatus.Checked ? 1 : 0;

            int  depotId = depotBll.AddEx(infoModel);
            if (depotId>0)
            {
                btnSave.Visible = false;
                 Common.MessageBox.ShowSuccessTipScript(this, "新增成功！","$(parent.document).find('[id$=btnSearch]').click()");
            }
            else
            {
                Common.MessageBox.ShowFailTip(this, "新增仓库失败！");
            }
        }
    }
}