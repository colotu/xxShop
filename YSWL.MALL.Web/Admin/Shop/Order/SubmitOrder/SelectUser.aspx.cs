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

namespace YSWL.MALL.Web.Admin.Shop.Order.SubmitOrder
{
    public partial class SelectUser : PageBaseAdmin
    {
        private  BLL.Members.Users userBll = new BLL.Members.Users();
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
                case "GetUserInfo": //获取用户信息及收货地址
                    writeText = GetUserInfo();
                    break;
                case "UpdateInfo"://修改用户信息及收货地址信息
                    writeText = UpdateInfo();
                    break;
                default:
                    break;
            }
            this.Response.Write(writeText);
            this.Response.End();
        }

        private string GetUserInfo()
        {
            JsonObject json = new JsonObject();
            int userId =Globals.SafeInt(this.Request.Form["UserId"], 0);
            if (userId<=0)
            {
                json.Put("STATUS", "FAILED");
                return json.ToString();
            }
            //获取用户信息 
            Model.Members.Users userModel=userBll.GetModel(userId);
            if (userModel== null) {
                json.Put("STATUS", "FAILED");
                return json.ToString();
            }
            JsonObject data = new JsonObject();
            data.Put("userName", userModel.UserName);
            //获取收获地址
            YSWL.MALL.Model.Shop.Shipping.ShippingAddress addressModel = shipAddressBll.GetModelByUserId(userId);
            if (addressModel == null)
            {
                addressModel = new Model.Shop.Shipping.ShippingAddress();            
            } 
            data.Put("shipName", addressModel.ShipName);
            data.Put("shipCelPhone", addressModel.CelPhone);
            data.Put("shipRegionId", addressModel.RegionId);
            data.Put("shipAddress", addressModel.Address);
            //data.Put("IsOpenDepot", IsMultiDepot);//是否开启分仓  
            //if (IsMultiDepot)
            //{
            //    int depotId = YSWL.MALL.BLL.Shop.DisDepot.DepotRegion.GetDepotByRegion(addressModel.RegionId);
            //    Model.Shop.DisDepot.Depot depotModel = depotBll.GetModel(depotId);
            //    if (depotModel == null || depotModel.DepotId <= 0)
            //    {
            //        data.Put("Depot", "DepotIsNot");//未设置仓                
            //    }
            //    else
            //    {
            //        data.Put("Depot", depotModel.Name);
            //    }
            //}
            json.Put("STATUS", "SUCCESS");
            json.Put("DATA", data);
            return json.ToString();
        }
        private string UpdateInfo()
        {
            JsonObject json = new JsonObject();
            int userId = Globals.SafeInt(this.Request.Form["userId"], 0);
            string shipname = this.Request.Form["shipname"];
            string shipphone = this.Request.Form["shipphone"];
            int regionId = Globals.SafeInt(this.Request.Form["regionId"], 0);
            string address =InjectionFilter.SqlFilter( this.Request.Form["address"]);
            if (userId <= 0)
            {
                json.Put("STATUS", "FAILED");
                json.Put("DATA", "UserIdIsNULL");//未选择用户
                return json.ToString();
            }
            //获取用户信息 
            Model.Members.Users userModel = userBll.GetModel(userId);
            if (userModel == null)
            {
                json.Put("DATA", "UserIdIsNULL");//未选择用户
                json.Put("STATUS", "FAILED");
                return json.ToString();
            }

            //获取收获地址
            Model.Shop.Shipping.ShippingAddress addressModel = shipAddressBll.GetModelByUserId(userId);
            if (addressModel == null)
            {
                addressModel = new Model.Shop.Shipping.ShippingAddress();
            }
            addressModel.Address = address;
            addressModel.CelPhone = shipphone;
            addressModel.ShipName = shipname;
            addressModel.RegionId = regionId;
            bool result;
            if (addressModel.ShippingId <= 0)
            {
                addressModel.UserId = userId;
                result = shipAddressBll.Add(addressModel)>0;
            }
            else {
                result= shipAddressBll.Update(addressModel);
            }
            if (!result)
            {
                json.Put("STATUS", "Error");
                return json.ToString();
            }
            if (!IsMultiDepot)
            {
                json.Put("STATUS", "SUCCESS");
                //写Cookie
                Cookies.setCookie("A_Order_SelectUserId", userModel.UserID.ToString(), 1440);
                return json.ToString();
            }
            int depotId = YSWL.MALL.BLL.Shop.DisDepot.DepotRegion.GetDepotByRegion(addressModel.RegionId);
            Model.Shop.DisDepot.Depot depotModel = depotBll.GetModel(depotId);
            if (depotModel == null || depotModel.DepotId == 0)
            {
                json.Put("STATUS", "FAILED");
                json.Put("DATA", "DepotIsNot");//未设置仓                
            }
            else
            {
                //写Cookie
                Cookies.setCookie("A_Order_SelectUserId", userModel.UserID.ToString(), 1440);
                Cookies.setCookie("A_Order_DepotId", depotId.ToString(), 1440);
                json.Put("STATUS", "SUCCESS");
            }          
            return json.ToString();
        }
        #endregion


    }
}