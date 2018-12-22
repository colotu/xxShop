/**
* QQServiceProvider.cs
*
* 功 能： [N/A]
* 类 名： QQServiceProvider
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

namespace YSWL.OAuth.Tencent.QQ
{
    public class QConnectServiceProvider : AbstractOAuth2ServiceProvider<IQConnect>
    {
        public static string AuthorizeUrl = "https://graph.qq.com/oauth2.0/authorize";
        public static string TokenUrl = "https://graph.qq.com/oauth2.0/token";

        protected string _clientId;

        public QConnectServiceProvider(string clientId, string clientSecret)
            : base(new QConnectOAuth2Template(clientId, clientSecret,
                AuthorizeUrl,
                TokenUrl))
        {
            if (string.IsNullOrWhiteSpace(AuthorizeUrl))
                throw new ArgumentNullException("AuthorizeUrl");
            if (string.IsNullOrWhiteSpace(TokenUrl))
                throw new ArgumentNullException("TokenUrl");
            _clientId = clientId;
        }

        public override IQConnect GetApi(AccessGrant accessGrant)
        {
            return new QConnectTemplate(accessGrant, _clientId);
        }
    }
}
