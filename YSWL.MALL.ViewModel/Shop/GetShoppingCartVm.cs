using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSWL.MALL.ViewModel.Shop
{
    public class ShoppingCartGetVm
    {
        public List<Order.ProductVm> ProductList { get; set; }
        public int UserId { get; set; }
        public int DepotId { get; set; }
    }
}
