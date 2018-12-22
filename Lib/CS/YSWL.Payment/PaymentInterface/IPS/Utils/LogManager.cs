using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// LogManager 的摘要说明
/// </summary>
public class LogManager
{

    private static string logPath = string.Empty;

    ///<summary>

    /// 保存日志的文件夹

    ///</summary>

    public static string LogPath
    {
        get
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            return logPath;
        }
        set { logPath = value; }
    }
    private static string logFielPrefix = string.Empty;
    ///<summary>
    /// 日志文件前缀
    ///</summary>
    public static string LogFielPrefix
    {
        get { return "IPS"; }
    }

    ///<summary>
    /// 写日志
    ///</summary>
    public static void WriteLog(string logFile, string msg)
    {
        try
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                LogPath + LogFielPrefix + logFile + " " +
                DateTime.Now.ToString("yyyyMMdd") + ".Log"
                );
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + msg);
            sw.Close();
        }
        catch
        { }
    }
    ///<summary>
    /// 写日志
    ///</summary>
    public static void WriteLog(LogFile logFile, string msg)
    {
        WriteLog(logFile.ToString(), msg);
    }
}

/// <summary>
/// 日志类型
/// </summary>
public enum LogFile
{
    Info,
    Warning,
    Error
}