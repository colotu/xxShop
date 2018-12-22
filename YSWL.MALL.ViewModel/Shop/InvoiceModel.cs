using System;
using System.Collections.Generic;

namespace YSWL.MALL.ViewModel.Shop
{
    public class InvoiceModel
    {
        public List<Model.Shop.Order.OrderLookupItems> Header { get; set; }
        public List<Model.Shop.Order.OrderLookupItems> Content { get; set; }
    }
}
