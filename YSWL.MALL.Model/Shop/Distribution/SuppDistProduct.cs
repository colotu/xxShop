/**  版本信息模板在安装目录下，可自行修改。
* SuppDistProduct.cs
*
* 功 能： N/A
* 类 名： SuppDistProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/27 17:36:24   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.Distribution
{
	/// <summary>
	/// SuppDistProduct:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SuppDistProduct
	{
		public SuppDistProduct()
		{}
		#region Model
		private long _productid;
		private int? _typeid;
		private int _brandid;
		private string _productname;
		private int _salestatus;
		private DateTime _addeddate= DateTime.Now;
		private int _salecounts=0;
		private decimal? _marketprice;
		private int _stock;
		private decimal _lowestsaleprice;
		private bool _hassku;
		private int _displaysequence=0;
		private string _imageurl;
		private string _thumbnailurl1;
		/// <summary>
		/// 商品ID
		/// </summary>
		public long ProductId
		{
			set{ _productid=value;}
			get{return _productid;}
		}
		/// <summary>
		/// 类型ID
		/// </summary>
		public int? TypeId
		{
			set{ _typeid=value;}
			get{return _typeid;}
		}
		/// <summary>
		/// 品牌Id
		/// </summary>
		public int BrandId
		{
			set{ _brandid=value;}
			get{return _brandid;}
		}
		/// <summary>
		/// 名称
		/// </summary>
		public string ProductName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// 状态  0:下架(仓库中)  1:上架 2:已删除
		/// </summary>
		public int SaleStatus
		{
			set{ _salestatus=value;}
			get{return _salestatus;}
		}
		/// <summary>
		/// 添加日期
		/// </summary>
		public DateTime AddedDate
		{
			set{ _addeddate=value;}
			get{return _addeddate;}
		}
		/// <summary>
		/// 售出总数
		/// </summary>
		public int SaleCounts
		{
			set{ _salecounts=value;}
			get{return _salecounts;}
		}
		/// <summary>
		/// 市场价
		/// </summary>
		public decimal? MarketPrice
		{
			set{ _marketprice=value;}
			get{return _marketprice;}
		}
		/// <summary>
		/// 库存
		/// </summary>
		public int Stock
		{
			set{ _stock=value;}
			get{return _stock;}
		}
		/// <summary>
		/// 最低价
		/// </summary>
		public decimal LowestSalePrice
		{
			set{ _lowestsaleprice=value;}
			get{return _lowestsaleprice;}
		}
		/// <summary>
		/// 是否有SKU
		/// </summary>
		public bool HasSKU
		{
			set{ _hassku=value;}
			get{return _hassku;}
		}
		/// <summary>
		/// 显示顺序
		/// </summary>
		public int DisplaySequence
		{
			set{ _displaysequence=value;}
			get{return _displaysequence;}
		}
		/// <summary>
		/// 图片路径
		/// </summary>
		public string ImageUrl
		{
			set{ _imageurl=value;}
			get{return _imageurl;}
		}
		/// <summary>
		/// 图片路径1
		/// </summary>
		public string ThumbnailUrl1
		{
			set{ _thumbnailurl1=value;}
			get{return _thumbnailurl1;}
		}
		#endregion Model

	}
}

