using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using YSWL.Common;
using YSWL.Json;
using YSWL.Accounts.Bus;

namespace YSWL.MALL.Web.Admin.Shop.Order.SubmitOrder
{
    public partial class AddUser : PageBaseAdmin
    {
        private  BLL.Members.Users userBll = new BLL.Members.Users();
        private YSWL.Accounts.Bus.User userBusManage = new YSWL.Accounts.Bus.User();
        private BLL.Shop.Shipping.ShippingAddress shipAddressBll = new BLL.Shop.Shipping.ShippingAddress();
        private BLL.Shop.DisDepot.Depot depotBll = new BLL.Shop.DisDepot.Depot();
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
           // string phone = this.Request.Form["phone"];
            string shipname = this.Request.Form["shipname"];
            string shipphone = this.Request.Form["shipphone"];
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
            User newUser = new User();
            newUser.UserName = username;
            newUser.NickName ="" ;  //昵称名称相同
            newUser.Password = AccountsPrincipal.EncryptPassword("1");
            newUser.Phone = "";
            newUser.Activity = true;
            newUser.UserType = "UU";
            newUser.Style = 1;
            newUser.User_dateCreate = DateTime.Now;
            newUser.User_cLang = "zh-CN";

            int userid = newUser.Create();
            if (userid == -100)
            {
                json.Put("STATUS", "FAILED");
                json.Put("DATA", "ADDFAILED");//新增失败
                return json.ToString();
            }

            #region ERP用户同步
            Common.WebApi.ApiHelper apiHelper = new Common.WebApi.ApiHelper();
            ViewModel.ERP.AccountInfo info = new ViewModel.ERP.AccountInfo
            {
                loginName = newUser.UserName,
                userName = newUser.UserName,
                password = "1",
                phone = newUser.Phone,
                userType = newUser.UserType
            };
            string tag = Common.CallContextHelper.GetDEncrypTag();
            apiHelper.GetDataFromApi<ViewModel.ERP.AccountInfo, YSWL.MALL.ViewModel.ERP.ResultModel>("api/account/create", info, tag);
            #endregion

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
            ue.SourceType = 5;// (int)YSWL.MALL.Model.Members.Enum.SourceType.PC;

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
            else
            {
                json.Put("STATUS", "SUCCESS");
                if (!IsMultiDepot) {//未开启分仓
                   //写Cookie
                    Cookies.setCookie("OMS_SelectUserId", userid.ToString(), 1440);
                    return json.ToString();
                }
                int depotId = YSWL.MALL.BLL.Shop.DisDepot.DepotRegion.GetDepotByRegion(addressModel.RegionId);
                Model.Shop.DisDepot.Depot depotModel = depotBll.GetModel(depotId);
                if (depotModel == null || depotModel.DepotId<= 0)
                {
                    json.Put("DATA", "DepotIsNot");//未设置仓      
                    json.Put("UserId", userid.ToString());
                    return json.ToString();
                }
                else
                {
                    json.Put("DATA", depotModel.Name);
                }           
                //有仓库信息  写Cookie
                Cookies.setCookie("A_Order_SelectUserId", userid.ToString(), 1440);
                Cookies.setCookie("A_Order_DepotId", depotId.ToString(), 1440);               
                return json.ToString();
            }

        }
        #endregion 新增用户信息

        #endregion Ajax方法



    }
}