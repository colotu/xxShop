using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Supplier
{
    public partial class WeChatUser : YSWL.WeChat.Model.Core.User
    {
        public string GroupName{get;set;}
        public string UserStatus { get; set; }
    }
}
