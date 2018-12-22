using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.Model.Shop.Order;
using YSWL.Common;
using YSWL.Json;
using System.IO;


namespace YSWL.MALL.Web.Admin.Shop.SubstituteReturn
{
    public partial class Apply : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrders returnorderBll = new BLL.Shop.ReturnOrder.ReturnOrders();
        private YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrderItems  returnitemBll= new BLL.Shop.ReturnOrder.ReturnOrderItems();
        private readonly YSWL.MALL.BLL.Shop.Order.Orders _orderManage = new BLL.Shop.Order.Orders();
        private readonly YSWL.MALL.BLL.Shop.Order.OrderItems itemBll = new BLL.Shop.Order.OrderItems();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))
            {
                this.Controls.Clear();
                this.DoCallback();
            }
            if (!IsPostBack)
            {
                hidoid.Value = OrderId.ToString();
                ShowInfo();
            }
        }
        #region OrderId
        /// <summary>
        /// OrderId
        /// </summary>
        public long OrderId
        {
            get
            {
                if(String.IsNullOrWhiteSpace( Request.QueryString["OrderId"])){
                    return 0;
                }
                return YSWL.Common.Globals.SafeLong(Request.QueryString["OrderId"].ToString(), 0);
            }
        }
      
        #endregion

        #region 加载
        private void ShowInfo()
        {
            OrderInfo orderModel = _orderManage.GetModelInfo(OrderId);
            //Safe
            if (orderModel == null || orderModel.HasChildren) return;
            Literal1.Text =string.Format( "正在进行订单【{0}】的退货申请",orderModel.OrderCode);
   
            if ( !returnorderBll.IsMeetCondition(orderModel, orderModel.BuyerID))
            {
                MessageBox.ShowFailTipScript(this, "该订单不满足退单规则！", "$(parent.document).find('[id$=btnSearch]').click();");
                return;
            }

            hfSelectedNode.Value = orderModel.RegionId.ToString();
            txtPick_Address.Text = orderModel.ShipAddress;
            txtApplyPhone.Text = orderModel.ShipCellPhone;
            txtApplyUserName.Text = orderModel.ShipName;
        }
        #endregion

        public void BindData()
        {
            gridView.DataSetSource = itemBll.GetListByCache(" OrderId=" + OrderId);
        }


        protected string GetSKUStr(object target)
        {
            string sku = target.ToString();
            List<YSWL.MALL.Model.Shop.Products.SKUItem> itemList = skuBll.GetSKUItemsBySku(sku);
            if (itemList == null || itemList.Count == 0)
            {
                return "";
            }
            string str = "";
            foreach (var item in itemList)
            {
                str += item.AttributeName + "：" + item.AV_ValueStr + "  ";
            }
            return str;
        }

        private void DoCallback()
        {
            string action = this.Request.Form["Action"];
            this.Response.Clear();
            this.Response.ContentType = "application/json";
            string writeText = string.Empty;

            switch (action)
            {
                case "Apply":
                    writeText = CreateApply();
                    break;
                default:
                    break;

            }
            this.Response.Write(writeText);
            this.Response.End();
        }

        private string CreateApply() {  
            #region 收集数据
            string Items = Request.Form["Items"];
            long oId = Common.Globals.SafeLong(Request.Form["oId"], 0);
            int serviceType = Common.Globals.SafeInt(Request.Form["ServiceType"], 0);
            string content = Common.InjectionFilter.SqlFilter(Request.Form["Content"]);
            int pickType = Common.Globals.SafeInt(Request.Form["PickType"], 0);
            int regionId = Common.Globals.SafeInt(Request.Form["RegionId"], 0);
            string address = Common.InjectionFilter.SqlFilter(Request.Form["Address"]);
            string name = Common.InjectionFilter.SqlFilter(Request.Form["Name"]);
            string phone = Common.InjectionFilter.SqlFilter(Request.Form["Phone"]);
            int rgoodsType = Common.Globals.SafeInt(Request.Form["RGoodsType"], (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.Part);//退货类型  
            string imagesurlPath = Globals.SafeString(Request.Form["ImagesurlPath"], "");
            string imagesurlName = Globals.SafeString(Request.Form["ImagesurlName"], "");

            #endregion
            if (rgoodsType == (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.Part)
            {
                if (String.IsNullOrWhiteSpace(Items))
                {
                    return "False";
                }
            }
            if (oId <= 0 || serviceType <= 0 || pickType <0 || String.IsNullOrWhiteSpace(content)  || String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(phone))
            {
                return "False";
            }
            OrderInfo orderModel = _orderManage.GetModelInfo(oId);//订单
            if (orderModel == null  || orderModel.HasChildren)
            {
                return "False";
            }
            if (!returnorderBll.IsMeetCondition(orderModel, orderModel.BuyerID))
            {
                return "IsNotMeetCondition";//不满足退单条件 
            }
            Model.Shop.ReturnOrder.ReturnOrders returnModel = new Model.Shop.ReturnOrder.ReturnOrders();
            List<Model.Shop.ReturnOrder.ReturnOrderItems> returnItems = new List<Model.Shop.ReturnOrder.ReturnOrderItems>();
            Model.Shop.ReturnOrder.ReturnOrderItems tmpItem;

            #region 填充退单主表
            returnModel.OrderId = orderModel.OrderId;
            returnModel.OrderCode = orderModel.OrderCode;
            returnModel.SupplierId = orderModel.SupplierId.HasValue ? orderModel.SupplierId.Value : 0;
            returnModel.SupplierName = orderModel.SupplierName;

            returnModel.Status = ((int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.Status.UnHandle);
            returnModel.RefundStatus = ((int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.RefundStatus.UnRefund);
            returnModel.ContactName = name;
            returnModel.ContactPhone = phone;
            returnModel.ReturnUserId = orderModel.BuyerID;
            returnModel.ReturnUserName =orderModel.BuyerName;
            returnModel.CreateUserId = CurrentUser.UserID;
            returnModel.CreatedDate = DateTime.Now;
            returnModel.CustomerReview = 0;
            returnModel.Description = content;
            returnModel.LogisticStatus = ((int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.LogisticStatus.UnPick);
            returnModel.ReturnType = pickType;
            returnModel.ServiceType = serviceType;
            returnModel.PickRegionId = regionId;
            returnModel.ReturnOrderCode = returnModel.ReturnOrderPrefix + returnModel.CreatedDate.ToString("yyyyMMddHHmmssfff");
            returnModel.ReturnGoodsType = rgoodsType;
            
            if (regionId > 0)
            {
                BLL.Ms.Regions regionsManage = new BLL.Ms.Regions();
                returnModel.PickRegion = regionsManage.GetFullNameById4Cache(regionId);
            }
            returnModel.PickAddress = address;
            #endregion

            if (orderModel.OrderItems == null || orderModel.OrderItems.Count <= 0)
            {
                return "False";
            }

            #region 金额
            decimal ActualSalesTotal = 0;//商品实际出售总价
            decimal Amount = 0;//实付金额
            decimal AmountAdjusted = 0;//应退金额
            #endregion

            switch (rgoodsType)
            {
                case (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.Part:
                    #region 部分退
                    long itemId = 0;
                    int count = 0;
                   JsonArray jsonArray = YSWL.Json.Conversion.JsonConvert.Import<JsonArray>(Items);

                    if (jsonArray == null)
                    {
                        return "False";
                    }
                    ///退单商品数量(不含赠品)
                    int returnProdCount = 0;
                    ///订单商品数量(不含赠品)
                    int orderProdCount = 0;
                    #region 匹配退单项并新增
                    foreach (JsonObject jsonObject in jsonArray)
                    {
                        itemId = Common.Globals.SafeLong(jsonObject["itemId"], 0);
                        count = Common.Globals.SafeInt(jsonObject["count"], 0);
                        if (itemId <= 0 || count <= 0)
                        {
                            return "False";
                        }

                        //安全检测
                        foreach (Model.Shop.ReturnOrder.ReturnOrderItems item in returnItems)
                        {
                            if (item.ItemId == itemId)
                            {
                                return "Illegal";//数据重复，属于非法请求
                            }
                        }

                        foreach (Model.Shop.Order.OrderItems item in orderModel.OrderItems)
                        {
                            if (item.ItemId != itemId)
                            {
                                continue;
                            }
                            if (item.Quantity < count)
                            {
                                return "COUNTISTOOBIG";//数量超过购买数量
                            }
                            //新增退货项
                            tmpItem = returnitemBll.GetReturnItemInfo(item, count, returnModel.ReturnOrderCode);
                            returnItems.Add(tmpItem);
                            ActualSalesTotal += item.AdjustedPrice * count;
                            AmountAdjusted += item.AdjustedPrice * count;
                            returnProdCount += count;
                            break;
                        }
                    }
                    #endregion

                    #region 新增赠品
                    foreach (Model.Shop.Order.OrderItems item in orderModel.OrderItems)
                    {
                        if (item.ProductType != 2)
                        {
                            orderProdCount += item.ShipmentQuantity;
                            continue;
                        }
                        tmpItem = returnitemBll.GetReturnItemInfo(item, item.ShipmentQuantity, returnModel.ReturnOrderCode);
                        returnItems.Add(tmpItem);
                        //ActualSalesTotal += item.AdjustedPrice*item.ShipmentQuantity;
                        //AmountAdjusted += item.AdjustedPrice*item.ShipmentQuantity; 
                    }
                    #endregion

                    if (orderProdCount == returnProdCount)
                    {
                        return  "SELECTALLITEMS";//使用部分退时选中了全部商品，应选择整单退
                    }

                    #region 得到实付金额
                    //由于优惠劵可能会指定到某一分类或者某一商品,这时就应根据指定的分类或商品来均分，因此先注释掉这个算法，,先赋值为0，由审核人员自己核算，以后再修改
                    //if (!String.IsNullOrWhiteSpace(orderModel.CouponCode) && orderModel.CouponAmount.HasValue)
                    //{
                    //    //原订单使用了优惠劵，减去优惠劵的钱(均分), 得到实付金额
                    //    Amount = ActualSalesTotal - ((orderModel.CouponAmount.Value / orderProdCount) * returnProdCount);
                    //}else
                    //{
                    //Amount = ActualSalesTotal;
                    //}
                    Amount = 0;
                    AmountAdjusted = 0;
                    #endregion

                    #endregion
                    break;
                case (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.All:
                    Amount = orderModel.Amount;
                    AmountAdjusted = orderModel.Amount;
                    #region 整单退
                    foreach (Model.Shop.Order.OrderItems item in orderModel.OrderItems)
                    {
                        //新增退货项
                        tmpItem = returnitemBll.GetReturnItemInfo(item, item.ShipmentQuantity, returnModel.ReturnOrderCode);
                        returnItems.Add(tmpItem);
                        ActualSalesTotal += item.AdjustedPrice * item.ShipmentQuantity;
                    }
                    #endregion
                    break;
                default:
                    return "False";
            }

            #region 优惠劵信息
            returnModel.CouponAmount = orderModel.CouponAmount;
            returnModel.CouponCode = orderModel.CouponCode;
            returnModel.CouponName = orderModel.CouponName;
            returnModel.CouponValue = orderModel.CouponValue;
            returnModel.CouponValueType = orderModel.CouponValueType;
            #endregion

            if (returnItems == null || returnItems.Count <= 0)
            {
                return "ITEMSISNULL";//项为空
            }
            //if (Amount <= 0)
            //{
            //    return "AMOUNTISNULL";//总金额小于=0
            //}
            returnModel.Items = returnItems;
            returnModel.ActualSalesTotal = ActualSalesTotal;
            returnModel.Amount = Amount;
            returnModel.AmountAdjusted = AmountAdjusted;
            returnModel.AmountActual = 0;

            #region 移动文件
            if (!String.IsNullOrWhiteSpace(imagesurlPath) && !String.IsNullOrWhiteSpace(imagesurlName))
            {
                //创建文件夹  移动文件
                string path = string.Format("/Upload/Shop/ReturnOrder/{0}/", DateTime.Now.ToString("yyyyMM"));
                string mapPath = Request.MapPath(path);
                if (!Directory.Exists(mapPath))
                {
                    Directory.CreateDirectory(mapPath);
                }
                string[] pathArr = imagesurlPath.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                string[] namesArr = imagesurlName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (pathArr.Length != namesArr.Length)
                {
                    throw new ArgumentOutOfRangeException("路径与文件名长度不匹配！");
                }
                for (int i = 0; i < pathArr.Length; i++)
                {
                    System.IO.File.Move(Request.MapPath(pathArr[i]), mapPath + namesArr[i]);

                }
                returnModel.ImageUrl = path + string.Join("|" + path, namesArr);
            }
            #endregion

            if (returnorderBll.CreateReturnOrder(returnModel,  CurrentUser) > 0)
            {
                return "True";
            }
            else
            {
                return "Fail";
            }   
        }
    }
}