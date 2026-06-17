using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using GameEngine.Core;
using Microsoft.Win32;
using Silk.NET.OpenGL;

namespace GameEngine.Utilities;

public static class SystemInfo
{
    /// <summary>Retorna a memória física usada pelo processo atual (bytes).</summary>
    public static long UsedMemoryBytes =>
        Process.GetCurrentProcess().WorkingSet64;

    /// <summary>Retorna a memória RAM total instalada no sistema (bytes).</summary>
    // public static long TotalMemoryBytes => GetTotalPhysicalMemory();
    public static long TotalMemoryBytes => GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;

    /// <summary>
    /// Versão do OpenGL suportada pelo driver.
    /// </summary>
    public static string OpenGLVersion
{
    get
    {
        if (Engine.GL == null)
            return "N/A";

        unsafe
        {
            byte* ptr = Engine.GL.GetString(StringName.Version);
            if (ptr == null)
                return "N/A";

            // Converte para string e garante não‑nulo com ?? "N/A"
            return Marshal.PtrToStringAnsi((IntPtr)ptr) ?? "N/A";
        }
    }
}

    public static string FormatBytes(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        int order = 0;
        double size = bytes;

        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }

        return $"{size:F0} {sizes[order]}";
    }

    /// <summary>
    /// Nome do processador (ex: "Intel(R) Core(TM) i7-9700K CPU @ 3.60GHz").
    /// Tenta obter de forma multiplataforma.
    /// </summary>
    public static string ProcessorName
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    string? name = Registry.GetValue(
                        @"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0",
                        "ProcessorNameString",
                        null) as string;

                    if (!string.IsNullOrWhiteSpace(name))
                        return name.Trim();
                }
                catch { }

                // Fallback: variável de ambiente (nome técnico)
                string? env = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
                return env ?? "CPU desconhecida";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                try
                {
                    // Lê /proc/cpuinfo para obter "model name"
                    foreach (string line in File.ReadLines("/proc/cpuinfo"))
                    {
                        if (line.StartsWith("model name", StringComparison.OrdinalIgnoreCase))
                        {
                            int idx = line.IndexOf(':');
                            if (idx >= 0)
                            {
                                string? name = line.Substring(idx + 1).Trim();
                                if (!string.IsNullOrWhiteSpace(name))
                                    return name;
                            }
                        }
                    }
                }
                catch { }
                return "CPU desconhecida";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                try
                {
                    var psi = new ProcessStartInfo("sysctl", "-n machdep.cpu.brand_string")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using var proc = Process.Start(psi);
                    if (proc != null)
                    {
                        string? result = proc.StandardOutput.ReadToEnd().Trim();
                        proc.WaitForExit();
                        if (!string.IsNullOrWhiteSpace(result))
                            return result;
                    }
                }
                catch { }
                return "CPU desconhecida";
            }

            // Sistema não reconhecido
            return "CPU desconhecida";
        }
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
