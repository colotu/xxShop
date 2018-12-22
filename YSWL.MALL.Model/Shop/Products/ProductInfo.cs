/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：Products.cs
// 文件功能描述：
// 
// 创建标识： [Ben]  2012/06/11 20:36:27
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using YSWL.MALL.Model.Shop.PromoteSales;

namespace YSWL.MALL.Model.Shop.Products
{
    /// <summary>
    /// ProductInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    //[Serializable]
    public partial class ProductInfo
    {
        public ProductInfo()
        {}
        #region Model
        private int _categoryid;
        private int? _typeid;
        private long _productid;
        private int _brandid;
        private string _productname;
        private string _productcode;
        private int _supplierid;
        private int? _regionid;
        private string _shortdescription;
        private string _unit;
        private string _description;
        private string _meta_title;
        private string _meta_description;
        private string _meta_keywords;
        private int _salestatus;
        private DateTime _addeddate = DateTime.Now;
        private int _visticounts = 0;
        private int _salecounts = 0;
        private int? _stock;
        private int _displaysequence = 0;
        private int _lineid;
        private decimal? _marketprice;
        private decimal _lowestsaleprice;
        private int _penetrationstatus;
        private string _maincategorypath;
        private string _extendcategorypath;
        private bool _hassku;
        private decimal? _points = 0M;
        private string _imageurl;
        private string _thumbnailurl1;
        private string _thumbnailurl2;
        private string _thumbnailurl3;
        private string _thumbnailurl4;
        private string _thumbnailurl5;
        private string _thumbnailurl6;
        private string _thumbnailurl7;
        private string _thumbnailurl8;
        private int? _maxquantity = 0;
        private int? _minquantity = 0;
        private string _tags;
        private string _seourl;
        private string _seoimagealt;
        private string _seoimagetitle;
        private int _salestype = 1;
        private int _restrictioncount = 0;
        private string _deliverytip;
        private string _remark;

       
        /// <summary>
        /// 类别ID  0:正常有分类  -1:无分类
        /// </summary>
        public int CategoryId
        {
            set { _categoryid = value; }
            get { return _categoryid; }
        }
        /// <summary>
        /// 类型ID
        /// </summary>
        public int? TypeId
        {
            set { _typeid = value; }
            get { return _typeid; }
        }
        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId
        {
            set { _productid = value; }
            get { return _productid; }
        }
        /// <summary>
        /// 品牌Id
        /// </summary>
        public int BrandId
        {
            set { _brandid = value; }
            get { return _brandid; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string ProductName
        {
            set { _productname = value; }
            get { return _productname; }
        }
        /// <summary>
        /// 编码
        /// </summary>
        public string ProductCode
        {
            set { _productcode = value; }
            get { return _productcode; }
        }
        /// <summary>
        /// 供应商Id
        /// </summary>
        public int SupplierId
        {
            set { _supplierid = value; }
            get { return _supplierid; }
        }
        /// <summary>
        /// 地区Id
        /// </summary>
        public int? RegionId
        {
            set { _regionid = value; }
            get { return _regionid; }
        }
        /// <summary>
        /// 介绍
        /// </summary>
        public string ShortDescription
        {
            set { _shortdescription = value; }
            get { return _shortdescription; }
        }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            set { _unit = value; }
            get { return _unit; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// SEO_Title
        /// </summary>
        public string Meta_Title
        {
            set { _meta_title = value; }
            get { return _meta_title; }
        }
        /// <summary>
        /// SEO_Description
        /// </summary>
        public string Meta_Description
        {
            set { _meta_description = value; }
            get { return _meta_description; }
        }
        /// <summary>
        /// SEO_KeyWord
        /// </summary>
        public string Meta_Keywords
        {
            set { _meta_keywords = value; }
            get { return _meta_keywords; }
        }
        /// <summary>
        /// 状态  0:下架(仓库中)  1:上架 2:已删除
        /// </summary>
        public int SaleStatus
        {
            set { _salestatus = value; }
            get { return _salestatus; }
        }
        /// <summary>
        /// 添加日期
        /// </summary>
        public DateTime AddedDate
        {
            set { _addeddate = value; }
            get { return _addeddate; }
        }
        /// <summary>
        /// 访问次数
        /// </summary>
        public int VistiCounts
        {
            set { _visticounts = value; }
            get { return _visticounts; }
        }
        /// <summary>
        /// 售出总数
        /// </summary>
        public int SaleCounts
        {
            set { _salecounts = value; }
            get { return _salecounts; }
        }
        /// <summary>
        /// 商品库存 (暂未使用, 使用时请设置为非空)
        /// </summary>
        public int? Stock
        {
            set { _stock = value; }
            get { return _stock; }
        }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplaySequence
        {
            set { _displaysequence = value; }
            get { return _displaysequence; }
        }
        /// <summary>
        /// 生产线
        /// </summary>
        public int LineId
        {
            set { _lineid = value; }
            get { return _lineid; }
        }
        /// <summary>
        /// 市场价
        /// </summary>
        public decimal? MarketPrice
        {
            set { _marketprice = value; }
            get { return _marketprice; }
        }
        /// <summary>
        /// 最低价
        /// </summary>
        public decimal LowestSalePrice
        {
            set { _lowestsaleprice = value; }
            get { return _lowestsaleprice; }
        }
        /// <summary>
        /// 铺货状态  : 0未铺货 1已铺货
        /// </summary>
        public int PenetrationStatus
        {
            set { _penetrationstatus = value; }
            get { return _penetrationstatus; }
        }
        /// <summary>
        /// 分类路径
        /// </summary>
        public string MainCategoryPath
        {
            set { _maincategorypath = value; }
            get { return _maincategorypath; }
        }
        /// <summary>
        /// 扩展路径
        /// </summary>
        public string ExtendCategoryPath
        {
            set { _extendcategorypath = value; }
            get { return _extendcategorypath; }
        }
        /// <summary>
        /// 是否有SKU
        /// </summary>
        public bool HasSKU
        {
            set { _hassku = value; }
            get { return _hassku; }
        }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal? Points
        {
            set { _points = value; }
            get { return _points; }
        }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImageUrl
        {
            set { _imageurl = value; }
            get { return _imageurl; }
        }
        /// <summary>
        /// 图片路径1
        /// </summary>
        public string ThumbnailUrl1
        {
            set { _thumbnailurl1 = value; }
            get { return _thumbnailurl1; }
        }
        /// <summary>
        /// 图片路径2
        /// </summary>
        public string ThumbnailUrl2
        {
            set { _thumbnailurl2 = value; }
            get { return _thumbnailurl2; }
        }
        /// <summary>
        /// 图片路径3
        /// </summary>
        public string ThumbnailUrl3
        {
            set { _thumbnailurl3 = value; }
            get { return _thumbnailurl3; }
        }
        /// <summary>
        /// 图片路径4
        /// </summary>
        public string ThumbnailUrl4
        {
            set { _thumbnailurl4 = value; }
            get { return _thumbnailurl4; }
        }
        /// <summary>
        /// 图片路径5
        /// </summary>
        public string ThumbnailUrl5
        {
            set { _thumbnailurl5 = value; }
            get { return _thumbnailurl5; }
        }
        /// <summary>
        /// 图片路径6
        /// </summary>
        public string ThumbnailUrl6
        {
            set { _thumbnailurl6 = value; }
            get { return _thumbnailurl6; }
        }
        /// <summary>
        /// 图片路径7
        /// </summary>
        public string ThumbnailUrl7
        {
            set { _thumbnailurl7 = value; }
            get { return _thumbnailurl7; }
        }
        /// <summary>
        /// 图片路径8
        /// </summary>
        public string ThumbnailUrl8
        {
            set { _thumbnailurl8 = value; }
            get { return _thumbnailurl8; }
        }
        /// <summary>
        /// 最大购买量
        /// </summary>
        public int? MaxQuantity
        {
            set { _maxquantity = value; }
            get { return _maxquantity; }
        }
        /// <summary>
        /// 最小购买量
        /// </summary>
        public int? MinQuantity
        {
            set { _minquantity = value; }
            get { return _minquantity; }
        }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tags
        {
            set { _tags = value; }
            get { return _tags; }
        }
        /// <summary>
        /// SEO Url地址优化规则
        /// </summary>
        public string SeoUrl
        {
            set { _seourl = value; }
            get { return _seourl; }
        }
        /// <summary>
        /// SEO 图片Alt信息
        /// </summary>
        public string SeoImageAlt
        {
            set { _seoimagealt = value; }
            get { return _seoimagealt; }
        }
        /// <summary>
        /// SEO 图片Title信息
        /// </summary>
        public string SeoImageTitle
        {
            set { _seoimagetitle = value; }
            get { return _seoimagetitle; }
        }
        /// <summary>
        /// 销售类型  1:正常    2:预定  3:赠品
        /// </summary>
        public int SalesType
        {
            set { _salestype = value; }
            get { return _salestype; }
        }
        /// <summary>
        /// 限购数
        /// </summary>
        public int RestrictionCount
        {
            set { _restrictioncount = value; }
            get { return _restrictioncount; }
        }
        /// <summary>
        /// 预定配送提示 (预计送达时间)
        /// </summary>
        public string DeliveryTip
        {
            set { _deliverytip = value; }
            get { return _deliverytip; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        private decimal? _gwjf;
        /// <summary>
        /// 抵扣积分金额
        /// </summary>
        public decimal? Gwjf
        {
            set { _gwjf = value; }
            get { return _gwjf; }
        }

        #endregion Model
        #region 扩展属性
        private string _staticurl;
        private int _limitCount;
        /// <summary>
        /// 
        /// </summary>
        public string StaticUrl
        {
            set { _staticurl = value; }
            get { return _staticurl; }
        }

        public int LimitCount
        {
            set { _limitCount = value; }
            get { return _limitCount; }
        }
        public decimal SalePrice
        {
            set; get;
        }


        public string CategoryName
        {
            set;
            get;
        }

        #region 促销扩展属性
        /// <summary>
        /// 促销价格
        /// </summary>
        public decimal ProSalesPrice
        {
            set;
            get;
        }

        /// <summary>
        /// 促销ID
        /// </summary>
        public int CountDownId
        {
            set;
            get;
        }
        /// <summary>
        /// 促销结束时间
        /// </summary>
        public DateTime ProSalesEndDate
        {
            set;
            get;
        }
        /// <summary>
        /// 促销说明
        /// </summary>
        public string ProSalesDescription
        {
            set;
            get;
        }


        #endregion
        #region 团购
        /// <summary>
        /// 团购ID
        /// </summary>
        public YSWL.MALL.Model.Shop.PromoteSales.GroupBuy GroupBuy=new GroupBuy();
       
        #endregion

        #region 商家分类
        /// <summary>
        /// 分类id
        /// </summary>
        public int SuppCategoryId
        {
            set;
            get;
        }

        /// <summary>
        /// 分类Path
        /// </summary>
        public string SuppCategoryPath
        {
            set;
            get;
        }
        #endregion
        #region 商家扩展属性
        /// <summary>
        /// 商家商品分类的父分类Id
        /// </summary>
        public int SuppParentCategoryId
        {
            set;
            get;
        }
        /// <summary>
        /// 商家商品分类名称
        /// </summary>
        public string  SuppCategoryName
        {
            set;
            get;
        }
        #endregion

        /// <summary>
        /// 库存
        /// </summary>
        public long StockNum
        {
            set;
            get;
        }
        public string[] SkuValues
        {
            get;
            set;
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            set;
            get;
        }
        #endregion
    }
}
 