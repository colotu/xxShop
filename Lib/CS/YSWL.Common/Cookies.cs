using System;
using System.Web;

namespace YSWL.Common
{
    /// <summary>
    /// Cookies封装操作类
    /// </summary>
    public class Cookies
    {
        #region 获取Cookie
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string getCookie(string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            try
            {
                if (cookie != null)
                {
                    return HttpUtility.UrlDecode(cookie.Values[value]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getKeyCookie(string name)
        {
            string value = "";
            if (System.Web.HttpContext.Current!=null&&System.Web.HttpContext.Current.Request.Cookies[name] != null)
            {
                value = HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request.Cookies[name].Value);
            }
            return value;
        }


        #endregion

        #region 设置Cookie
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="time">过期时间</param>
        /// <returns></returns>
        public static bool setCookie(string key, string value, double time)
        {
            try
            {
                string s = HttpUtility.UrlEncode(value);
                HttpCookie cookie = new HttpCookie(key)
                {
                    Expires = DateTime.Now.AddMinutes(time)
                };
                cookie.Values.Add("Value", HttpContext.Current.Server.UrlEncode(s));
                HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool setKeyCookie(string key, string value, double time)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(key)
                {
                    Expires = DateTime.Now.AddMinutes(time)
                };
                cookie.Value = HttpUtility.UrlEncode(value);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="domin"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool setKeyCookie(string key, string value,string domin, double time)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(key)
                {
                    Expires = DateTime.Now.AddMinutes(time),
                };
                if (!string.IsNullOrEmpty(domin))
                {
                    cookie.Domain = domin;
                }
                cookie.Value = HttpUtility.UrlEncode(value);
                System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 更新Cookie
        /// <summary>
        /// 更新Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="time">过期时间</param>
        /// <returns></returns>
        public static bool updateCookies(string key, string value, double time)
        {
            bool flag;
            try
            {
                HttpContext.Current.Response.Cookies[key]["Value"] = value;
                flag = setCookie(key, value, time);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        /// <summary>
        /// 更新KeyCookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="time">过期时间</param>
        /// <returns></returns>
        public static bool updateKeyCookies(string key, string value, double time)
        {
            bool flag;
            try
            {
                flag = setKeyCookie(key, value, time);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        } 

        #endregion
    }
}
