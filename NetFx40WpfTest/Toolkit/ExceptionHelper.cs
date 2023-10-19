using System;
using System.Net.Http;
using NLog;

namespace NetFx40WpfTest.Toolkit
{
    /// <summary>
    /// 异常处理 工具类
    /// </summary>
    public static class ExceptionHelper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="e">异常对象</param>
        public static bool Process(Exception e)
        {
            if (null != e)
            {
                if (e is HttpRequestException)
                {
                    if (!NetworkHelper.IsNetworkAvailable())
                    {
                        // 网络环境异常
                        Log.Error("检测到计算机联网异常，请稍后重试");
                    }
                }
                else
                {
                    // 其他异常
                    Log.Error(e.ToString());
                    //MessageBox.Show(e.ToString(), "Unknown Exception");
                }
            }

            return true;
        }
    }
}