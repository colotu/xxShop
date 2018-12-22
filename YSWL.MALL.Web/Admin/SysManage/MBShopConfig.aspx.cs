using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.MALL.Model.SysManage;
using System.Collections;

namespace YSWL.MALL.Web.Admin.SysManage
{
    public partial class MBShopConfig : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 160; } } //网站管理_是否显示网站设置页面
        protected new int Act_UpdateData = 161;    //网站管理_网站设置_编辑网站信息
        public string SeoSetting = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateData)) && GetPermidByActID(Act_UpdateData) != -1)
                {
                    btnSave.Visible = false;
                }
                BoundData();
                //MBShop_IsOpenRegister

                if (string.IsNullOrEmpty(YSWL.MALL.BLL.SysManage.ConfigSystem.GetValue("MBShop_IsOpenLogin")))
                {
                    this.OpenLogin.Checked = true;
                    YSWL.MALL.BLL.SysManage.ConfigSystem.Modify("MBShop_IsOpenLogin", OpenLogin.Checked.ToString(), "是否开启前台商城登陆");
                    Cache.Remove("ConfigSystemHashList_" + ApplicationKeyType.System);//清除网站设置的缓存文件
                    Cache.Remove("ConfigSystemHashList");
                }
                else
                {
                    this.OpenLogin.Checked = Common.Globals.SafeBool(YSWL.MALL.BLL.SysManage.ConfigSystem.GetValue("MBShop_IsOpenLogin"), false);//是否开启登陆认证
                }

                //判断手否需要注册
                this.OpenRegister.Checked = Common.Globals.SafeBool(YSWL.MALL.BLL.SysManage.ConfigSystem.GetValue("MBShop_IsOpenRegister"), false);//是否开启登陆认证
            }
        }

        private void BoundData()
        {
            this.txtMBShopName.Text = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MBShop_Name");
            string MBShoploginLogo = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MBhop_Login_Logo");
            if (!string.IsNullOrWhiteSpace(MBShoploginLogo))
            {
                this.imgLoginLogo.ImageUrl = MBShoploginLogo;
            }

            string MBShopIndexLogo = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MBhop_Index_Logo");
            if (!string.IsNullOrWhiteSpace(MBShopIndexLogo))
            {
                this.imgIndexLogo.ImageUrl = MBShopIndexLogo;
            }


            this.txtMShopTel.Text = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MBShop_Phone");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.SysManage.ConfigSystem.Modify("WeChat_MBShop_Name", Globals.HtmlEncode(this.txtMBShopName.Text), "云订货商城名称", ApplicationKeyType.System);
                string tempFile = string.Format("/Upload/Temp/{0}", DateTime.Now.ToString("yyyyMMdd"));
                string ImageFile = "/Upload/WebSiteLogo";
                ArrayList imageList = new ArrayList();
                if (!string.IsNullOrWhiteSpace(this.hfs_ICOPath.Value))
                {
                    string imageUrl = string.Format(this.hfs_ICOPath.Value, "");
                    imageList.Add(imageUrl.Replace(tempFile, ""));
                    string logoPath = imageUrl.Replace(tempFile, ImageFile);
                    BLL.SysManage.ConfigSystem.Modify("WeChat_MBhop_Index_Logo", logoPath, "云订货首页Logo", ApplicationKeyType.System);
                }
                if (!string.IsNullOrWhiteSpace(this.hfs_LoginPath.Value))
                {
                    string imageUrl = string.Format(this.hfs_LoginPath.Value, "");
                    imageList.Add(imageUrl.Replace(tempFile, ""));
                    string logoPath = imageUrl.Replace(tempFile, ImageFile);
                    BLL.SysManage.ConfigSystem.Modify("WeChat_MBhop_Login_Logo", logoPath, "云订货登录Logo", ApplicationKeyType.System);
                }

                //是否开启商城登陆
                YSWL.MALL.BLL.SysManage.ConfigSystem.Modify("MBShop_IsOpenLogin", OpenLogin.Checked.ToString(), "是否开启前台商城登陆");
                //判断是否开启注册
                YSWL.MALL.BLL.SysManage.ConfigSystem.Modify("MBShop_IsOpenRegister", OpenRegister.Checked.ToString(), "是否开启注册");

                BLL.SysManage.ConfigSystem.Modify("WeChat_MBShop_Phone", this.txtMShopTel.Text, "云订货号码", ApplicationKeyType.System);

                Cache.Remove("ConfigSystemHashList_" + ApplicationKeyType.System);//清除网站设置的缓存文件
                Cache.Remove("ConfigSystemHashList");

                this.btnSave.Enabled = false;
                Common.FileManage.MoveFile(Server.MapPath(tempFile), Server.MapPath(ImageFile), imageList);

                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipUpdateOK, "MBShopConfig.aspx");
            }
            catch (Exception)
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Site.TooltipTryAgainLater, "MBShopConfig.aspx");
            }
        }
    }
}