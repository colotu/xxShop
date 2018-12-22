using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSWL.MALL.Model.Shop
{
  public class BaseInfo
  {
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }
  }
}
