/**
* IWeibo.cs
*
* 功 能： [N/A]
* 类 名： IWeibo
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/5 12:22:25  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Threading.Tasks;
using YSWL.OAuth.Json;
using YSWL.OAuth.Rest.Client;

namespace YSWL.OAuth.Tencent.Weibo
{
    public interface IWeibo : IApiBinding
    {
        Task<JsonValue> GetUserProfileAsync();
        Task UpdateStatusAsync(string status);
        Task UploadStatusAsync(string status, System.IO.FileInfo fileInfo);

        /// <summary>
        /// Gets the underlying <see cref="OAuth.Rest.Client.IRestOperations"/> object allowing for consumption of Do endpoints 
        /// that may not be otherwise covered by the API binding. 
        /// </summary>
        /// <remarks>
        /// The <see cref="OAuth.Rest.Client.IRestOperations"/> object returned is configured to include an OAuth "Authorization" header on all requests.
        /// </remarks>
        IRestOperations RestOperations { get; }
    }
}
