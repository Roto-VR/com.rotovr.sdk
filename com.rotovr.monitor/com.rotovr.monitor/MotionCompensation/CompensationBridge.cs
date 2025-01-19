using RotoVR.Common.Model;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RotoVR.MotionCompensation;

public class CompensationBridge : ICompensationBridge
{
    private RotoDataModel m_rotoData = new();
    private bool m_isRunning = false;

    public void Init()
    {
        Debug.WriteLine("Init");
        var result = MotionCompensationNative.LoadLibrary(@"driver_monitor.dll");

        if (result == IntPtr.Zero)
            Log.Error($"Init with result: {Marshal.GetLastWin32Error().ToString()}");       
        else
            Log.Info($"Init with result: {result.ToString()}");       

        MotionCompensationNative.Init();
    }

    public void Start()
    {
        Log.Info("Start Monitor");
        Debug.WriteLine("Start");
        MotionCompensationNative.Start();
    }

    public void SetRotoData(RotoDataModel data)
    {
        m_rotoData = data;
        MotionCompensationNative.UpdateAngle(data.Angle);
    }

    public void Stop()
    {
        Log.Info("Stop Monitor");
        Debug.WriteLine("Stop");
        MotionCompensationNative.Stop();
    }
}