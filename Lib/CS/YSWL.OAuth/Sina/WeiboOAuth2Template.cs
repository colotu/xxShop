/**
* WeiboOAuth2Template.cs
*
* 功 能： [N/A]
* 类 名： WeiboOAuth2Template
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/5 19:04:27  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using YSWL.OAuth.v2;

namespace YSWL.OAuth.Sina
{
    public class WeiboOAuth2Template : OAuth2Template
    {
        public WeiboOAuth2Template(string clientId, string clientSecret, string authorizeUrl, string accessTokenUrl)
            : base(clientId, clientSecret,
                authorizeUrl,
                accessTokenUrl,
                true)
        {

        }
        protected override Rest.Client.RestTemplate CreateRestTemplate()
        {
            Rest.Client.RestTemplate restTemplate = new Rest.Client.RestTemplate();
            ((Http.Client.WebClientHttpRequestFactory)restTemplate.RequestFactory).Expect100Continue = false;

            System.Collections.Generic.IList<Http.Converters.IHttpMessageConverter> converters = new System.Collections.Generic.List<Http.Converters.IHttpMessageConverter>(2);
            Http.Converters.FormHttpMessageConverter formConverter = new Http.Converters.FormHttpMessageConverter();
            // Always read NameValueCollection as 'application/x-www-form-urlencoded' even if contentType not set properly by provider
            formConverter.SupportedMediaTypes.Add(Http.MediaType.ALL);
            converters.Add(formConverter);
            converters.Add(new Http.Converters.Json.MsJsonHttpMessageConverter());
            restTemplate.MessageConverters = converters;

            return restTemplate;
        }

        protected override AccessGrant CreateAccessGrant(string accessToken, string scope, string refreshToken, int? expiresIn, Json.JsonValue response)
        {
            //获取扩展数据 新浪uid
            return new AccessGrant(accessToken, scope, refreshToken, expiresIn, new[] { response.GetValue<string>("uid") });
        }
    }
}
