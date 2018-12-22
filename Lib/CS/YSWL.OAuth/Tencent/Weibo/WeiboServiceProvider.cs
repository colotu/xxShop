/**
* WeiboServiceProvider.cs
*
* 功 能： [N/A]
* 类 名： WeiboServiceProvider
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/5 12:22:06  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using YSWL.OAuth.v2;

namespace YSWL.OAuth.Tencent.Weibo
{
    public class WeiboServiceProvider : AbstractOAuth2ServiceProvider<Weibo.IWeibo>
    {
        public static string AuthorizeUrl = "https://open.t.qq.com/cgi-bin/oauth2/authorize";
        public static string TokenUrl = "https://open.t.qq.com/cgi-bin/oauth2/access_token";

        protected string _clientId;

        public WeiboServiceProvider(string clientId, string clientSecret)
            : base(new Weibo.WeiboOAuth2Template(clientId, clientSecret,
                AuthorizeUrl,
                TokenUrl))
        {
            if (string.IsNullOrWhiteSpace(AuthorizeUrl))
                throw new ArgumentNullException("AuthorizeUrl");
            if (string.IsNullOrWhiteSpace(TokenUrl))
                throw new ArgumentNullException("TokenUrl");
            _clientId = clientId;
        }

        public override Weibo.IWeibo GetApi(AccessGrant accessGrant)
        {
            return new WeiboTemplate(accessGrant, _clientId);
        }
    }
}
