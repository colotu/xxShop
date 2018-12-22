using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YSWL.MALL.Web.Components
{
    public  class DateTimeHelper
    {
        /// <summary>
        /// 将时间转换成几小时、几天之类的格式
        /// </summary>
        /// <param name="maxDays">最多显示几天前</param>
        /// <param name="minuteDif">几分钟之内显示成刚刚</param>
        /// <param name="dateTime">需要转换的时间</param>
        /// <returns></returns>
        public static string ConvertDateToTime(DateTime? dateTime,int maxDays=3,int minuteDif=3)
        {
            string hoursAgo = "小时前";
            string minuteAgo = "分钟前";
            string dayAgo = "天前";
            if (dateTime.HasValue)
            {
                if (dateTime.Value.AddMinutes(minuteDif) > DateTime.Now)//小于设置的时间则显示为刚刚
                {
                    return "刚刚";
                }else if (dateTime.Value.AddMinutes(60) > DateTime.Now)//一小时之内显示分钟
                {
                    int maxMinute = DateTime.Now.Minute;
                    int minMinute=dateTime.Value.Minute;
                    return GetPositiveNumber(minMinute,maxMinute,60)+minuteAgo;
                }else if((dateTime.Value.AddHours(24)>DateTime.Now))//一天之内显示几小时之前
                {
                    int maxHour = DateTime.Now.Hour;
                    int minHour = dateTime.Value.Hour;
                    return GetPositiveNumber(minHour, maxHour, 24) + hoursAgo;
                }else if (dateTime.Value.AddDays(maxDays) > DateTime.Now)//显示几天前
                {
                    int maxDay = DateTime.Now.Day;
                    int minDay = dateTime.Value.Day;
                    return GetPositiveNumber(minDay, maxDay, 30) + dayAgo;
                }
                else
                {
                    return dateTime.Value.Date.ToString("MM-dd");
                }
            }
            return null;
        }
        /// <summary>
        /// 主要计算时间差
        /// </summary>
        /// <param name="minValue">需要换算时间值</param>
        /// <param name="maxValue">当前时间值</param>
        /// <param name="reference">换算率</param>
        /// <returns></returns>
        public static int GetPositiveNumber(int minValue, int maxValue, int reference)
        {
            return maxValue - minValue > 0 ? maxValue - minValue : maxValue - minValue + reference;
        }
    }
}