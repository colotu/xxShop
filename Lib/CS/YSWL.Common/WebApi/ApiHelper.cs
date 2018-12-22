using System;

namespace YSWL.Common.WebApi
{
    /// <summary>
    /// Api帮助类
    /// </summary>
    public class ApiHelper
    {
        //private string address = string.Empty;
        private string _configName;
        /// <summary>
        /// 配置节点名称
        /// </summary>
        public string ConfigName
        {
            get
            {
                if (string.IsNullOrEmpty(_configName))
                    _configName = "ApiUrl";
                return _configName;
            }
            set
            {
                _configName = value;
            }
        }
        /// <summary>
        /// 获取接口地址
        /// </summary>
        private string Address
        {
            get
            {
                string root = System.Configuration.ConfigurationManager.AppSettings[ConfigName];
                if (string.IsNullOrEmpty(root))
                {
                    throw new Exception($"接口节点【{ConfigName}】未配置");
                }
                return root;
            }
        }

        /// <summary>
        /// 请求接口数据转并换成实体
        /// </summary>
        /// <typeparam name="T">请求实体类型</typeparam>
        /// <typeparam name="TReslut">返回实体类型</typeparam>
        /// <param name="url">接口地址（不包含域名或者IP端口号）</param>
        /// <param name="model">请求数据</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public TReslut GetDataFromApi<T, TReslut>(string url, T model, string key=null)
            where T : class, new()
            where TReslut : class, new()
        {
            url = Address + url;
            string data = HttpUtil.PostDataToServer(url, model.ToJson(), key);
            return data.JsonToModel<TReslut>();
        }

        /// <summary>
        /// 请求接口数据转并换成实体
        /// </summary>
        /// <typeparam name="TReslut">返回实体类型</typeparam>
        /// <param name="url">接口地址（不包含域名或者IP端口号）</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public TReslut GetDataFromApi<TReslut>(string url,string key = null)
            where TReslut : class, new()
        {
            url = Address + url;
            string data = HttpUtil.PostDataToServer(url,null,key,"GET");
            return data.JsonToModel<TReslut>();
        }
    }
}