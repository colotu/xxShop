/**
* QQTemplatecs.cs
*
* 功 能： [N/A]
* 类 名： QQTemplatecs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/5 12:21:36  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System.Collections.Generic;
using YSWL.OAuth.Http.Converters;
using YSWL.OAuth.v2;

namespace YSWL.OAuth.Tencent.QQ
{
    public class QConnectOpenIdTemplate : QConnectTemplate
    {
        public QConnectOpenIdTemplate(AccessGrant accessGrant)
            : base(accessGrant, null)
        {
            _accessGrant = accessGrant;
        }

        protected override IList<IHttpMessageConverter> GetMessageConverters()
        {
            IList<IHttpMessageConverter> converters = base.GetMessageConverters();
            converters.Clear();
            converters.Add(new Converters.OpenIdJsonHttpMessageConverter());
            return converters;
        }
    }
}
