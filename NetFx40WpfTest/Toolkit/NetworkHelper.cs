using System.Runtime.InteropServices;

namespace NetFx40WpfTest.Toolkit
{
    public static class NetworkHelper
    {
        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        public extern static bool InternetGetConnectedState(out int conState, int reader);

        /// <summary>
        /// 检查网络是否可用
        /// </summary>
        /// <returns></returns>
        public static bool IsNetworkAvailable()
        {
            int description = 0;
            return InternetGetConnectedState(out description, 0);
        }

    }
}
