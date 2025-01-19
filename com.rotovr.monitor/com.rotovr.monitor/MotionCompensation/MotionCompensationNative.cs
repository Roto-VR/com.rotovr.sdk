using System.Runtime.InteropServices;

namespace RotoVR.MotionCompensation;

public static class MotionCompensationNative
{
    [DllImport("kernel32.dll", EntryPoint = "LoadLibrary", SetLastError = true)]
    internal static extern IntPtr LoadLibrary(string dll);

    [DllImport("driver_monitor.dll")]
    internal static extern void Init(); 

    [DllImport("driver_monitor.dll")]
    internal static extern void Start();

    [DllImport("driver_monitor.dll")]
    internal static extern void Stop();

    [DllImport("driver_monitor.dll")]
    internal static extern void UpdateAngle(int angle);
}