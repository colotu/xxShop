/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：Attributes.cs
// 文件功能描述：
// 
// 创建标识： [Ben]  2012/06/15 14:25:00
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
namespace YSWL.MALL.Model.Shop.Products
{
    /// <summary>
    /// AttributeInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public partial class AttributeInfo
    {
        private List<AttributeValue> listAttributeValues = new List<AttributeValue>();

        public List<AttributeValue> AttributeValues
        {
            get { return listAttributeValues; }
            set { listAttributeValues = value; }
        }

        private List<string> _valueStr = new List<string>();

        public List<string> ValueStr
        {
            get { return _valueStr; }
            set { _valueStr = value; }
        }

        private bool _userDefinedPic;

        public bool UserDefinedPic
        {
            get { return _userDefinedPic; }
            set { _userDefinedPic = value; }
        }
    }
}

