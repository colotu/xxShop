using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Common.Video
{
    /********************************************************************************

        ** 作者： Rock

        ** 创始时间：2012年4月11日 14:40:46

         ** 功能描述：用来接收视频上传、转换、截图、时间获取之后的值
     
        ** 修改人：

        ** 修改时间：

        **修改描述：

*********************************************************************************/
    public class VideoModel
    {
        private string _savePath;
        /// <summary>
        /// 视频保存路径
        /// </summary>
        public string SavePath
        {
            get { return _savePath; }
            set { _savePath = value; }
        }

        private string _imgPath;
        /// <summary>
        /// 截图保存地址
        /// </summary>
        public string ImgPath
        {
            get { return _imgPath; }
            set { _imgPath = value; }
        }

        private int _videoSpan;
        /// <summary>
        /// 视频长度
        /// </summary>
        public int VideoSpan
        {
            get { return _videoSpan; }
            set { _videoSpan = value; }
        }
    }
}
