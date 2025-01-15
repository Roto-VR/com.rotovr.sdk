using System.Runtime.InteropServices;

namespace RotoVR.MotionCompensation;

public static class MotionCompensationNative
{
    [DllImport("kernel32.dll")]
    internal static extern IntPtr LoadLibrary(string dll);

    [DllImport("driver_rotovr.dll")]
    internal static extern void InitFacade(); 

    [DllImport("driver_rotovr.dll")]
    internal static extern void Start();

    [DllImport("driver_rotovr.dll")]
    internal static extern void Stop();

    [DllImport("driver_rotovr.dll")]
    internal static extern void UpdateAngle(int angle);
}