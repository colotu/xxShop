/*
 ASP.NET MvcPager 分页组件
 Copyright:2009-2011 陕西省延安市吴起县 杨涛\Webdiyer (http://www.webdiyer.com)
 Source code released under Ms-PL license
 */

using System.Collections.Generic;
using System.Linq;

namespace YSWL.Common.Page
{
    public static class PageLinqExtensions
    {
        public static Page.PagedList<T> ToPagedList<T>
            (
                this IQueryable<T> allItems,
                int pageIndex,
                int pageSize
            )
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            return new Page.PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }

        /// <summary>
        /// 转换为分页集合
        /// </summary>
        /// <typeparam name="T">List数据对象</typeparam>
        /// <param name="list">List数据</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页集合对象</returns>
        public static Page.PagedList<T> ToPagedList<T>
            (
                this IList<T> list,
                int pageIndex,
                int pageSize,
                int? totalCount
            )
        {
            if (pageIndex < 1) pageIndex = 1;
            int totalItemCount = totalCount ?? list.Count();
            return new Page.PagedList<T>(list, pageIndex, pageSize, totalItemCount);
        }
    }
}
