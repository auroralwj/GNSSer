using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Geo.Utils
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryStatus
    {
        public uint Length;
        public uint MemoryLoad;
        public ulong TotalPhysical;
        public ulong AvailablePhysical;
        public ulong TotalPageFile;
        public ulong AvailablePageFile;
        public ulong TotalVirtual;
        public ulong AvailableVirtual;
    }

    ///http://www.cnblogs.com/bandik/archive/2011/10/25/2224243.html， 2013.06.27
     /// <summary>
     /// 内存类。
     /// </summary>
    public class MemoryUtil
    {
        [DllImport("kernel32.dll")]
        public static extern void GlobalMemoryStatusEx(ref MEMORYSTATUSEX stat);

        [DllImport("kernel32.dll")]
        public static extern void GlobalMemoryStatus(ref MemoryStatus stat);

        public static MEMORYSTATUSEX GetMEMORYSTATUSEX()
        {
            MEMORYSTATUSEX stat = new MEMORYSTATUSEX();
            stat.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));

            GlobalMemoryStatusEx(ref stat);
            return stat;
        }

        public static MemoryStatus GetMemoryStatus()
        {
            MemoryStatus stat2 = new MemoryStatus();
            stat2.Length = (uint)Marshal.SizeOf(typeof(MemoryStatus));
            GlobalMemoryStatus(ref stat2);
            return stat2;
        }
        public static void Main()
        {
            MEMORYSTATUSEX stat = new MEMORYSTATUSEX();
            stat.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));

            GlobalMemoryStatusEx(ref stat);
            long ram = (long)stat.ullAvailPhys / 1024 / 1024;
            Console.WriteLine(stat.ullAvailPhys / 1024 / 1024);
            Console.WriteLine(stat.ullTotalPhys / 1024 / 1024);
            Console.WriteLine(stat.ullTotalVirtual / 1024 / 1024 / 1024);
            //Console.ReadKey();
            MemoryStatus stat2 = new MemoryStatus();
            stat2.Length = (uint)Marshal.SizeOf(typeof(MemoryStatus));
            GlobalMemoryStatus(ref stat2);
            Console.WriteLine(stat2.AvailablePhysical / 1024 / 1024);

        }
    }
}
