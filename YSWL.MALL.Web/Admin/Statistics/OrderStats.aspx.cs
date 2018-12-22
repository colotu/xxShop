using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using System.Drawing;
using System.Data;
using System.Text;
using YSWL.Json;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class OrderStats : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Members.UsersExp userExBll = new BLL.Members.UsersExp();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_PageLoad)) && GetPermidByActID(Act_PageLoad) != -1)
                {
                    MessageBox.ShowAndBack(this, "您没有权限查看此页面");
                    return;
                }
                ShowInfo();
            }
        }

        public void ShowInfo()
        {
            JsonArray countArry = new JsonArray();
            JsonArray priceArry = new JsonArray();
            JsonObject countItemObj = null;
            JsonObject priceItemObj = null;
            this.hfCountData.Value = "";
            this.hfPriceData.Value = "";

            DateTime startDate = Common.Globals.SafeDateTime(this.txtStartDate.Text,new DateTime(1900,1,1));
            DateTime endDate = Common.Globals.SafeDateTime(this.txtEndDate.Text, DateTime.Now).Date.AddDays(1).AddSeconds(-1);

            //PC用户订单 已支付
            DataSet dsPC = YSWL.MALL.BLL.Shop.Order.OrderManage.Stat4OrderStatus(-1, startDate, endDate, 1, null);
            int CountPC = 0;
            decimal PricePC = 0;
            if (dsPC.Tables[0].Rows.Count > 0)
            {
                int? CountPC2 = dsPC.Tables[0].Rows[0].Field<int?>("ToalQuantity");
                CountPC = CountPC2.HasValue ? CountPC2.Value : 0;
                decimal? PricePC2 = dsPC.Tables[0].Rows[0].Field<decimal?>("ToalPrice");
                PricePC = PricePC2.HasValue ? PricePC2.Value : 0;
            }
            PricePC = Math.Round(PricePC,2);
            lblPricePC.Text = PricePC.ToString("F");
            lblCountPC.Text = CountPC.ToString();




            //微信用户订单 已支付
            DataSet dsWe = YSWL.MALL.BLL.Shop.Order.OrderManage.Stat4OrderStatus(-1, startDate, endDate, 2, null);
            int CountWe = 0;
            decimal PriceWe = 0;
            if (dsWe.Tables[0].Rows.Count > 0)
            {
                int? CountWe2 = dsWe.Tables[0].Rows[0].Field<int?>("ToalQuantity");
                CountWe = CountWe2.HasValue ? CountWe2.Value : 0;
                decimal? PriceWe2 = dsWe.Tables[0].Rows[0].Field<decimal?>("ToalPrice");
                PriceWe = PriceWe2.HasValue ? PriceWe2.Value : 0;
            }
            PriceWe = Math.Round(PriceWe,2);
            lblPriceWe.Text = PriceWe.ToString("F");
            lblCountWe.Text = CountWe.ToString();



            //代下单用户订单 已支付
            DataSet dsRen = YSWL.MALL.BLL.Shop.Order.OrderManage.Stat4OrderStatus(-1, startDate, endDate, 3, null);
            int CountRen = 0;
            decimal PriceRen = 0;
            if (dsRen.Tables[0].Rows.Count > 0)
            {
                int? CountRen2 = dsRen.Tables[0].Rows[0].Field<int?>("ToalQuantity");
                CountRen = CountRen2.HasValue ? CountRen2.Value : 0;
                decimal? PriceRen2 = dsRen.Tables[0].Rows[0].Field<decimal?>("ToalPrice");
                PriceRen = PriceRen2.HasValue ? PriceRen2.Value : 0;
            }
            PriceRen = Math.Round(PriceRen,2);
            lblPriceRen.Text = PriceRen.ToString("F");
            lblCountRen.Text = CountRen.ToString();



            //APP用户订单 已支付
            DataSet dsAPP = YSWL.MALL.BLL.Shop.Order.OrderManage.Stat4OrderStatus(-1, startDate, endDate, 5, null);
            int CountAPP = 0;
            decimal PriceAPP = 0;
            if (dsAPP.Tables[0].Rows.Count > 0)
            {
                int? CountAPP2 = dsAPP.Tables[0].Rows[0].Field<int?>("ToalQuantity");
                CountAPP = CountAPP2.HasValue ? CountAPP2.Value : 0;
                decimal? PriceAPP2 = dsAPP.Tables[0].Rows[0].Field<decimal?>("ToalPrice");
                PriceAPP = PriceAPP2.HasValue ? PriceAPP2.Value : 0;
            }
            PriceAPP = Math.Round(PriceAPP, 2);
            lblPriceAPP.Text = PriceAPP.ToString("F");
            lblCountAPP.Text = CountAPP.ToString();


            //APP用户订单 已支付
            DataSet dsCust = YSWL.MALL.BLL.Shop.Order.OrderManage.Stat4OrderStatus(-1, startDate, endDate, 4, null);
            int CountCust = 0;
            decimal PriceCust = 0;
            if (dsCust.Tables[0].Rows.Count > 0)
            {
                int? CountCust2 = dsCust.Tables[0].Rows[0].Field<int?>("ToalQuantity");
                CountCust = CountCust2.HasValue ? CountCust2.Value : 0;
                decimal? PriceCust2 = dsCust.Tables[0].Rows[0].Field<decimal?>("ToalPrice");
                PriceCust = PriceCust2.HasValue ? PriceCust2.Value : 0;
            }
            PriceCust = Math.Round(PriceCust, 2);
            lblPriceCust.Text = PriceCust.ToString("F");
            lblCountCust.Text = CountCust.ToString();

            if ((CountPC + CountWe + CountRen + CountAPP + CountCust) != 0)
            {
                countItemObj = new JsonObject();
                countItemObj.Accumulate("name", "PC商城");
                countItemObj.Accumulate("y", CountPC);
                countArry.Add(countItemObj);

                countItemObj = new JsonObject();
                countItemObj.Accumulate("name", "微信商城");
                countItemObj.Accumulate("y", CountWe);
                countArry.Add(countItemObj);

                countItemObj = new JsonObject();
                countItemObj.Accumulate("name", "业务员代下单");
                countItemObj.Accumulate("y", CountRen);
                countArry.Add(countItemObj);

                countItemObj = new JsonObject();
                countItemObj.Accumulate("name", "客服代下单");
                countItemObj.Accumulate("y", CountCust);
                countArry.Add(countItemObj);

                countItemObj = new JsonObject();
                countItemObj.Accumulate("name", "订货");
                countItemObj.Accumulate("y", CountAPP);
                countArry.Add(countItemObj);
                this.hfCountData.Value = countArry.ToString();
            }

            if ((PricePC + PriceWe + PriceRen + PriceAPP + PriceCust )!= 0)
            {
                priceItemObj = new JsonObject();
                priceItemObj.Accumulate("name", "PC商城");
                priceItemObj.Accumulate("y", Math.Round(PricePC,2));
                priceArry.Add(priceItemObj);

                priceItemObj = new JsonObject();
                priceItemObj.Accumulate("name", "微信商城");
                priceItemObj.Accumulate("y", Math.Round(PriceWe,2));
                priceArry.Add(priceItemObj);

                priceItemObj = new JsonObject();
                priceItemObj.Accumulate("name", "业务员代下单");
                priceItemObj.Accumulate("y", Math.Round(PriceRen,2));
                priceArry.Add(priceItemObj);

                priceItemObj = new JsonObject();
                priceItemObj.Accumulate("name", "客服代下单");
                priceItemObj.Accumulate("y", Math.Round(PriceCust, 2));
                priceArry.Add(priceItemObj);

                priceItemObj = new JsonObject();
                priceItemObj.Accumulate("name", "订货");
                priceItemObj.Accumulate("y", Math.Round(PriceAPP,2));
                priceArry.Add(priceItemObj);
                this.hfPriceData.Value = priceArry.ToString();
            }


        }


        protected void btnReStatistic_Click(object sender, EventArgs e)
        {
            ShowInfo();
        }
    }
}