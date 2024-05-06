using RotoVR.Common.Model;

namespace RotoVR.MotionCompensation;

public class CompensationBridge : ICompensationBridge
{
    private RotoDataModel m_rotoData = new();
    private bool m_isRunning = false;

    public void Init()
    {
        MotionCompensationNative.LoadLibrary(@"RotoVR.MC.dll");
        MotionCompensationNative.InitFacade();
    }

    public void SetCompensationValue(CompensationModel model)
    {
        MotionCompensationNative.InitOffset(model.X, model.Y);
    }

    public void Start()
    {
        MotionCompensationNative.Start();
    }

    public void SetRotoData(RotoDataModel data)
    {
        Console.WriteLine($"Angle: {data.Angle}");
        m_rotoData = data;
        MotionCompensationNative.UpdateAngle(data.Angle);
    }

    public void Stop()
    {
        MotionCompensationNative.Stop();
    }
}