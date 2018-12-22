using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace YSWL.Common
{
    public sealed class Globals
    {
        /// <summary>
        /// Admin后台SessionKey
        /// </summary>
        public static string SESSIONKEY_ADMIN = "Admin_UserInfo";
        /// <summary>
        /// 企业后台SessionKey
        /// </summary>
        public static string SESSIONKEY_ENTERPRISE = "Enterprise_UserInfo";
        /// <summary>
        /// 供应商后台SessionKey
        /// </summary>
        public static string SESSIONKEY_SUPPLIER = "Supplier_UserInfo";
        /// <summary>
        /// 业务员用户
        /// </summary>
        public static string SESSIONKEY_SALES = "Sales_UserInfo";
        /// <summary>
        /// 代理商后台SessionKey
        /// </summary>
        public static string SESSIONKEY_AGENTS = "Agents_UserInfo";

        /// <summary>
        /// 用户后台SessionKey
        /// </summary>
        public static string SESSIONKEY_USER = SESSIONKEY_AGENTS = SESSIONKEY_ENTERPRISE = SESSIONKEY_SUPPLIER = SESSIONKEY_ADMIN = "UserInfo";
        /// <summary>
        /// 所有后台共享Session
        /// </summary>
        public static bool IsPublicSession
        {
            get { return false; }
        }

        /// <summary>
        /// 域名后缀规则
        /// </summary>
        private const string DOMAIN_RULES =
            "||com.cn|net.cn|org.cn|gov.cn|com.hk|公司|中国|网络|com|net|org|int|edu|gov|mil|arpa|Asia|biz|info|name|pro|coop|aero|museum|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cf|cg|ch|ci|ck|cl|cm|cn|co|cq|cr|cu|cv|cx|cy|cz|de|dj|dk|dm|do|dz|ec|ee|eg|eh|es|et|ev|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gh|gi|gl|gm|gn|gp|gr|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|in|io|iq|ir|is|it|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|ml|mm|mn|mo|mp|mq|mr|ms|mt|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nt|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|pt|pw|py|qa|re|ro|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr|st|su|sy|sz|tc|td|tf|tg|th|tj|tk|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|va|vc|ve|vg|vn|vu|wf|ws|ye|yu|za|zm|zr|zw|";

        /// <summary>
        /// 站内订单支付密钥
        /// </summary>
        public const string PAY_SECURITY_CODE = "YSWL_SENDING";
        public const string PAY_SECURITY_KEY = "YSWL_SECURITY_CODE";

        /// <summary>
        /// 余额支付接口ID
        /// </summary>
        public const int PAY_BALANCE_PAYMENTMODEID = -2;

        /// <summary>
        /// 积分比例
        /// </summary>
        public static decimal POINT_RATIO = 1;

        private Globals()
        {
        }

        public static string AppendQuerystring(string url, string querystring)
        {
            return AppendQuerystring(url, querystring, false).Trim();
        }

        public static string AppendQuerystring(string url, string querystring, bool urlEncoded)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            string str = "?";
            if (url.IndexOf('?') > -1)
            {
                if (!urlEncoded)
                {
                    str = "&";
                }
                else
                {
                    str = "&amp;";
                }
            }
            return (url + str + querystring);
        }

        public static string FullPath(string local)
        {
            if (string.IsNullOrEmpty(local))
            {
                return local;
            }
            if (local.ToLower(CultureInfo.InvariantCulture).StartsWith("http://"))
            {
                return local;
            }
            if (HttpContext.Current == null)
            {
                return local;
            }
            return FullPath(HostPath(HttpContext.Current.Request.Url), local);
        }

        public static string FullPath(string hostPath, string local)
        {
            return (hostPath + local);
        }

        public static string HostPath(Uri uri)
        {
            if (uri == null)
            {
                return string.Empty;
            }
            string str = (uri.Port == 80) ? string.Empty : (":" + uri.Port.ToString(CultureInfo.InvariantCulture));
            return string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[] { uri.Scheme, uri.Host, str });
        }

        public static string HtmlDecode(object target)
        {
            if (StringPlus.IsNullOrEmpty(target))
            {
                return "";
            }
            return HttpUtility.HtmlDecode(target.ToString().Trim());
        }

        public static string HtmlEncode(object target)
        {
            if (StringPlus.IsNullOrEmpty(target))
            {
                return "";
            }
            return HttpUtility.HtmlEncode(target.ToString().Trim());
        }

        public static void RedirectToSSL(HttpContext context)
        {
            if ((context != null) && !context.Request.IsSecureConnection)
            {
                Uri url = context.Request.Url;
                context.Response.Redirect("https://" + url.ToString().Substring(7));
            }
        }

        #region SafeParse
        public static bool SafeBool(object target, bool defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeBool(tmp, defaultValue);
        }
        public static bool SafeBool(string text, bool defaultValue)
        {
            bool flag;
            if (bool.TryParse(text, out flag))
            {
                defaultValue = flag;
            }
            return defaultValue;
        }

        public static DateTime SafeDateTime(object target, DateTime defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeDateTime(tmp, defaultValue);
        }
        public static DateTime SafeDateTime(string text, DateTime defaultValue)
        {
            DateTime time;
            if (DateTime.TryParse(text, out time))
            {
                defaultValue = time;
            }
            return defaultValue;
        }

        public static decimal SafeDecimal(object target, decimal defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeDecimal(tmp, defaultValue);
        }
        public static decimal SafeDecimal(string text, decimal defaultValue)
        {
            decimal num;
            if (decimal.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static short SafeShort(object target, short defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeShort(tmp, defaultValue);
        }
        public static short SafeShort(string text, short defaultValue)
        {
            short num;
            if (short.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static int SafeInt(object target, int defaultValue)
        {
            if (target == null) return defaultValue;
            if (target is decimal)
            {
                return decimal.ToInt32(Common.Globals.SafeDecimal(target,0));
            }
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeInt(tmp, defaultValue);
        }
        public static int SafeInt(string text, int defaultValue)
        {
            int num;
            if (int.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static int SafeInt(decimal target, int defaultValue)
        {
            return decimal.ToInt32(target);
        }

        public static int[] SafeInt(string[] text, int defaultValue)
        {
            if (text == null || text.Length < 1) return new[] { defaultValue };

            int[] nums = new int[text.Length];
            int tmp;
            for (int i = 0; i < text.Length; i++)
            {
                if (int.TryParse(text[i], out tmp)) nums[i] = tmp;
                else nums[i] = defaultValue;
            }
            return nums;
        }
        public static string SafeIntFilter(string text, int defaultValue, char split = ',')
        {
            if (string.IsNullOrWhiteSpace(text)) return defaultValue.ToString(CultureInfo.InvariantCulture);
            string[] tmpSplit = text.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries);
            if (tmpSplit.Length < 1) return defaultValue.ToString(CultureInfo.InvariantCulture);

            int tmp;
            for (int i = 0; i < tmpSplit.Length; i++)
            {
                if (int.TryParse(tmpSplit[i], out tmp))
                    tmpSplit[i] = tmp.ToString(CultureInfo.InvariantCulture);
                else
                    tmpSplit[i] = defaultValue.ToString(CultureInfo.InvariantCulture);
            }
            return string.Join(split.ToString(CultureInfo.InvariantCulture), tmpSplit);
        }

        public static long SafeLong(object target, long defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeLong(tmp, defaultValue);
        }
        public static long SafeLong(string text, long defaultValue)
        {
            long num;
            if (long.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static long[] SafeLong(string[] text, long defaultValue)
        {
            if (text == null || text.Length < 1) return new[] { defaultValue };

            long[] nums = new long[text.Length];
            long tmp;
            for (int i = 0; i < text.Length; i++)
            {
                if (long.TryParse(text[i], out tmp)) nums[i] = tmp;
                else nums[i] = defaultValue;
            }
            return nums;
        }

        public static string SafeLongFilter(string text, long defaultValue, char split = ',')
        {
            if (string.IsNullOrWhiteSpace(text)) return defaultValue.ToString(CultureInfo.InvariantCulture);
            string[] tmpSplit = text.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries);
            if (tmpSplit.Length < 1) return defaultValue.ToString(CultureInfo.InvariantCulture);

            long tmp;
            for (int i = 0; i < tmpSplit.Length; i++)
            {
                if (long.TryParse(tmpSplit[i], out tmp))
                    tmpSplit[i] = tmp.ToString(CultureInfo.InvariantCulture);
                else
                    tmpSplit[i] = defaultValue.ToString(CultureInfo.InvariantCulture);
            }
            return string.Join(split.ToString(CultureInfo.InvariantCulture), tmpSplit);
        }

        public static string SafeString(object target, string defaultValue)
        {
            if (null != target && "" != target.ToString())
            {
                return target.ToString();
            }
            return defaultValue;
        }

        #region SafeNullParse
        public static bool? SafeBool(object target, bool? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeBool(tmp, defaultValue);
        }
        public static bool? SafeBool(string text, bool? defaultValue)
        {
            bool flag;
            if (bool.TryParse(text, out flag))
            {
                defaultValue = flag;
            }
            return defaultValue;
        }

        public static DateTime? SafeDateTime(object target, DateTime? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeDateTime(tmp, defaultValue);
        }
        public static DateTime? SafeDateTime(string text, DateTime? defaultValue)
        {
            DateTime time;
            if (DateTime.TryParse(text, out time))
            {
                defaultValue = time;
            }
            return defaultValue;
        }

        public static decimal? SafeDecimal(object target, decimal? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeDecimal(tmp, defaultValue);
        }
        public static decimal? SafeDecimal(string text, decimal? defaultValue)
        {
            decimal num;
            if (decimal.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static short? SafeShort(object target, short? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeShort(tmp, defaultValue);
        }
        public static short? SafeShort(string text, short? defaultValue)
        {
            short num;
            if (short.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static int? SafeInt(object target, int? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeInt(tmp, defaultValue);
        }
        public static int? SafeInt(string text, int? defaultValue)
        {
            int num;
            if (int.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static long? SafeLong(object target, long? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeLong(tmp, defaultValue);
        }
        public static long? SafeLong(string text, long? defaultValue)
        {
            long num;
            if (long.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }
        #endregion

        #region SafeEnum
        /// <summary>
        /// 将枚举数值or枚举名称 安全转换为枚举对象
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">数值or名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <remarks>转换区分大小写</remarks>
        /// <returns></returns>
        public static T SafeEnum<T>(string value, T defaultValue) where T : struct
        {
            return SafeEnum<T>(value, defaultValue, false);
        }

        /// <summary>
        /// 将枚举数值or枚举名称 安全转换为枚举对象
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">数值or名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="ignoreCase">是否忽略大小写 true 不区分大小写 | false 区分大小写</param>
        /// <returns></returns>
        public static T SafeEnum<T>(string value, T defaultValue, bool ignoreCase) where T : struct
        {
            T result;
            if (Enum.TryParse<T>(value, ignoreCase, out result))
            {
                if (Enum.IsDefined(typeof(T), result))
                {
                    defaultValue = result;
                }
            }
            return defaultValue;
        }
        #endregion

        #endregion

        public static string StripAllTags(string strToStrip)
        {
            strToStrip = Regex.Replace(strToStrip, @"</p(?:\s*)>(?:\s*)<p(?:\s*)>", "\n\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = Regex.Replace(strToStrip, @"<br(?:\s*)/>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = Regex.Replace(strToStrip, "\"", "''", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = StripHtmlXmlTags(strToStrip);
            return strToStrip;
        }

        public static string StripForPreview(string content)
        {
            content = Regex.Replace(content, "<br>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<br/>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<br />", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<p>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = content.Replace("'", "&#39;");
            return StripHtmlXmlTags(content);
        }

        public static string HtmlEncodeForSpaceWrap(string content)
        {
            if (string.IsNullOrEmpty(content)) return string.Empty;
            return HttpUtility.HtmlEncode(content).Replace(" ", "&nbsp;").Replace("\n", "<br />");
        }

        public static string HtmlDecodeForSpaceWrap(string content)
        {
            if (string.IsNullOrEmpty(content)) return string.Empty;
            return HttpUtility.HtmlDecode(content).Replace("<br />", "\n").Replace("&nbsp;", " ");
        }

        public static string StripHtmlXmlTags(string content)
        {
            return Regex.Replace(content, "<[^>]+>", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static string StripScriptTags(string content)
        {
            content = Regex.Replace(content, "<script((.|\n)*?)</script>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "'javascript:", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return Regex.Replace(content, "\"javascript:", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        public static string ToDelimitedString(ICollection collection, string delimiter)
        {
            if (collection == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            if (collection is Hashtable)
            {
                foreach (object obj2 in ((Hashtable)collection).Keys)
                {
                    builder.Append(obj2.ToString() + delimiter);
                }
            }
            if (collection is ArrayList)
            {
                foreach (object obj3 in (ArrayList)collection)
                {
                    builder.Append(obj3.ToString() + delimiter);
                }
            }
            if (collection is string[])
            {
                foreach (string str in (string[])collection)
                {
                    builder.Append(str + delimiter);
                }
            }
            if (collection is MailAddressCollection)
            {
                foreach (MailAddress address in (MailAddressCollection)collection)
                {
                    builder.Append(address.Address + delimiter);
                }
            }
            return builder.ToString().TrimEnd(new char[] { Convert.ToChar(delimiter, CultureInfo.InvariantCulture) });
        }

        public static string UnHtmlEncode(string formattedPost)
        {
            RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
            formattedPost = Regex.Replace(formattedPost, "&quot;", "\"", options);
            formattedPost = Regex.Replace(formattedPost, "&lt;", "<", options);
            formattedPost = Regex.Replace(formattedPost, "&gt;", ">", options);
            return formattedPost;
        }

        public static string UrlDecode(string urlToDecode)
        {
            if (string.IsNullOrEmpty(urlToDecode))
            {
                return urlToDecode;
            }
            return HttpUtility.UrlDecode(urlToDecode, Encoding.UTF8);
        }

        public static string UrlEncode(string urlToEncode)
        {
            if (string.IsNullOrEmpty(urlToEncode))
            {
                return urlToEncode;
            }
            return HttpUtility.UrlEncode(urlToEncode, Encoding.UTF8);
        }

        public static string ApplicationPath
        {
            get
            {
                string applicationPath = "/";
                if (HttpContext.Current != null)
                {
                    applicationPath = HttpContext.Current.Request.ApplicationPath;
                }
                if (applicationPath == "/")
                {
                    return string.Empty;
                }
                return applicationPath.ToLower(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// 获取当前请求的客户端IP
        /// </summary>
        public static string ClientIP
        {
            get
            {
                string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.UserHostAddress;

                return result;
            }
        }

        /// <summary>
        /// 获取此实例的主机部分
        /// </summary>
        /// <remarks>此属性值不包括端口号</remarks>
        public static string DomainName
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return string.Empty;
                }
                return HttpContext.Current.Request.Url.Host;
            }
        }
        /// <summary>
        /// 获取服务器的域名系统 (DNS) 主机名或 IP 地址和端口号
        /// </summary>
        public static string DomainFullName
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return string.Empty;
                }
                return HttpContext.Current.Request.Url.Authority;
            }
        }

        public static string GenRandomCodeFor6()
        {
            //生成6位随机数验证码
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return ran.Next(000000, 999999).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageIndex">页索引,注意：从1开始</param>
        /// <param name="PageSize">每页大小</param>
        /// <returns>分好页的DataTable数据</returns>
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0) { return dt; }
            DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
            { return newdt; }

            if (rowend > dt.Rows.Count)
            { rowend = dt.Rows.Count; }
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }

        /// <summary>
        /// 返回分页的页数
        /// </summary>
        /// <param name="count">总条数</param>
        /// <param name="pageye">每页显示多少条</param>
        /// <returns>如果 结尾为0：则返回1</returns>
        public static int PageCount(int count, int pageye)
        {
            int page = 0;
            int sesepage = pageye;
            if (count % sesepage == 0) { page = count / sesepage; }
            else { page = (count / sesepage) + 1; }
            if (page == 0) { page += 1; }
            return page;
        }

        #region 获取顶级域名
        /// <summary>
        /// 获取顶级域名
        /// </summary>
        /// <remarks>如域名是IP此属性值不包括端口号</remarks>
        public static string TopLevelDomain
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return string.Empty;
                }
                return GetTopLevelDomain(HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower());
            }
        }
        /// <summary>
        /// 解析顶级域名
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns>如域名是IP此属性值不包括端口号</returns>
        public static string GetTopLevelDomain(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain)) return string.Empty;

            if (domain.IndexOf(".") < 1)
            {
                domain = domain.Split(':')[0];
                return domain;
            }

            string[] strArr = domain.Split(':')[0].Split('.');
            if (IsNumeric(strArr[strArr.Length - 1]))
            {
                return domain.Split(':')[0];
            }
            else
            {
                string tempDomain;
                if (strArr.Length >= 4)
                {
                    tempDomain = strArr[strArr.Length - 3] + "." + strArr[strArr.Length - 2] + "." +
                                 strArr[strArr.Length - 1];
                    if (DOMAIN_RULES.IndexOf("|" + tempDomain + "|") > 0)
                    {
                        return strArr[strArr.Length - 4] + "." + tempDomain;
                    }
                }
                if (strArr.Length >= 3)
                {
                    tempDomain = strArr[strArr.Length - 2] + "." + strArr[strArr.Length - 1];
                    if (DOMAIN_RULES.IndexOf("|" + tempDomain + "|") > 0)
                    {
                        return strArr[strArr.Length - 3] + "." + tempDomain;
                    }
                }
                if (strArr.Length >= 2)
                {
                    tempDomain = strArr[strArr.Length - 1];
                    if (DOMAIN_RULES.IndexOf("|" + tempDomain + "|") > 0)
                    {
                        return strArr[strArr.Length - 2] + "." + tempDomain;
                    }
                }
            }
            return domain;
        }
        private static bool IsNumeric(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            int len = value.Length;
            if ('-' != value[0] && '+' != value[0] && !char.IsNumber(value[0]))
            {
                return false;
            }
            for (int i = 1; i < len; i++)
            {
                if (!char.IsNumber(value[i]))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion


        #region 获取产品信息

        private static string _assemblyProduct = null;
        public static string AssemblyProduct
        {
            get
            {
                if (_assemblyProduct != null) return _assemblyProduct;

                // 获取此程序集上的所有 Product 属性
                Assembly assemblie = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly =>
                {
                    string name = assembly.GetName().Name.ToUpper();
                    return name.Contains("YSWL") && name.Contains("WEB");
                });

                if (assemblie == null) return string.Empty;
                
                    object[] attributes = assemblie.GetCustomAttributes(typeof (System.Reflection.AssemblyProductAttribute), false);
                    // 如果 Product 属性不存在，则返回一个空字符串
                    if (attributes.Length == 0)
                        return string.Empty;
                // 如果有 Product 属性，则返回该属性的值
                return _assemblyProduct = ((System.Reflection.AssemblyProductAttribute)attributes[0]).Product;
            }
        }
        #endregion

    }
}
