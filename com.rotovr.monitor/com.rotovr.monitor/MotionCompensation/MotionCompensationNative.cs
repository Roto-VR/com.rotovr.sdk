using System.Runtime.InteropServices;

namespace RotoVR.MotionCompensation;

public static class MotionCompensationNative
{
    [DllImport("kernel32.dll")]
    internal static extern IntPtr LoadLibrary(string dll);

    [DllImport("RotoVR.MC.dll")]
    internal static extern void InitFacade();

    [DllImport("RotoVR.MC.dll")]
    internal static extern void InitOffset(double x, double y);

    [DllImport("RotoVR.MC.dll")]
    internal static extern void Start();

    [DllImport("RotoVR.MC.dll")]
    internal static extern void Stop();

    [DllImport("RotoVR.MC.dll")]
    internal static extern void UpdateAngle(int angle);
}