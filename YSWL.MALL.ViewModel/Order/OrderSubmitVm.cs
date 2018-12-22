using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSWL.Json;

namespace YSWL.MALL.ViewModel.Order
{
    public class OrderSubmitVm
    {
        public int salesUserId { get; set; }
        public int userId{ get; set; }
        public int shipId{ get; set; }
        public int regionId{ get; set; }
        public int shipTypeId{ get; set; }
        public int paymentId{ get; set; }
        public List<ProductVm> productList{ get; set; }
        public string remark{ get; set; }
        public string invoiceHeader{ get; set; }
        public string invoiceContent{ get; set; }
        public string couponCode{ get; set; }
        public decimal couponAmount { get; set; }//优惠金额
        public int proSaleId { get; set; } = -1;
        public int groupBuyId { get; set; } = -1;
    }

    public class ProductVm
    {
        public string SKU { get; set; }
        public int Count { get; set; }
        public decimal AdjustPrice { get; set; }
    }
}
