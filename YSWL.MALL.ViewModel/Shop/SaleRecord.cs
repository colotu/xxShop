using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Shop
{
   public class SaleRecord
    {
        public string BuyName { set; get; }
        public decimal BuyPrice  { set; get; }
        public int BuyCount { set; get; }
        public DateTime BuyDate { set; get; }
    }
}
