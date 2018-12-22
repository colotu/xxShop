/**
* WeiboTemplate.cs
*
* 功 能： [N/A]
* 类 名： WeiboTemplate
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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using YSWL.OAuth.Http.Converters;
using YSWL.OAuth.Http.Converters.Json;
using YSWL.OAuth.Json;
using YSWL.OAuth.Rest.Client;
using YSWL.OAuth.v2;

namespace YSWL.OAuth.Tencent.Weibo
{
    public class WeiboTemplate : AbstractOAuth2ApiBinding, Weibo.IWeibo
    {
        internal static readonly Uri API_URI_BASE = new Uri("https://open.t.qq.com/");
        internal const string PROFILE_URL = "api/user/info?openid={openid}&clientip={clientip}&access_token={access_token}&oauth_consumer_key={oauth_consumer_key}&oauth_version={oauth_version}&scope={scope}";

        private const string STATUSES_URL = "api/t/{method}";

        /// <summary>
        /// 腾讯帐号 OpenId
        /// </summary>
        protected AccessGrant _accessGrant;

        protected string _clientId;

        public WeiboTemplate(AccessGrant accessGrant, string clientId)
            : base(accessGrant)
        {
            _clientId = clientId;
            _accessGrant = accessGrant;
        }

        protected override void ConfigureRestTemplate(RestTemplate restTemplate)
        {
            restTemplate.BaseAddress = API_URI_BASE;
        }

        protected override IList<IHttpMessageConverter> GetMessageConverters()
        {
            IList<IHttpMessageConverter> converters = base.GetMessageConverters();
            converters.Add(new MsJsonHttpMessageConverter()); // Ms light-weight JSON
            return converters;
        }

        protected override OAuth2Version GetOAuth2Version()
        {
            return OAuth2Version.Bearer;
        }

        #region IWeibo 成员

        public System.Threading.Tasks.Task<JsonValue> GetUserProfileAsync()
        {
            if (_accessGrant.ExtraData == null || _accessGrant.ExtraData.Length < 3) throw new ArgumentNullException("No openid/openkey/clientip");
            IDictionary<string, object> qqParams = new Dictionary<string, object>();
            qqParams.Add("openid", _accessGrant.ExtraData[0]);
            qqParams.Add("access_token", _accessGrant.AccessToken);
            qqParams.Add("oauth_consumer_key", _clientId);
            qqParams.Add("clientip", _accessGrant.ExtraData[2]);
            qqParams.Add("oauth_version", "2.a");
            qqParams.Add("scope", "all");
            return this.RestTemplate.GetForObjectAsync<JsonValue>(PROFILE_URL, qqParams);
        }

        #region 发布微博
        /// <summary>
        /// 发布文字微博
        /// </summary>
        /// <param name="status">内容</param>
        public System.Threading.Tasks.Task UpdateStatusAsync(string status)
        {
            NameValueCollection qqParams = new NameValueCollection(1);
            qqParams.Add("openid", _accessGrant.ExtraData[0]);
            qqParams.Add("access_token", _accessGrant.AccessToken);
            qqParams.Add("oauth_consumer_key", _clientId);
            qqParams.Add("content", status);
            qqParams.Add("clientip", _accessGrant.ExtraData[2]);
            qqParams.Add("oauth_version", "2.a");
            qqParams.Add("scope", "all");
            return this.RestTemplate.PostForMessageAsync(STATUSES_URL, qqParams, "add");
        }

        /// <summary>
        /// 发布图片微博
        /// </summary>
        /// <param name="status">内容</param>
        /// <param name="fileInfo">图片</param>
        public System.Threading.Tasks.Task UploadStatusAsync(string status, System.IO.FileInfo fileInfo)
        {
            IDictionary<string, object> qqParams = new Dictionary<string, object>();
            qqParams.Add("openid", _accessGrant.ExtraData[0]);
            qqParams.Add("access_token", _accessGrant.AccessToken);
            qqParams.Add("oauth_consumer_key", _clientId);
            qqParams.Add("content", status);
            qqParams.Add("pic", fileInfo);
            qqParams.Add("clientip", _accessGrant.ExtraData[2]);
            qqParams.Add("oauth_version", "2.a");
            qqParams.Add("scope", "all");
            return this.RestTemplate.PostForObjectAsync<JsonValue>(STATUSES_URL, qqParams, "add_pic");
        }
        #endregion

        public IRestOperations RestOperations
        {
            get { return this.RestTemplate; }
        }

        #endregion
    }
}
