using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YSWL.MALL.ViewModel.ERP
{
    public class AccountInfo
    {
        public string loginName { get; set; }
        public string userName { get; set; }
        public string trueName { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public int userId { get; set; }
        public string userType { get; set; }
        public bool activity { get; set; }
    }
}