using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSWL.Json;

namespace YSWL.MALL.ViewModel.Order
{
    public class FreightVm
    {
        public int userId { get; set; }
        public int shipId { get; set; }
        public int shipTypeId { get; set; }
        public List<ProductVm> productList { get; set; }
    }
}
