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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using YSWL.OAuth.Http.Converters;
using YSWL.OAuth.Http.Converters.Json;
using YSWL.OAuth.Json;
using YSWL.OAuth.Rest.Client;
using YSWL.OAuth.v2;

namespace YSWL.OAuth.Sina
{
    public class WeiboTemplate : AbstractOAuth2ApiBinding, IWeibo
    {
        private static readonly Uri API_URI_BASE = new Uri("https://api.weibo.com/2/");
        private const string STATUSES_URL = "statuses/{method}.json";

        private AccessGrant _accessGrant;

        public WeiboTemplate(AccessGrant accessGrant)
            : base(accessGrant)
        {
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

        #region 获取用户信息
        private const string PROFILE_URL = "users/show.json?uid={uid}&access_token={access_token}";
        public System.Threading.Tasks.Task<JsonValue> GetUserProfileAsync()
        {
            if (_accessGrant.ExtraData == null || _accessGrant.ExtraData.Length < 1) throw new ArgumentNullException("No uid");
            IDictionary<string, object> sinaParams = new Dictionary<string, object>();
            sinaParams.Add("uid", _accessGrant.ExtraData[0]);
            sinaParams.Add("access_token", _accessGrant.AccessToken);
            return this.RestTemplate.GetForObjectAsync<JsonValue>(PROFILE_URL, sinaParams);
        }
        #endregion

        #region 发布微博
        /// <summary>
        /// 发布文字微博
        /// </summary>
        /// <param name="status">内容</param>
        public System.Threading.Tasks.Task UpdateStatusAsync(string status)
        {
            //IDictionary<string, object> sinaParams = new Dictionary<string, object>();
            //sinaParams.Add("access_token", _accessGrant.AccessToken);
            //sinaParams.Add("status", status);
            NameValueCollection content = new NameValueCollection(1);
            content.Add("access_token", _accessGrant.AccessToken);
            content.Add("status", status);
            //JsonObject json = new JsonObject();
            //json.AddValue("access_token", new JsonValue(_accessGrant.AccessToken));
            //json.AddValue("status", new JsonValue(status));
            return this.RestTemplate.PostForMessageAsync(STATUSES_URL, content, "update");
        }

        /// <summary>
        /// 发布图片微博
        /// </summary>
        /// <param name="status">内容</param>
        /// <param name="fileInfo"></param>
        public System.Threading.Tasks.Task UploadStatusAsync(string status, System.IO.FileInfo fileInfo)
        {
            IDictionary<string, object> sinaParams = new Dictionary<string, object>();
            sinaParams.Add("access_token", _accessGrant.AccessToken);
            sinaParams.Add("status", status);
            sinaParams.Add("pic", fileInfo);
            return this.RestTemplate.PostForObjectAsync<JsonValue>(STATUSES_URL, sinaParams, "upload");
        }
        #endregion

        public IRestOperations RestOperations
        {
            get { return this.RestTemplate; }
        }

        #endregion

    }
}
