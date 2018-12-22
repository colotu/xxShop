using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Json;
using YSWL.Json.Conversion;

namespace YSWL.MALL.Web.Admin
{
    public partial class AutoUpdate : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 62; } } //系统管理_是否显示清空缓存

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
                
            }
        }

        protected void btnCheck_Click(object sender, System.EventArgs e)
        {
           
        }



        private YSWL.Json.JsonObject GetVersionInfo()
        {
            StreamReader reader = null;
            string posturl = "http://shop1.maticsoft.com/api/v1/shop.aspx";    //接口调用地址
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(posturl);
                string requestId = Guid.NewGuid().ToString();   //随机的请求ID
                string timespan = DateTime.Now.ToString("yyyyMMddHHmmss");  //时间搓
                string apiKey = "YSWL";   //接口Key,与系统配置一致

                #region  双重MD5 加密
                MD5 md5 = MD5.Create();
                string firstMd5;
                string secondMd5;
                byte[] bs = Encoding.UTF8.GetBytes(requestId + timespan + apiKey);
                byte[] hs = md5.ComputeHash(bs);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hs)
                {
                    // 以十六进制格式格式化
                    sb.Append(b.ToString("x2"));
                }
                firstMd5 = sb.ToString();
                sb.Clear();
                byte[] bss = Encoding.UTF8.GetBytes(requestId + timespan + firstMd5);
                byte[] hss = md5.ComputeHash(bss);
                foreach (byte b in hss)
                {
                    // 以十六进制格式格式化
                    sb.Append(b.ToString("x2"));
                }
                secondMd5 = sb.ToString();
                #endregion
                //安全验证的相关数据放入报文中
                request.Method = "POST";
                request.Headers.Add("requestId", requestId);
                request.Headers.Add("timespan", timespan);
                request.Headers.Add("md5hex", secondMd5);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                JsonObject postObject = new JsonObject();
                postObject.Put("id", requestId);        //请求ID与报文中的一致
                postObject.Put("method", "checkVersion");  //调用接口方法名称
                //传方法参数值，采用Json的形式传值
                //JsonObject paramObject = new JsonObject();
                //paramObject.Put("UserName", "13121455506");
                //paramObject.Put("Password", "1111");
                //postObject.Put("params", paramObject);
                string postData = System.Text.RegularExpressions.Regex.Unescape(jss.Serialize(postObject));
                byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(postData);
                request.ContentLength = postdata.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(postdata, 0, postdata.Length);
                newStream.Close();
                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();//得到结果
                //返回的结果是Json字符串的形式
                YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(content);
                return jsonObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}