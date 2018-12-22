/**
* ComparerFactroy.cs
*
* 功 能： [N/A]
* 类 名： ComparerFactroy
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/27 18:24:28  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;

namespace YSWL.Common
{
    /// <summary>
    /// Common Comaparer Generate Class, supported 4 paramers limitted
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ComparerFactroy<T>
    {
        public static IComparer<T> Create<V1>(Func<T, V1> keySelector1)
        {
            return new CommonComparer<V1, Object, Object, Object>(keySelector1, Comparer<V1>.Default, null, null, null, null, null, null);
        }
        public static IComparer<T> Create<V1, V2>(Func<T, V1> keySelector1, Func<T, V2> keySelector2)
        {
            return new CommonComparer<V1, V2, Object, Object>(keySelector1, Comparer<V1>.Default, keySelector2, Comparer<V2>.Default, null, null, null, null);
        }
        public static IComparer<T> Create<V1, V2, V3>(Func<T, V1> keySelector1, Func<T, V2> keySelector2, Func<T, V3> keySelector3)
        {
            return new CommonComparer<V1, V2, V3, Object>(keySelector1, Comparer<V1>.Default, keySelector2, Comparer<V2>.Default, keySelector3, Comparer<V3>.Default, null, null);
        }
        public static IComparer<T> Create<V1, V2, V3, V4>(Func<T, V1> keySelector1, Func<T, V2> keySelector2, Func<T, V3> keySelector3, Func<T, V4> keySelector4)
        {
            return new CommonComparer<V1, V2, V3, V4>(keySelector1, Comparer<V1>.Default, keySelector2, Comparer<V2>.Default, keySelector3, Comparer<V3>.Default, keySelector4, Comparer<V4>.Default);
        }

        private class CommonComparer<V1, V2, V3, V4> : IComparer<T>
        {
            Func<T, V1> keySelector1;
            Func<T, V2> keySelector2;
            Func<T, V3> keySelector3;
            Func<T, V4> keySelector4;

            IComparer<V1> comparer1;
            IComparer<V2> comparer2;
            IComparer<V3> comparer3;
            IComparer<V4> comparer4;

            public CommonComparer(Func<T, V1> keySelector1, IComparer<V1> compare1,
                                    Func<T, V2> keySelector2, IComparer<V2> compare2,
                                    Func<T, V3> keySelector3, IComparer<V3> compare3,
                                    Func<T, V4> keySelector4, IComparer<V4> compare4)
            {
                this.keySelector1 = keySelector1;
                this.keySelector2 = keySelector2;
                this.keySelector3 = keySelector3;
                this.keySelector4 = keySelector4;
                this.comparer1 = compare1;
                this.comparer2 = compare2;
                this.comparer3 = compare3;
                this.comparer4 = compare4;
            }

            #region IComparer<T> 成员

            public int Compare(T x, T y)
            {
                int retVal = 0;
                if (keySelector1 != null)
                    retVal = comparer1.Compare(keySelector1(x), keySelector1(y));
                if (keySelector2 != null && retVal == 0)
                    retVal = comparer2.Compare(keySelector2(x), keySelector2(y));
                if (keySelector3 != null && retVal == 0)
                    retVal = comparer3.Compare(keySelector3(x), keySelector3(y));
                if (keySelector4 != null && retVal == 0)
                    retVal = comparer4.Compare(keySelector4(x), keySelector4(y));

                return retVal;
            }
            #endregion
        }
    }
}
