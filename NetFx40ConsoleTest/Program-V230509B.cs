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

        // [DllImport("kernel32.dll", SetLastError = true)]
        // static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        // [DllImport("kernel32.dll", SetLastError = true)]
        // static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);

        // static IntPtr handle1 = CreateEvent(IntPtr.Zero, true, false, "MyEvent");
        // static IntPtr handle2 = CreateMutex(IntPtr.Zero, false, "MyMutex");

        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        // static IntPtr handle1 = /* initialize your handle here */;
        // static IntPtr handle2 = /* initialize your handle here */;
        // static IntPtr handle3 = /* initialize your handle here */;

        static IntPtr handle1 = CreateWork1();
        static IntPtr handle2 = CreateWork2();
        // static IntPtr handle3 = CreateWork3();

        static void Main(string[] args)
        {
            // Define an array of handles to wait on
            IntPtr[] handles = new IntPtr[] { handle1, handle2, /*handle3*/ };

            while (true)
            {
                
                // Wait for any of the handles to become signaled
                int result = WaitForMultipleObjects(handles.Length, handles, false, MAX_TIMEOUT_5S);

                // Check the result
                if (result >= 0 && result < handles.Length)
                {
                    Console.WriteLine("Handle {0} was signaled", result);
                }
                else
                {
                    Console.WriteLine("WaitForMultipleObjects failed with error code {0}", Marshal.GetLastWin32Error());
                }
                
                // Sleep 3s
                Thread.Sleep(1000 * 1);
            }
        }

        static IntPtr CreateWork1()
        {
            Thread thread1 = new Thread(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("Thread1 -> {0}", i);
                    Thread.Sleep(1000 * 5);
                }
            });
            thread1.Start();
            return OpenThread(ThreadAccess.QUERY_INFORMATION, false, (uint)thread1.ManagedThreadId);
        }

        static IntPtr CreateWork2()
        {
            Thread thread2 = new Thread(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("Thread2 -> {0}", i);
                    Thread.Sleep(1000 * 3);
                }
            });
            thread2.Start();
            return OpenThread(ThreadAccess.QUERY_INFORMATION, false, (uint)thread2.ManagedThreadId);
        }

        static IntPtr CreateWork3()
        {
            Thread thread3 = new Thread(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("Thread3 -> {0}", i);
                    Thread.Sleep(1000 * 1);
                }
            });
            thread3.Start();
            return OpenThread(ThreadAccess.QUERY_INFORMATION, false, (uint)thread3.ManagedThreadId);
        }
    }
}