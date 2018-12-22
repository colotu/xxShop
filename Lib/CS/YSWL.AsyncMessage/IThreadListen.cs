using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.AsyncMessage
{
    /// <summary>
    /// 线程监听接口
    /// </summary>
    public interface IThreadListen
    {
        /// <summary>
        /// 线程方法执行之前发生
        /// </summary>
        /// <param name="contextObj"></param>
        void BeforeExcuting(object contextObj);

        /// <summary>
        /// 线程方法执行完成发生
        /// </summary>
        /// <param name="contextObj"></param>
        void AfterExcuted(object contextObj);

        /// <summary>
        /// 异常时发生
        /// </summary>
        /// <param name="contextObj"></param>
        void ExceptionExcute(object contextObj,Exception e);
    }
}
