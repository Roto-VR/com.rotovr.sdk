using RotoVR.Common.Model;
using System.Diagnostics;

namespace RotoVR.MotionCompensation;

public class CompensationBridge : ICompensationBridge
{
    private RotoDataModel m_rotoData = new();
    private bool m_isRunning = false;

    public void Init()
    {
        Debug.WriteLine("Init");
        MotionCompensationNative.LoadLibrary(@"driver_rotovr.dll");
        MotionCompensationNative.InitFacade();
    }  

    public void Start()
    {
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
        Debug.WriteLine("Stop");
        MotionCompensationNative.Stop();
    }
}