using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.UserCenter
{
    public class UserBindList
    {
        public int Count = 0;
        public YSWL.MALL.Model.Members.UserBind Sina = null;//新浪微博
        public YSWL.MALL.Model.Members.UserBind TenCent = null;//腾讯微博
        public YSWL.MALL.Model.Members.UserBind QZone = null;//QQ空间
    }
}
