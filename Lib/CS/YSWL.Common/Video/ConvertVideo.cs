/********************************************************************************

        ** 作者： Rock

        ** 创始时间：2012年4月11日 15:59:46

         ** 功能描述：视频上传。可选功能：视频格式转换、视频截图、视频时间长度的获取
     
        ** 修改人：

        ** 修改时间：

        **修改描述：

*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Diagnostics;
using System.IO;
using VideoEncoder;
using System.Configuration;

namespace YSWL.Common.Video
{
    public class ConvertVideo
    {
        string ffmpegTools = ConfigHelper.GetConfigString("ffmpeg");
        protected static readonly string[] AllowFileExt = ".rar|.zip|.doc|.docx|.xls|.swf|.xlsx|.jpg|.jpeg|.gif|.png|.bmp|.flv".Split('|');
        /// <summary>
        /// 获取文件的名字
        /// </summary>
        public static string GetFileName(string fileName)
        {
            int i = fileName.LastIndexOf("\\") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        public static string GetExtension(string fileName)
        {
            int i = fileName.LastIndexOf(".") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }

        private string _errorMessage;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errorMessage
        {
            get { return _errorMessage; }
        }
        
        /// <summary>
        /// 上传视频
        /// </summary>
        /// <param name="postFile"></param>
        /// <param name="isConvert">是否进行格式转换</param>
        /// <param name="savePath">视频保存的路径</param>
        /// <param name="configSize">上传视频限制的大小</param>
        /// <param name="isGetImg">是否获取视频缩略图</param>
        /// <param name="isGetSpan">是否获取视频的时间</param>
        /// <param name="model">返回处理结果</param>
        /// <param name="extend">转换为哪种格式的视频 eg：".flv"</param>
        /// <returns></returns>
        public bool UploadVideo(HttpPostedFile postFile, bool isConvert, string savePath, int? configSize, bool isGetImg, bool isGetSpan, out VideoModel model,string extend)
        {
            model = new VideoModel();
            if (postFile != null)
            {
                if (configSize.HasValue)
                {
                    if (postFile.ContentLength > configSize.Value)
                    {
                        _errorMessage = "上传文件过大";
                        return false;
                    }
                }
                #warning 上传目录待统一
                string ext = Path.GetExtension( postFile.FileName);
                ext = String.IsNullOrWhiteSpace(ext) ? "" : ext.ToLower();
                if (!AllowFileExt.Contains(ext) || String.IsNullOrWhiteSpace(ext))
                {
                    _errorMessage = "上传文件格式不正确！";
                    return false;
                } 
                string fileName = Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) + ext;
                string pathStr = HttpContext.Current.Server.MapPath("/" + savePath);
                if (!System.IO.Directory.Exists(pathStr))
                {
                    System.IO.Directory.CreateDirectory(pathStr);
                }
                string path = savePath + fileName;
                postFile.SaveAs(HttpContext.Current.Server.MapPath(path));
                model.SavePath = path;
                if (isConvert)
                {
                    string convertPath = Path.ChangeExtension(path, extend);
                    string destFileName = Path.Combine(pathStr, Path.ChangeExtension(postFile.FileName, extend));
                    ConvertFlv(HttpContext.Current.Server.MapPath(path), destFileName);
                    model.SavePath = convertPath;
                }
                if (isGetImg)
                {
                    string imgpath = Path.ChangeExtension(path, ".jpg");
                    string jpgFileName = Path.Combine(HttpContext.Current.Server.MapPath(path), Path.ChangeExtension(path, ".jpg"));
                    CreateThumb(HttpContext.Current.Server.MapPath(path), HttpContext.Current.Server.MapPath(jpgFileName));
                    model.ImgPath = imgpath;
                }
                if (isGetSpan)
                {
                    TimeSpan totalTime = GetVideoTotalTime(HttpContext.Current.Server.MapPath(path));
                    int ts = TimeParser.TimeToSecond(totalTime.Hours, totalTime.Minutes, totalTime.Seconds);
                    model.VideoSpan = ts;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 视频格式转换
        /// </summary>
        private void ConvertFlv(string srcFileName, string destFileName)
        {
            //创建并启动一个新进程
            Process p = new Process();
            //设置进程启动信息属性StartInfo，这是ProcessStartInfo类，包括了一些属性和方法：
            p.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + ffmpegTools;           //程序名
            p.StartInfo.UseShellExecute = false;
            //-y选项的意思是当输出文件存在的时候自动覆盖输出文件，不提示“y/n”这样才能自动化
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            //p.StartInfo.Arguments = "-i " + srcFileName + " -y -ab 56 -ar 22050 -b 1500  -r 29.97 -s 600x520 " + destFileName;    //执行参数
            //高品质：ffmpeg -i infile -ab 128 -acodec libmp3lame -ac 1 -ar 22050 -r 29.97 -qscale 6 -y outfile
            p.StartInfo.Arguments = "-i " + srcFileName + " -ab 128 -acodec libmp3lame -ac 1 -ar 22050 -r 29.97 -qscale 6 -y " + destFileName;    //执行参数

            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中
            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            p.Start();
            p.BeginErrorReadLine();//开始异步读取
            p.WaitForExit();//阻塞等待进程结束
            p.Close();//关闭进程
            p.Dispose();//释放资源
        }

        /// <summary>
        /// 视频截图
        /// </summary>
        private void CreateThumb(string srcFileName, string jpgFileName)
        {
            try
            {
                //创建并启动一个新进程
                Process p = new Process();
                //设置进程启动信息属性StartInfo，这是ProcessStartInfo类，包括了一些属性和方法：
                p.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + ffmpegTools;           //程序名
                p.StartInfo.UseShellExecute = false;
                //-y选项的意思是当输出文件存在的时候自动覆盖输出文件，不提示“y/n”这样才能自动化

                p.StartInfo.Arguments = "-i " + srcFileName + " -y -f mjpeg  -ss 2 -t 3 -s 598x520 " + jpgFileName;    //执行参数 截取第3秒图片
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中
                p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
                p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                p.Start();
                p.BeginErrorReadLine();//开始异步读取
                p.WaitForExit();//阻塞等待进程结束
                p.Close();//关闭进程
                p.Dispose();//释放资源
            }
            catch (Exception)
            {
                _errorMessage = "图片截取失败！";
            }
        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e) { }

        void p_ErrorDataReceived(object sender, DataReceivedEventArgs e) { }

        /// <summary>
        /// 获得视频总时长
        /// </summary>
        public TimeSpan GetVideoTotalTime(string videoPath)
        {
            VideoEncoder.Encoder enc = new VideoEncoder.Encoder();
            //ffmpeg.exe的路径，程序会在执行目录下找此文件，
            enc.FFmpegPath = AppDomain.CurrentDomain.BaseDirectory + ffmpegTools;
            //视频路径
            string videoFilePath = videoPath;
            VideoFile videoFile = new VideoFile(videoFilePath);

            enc.GetVideoInfo(videoFile);

            TimeSpan totaotp = videoFile.Duration;
            return totaotp;
        }
    }
}
