using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameEngine.Utilities;

public static class SystemInfo
{
    /// <summary>Retorna a memória física usada pelo processo atual (bytes).</summary>
    public static long UsedMemoryBytes =>
        Process.GetCurrentProcess().WorkingSet64;

    /// <summary>Retorna a memória RAM total instalada no sistema (bytes).</summary>
    public static long TotalMemoryBytes => GetTotalPhysicalMemory();

    public static string FormatBytes(long bytes)
    {
        if (bytes < 1024)
        {
            return $"{bytes} B";
        }

        double kb = bytes / 1024.0;

        if (kb < 1024)
        {
            return $"{kb:F0} KB";
        }

        double mb = kb / 1024.0;

        if (mb < 1024)
        {
            return $"{mb:F0} MB";
        }

        double gb = mb / 1024.0;

        return $"{gb:F0} GB";
    }

    private static long GetTotalPhysicalMemory()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return GetWindowsTotalMemory();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return GetLinuxTotalMemory();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return GetMacTotalMemory();
        return 0; // fallback
    }

    // --- Windows ---
    [StructLayout(LayoutKind.Sequential)]
    private struct MEMORYSTATUSEX
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

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    private static long GetWindowsTotalMemory()
    {
        var memStatus = new MEMORYSTATUSEX { dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>() };
        if (GlobalMemoryStatusEx(ref memStatus))
            return (long)memStatus.ullTotalPhys;
        return 0;
    }

    // --- Linux ---
    private static long GetLinuxTotalMemory()
    {
        try
        {
            foreach (var line in File.ReadAllLines("/proc/meminfo"))
            {
                if (line.StartsWith("MemTotal:"))
                {
                    // Formato: "MemTotal:       16384000 kB"
                    string value = line.Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                    value = value.Replace(" kB", "");
                    if (long.TryParse(value, out long kb))
                        return kb * 1024;
                }
            }
        }
        catch { }
        return 0;
    }

    // --- macOS ---
    private static long GetMacTotalMemory()
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/usr/sbin/sysctl",
                Arguments = "-n hw.memsize",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var proc = Process.Start(psi)!;
            string output = proc.StandardOutput.ReadToEnd().Trim();
            proc.WaitForExit();
            if (long.TryParse(output, out long bytes))
                return bytes;
        }
        catch { }
        return 0;
    }
}
