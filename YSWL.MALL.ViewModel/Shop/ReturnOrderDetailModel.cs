using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Shop
{
    public  class ReturnOrderDetailModel
    {
        public Model.Shop.ReturnOrder.ReturnOrders Info { get; set; }
        public List<Model.Shop.ReturnOrder.ReturnOrderItems> ListItems { get; set; }
        public List<Model.Shop.ReturnOrder.ReturnOrderAction> ListAction { get; set; }
    }
}
