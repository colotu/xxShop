using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSWL.MALL.Model.Shop.Products
{
    public class Reviews
    {
        /// <summary>
        /// 评论Id
        /// </summary>
        public int ReviewId { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string ReviewText { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 属性信息
        /// </summary>
        public string Attribute { get; set; }
        /// <summary>
        /// 评论图片路径
        /// </summary>
        public string ImagesPath { get; set; }
        /// <summary>
        /// 评论图片名字
        /// </summary>
        public string ImagesNames { get; set; }
        /// <summary>
        /// 商品主图片
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Gravatar { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
    }
}
