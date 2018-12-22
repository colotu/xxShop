using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSWL.MALL.ViewModel.Shop
{
    public class ShipTypeModel:YSWL.MALL.Model.Shop.Shipping.ShippingType
    {
        private int shipType;

        /// <summary>
        /// 配送方式选择
        /// </summary>
        public int ShipType
        {
            set { shipType = value; }
            get { return shipType; }
        }
    }
}
