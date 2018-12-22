using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSWL.MALL.ViewModel
{
    public class SaleData
    {
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sequence { set; get; }

        /// <summary>
        /// 相对宽度
        /// </summary>
        public string Width { set; get; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { set; get; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { set; get; }
    }
}
