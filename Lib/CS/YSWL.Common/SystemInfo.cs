﻿/**
* SystemInfo.cs
*
* 功 能： [N/A]
* 类 名： SystemInfo
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/4/18 15:12:23  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace YSWL.Common
{
    #region Assistance Class
    public class SystemInfoImport
    {
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern void GetSystemInfo(ref CpuInfo cpuinfo);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern void GlobalMemoryStatus(ref MemoryInfo meminfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct CpuInfo
        {
            public uint dwOemId;
            public uint dwPageSize;
            public uint lpMinimumApplicationAddress;
            public uint lpMaximumApplicationAddress;
            public uint dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public uint dwProcessorLevel;
            public uint dwProcessorRevision;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MemoryInfo
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }

        public static MemoryInfo Memory
        {
            get
            {
                MemoryInfo obj = new MemoryInfo();
                GlobalMemoryStatus(ref obj);
                return obj;
            }
        }

        public static CpuInfo Cpu
        {
            get
            {
                CpuInfo obj = new CpuInfo();
                GetSystemInfo(ref obj);
                return obj;
            }
        }
    }
    #endregion

    public class SystemInfo
    {
        #region 外部调用

        #region 系统版本

        /// <summary>
        /// 系统版本
        /// </summary>
        public static string OperatingSystemOSVersion
        {
            get
            {
                return Environment.OSVersion.ToString();
            }
        }
        /// <summary>
        /// 系统版本
        /// </summary>
        public static string OperatingSystemSimple
        {
            get
            {
                //	Platform
                OperatingSystem os = Environment.OSVersion;
                string text = string.Empty;
                switch (os.Platform)
                {
                    case PlatformID.Win32Windows:
                        switch (os.Version.Minor)
                        {
                            case 0:
                                text = "Microsoft Windows 95";
                                break;
                            case 10:
                                text = "Microsoft Windows 98";
                                break;
                            case 90:
                                text = "Microsoft Windows Millennium Edition";
                                break;
                            default:
                                text = "Microsoft Windows 95 or later";
                                break;
                        }
                        break;
                    case PlatformID.Win32NT:
                        switch (os.Version.Major)
                        {
                            case 3:
                                text = "Microsoft Windows NT 3.51";
                                break;
                            case 4:
                                text = "Microsoft Windows NT 4.0";
                                break;
                            case 5:
                                switch (os.Version.Minor)
                                {
                                    case 0:
                                        text = "Microsoft Windows 2000";
                                        break;
                                    case 1:
                                        text = "Microsoft Windows XP";
                                        break;
                                    case 2:
                                        text = "Microsoft Windows 2003";
                                        break;
                                    default:
                                        text = "Microsoft NT 5.x";
                                        break;
                                }
                                break;
                            case 6:
                                switch (os.Version.Minor)
                                {
                                    case 0:
                                        text = "Microsoft Windows Vista or Server 2008";
                                        break;
                                    case 1:
                                        text = "Microsoft Windows 7 or Server 2008 R2";
                                        break;
                                    case 2:
                                        text = "Microsoft Windows 8 or Server 2012";
                                        break;
                                    default:
                                        text = "Microsoft NT 6.x";
                                        break;
                                }
                                break;
                            case 10:
                                text = "Microsoft Windows 10.0";
                                break;
                        }
                        break;
                    default:
                        if ((int)os.Platform > 3)
                        {
                            string name = "/proc/version";
                            if (File.Exists(name))
                            {
                                using (StreamReader reader = new StreamReader(name))
                                {
                                    text = reader.ReadToEnd().Trim();
                                }
                            }
                        }
                        break;
                }
                return text;
            }
        }
        /// <summary>
        /// 系统版本
        /// </summary>
        public static string OperatingSystemFull
        {
            get
            {
                return string.Format("{0} -- {1}", OperatingSystemOSVersion, OperatingSystemSimple);
            }
        }
        #endregion

        /// <summary>
        /// 服务器的计算机名称
        /// </summary>
        public static string ServerName
        {
            get { return HttpContext.Current.Server.MachineName; }
        }
        /// <summary>
        /// Web服务器IP
        /// </summary>
        public static string ServerIP
        {
            get { return HttpContext.Current.Request.ServerVariables["LOCAl_ADDR"]; }
        }
        /// <summary>
        /// Web服务器域名
        /// </summary>
        public static string ServerDomain
        {
            get { return HttpContext.Current.Request.ServerVariables["Server_Name"]; }
        }
        /// <summary>
        /// Web服务器端口
        /// </summary>
        public static string ServerPort
        {
            get { return HttpContext.Current.Request.ServerVariables["Server_Port"]; }
        }
        /// <summary>
        /// Web服务器IIS版本
        /// </summary>
        public static string WebServerVersion
        {
            get { return HttpContext.Current.Request.ServerVariables["Server_SoftWare"]; }
        }
        /// <summary>
        /// .NET Framework 版本
        /// </summary>
        public static Version DotNetVersion
        {
            get { return Environment.Version; }
        }
        /// <summary>
        /// .NET Framework 区域
        /// </summary>
        public static string DotNetLanguage
        {
            get { return System.Globalization.CultureInfo.InstalledUICulture.EnglishName; }
        }
        /// <summary>
        /// 系统启动时间(已开机xxx毫秒)
        /// </summary>
        public static TimeSpan SystemUptime
        {
            get { return TimeSpan.FromMilliseconds(Environment.TickCount); }
        }
        /// <summary>
        /// 执行超时时间
        /// </summary>
        public static TimeSpan ScriptTimeout
        {
            get { return TimeSpan.FromSeconds(HttpContext.Current.Server.ScriptTimeout); }
        }
        #endregion

        #region Get Information Function

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    LoadInformation(GetSystemInfo());
        //    LoadInformation(GetSystemProcessorInfo());
        //    LoadInformation(GetSystemMemoryInfo());
        //    LoadInformation(GetSystemStorageInfo());
        //    LoadInformation(GetRequestHeaderInfo());
        //    LoadInformation(GetServerVariables());
        //    LoadInformation(GetEnvironmentVariables());
        //    LoadInformation(GetSessionInfo());
        //    LoadInformation(GetSystemObjectInfo());
        //    LoadInformation(GetMailObjectInfo());
        //    LoadInformation(GetUploadObjectInfo());
        //    LoadInformation(GetGraphicsObjectInfo());
        //    LoadInformation(GetOtherObjectInfo());
        //}
        private DataTable GetSystemInfo()
        {
            DataTable table = GenerateDataTable("System Information");
            //	Server Name
            Assign(table, "Server Name", HttpContext.Current.Server.MachineName);
            Assign(table, "Server IP", HttpContext.Current.Request.ServerVariables["LOCAl_ADDR"]);
            Assign(table, "Server Domain", HttpContext.Current.Request.ServerVariables["Server_Name"]);
            Assign(table, "Server Port", HttpContext.Current.Request.ServerVariables["Server_Port"]);
            //	Web Server
            Assign(table, "Web Server Version", HttpContext.Current.Request.ServerVariables["Server_SoftWare"]);
            //	Path
            Assign(table, "Virtual Request Path", HttpContext.Current.Request.FilePath);
            Assign(table, "Physical Request Path", HttpContext.Current.Request.PhysicalPath);
            Assign(table, "Virtual Application Root Path", HttpContext.Current.Request.ApplicationPath);
            Assign(table, "Physical Application Root Path", HttpContext.Current.Request.PhysicalApplicationPath);

            Assign(table, "Operating System", OperatingSystemOSVersion);
            Assign(table, "Operating System Installation Directory", Environment.SystemDirectory);
            Assign(table, ".Net Version", DotNetVersion.ToString());
            Assign(table, ".Net Language", DotNetLanguage);
            Assign(table, "Server Current Time", DateTime.Now.ToString());
            Assign(table, "System Uptime", SystemUptime.ToString());
            Assign(table, "Script Timeout", ScriptTimeout.ToString());
            return table;
        }

        private void GetSystemStorageInfo_DriveInfo(DataTable table)
        {
            try
            {
                Type typeDriveInfo = Type.GetType("System.IO.DriveInfo");
                MethodInfo get_drives = typeDriveInfo.GetMethod("GetDrives");
                object result = get_drives.Invoke(null, null);

                foreach (object o in (IEnumerable)result)
                {
                    try
                    {
                        //  Use reflection to call DriveInfo.GetProperties() to make 1.x compiler don't complain.
                        PropertyInfo[] props = typeDriveInfo.GetProperties();
                        bool is_ready = (bool)typeDriveInfo.GetProperty("IsReady").GetValue(o, null);
                        string name = string.Empty;
                        string volume_label = string.Empty;
                        string drive_format = string.Empty;
                        string drive_type = string.Empty;
                        ulong total_free_space = 0;
                        ulong total_space = 0;
                        foreach (PropertyInfo prop in props)
                        {
                            switch (prop.Name)
                            {
                                case "Name":
                                    name = (string)prop.GetValue(o, null);
                                    break;
                                case "VolumeLabel":
                                    if (is_ready)
                                        volume_label = (string)prop.GetValue(o, null);
                                    break;
                                case "DriveFormat":
                                    if (is_ready)
                                        drive_format = (string)prop.GetValue(o, null);
                                    break;
                                case "DriveType":
                                    drive_type = prop.GetValue(o, null).ToString();
                                    break;
                                case "TotalFreeSpace":
                                    if (is_ready)
                                        total_free_space = (ulong)(long)prop.GetValue(o, null);
                                    break;
                                case "TotalSize":
                                    if (is_ready)
                                        total_space = (ulong)(long)prop.GetValue(o, null);
                                    break;
                            }
                        }

                        string label = string.Empty;
                        string size = string.Empty;

                        if (is_ready)
                        {
                            label = string.Format("{0} - <{1}> [{2}] - {3,-10}", name, volume_label, drive_format, drive_type);
                            if (total_space > 0 && total_space != ulong.MaxValue && total_space != int.MaxValue)
                            {
                                size = string.Format("Free {0} / Total {1}", FormatNumber(total_free_space), FormatNumber(total_space));
                            }
                        }
                        else
                        {
                            label = string.Format("{0} {1,-10}", name, drive_type);
                        }

                        Assign(table, label, size);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
        }
        private void GetSystemStorageInfo_WMI(DataTable table)
        {
            try
            {
                //  Use reflection to call WMI to make Mono compiler don't complain about assembly reference
                Assembly dSystemManangement = Assembly.Load("System.Management, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A");
                if (dSystemManangement == null) return;

                Type tManagementObjectSearcher = dSystemManangement.GetType("System.Management.ManagementObjectSearcher");
                if (dSystemManangement == null) return;

                MethodInfo mGet = tManagementObjectSearcher.GetMethod("Get", new Type[] { });

                ConstructorInfo ctor = tManagementObjectSearcher.GetConstructor(new Type[] { typeof(string) });

                object searcher = ctor.Invoke(new object[] { "Select * From Win32_LogicalDisk" });
                if (dSystemManangement == null) return;

                object disks = mGet.Invoke(searcher, null);

                //  ManagementObject
                Type tManagementObject = dSystemManangement.GetType("System.Management.ManagementObject");

                foreach (object disk in (IEnumerable)disks)
                {
                    try
                    {
                        PropertyInfo prop = tManagementObject.GetProperty("Item", new Type[] { typeof(string) });
                        uint i_drive_type = (uint)prop.GetValue(disk, new object[] { "DriveType" });
                        string drive_type = string.Empty;
                        switch (i_drive_type)
                        {
                            case 1:
                                drive_type = "No Root Directory";
                                break;
                            case 2:
                                drive_type = "Removable Disk";
                                break;
                            case 3:
                                drive_type = "Local Disk";
                                break;
                            case 4:
                                drive_type = "Network Drive";
                                break;
                            case 5:
                                drive_type = "Compact Disc";
                                break;
                            case 6:
                                drive_type = "RAM Disk";
                                break;
                            default:
                                drive_type = "Unknown";
                                break;
                        }
                        string name = prop.GetValue(disk, new object[] { "Name" }) as string;
                        string volume_label = prop.GetValue(disk, new object[] { "VolumeName" }) as string;
                        string filesystem = prop.GetValue(disk, new object[] { "FileSystem" }) as string;

                        string free_space = string.Empty;
                        try { free_space = FormatNumber((ulong)prop.GetValue(disk, new object[] { "FreeSpace" })); }
                        catch (Exception) { }

                        string total_space = string.Empty;
                        try { total_space = FormatNumber((ulong)prop.GetValue(disk, new object[] { "Size" })); }
                        catch (Exception) { }

                        string left = string.Format("{0} - <{1}> [{2}] - {3,-10}",
                                name,
                                volume_label,
                                filesystem,
                                drive_type
                            );

                        string right = ((free_space == null || free_space == "") ? string.Empty : string.Format("Free {0} / Total {1}", free_space, total_space));

                        Assign(table, left, right);
                    }
                    catch (Exception exception) { Assign(table, "Exception Occurs", exception.ToString()); }
                }
            }
            catch (Exception exception) { Assign(table, "Exception Occurs", exception.ToString()); }
        }
        private DataTable GetSystemStorageInfo()
        {
            DataTable table = GenerateDataTable("Storage Information");

            try { Assign(table, "Logical Driver Information", string.Join(", ", Directory.GetLogicalDrives())); }
            catch (Exception) { }

            if (Environment.Version.Major >= 2)
            {
                GetSystemStorageInfo_DriveInfo(table);
            }
            else
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    GetSystemStorageInfo_WMI(table);
                }
            }

            return table;
        }
        private void GetSystemMemoryInfo_proc(DataTable table)
        {
            string name = "/proc/meminfo";
            if (File.Exists(name))
            {
                using (StreamReader reader = new StreamReader(name, Encoding.ASCII))
                {
                    Hashtable ht = new Hashtable();
                    string line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] item = line.Split(":".ToCharArray());
                        if (item.Length == 2)
                        {
                            string k = item[0].Trim();
                            string v = item[1].Trim();
                            ht.Add(k, v);
                        }
                    }
                    Assign(table, "Physical Memory Size", string.Format("{0}", ht["MemTotal"]));
                    Assign(table, "Physical Free Memory Size", string.Format("{0}", ht["MemFree"]));
                    Assign(table, "Swap Total Size", string.Format("{0}", ht["SwapTotal"]));
                    Assign(table, "Swap Free Size", string.Format("{0}", ht["SwapFree"]));
                }
            }
        }
        private DataTable GetSystemMemoryInfo()
        {
            DataTable table = GenerateDataTable("Memory Information"); ;
            Assign(table, "Current Working Set", FormatNumber((ulong)Environment.WorkingSet));
            try
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SystemInfoImport.MemoryInfo memory = SystemInfoImport.Memory;
                    Assign(table, "Physical Memory Size", FormatNumber(memory.dwTotalPhys));
                    Assign(table, "Physical Free Memory Size", FormatNumber(memory.dwAvailPhys));
                    Assign(table, "PageFile Size", FormatNumber(memory.dwTotalPageFile));
                    Assign(table, "Available PageFile Size", FormatNumber(memory.dwAvailPageFile));
                    Assign(table, "Virtual Memory Size", FormatNumber(memory.dwTotalVirtual));
                    Assign(table, "Available Memory Size", FormatNumber(memory.dwAvailVirtual));
                    Assign(table, "Memory Load", string.Format("{0} %", memory.dwMemoryLoad.ToString("N")));
                }
                else if ((int)Environment.OSVersion.Platform > 3)
                {
                    GetSystemMemoryInfo_proc(table);
                }
            }
            catch (Exception) { }
            return table;
        }
        private void GetSystemProcessorInfo_WMI(DataTable table)
        {
            //  Use reflection to call WMI to make Mono compiler don't complain about assembly reference
            Assembly dSystemManangement = Assembly.Load("System.Management, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A");
            if (dSystemManangement == null) return;

            Type tManagementObjectSearcher = dSystemManangement.GetType("System.Management.ManagementObjectSearcher");
            if (dSystemManangement == null) return;

            MethodInfo mGet = tManagementObjectSearcher.GetMethod("Get", new Type[] { });

            ConstructorInfo ctor = tManagementObjectSearcher.GetConstructor(new Type[] { typeof(string) });

            object searcher = ctor.Invoke(new object[] { "Select * From Win32_Processor" });
            if (dSystemManangement == null) return;

            object processors = mGet.Invoke(searcher, null);

            //  ManagementObject
            Type tManagementObject = dSystemManangement.GetType("System.Management.ManagementObject");
            foreach (object processor in (IEnumerable)processors)
            {
                try
                {
                    try
                    {
                        PropertyInfo prop = tManagementObject.GetProperty("Item", new Type[] { typeof(string) });
                        StringBuilder sb = new StringBuilder();
                        //  Unique ID
                        string name = (string)prop.GetValue(processor, new object[] { "Name" });
                        sb.Append(name);
                        //  Clock Speed
                        uint clock_speed = (uint)prop.GetValue(processor, new object[] { "CurrentClockSpeed" });
                        //  Max Clock Speed
                        uint max_clock_speed = (uint)prop.GetValue(processor, new object[] { "MaxClockSpeed" });
                        sb.AppendFormat(" - {0} MHz / {1} MHz", clock_speed, max_clock_speed);
                        //  Current Voltage
                        ushort i_current_voltage = (ushort)prop.GetValue(processor, new object[] { "CurrentVoltage" });
                        double current_voltage = 0;
                        if (((uint)i_current_voltage & 0x80) == 0)
                        {
                            current_voltage = (double)(i_current_voltage & 0x7F) / 10.0;
                        }
                        else
                        {
                            try
                            {
                                uint caps = (uint)prop.GetValue(processor, new object[] { "VoltageCaps" });
                                switch (caps & 0xF)
                                {
                                    case 1:
                                        current_voltage = 5;
                                        break;
                                    case 2:
                                        current_voltage = 3.3;
                                        break;
                                    case 3:
                                        current_voltage = 2.9;
                                        break;
                                }
                            }
                            catch (Exception) { }
                        }
                        if (current_voltage > 0)
                        {
                            sb.AppendFormat(" - {0}v", current_voltage);
                        }
                        //  Load Percentage 
                        ushort load_percentage = (ushort)prop.GetValue(processor, new object[] { "LoadPercentage" });
                        sb.AppendFormat(" - Load = {0} %", load_percentage);
                        Assign(table, "Processor", sb.ToString());
                    }
                    catch (Exception exception) { Assign(table, "Exception Occurs", exception.ToString()); }
                }
                catch (Exception) { }
            }
        }
        private void GetSystemProcessorInfo_proc(DataTable table)
        {
            string name = "/proc/cpuinfo";
            if (File.Exists(name))
            {
                using (StreamReader reader = new StreamReader(name, Encoding.ASCII))
                {
                    ArrayList processors = new ArrayList();
                    Hashtable ht = new Hashtable();
                    string line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim().Length == 0)
                        {
                            processors.Add(ht);
                            ht = new Hashtable();
                        }
                        string[] item = line.Split(":".ToCharArray());
                        if (item.Length == 2)
                        {
                            string k = item[0].Trim();
                            string v = item[1].Trim();
                            ht.Add(k, v);
                        }
                    }

                    foreach (Hashtable processor in processors)
                    {
                        string n = string.Format("Processor {0}", processor["processor"]);
                        string v = string.Format("{0}{1}", processor["model name"],
                                                 (processor["cpu MHz"] != null) ? string.Format(" - {0} MHz", processor["cpu MHz"]) : string.Empty);
                        Assign(table, n, v);
                    }
                }
            }
        }
        private DataTable GetSystemProcessorInfo()
        {
            DataTable table = GenerateDataTable("Processor Information");
            try
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    Assign(table, "Number of Processors", Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"));
                    Assign(table, "Processor Id", Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER"));
                    SystemInfoImport.CpuInfo cpu = SystemInfoImport.Cpu;
                    Assign(table, "Processor Type", cpu.dwProcessorType.ToString());
                    Assign(table, "Processor Level", cpu.dwProcessorLevel.ToString());
                    Assign(table, "Processor OEM Id", cpu.dwOemId.ToString());
                    Assign(table, "Page Size", cpu.dwPageSize.ToString());
                    GetSystemProcessorInfo_WMI(table);
                }
                else if ((int)Environment.OSVersion.Platform > 3)
                {
                    GetSystemProcessorInfo_proc(table);
                }
            }
            catch (Exception) { }
            return table;
        }

        private DataTable GetServerVariables()
        {
            DataTable table = GenerateDataTable("Server Variables");
            foreach (string key in HttpContext.Current.Request.ServerVariables.AllKeys)
            {
                Assign(table, key, HttpContext.Current.Request.ServerVariables[key]);
            }
            return table;
        }
        private DataTable GetEnvironmentVariables()
        {
            DataTable table = GenerateDataTable("Environment Variables");
            foreach (DictionaryEntry de in System.Environment.GetEnvironmentVariables())
            {
                Assign(table, de.Key.ToString(), de.Value.ToString());
            }
            return table;
        }

        private DataTable GetSystemObjectInfo()
        {
            DataTable table = GenerateDataTable("System COM Component Information");
            Assign(table, "Adodb.Connection", TestObject("Adodb.Connection").ToString());
            Assign(table, "Adodb.RecordSet", TestObject("Adodb.RecordSet").ToString());
            Assign(table, "Adodb.Stream", TestObject("Adodb.Stream").ToString());
            Assign(table, "Scripting.FileSystemObject", TestObject("Scripting.FileSystemObject").ToString());
            Assign(table, "Microsoft.XMLHTTP", TestObject("Microsoft.XMLHTTP").ToString());
            Assign(table, "WScript.Shell", TestObject("WScript.Shell").ToString());
            Assign(table, "MSWC.AdRotator", TestObject("MSWC.AdRotator").ToString());
            Assign(table, "MSWC.BrowserType", TestObject("MSWC.BrowserType").ToString());
            Assign(table, "MSWC.NextLink", TestObject("MSWC.NextLink").ToString());
            Assign(table, "MSWC.Tools", TestObject("MSWC.Tools").ToString());
            Assign(table, "MSWC.Status", TestObject("MSWC.Status").ToString());
            Assign(table, "MSWC.Counters", TestObject("MSWC.Counters").ToString());
            Assign(table, "IISSample.ContentRotator", TestObject("IISSample.ContentRotator").ToString());
            Assign(table, "IISSample.PageCounter", TestObject("IISSample.PageCounter").ToString());
            Assign(table, "MSWC.PermissionChecker", TestObject("MSWC.PermissionChecker").ToString());
            return table;
        }
        private DataTable GetMailObjectInfo()
        {
            DataTable table = GenerateDataTable("Mail COM Component Information");
            Assign(table, "JMail.SMTPMail", TestObject("JMail.SMTPMail").ToString());
            Assign(table, "JMail.Message", TestObject("JMail.Message").ToString());
            Assign(table, "CDONTS.NewMail", TestObject("CDONTS.NewMail").ToString());
            Assign(table, "CDO.Message", TestObject("CDO.Message").ToString());
            Assign(table, "Persits.MailSender", TestObject("Persits.MailSender").ToString());
            Assign(table, "SMTPsvg.Mailer", TestObject("SMTPsvg.Mailer").ToString());
            Assign(table, "DkQmail.Qmail", TestObject("DkQmail.Qmail").ToString());
            Assign(table, "SmtpMail.SmtpMail.1", TestObject("SmtpMail.SmtpMail.1").ToString());
            Assign(table, "Geocel.Mailer.1", TestObject("Geocel.Mailer.1").ToString());
            return table;
        }
        private DataTable GetUploadObjectInfo()
        {
            DataTable table = GenerateDataTable("Upload COM Component Information");
            Assign(table, "LyfUpload.UploadFile", TestObject("LyfUpload.UploadFile").ToString());
            Assign(table, "Persits.Upload", TestObject("Persits.Upload").ToString());
            Assign(table, "Ironsoft.UpLoad", TestObject("Ironsoft.UpLoad").ToString());
            Assign(table, "aspcn.Upload", TestObject("aspcn.Upload").ToString());
            Assign(table, "SoftArtisans.FileUp", TestObject("SoftArtisans.FileUp").ToString());
            Assign(table, "SoftArtisans.FileManager", TestObject("SoftArtisans.FileManager").ToString());
            Assign(table, "Dundas.Upload", TestObject("Dundas.Upload").ToString());
            Assign(table, "w3.upload", TestObject("w3.upload").ToString());
            return table;
        }
        private DataTable GetGraphicsObjectInfo()
        {
            DataTable table = GenerateDataTable("Graphics COM Component Information");
            Assign(table, "SoftArtisans.ImageGen", TestObject("SoftArtisans.ImageGen").ToString());
            Assign(table, "W3Image.Image", TestObject("W3Image.Image").ToString());
            Assign(table, "Persits.Jpeg", TestObject("Persits.Jpeg").ToString());
            Assign(table, "XY.Graphics", TestObject("XY.Graphics").ToString());
            Assign(table, "Ironsoft.DrawPic", TestObject("Ironsoft.DrawPic").ToString());
            Assign(table, "Ironsoft.FlashCapture", TestObject("Ironsoft.FlashCapture").ToString());
            return table;
        }
        private DataTable GetOtherObjectInfo()
        {
            DataTable table = GenerateDataTable("Other COM Component Information");
            Assign(table, "dyy.zipsvr", TestObject("dyy.zipsvr").ToString());
            Assign(table, "hin2.com_iis", TestObject("hin2.com_iis").ToString());
            Assign(table, "Socket.TCP", TestObject("Socket.TCP").ToString());
            return table;
        }

        private DataTable GetSessionInfo()
        {
            DataTable table = GenerateDataTable("Session Information");
            Assign(table, "Session Count", HttpContext.Current.Session.Contents.Count.ToString());
            Assign(table, "Application Count", HttpContext.Current.Application.Contents.Count.ToString());
            return table;
        }
        private DataTable GetRequestHeaderInfo()
        {
            DataTable table = GenerateDataTable("Request Headers");
            foreach (string key in HttpContext.Current.Request.Headers.AllKeys)
            {
                Assign(table, key, HttpContext.Current.Request.Headers[key]);
            }
            return table;
        }

        #endregion

        #region Helper Methods

        private string FormatNumber(ulong value)
        {
            if (value < 4 * 1024)
            {
                return string.Format("{0} Bytes", value);
            }
            else if (value < (long)4 * 1024 * 1024)
            {
                return string.Format("{0} KB", (value / (double)((long)1024)).ToString("N"));
            }
            else if (value < (long)4 * 1024 * 1024 * 1024)
            {
                return string.Format("{0} MB", (value / (double)((long)1024 * 1024)).ToString("N"));
            }
            else if (value < (long)4 * 1024 * 1024 * 1024 * 1024)
            {
                return string.Format("{0} GB", (value / (double)((long)1024 * 1024 * 1024)).ToString("N"));
            }
            else
            {
                return string.Format("{0} TB", (value / (double)((long)1024 * 1024 * 1024 * 1024)).ToString("N"));
            }
        }

        private DataTable GenerateDataTable(string name)
        {
            DataTable table = new DataTable(name);
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Value", typeof(string));
            return table;
        }

        private bool TestObject(string progID)
        {
            try
            {
                HttpContext.Current.Server.CreateObject(progID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void Assign(DataTable table, string name, string value)
        {
            DataRow row = table.NewRow();
            row["Name"] = name;
            row["Value"] = value;
            table.Rows.Add(row);
        }

        //private void LoadInformation(DataTable table)
        //{
        //    DataGrid grid = new DataGrid();
        //    BoundColumn col;

        //    col = new BoundColumn();
        //    col.DataField = "Name";
        //    col.HeaderText = "Name";
        //    col.ItemStyle.CssClass = "name";
        //    grid.Columns.Add(col);

        //    col = new BoundColumn();
        //    col.DataField = "Value";
        //    col.HeaderText = "Value";
        //    col.ItemStyle.CssClass = "value";
        //    grid.Columns.Add(col);

        //    grid.AutoGenerateColumns = false;
        //    grid.HeaderStyle.CssClass = "header";
        //    grid.DataSource = new DataView(table);
        //    grid.DataBind();


        //    foreach (DataGridItem item in grid.Items)
        //    {
        //        if (item.Cells.Count == 2)
        //        {
        //            TableCell cell = item.Cells[1];
        //            //  change true/false style
        //            switch (cell.Text.ToLower())
        //            {
        //                case "true":
        //                    cell.CssClass = "value_true";
        //                    break;
        //                case "false":
        //                    cell.CssClass = "value_false";
        //                    break;
        //            }
        //            //  wrap <pre> for text contain newline.
        //            if (cell.Text.IndexOf(Environment.NewLine) >= 0)
        //            {
        //                cell.Text = string.Format("<pre>{0}</pre>", cell.Text);
        //            }
        //        }
        //    }


        //    HtmlGenericControl title = new HtmlGenericControl("h1");
        //    title.InnerText = HttpContext.Current.Server.HtmlEncode(table.TableName);
        //    title.Attributes.Add("class", "title");

        //    HtmlGenericControl div = new HtmlGenericControl("div");
        //    div.Attributes.Add("class", "section");
        //    div.Controls.Add(new HtmlGenericControl("p"));
        //    div.Controls.Add(title);
        //    div.Controls.Add(grid);
        //    div.Controls.Add(new HtmlGenericControl("p"));

        //    divCenter.Controls.Add(div);
        //}

        #endregion

        #region 硬件信息
        ///// <summary>
        ///// 获取CPU序列号
        ///// </summary>
        ///// <returns></returns>
        //private static String GetCpuId()
        //{
        //    var myCpu = new ManagementClass("win32_Processor").GetInstances();
        //    var serial = "";
        //    foreach (ManagementObject cpu in myCpu)
        //    {
        //        var val = cpu.Properties["Processorid"].Value;
        //        serial += val == null ? "" : val.ToString();
        //    }
        //    return serial;
        //}

        ///// <summary>
        ///// 获取主板序列号
        ///// </summary>
        ///// <returns></returns>
        //private static String GetMbId()
        //{
        //    var myMb = new ManagementClass("Win32_BaseBoard").GetInstances();
        //    var serial = "";
        //    foreach (ManagementObject mb in myMb)
        //    {
        //        var val = mb.Properties["SerialNumber"].Value;
        //        serial += val == null ? "" : val.ToString();
        //    }
        //    return serial;
        //}

        ///// <summary>
        ///// 获取硬盘序列号
        ///// </summary>
        ///// <returns></returns>
        //private static String GetHdId()
        //{
        //    var lpm = new ManagementClass("Win32_PhysicalMedia").GetInstances();
        //    var serial = "";
        //    foreach (ManagementObject hd in lpm)
        //    {
        //        var val = hd.Properties["SerialNumber"].Value;
        //        serial += val == null ? "" : val.ToString().Trim();
        //    }
        //    return serial;
        //}
        #endregion
    }


}
