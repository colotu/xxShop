/**
* CookieProvider.cs
*
* 功 能： Cookie购物车抽象类
* 类 名： CookieProvider
*
* Ver   变更日期    部门      担当者 变更内容
* ─────────────────────────────────
* V0.01 2013/05/08  研发部    姚远   初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌─────────────────────────────────┐
*│ 此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露． │
*│ 版权所有：云商未来（北京）科技有限公司                           │
*└─────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Web;
using YSWL.Common.DEncrypt;
using YSWL.Json.Conversion;
using YSWL.ShoppingCart.Model;

namespace YSWL.ShoppingCart.Core
{
    //TODO: Cookie Json 购物车
    /// <summary>
    /// Cookie购物车类
    /// </summary>
    /// <remarks>虚实现购物车基础功能</remarks>
    /// <typeparam name="TCartInfo">购物车对象</typeparam>
    /// <typeparam name="TCartItemInfo">项对象</typeparam>
    public class CookieProvider<TCartInfo, TCartItemInfo> : ICartProvider<TCartInfo, TCartItemInfo>
        where TCartInfo : CartInfo<TCartItemInfo>, new()
        where TCartItemInfo : CartItemInfo, new()
    {
        protected readonly int UserId;
        protected const int NOLOGIN_USERID = -1;
        protected const string BASE_COOKIE_INDEXKEY = "yswl_v1.2_shoppingcart_index_{0}";
        protected const string BASE_COOKIE_DATAKEY = "yswl_v1.2_shoppingcart_data_{0}";
        protected readonly string CartDataCookieIndexKey;
        protected readonly string CartDataCookieDataKey;
        protected const int CookieItemsMaxNum = 20;
        protected const int CookieKeyMaxNum = 15;
        protected readonly double CookieExpiresDayNum = 1.0;
        protected readonly bool CancelCompress;

        #region 构造方法

        /// <summary>
        /// 构造Cookie购物车
        /// </summary>
        /// <param name="userId">用户Id 未登录时为-1</param>
        public CookieProvider(int userId,bool cancelCompress = false)
        {
            UserId = userId;
            CancelCompress = cancelCompress;

            CartDataCookieIndexKey = string.Format(BASE_COOKIE_INDEXKEY, userId);
            CartDataCookieDataKey = string.Format(BASE_COOKIE_DATAKEY, userId + "_{0}");
        }

        public CookieProvider(string key)
        {
            CartDataCookieIndexKey = string.Format(BASE_COOKIE_INDEXKEY, key);
            CartDataCookieDataKey = string.Format(BASE_COOKIE_DATAKEY, key + "_{0}");
        }
        //protected readonly IDataProvider<TCartInfo, TCartItemInfo> _provider;

        //protected CookieProviderBase(IDataProvider<TCartInfo, TCartItemInfo> provider)
        //{
        //    if (provider == null) throw new ArgumentNullException("provider");
        //    _provider = provider;
        //}
        #endregion

        #region 将未登录的购物车加载到当前用户下
        /// <summary>
        /// 将未登录的购物车加载到当前用户下
        /// </summary>
        /// <param name="userId">用户Id</param>
        public static void LoadShoppingCart(int userId, bool cancelCompress = false)
        {
            if (userId < 1) return;

            //判断 未登录购物车 是否存在
            HttpCookie cookie = GetCookie(
                string.Format(BASE_COOKIE_INDEXKEY, NOLOGIN_USERID));
            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value)) return;

            //Load OldCart
            CookieProvider<TCartInfo, TCartItemInfo> cartOld =
                new CookieProvider<TCartInfo, TCartItemInfo>(NOLOGIN_USERID, cancelCompress);
            //Load newCart
            CookieProvider<TCartInfo, TCartItemInfo> cartNew =
                new CookieProvider<TCartInfo, TCartItemInfo>(userId, cancelCompress);

            TCartInfo cart = cartOld.GetShoppingCart();
            //更新CartItem中的UserId
            if (cart.Items != null && cart.Items.Count > 0)
            {
                foreach (var item in cart.Items)
                {
                    item.UserId = userId;
                }
                cartNew.SaveShoppingCart(cart);
            }


            cartOld.ClearShoppingCart();
        }

        #endregion

        #region 添加指定项

        /// <summary>
        /// 添加指定项
        /// </summary>
        public virtual void AddItem(TCartItemInfo itemInfo)
        {
            if (itemInfo == null) return;
            if (itemInfo.Quantity <= 0)
            {
                itemInfo.Quantity = 1;
            }
            TCartInfo cartInfo = this.GetShoppingCart();
            TCartItemInfo tmpItem = cartInfo[itemInfo.SKU];
            if (tmpItem != null)
                tmpItem.Quantity += itemInfo.Quantity;
            else
            {
                itemInfo.ItemId = GenerateLastItemId(cartInfo.Items);
                cartInfo.Items.Add(itemInfo);
            }
            this.SaveShoppingCart(cartInfo);
        }

        #endregion

        #region 检查数量 子类实现
        ///// <summary>
        ///// 检查数量
        ///// </summary>
        //public abstract string CheckQuantity(TCartInfo cartInfo);
        #endregion

        #region 清空购物车
        /// <summary>
        /// 清空购物车
        /// </summary>
        public virtual void ClearShoppingCart()
        {
            HttpCookie cookie = GetCookie(CartDataCookieIndexKey);
            if (cookie == null) return;

            ResolveCartItemCookie(cookie, itemCookie =>
                {
                    //ClearItem
                    itemCookie = new HttpCookie(itemCookie.Name)
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    System.Web.HttpContext.Current.Response.Cookies.Set(itemCookie);
                });

            //ClearIndex
            cookie = new HttpCookie(cookie.Name)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
        }
        #endregion

        #region 获取购物车数据
        #region 解析CookieItem数据

        private string Encode(string data)
        {
            //if (CancelCompress) return data;
            return HttpUtility.UrlEncode(GZip.DeflateCompress(data));
        }
        private string Decode(string data)
        {
            //if (CancelCompress) return data;
            return GZip.DeflateDecompress(HttpUtility.UrlDecode(data));
        }

        /// <summary>
        /// 解析CookieItem数据
        /// </summary>
        /// <param name="cookieIndex">cookieIndex对象</param>
        /// <param name="methodProcessCartItem">处理每项的方法</param>
        protected virtual void ResolveCartItemCookie(HttpCookie cookieIndex,
            Action<HttpCookie> methodProcessCartItem)
        {
            if (cookieIndex == null) return;

            string tmpKeys;
            try
            {
                tmpKeys = Decode(cookieIndex.Value);
            }
            catch (Exception)
            {
                cookieIndex = new HttpCookie(cookieIndex.Name)
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                System.Web.HttpContext.Current.Response.Cookies.Set(cookieIndex);
                return;
            }
            if (string.IsNullOrWhiteSpace(tmpKeys)) return;
            string[] keys = tmpKeys.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (keys.Length < 1) return;

            foreach (string itemKey in keys)
            {
                if (string.IsNullOrWhiteSpace(itemKey)) continue;
                HttpCookie itemCookie = GetCookie(itemKey);
                if (itemCookie == null) continue;
                try
                {
                    methodProcessCartItem(itemCookie);
                }
                catch (Exception)
                {
                    itemCookie = new HttpCookie(itemCookie.Name)
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    System.Web.HttpContext.Current.Response.Cookies.Set(itemCookie);
                    continue;
                }
            }
        }

        /// <summary>
        /// 获取Cookie 以Response为主
        /// </summary>
        public static HttpCookie GetCookie(string name)
        {
            foreach (string strCookie in HttpContext.Current.Response.Cookies.AllKeys)
            {
                if (strCookie == name)
                {
                    return HttpContext.Current.Response.Cookies[strCookie];
                }
            }

            foreach (string strCookie in HttpContext.Current.Request.Cookies.AllKeys)
            {
                if (strCookie == name)
                {
                    return HttpContext.Current.Request.Cookies[strCookie];
                }
            }

            return null;
        }
        #endregion

        /// <summary>
        /// 获取购物车NextItemId
        /// </summary>
        protected virtual int GenerateLastItemId(List<TCartItemInfo> list)
        {
            if (list == null || list.Count < 1) return 1;
            return list[list.Count - 1].ItemId + 1;
        }

        /// <summary>
        /// 获取购物车
        /// </summary>
        public virtual TCartInfo GetShoppingCart()
        {
            HttpCookie cookie = GetCookie(CartDataCookieIndexKey);
            return GetShoppingCart(cookie);
        }

        /// <summary>
        /// 获取已选内容的购物车
        /// </summary>
        /// <remarks>提交订单页面专用</remarks>
        public virtual TCartInfo GetShoppingCart4Selected()
        {
            HttpCookie cookie = GetCookie(CartDataCookieIndexKey);
            TCartInfo cartInfo = GetShoppingCart(cookie);
            if (cartInfo.Quantity > 0)
            {
                cartInfo.Items = cartInfo.Items.FindAll(xx => xx.Selected);
            }
            return cartInfo;
        }

        /// <summary>
        /// 获取购物车
        /// </summary>
        public virtual TCartInfo GetShoppingCart(HttpCookie cookie)
        {
            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
                return new TCartInfo();

            TCartInfo shoppingCart = new TCartInfo();
            shoppingCart.Items = new List<TCartItemInfo>();
            ResolveCartItemCookie(cookie, itemCookie =>
            {
                string items = itemCookie.Value;
                if (string.IsNullOrWhiteSpace(items)) return;

                List<TCartItemInfo> list;
                try
                {
                    list = (List<TCartItemInfo>)
                        JsonConvert.Import<IList<TCartItemInfo>>(
                            Decode(items));
                }
                catch
                {
                    //Error
                    throw;
                }
                if (list == null || list.Count < 1) return;
                //Add
                list.ForEach(xx => shoppingCart.Items.Add(xx));
            });
            return shoppingCart;
        }
        #endregion

        #region 删除指定项
        /// <summary>
        /// 删除指定项
        /// </summary>
        /// <param name="itemId"></param>
        public virtual void RemoveItem(int itemId)
        {
            TCartInfo shoppingCart = GetShoppingCart();
            if (shoppingCart.Items == null || shoppingCart.Items.Count < 1) return;

            shoppingCart.Items.RemoveAll(xx => xx.ItemId == itemId);
            this.SaveShoppingCart(shoppingCart);
        }
        #endregion

        #region 保存购物车到Cookie
        /// <summary>
        /// 保存购物车到Cookie
        /// </summary>
        public void SaveShoppingCart(TCartInfo cartInfo)
        {
            if (cartInfo == null || cartInfo.Items == null || cartInfo.Items.Count < 0)
            {
                this.ClearShoppingCart();
                return;
            }
            if (CookieItemsMaxNum * CookieKeyMaxNum < cartInfo.Items.Count)
            {
                throw new IndexOutOfRangeException("SaveShoppingCart: MaxCount");
            }


            int indexKey = 1, dataCount = cartInfo.Items.Count - 1;
            List<TCartItemInfo> list = new List<TCartItemInfo>();
            List<string> listKeyName = new List<string>();
            string cookieName = string.Format(CartDataCookieDataKey, indexKey++);
            listKeyName.Add(cookieName);

            HttpCookie itemCookie = new HttpCookie(cookieName);
            for (int i = 0; i < cartInfo.Items.Count; i++)
            {
                if (i > 0 && i % CookieItemsMaxNum == 0)
                {
                    //SaveData
                    itemCookie.Value = Encode(JsonConvert.ExportToString(list));
                    itemCookie.Expires = DateTime.Now.AddDays(CookieExpiresDayNum);
                    HttpContext.Current.Response.Cookies.Set(itemCookie);

                    //Reset
                    if (i < dataCount)
                    {
                        listKeyName.Add(string.Format(CartDataCookieDataKey, indexKey));
                        itemCookie = new HttpCookie(string.Format(CartDataCookieDataKey, indexKey++));
                        list.Clear();
                    }
                }
                list.Add(cartInfo.Items[i]);
            }
            //SaveLastData
            itemCookie.Value = Encode(JsonConvert.ExportToString(list));
            itemCookie.Expires = DateTime.Now.AddDays(CookieExpiresDayNum);
            HttpContext.Current.Response.Cookies.Set(itemCookie);

            HttpCookie cookie = new HttpCookie(CartDataCookieIndexKey);

            //SaveIndexKey
            cookie.Value = Encode(string.Join(",", listKeyName));
            cookie.Expires = DateTime.Now.AddDays(CookieExpiresDayNum);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
        #endregion

        #region 更新指定项数量
        /// <summary>
        /// 更新指定项数量
        /// </summary>
        public virtual void UpdateItemQuantity(int itemId, int quantity)
        {
            if (quantity <= 0)
            {
                this.RemoveItem(itemId);
            }
            else
            {
                TCartInfo shoppingCart = GetShoppingCart();
                if (shoppingCart.Items == null || shoppingCart.Items.Count < 1) return;

                shoppingCart.Items.ForEach(xx => { if (xx.ItemId == itemId) xx.Quantity = quantity; });
                this.SaveShoppingCart(shoppingCart);
            }
        }
        #endregion

    }
}

