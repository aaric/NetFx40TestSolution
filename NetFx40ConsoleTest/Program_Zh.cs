using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace NetFx40ConsoleTest
{
    class Program
    {
        // const int INFINITE = -1;
        const int MAX_TIMEOUT_5S = 1000 * 5;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WaitForMultipleObjects(int nCount, IntPtr[] lpHandles, bool bWaitAll, int dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);
        
        [DllImport("kernel32.dll")]
        public static extern bool SetEvent(IntPtr handle);
        
        [DllImport("kernel32.dll")]
        public static extern bool ResetEvent(IntPtr handle);
        static IntPtr handle1 = CreateEvent(IntPtr.Zero, true, false, "MyEvent");

        private static int count = 0;

        static void Main(string[] args)
        {
            // TODO 外部事件，比如查询状态
            Thread queryStateThread = new Thread(() =>
            {
                while (true)
                {
                    // 第3秒查询一次外事事件
                    Thread.Sleep(1000 * 5);
                
                    // 状态改变，设置事件
                    bool result = SetEvent(handle1);
                    if (!result)
                    {
                        // 设置失败
                        Console.WriteLine("handle1 set error");
                    }
                    else
                    {
                        // 设置成功
                        Console.WriteLine("handle1 set error");
                        ResetEvent(handle1);
                    }
                }
            });
            queryStateThread.Start();
            
            // 外部事件处理器
            IntPtr[] handles = new IntPtr[] { handle1 };

            while (true)
            {
                // 等待事件信号
                int result = WaitForMultipleObjects(handles.Length, handles, false, MAX_TIMEOUT_5S);

                // 检查
                if (result >= 0 && result < handles.Length)
                {
                    Console.WriteLine("Handle {0} was signaled", result);
                }
                else
                {
                    // TODO 主业务逻辑
                    Console.WriteLine("WaitForMultipleObjects failed with error code {0}", Marshal.GetLastWin32Error());
                }
                
                // 重置事件
                count++;
                if (count % 10 == 0)
                {
                    ResetEvent(handle1);
                    Console.WriteLine("handle1 has reset");
                }
                
                // Wait for 1 seconds
                Thread.Sleep(1000 * 1);
            }
        }
    }
}