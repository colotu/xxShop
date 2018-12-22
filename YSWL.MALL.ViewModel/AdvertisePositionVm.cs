using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSWL.MALL.ViewModel
{
    public class AdvertisePositionVm
    {
        /// <summary>
        /// 广告位Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 广告位名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否轮播
        /// </summary>
        public bool IsOne { get; set; }
        /// <summary>
        /// 广告信息
        /// </summary>
        public List<AdvertisementVm> Advertisement { get; set; }
    }

    public class AdvertisementVm
    {
        /// <summary>
        /// 广告Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        public string ImageName { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }
    }
}
