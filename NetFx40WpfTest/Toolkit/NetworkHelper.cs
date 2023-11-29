using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using NLog;

namespace NetFx40WpfTest.Toolkit
{
    public static class NetworkHelper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        public extern static bool InternetGetConnectedState(out int conState, int reader);

        /// <summary>
        /// 检查网络是否可用
        /// </summary>
        /// <returns></returns>
        public static bool IsNetworkAvailable()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        /// <summary>
        /// 主动检查服务器地址是否可用
        /// </summary>
        /// <param name="hostNameOrAddress">服务器域名或IP</param>
        /// <returns></returns>
        public static bool IsPingOk(string hostNameOrAddress)
        {
            bool flag = false;
            try
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(hostNameOrAddress);
                if (null != pingReply && IPStatus.Success == pingReply.Status)
                {
                    flag = true;
                }
            }
            catch (Exception e)
            {
                Log.Trace(e);
            }

            return flag;
        }
    }
}