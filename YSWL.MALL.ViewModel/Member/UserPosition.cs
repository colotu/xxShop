using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Member
{
  public  class UserPosition
    {
      public YSWL.MALL.Model.Members.Users UserModel { get; set; }
      public decimal Longitude { get; set; }
      public decimal Latitude { get; set; }
      public int RegionId { get; set; }
      public string ShopPhoto { get; set; }
    }
}
