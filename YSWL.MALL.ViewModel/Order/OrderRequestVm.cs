using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSWL.MALL.ViewModel.Order
{
    public class OrderRequestVm
    {
        public int salesUserId { get; set; }
        public int custUserId { get; set; }
        public string startDate {get; set; }
        public string endDate { get; set; }
        public string keyWords { get; set; }
        public int type { get; set; }
        public string customerName { get; set; }
        public string shipName { get; set; }
        public string shipCellPhone { get; set; }
        public string source { get; set; }
    }
}
