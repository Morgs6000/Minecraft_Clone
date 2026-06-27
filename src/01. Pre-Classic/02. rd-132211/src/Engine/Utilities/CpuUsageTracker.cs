using System;
using System.Diagnostics;
using System.Threading;

namespace GameEngine.Utilities;

public class CpuUsageTracker
{
    private readonly Process _process;
    private TimeSpan _lastProcessorTime;
    private DateTime _lastCheckTime;

    public CpuUsageTracker()
    {
        _process = Process.GetCurrentProcess();
        _lastProcessorTime = _process.TotalProcessorTime;
        _lastCheckTime = DateTime.UtcNow;
    }

    /// <summary>Retorna a % de CPU do processo (0–100)</summary>
    public float GetUsage()
    {
        TimeSpan currentProcessorTime = _process.TotalProcessorTime;
        DateTime currentTime = DateTime.UtcNow;

        double timeElapsed = (currentTime - _lastCheckTime).TotalSeconds;
        double cpuUsed = (currentProcessorTime - _lastProcessorTime).TotalSeconds;

        _lastProcessorTime = currentProcessorTime;
        _lastCheckTime = currentTime;

        if (timeElapsed <= 0) return 0;
        // cpuUsed é o tempo de CPU de todos os núcleos; divida pelo tempo real e converta em %
        return (float)(cpuUsed / (Environment.ProcessorCount * timeElapsed) * 100);
    }
}
