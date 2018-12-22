using System;
using YSWL.Accounts.Bus;
using YSWL.MALL.BLL.Members;
using YSWL.Common;
using YSWL.Json;
using YSWL.MALL.Model.Members;

namespace YSWL.MALL.Web.Admin.Members.MembershipManage
{
    public partial class AddUser : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 196; } } //系统管理_是否显示用户管理 
        private BLL.Members.Users userBll = new BLL.Members.Users();
        private YSWL.Accounts.Bus.User userBusManage = new YSWL.Accounts.Bus.User();
        private BLL.Shop.Shipping.ShippingAddress shipAddressBll = new BLL.Shop.Shipping.ShippingAddress();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))

            {

                this.Controls.Clear();

                this.DoCallback();

            }
        }
  

        #region Ajax方法
        private void DoCallback()
        {
            string action = this.Request.Form["Action"];
            this.Response.Clear();
            this.Response.ContentType = "application/json";
            string writeText = string.Empty;

            switch (action)
            {
                case "AddInfo": //新增用户信息及收货地址
                    writeText = AddInfo();
                    break;
                default:
                    break;
            }
            this.Response.Write(writeText);
            this.Response.End();
        }

        #region  新增用户信息
        private string AddInfo()
        {
            JsonObject json = new JsonObject();
            string username = InjectionFilter.SqlFilter(this.Request.Form["username"]);
            string trueName = InjectionFilter.SqlFilter(this.Request.Form["trueName"]);
            
            string password = InjectionFilter.SqlFilter(this.Request.Form["password"]);
            string phone = InjectionFilter.SqlFilter(this.Request.Form["phone"]);
            string email = InjectionFilter.SqlFilter(this.Request.Form["email"]);           
            string shipname = InjectionFilter.SqlFilter(this.Request.Form["shipname"]);
            string shipphone = InjectionFilter.SqlFilter(this.Request.Form["shipphone"]);
            int regionId = Globals.SafeInt(this.Request.Form["regionId"], 0);
            string address = InjectionFilter.SqlFilter(this.Request.Form["address"]);
            if (String.IsNullOrWhiteSpace(username))
            {
                json.Put("STATUS", "FAILED");
                json.Put("DATA", "UserNameIsNull");
                return json.ToString();
            }
            //用户信息    
            if (userBusManage.HasUserByUserName(username))
            {
                json.Put("STATUS", "FAILED");
                json.Put("DATA", "HasUserName"); //用户名已经存在
                return json.ToString();
            }
            if (String.IsNullOrWhiteSpace(password))
            {
                json.Put("STATUS", "FAILED");
                json.Put("DATA", "PasswordIsNull");
                return json.ToString();
            }
            //if (String.IsNullOrWhiteSpace(phone))
            //{
            //    json.Put("STATUS", "FAILED");
            //    json.Put("DATA", "PhoneIsNull");
            //    return json.ToString();
            //}
            User newUser = new User();
            newUser.UserName = username;
            newUser.TrueName = trueName;
            newUser.NickName = trueName;  //昵称名称相同
            newUser.Password = AccountsPrincipal.EncryptPassword(password);
            newUser.Phone = phone;
            newUser.Activity = true;
            newUser.UserType = "UU";
            newUser.Style = 1;
            newUser.User_dateCreate = DateTime.Now;
            newUser.User_cLang = "zh-CN";
            newUser.Email = email;
            int userid = newUser.Create();
            if (userid == -100)
            {
                json.Put("STATUS", "FAILED");
                json.Put("DATA", "ADDFAILED");//新增失败
                return json.ToString();
            }

            //新增用户扩展表数据
            BLL.Members.UsersExp ue = new BLL.Members.UsersExp();
            ue.UserID = userid;
            ue.BirthdayVisible = 0;
            ue.BirthdayIndexVisible = false;
            ue.Gravatar = "";// string.Format("/{0}/User/Gravatar/{1}", MvcApplication.UploadFolder, userid);
            ue.ConstellationVisible = 0;
            ue.ConstellationIndexVisible = false;
            ue.NativePlaceVisible = 0;
            ue.NativePlaceIndexVisible = false;
            ue.RegionId = 0;
            ue.AddressVisible = 0;
            ue.AddressIndexVisible = false;
            ue.BodilyFormVisible = 0;
            ue.BodilyFormIndexVisible = false;
            ue.BloodTypeVisible = 0;
            ue.BloodTypeIndexVisible = false;
            ue.MarriagedVisible = 0;
            ue.MarriagedIndexVisible = false;
            ue.PersonalStatusVisible = 0;
            ue.PersonalStatusIndexVisible = false;
            ue.LastAccessIP = "";
            ue.LastAccessTime = DateTime.Now;
            ue.LastLoginTime = DateTime.Now;
            ue.LastPostTime = DateTime.Now;
            ue.NickName = newUser.NickName;
            //注册来源
            ue.SourceType = (int)YSWL.MALL.Model.Members.Enum.SourceType.Cust;

            //收货地址
            Model.Shop.Shipping.ShippingAddress addressModel = new Model.Shop.Shipping.ShippingAddress();
            addressModel.Address = address;
            addressModel.CelPhone = shipphone;
            addressModel.ShipName = shipname;
            addressModel.RegionId = regionId;
            addressModel.UserId = ue.UserID;
            if (!ue.AddEx(ue, addressModel))
            {
                userBll.Delete(userid);
                json.Put("STATUS", "FAILED");
                json.Put("DATA", "ADDFAILED");//注册失败
                return json.ToString();
            }
            json.Put("STATUS", "SUCCESS");
            return json.ToString();
        }
        #endregion 新增用户信息

        #endregion Ajax方法
    }
}
