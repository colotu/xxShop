using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Net;
using CapCRL.Common.Bus.Jsons;
using System.Text;

namespace YSWL.MALL.Web
{
    public class shopCom
    {
        BLL.Pay.BalanceDetails balanceManage = new BLL.Pay.BalanceDetails();//转进钱包

        BLL.Members.PointsDetail pointbll = new BLL.Members.PointsDetail();

        public readonly string strZjbHost = ConfigurationSettings.AppSettings["zjbAPI"].ToString();//
        public readonly string strfwzxTC = ConfigurationSettings.AppSettings["fwzxTC"].ToString();//服务中心付款金额的提成比例
        public readonly string strshenghuoguanTC = ConfigurationSettings.AppSettings["shenghuoguanTC"].ToString();//生活馆付款金额的提成比例

        /// <summary>
        /// 输入用户名，获取用在商城的商城积分
        /// </summary>
        /// <param name="strUname"></param>
        /// <returns></returns>
        public int GetPointByUsername(string strUname)
        {
            try
            {
                return pointbll.GetPointByUsername(strUname);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 输入店铺编号，返回编号名称
        /// </summary>
        /// <param name="strUname"></param>
        /// <returns></returns>
        public string GetIsWdbh(string strIswdbh)
        {
            string strReturn = "";
            try
            {
                //string strUrl = string.Format("" + strZjbHost + "/by/user/registerUser!apiIsShenghuoguan.action?loginName={0}", strIswdbh);

                //strReturn = SendPostData(strUrl, "");

                //strReturn = strReturn.Replace("{", "").Replace("}", "").Replace("\"", "").Replace(":", ",");

                return strReturn.Trim();
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// 输入用户名，获取用户的姓名
        /// </summary>
        /// <param name="strUname"></param>
        /// <returns></returns>
        public string GetUserTrueName(string strUname)
        {
            string strReturn = "";
            try
            {
                //string strUrl = string.Format("" + strZjbHost + "/by/user/registerUser!apiFindUserByLoginName.action?loginName={0}", strUname);

                //strReturn = SendPostData(strUrl, "");

                //string[] arrMsg = strReturn.Replace("{", "").Replace("}", "").Replace("\"", "").Replace(":", ",").Split(',');

                //strReturn = arrMsg[3].ToString().Trim();
                return strReturn.Trim();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 输入用户名，获取用户的级别
        /// </summary>
        /// <param name="strUname"></param>
        /// <returns></returns>
        public string GetUserLeave(string strUname)
        {
            string strReturn = "";
            try
            {
                //string strUrl = string.Format("" + strZjbHost + "/by/user/registerUser!apiFindUserByLoginName.action?loginName={0}", strUname);

                //strReturn = SendPostData(strUrl, "");

                //string[] arrMsg = strReturn.Replace("{", "").Replace("}", "").Replace("\"", "").Replace(":", ",").Split(',');

               // strReturn = arrMsg[7].ToString();
                return strReturn;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 输入用户名，获取用户所属的生活馆
        /// </summary>
        /// <param name="strUname"></param>
        /// <returns></returns>
        public string GetUserShenghuoguanUsername(string strUname)
        {
            string strReturn = "";
            try
            {
                //string strUrl = string.Format("" + strZjbHost + "/by/user/registerUser!apiFindUserByLoginName.action?loginName={0}", strUname);

                //strReturn = SendPostData(strUrl, "");

                //string[] arrMsg = strReturn.Replace("{", "").Replace("}", "").Replace("\"", "").Replace(":", ",").Split(',');
                //strReturn = arrMsg[9].ToString();
                return strReturn;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 输入用户名和密码，判断用户名和密码是否存在，是否正确
        /// </summary>
        /// <param name="strUname"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        public bool IsHaveUsername(string strUname, string strPwd)
        {
            string strReturn = "";
            try
            {
                //string strUrl = string.Format("" + strZjbHost + "/by/user/registerUser!apiUserPassAuth.action?registerUserDTO.loginName={0}&registerUserDTO.password1={1}", strUname,strPwd);

                //strReturn = SendPostData(strUrl, "");

                //strReturn = strReturn.Replace("{", "").Replace("}", "").Replace("\"", "").Replace(":", ",").ToString();

                if (strReturn.Trim().ToUpper()=="YES")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 输入用户名，获取VIP用户的现金积分
        /// </summary>
        /// <param name="strUname"></param>
        /// <returns></returns>
        public decimal GetVipXJjfByusername(string strUname)
        {
            string strReturn = "";
            try
            {
                //string strUrl = string.Format("" + strZjbHost + "/by/user/registerUser!apiGetUserJiFen.action?registerUserDTO.loginName={0}", strUname);

                //strReturn = SendPostData(strUrl, "");

                //string[] arrMsg = strReturn.Replace("{", "").Replace("}", "").Replace("\"", "").Replace(":", ",").Split(',');
                //strReturn = arrMsg[0].ToString();

                return decimal.Parse(strReturn);
            }
            catch (Exception ex)
            {
                return 0;
            }
            // return decimal.Parse(arrMsg[1].ToString());
        }

        /// <summary>
        /// 输入用户名，获取vip用户的购物积分
        /// </summary>
        /// <param name="strUname"></param>
        /// <returns></returns>
        public decimal GetVipGwjfByusername(string strUname)
        {
            string strReturn = "";
            try
            {
                //string strUrl = string.Format("" + strZjbHost + "/by/user/registerUser!apiGetUserJiFen.action?registerUserDTO.loginName={0}", strUname);

                //strReturn = SendPostData(strUrl, "");

                //string[] arrMsg = strReturn.Replace("{", "").Replace("}", "").Replace("\"", "").Replace(":", ",").Split(',');

                //strReturn = arrMsg[1].ToString();

                return decimal.Parse(strReturn);
            }
            catch (Exception ex)
            {
                return 0;
            }
            // return decimal.Parse(arrMsg[1].ToString());
        }

        /// <summary>
        /// 输入用户名，获取VIP用户的商城积分
        /// </summary>
        /// <param name="strUname"></param>
        /// <returns></returns>
        public int GetVipShopjfByusername(string strUname)
        {
            string strReturn = "";
            try
            {
                //string strUrl = string.Format("" + strZjbHost + "/by/user/registerUser!apiGetUserJiFen.action?registerUserDTO.loginName={0}", strUname);

                //strReturn = SendPostData(strUrl, "");

                //string[] arrMsg = strReturn.Replace("{", "").Replace("}", "").Replace("\"", "").Replace(":", ",").Split(',');
                //string shjf = arrMsg[3].ToString();

                //strReturn=shjf.ToString().Substring(0, shjf.ToString().IndexOf('.'));

                return int.Parse(strReturn);
            }
            catch (Exception ex)
            {
                return 0;
            }
            // return decimal.Parse(arrMsg[1].ToString());
        }

        /// <summary>
        /// 操作VIP用户积分
        /// </summary>
        /// <param name="strMbNo">VIP用户名</param>
        /// <param name="strDtype">积分类型Dynamic(现金积分)，Shopping（购物积分），Mall_Consumption（商城积分</param>
        /// <param name="strmoeny">积分金额</param>
        public bool UpVIPjfByuser(string strMbNo, string strDtype, string strmoeny)
        {
            string strReturn = "";
            try
            {
                //string strUrl = string.Format("" + strZjbHost + "/by/user/registerUser!apiZhifu.action?registerUserDTO.loginName={0}&dtype={1}&moeny={2}", strMbNo, strDtype,strmoeny);

                //strReturn = SendPostData(strUrl, "");

                //strReturn = strReturn.Replace("{", "").Replace("}", "").Replace("\"", "").Replace(":", ",").ToString();

                if (strReturn.ToLower() == "success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        
        /// <summary>
        /// 输入信息，会员升级VIP
        /// </summary>
        /// <param name="strUname"></param>
        /// <returns></returns>
        public string UpVIPuser(string strloginname,string strNicename,string strpwd1,string strpwd2,string struserleave,string tjruser,string usertel,string Issup,string strShenghuoguan)
        {
            string strReturn = "";
            try
            {
                //string strUrl = string.Format("" + strZjbHost + "/by/user/registerUser!apiRegiserUser.action?registerUserDTO.loginName={0}&registerUserDTO.userName={1}&registerUserDTO.password1={2}&registerUserDTO.password2={3}&registerUserDTO.registerLevel={4}&registerUserDTO.recommendUser={5}&registerUserDTO.mobile={6}&registerUserDTO.supplier={7}&registerUserDTO.shenghuoguan={8}&registerUserDTO.authCode={9}", strloginname, strNicename, strpwd1, strpwd2, struserleave, tjruser, usertel, Issup, strShenghuoguan,"NOMOBILEAUTH");

                //strReturn = SendPostData(strUrl, "");

                //strReturn= strReturn.Replace("{", "").Replace("}", "").Replace("\"", "").Replace(":", ",").ToString();

                return strReturn;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 输入用户编号，激活会员
        /// </summary>
        /// <param name="strUname"></param>
        /// <returns></returns>
        public string UpVipJihuo(string strloginname)
        {
            string strReturn = "";
            try
            {
                //string strUrl = string.Format("" + strZjbHost + "/by/user/registerUser!apiActiveUser.action?registerUserDTO.loginName={0}&activeType={1}&optType={2}", strloginname,"SHOP", "userActive");

                //strReturn = SendPostData(strUrl, "");

                //strReturn = strReturn.Replace("{", "").Replace("}", "").Replace("\"", "").Replace(":", ",").ToString();

                return strReturn;
            }
            catch
            {
                return "";
            }
        }


        #region 发送请求API
        private string SendPostData(string Url, string strMessage)
        {
            string strResponse;
            // 初始化WebClient 
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.Headers.Add("Accept", "*/*");
            webClient.Headers.Add("Accept-Language", "zh-cn");
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            try
            {
                //byte[] responseData = webClient.UploadData(Url, "POST", Encoding.GetEncoding("UTF-8").GetBytes(strMessage));
                byte[] responseData = webClient.UploadData(Url, "POST", Encoding.GetEncoding("UTF-8").GetBytes("1"));
                string srcString = Encoding.UTF8.GetString(responseData); //Encoding.GetEncoding("GB2312").GetString(responseData);
                strResponse = srcString;
                return strResponse;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 签名字符串
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="sign_type">签名类型</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>签名结果</returns>
        private string Sign(string prestr)
        {
            string _input_charset = "UTF-8";
            StringBuilder sb = new StringBuilder(32);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();

        }
        #endregion


    }



}
