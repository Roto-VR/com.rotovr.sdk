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

        Task.Run(async () =>
        {
            await Task.Delay(1000);
            while (m_isRunning)
            {
                MotionCompensationNative.RunFrame(m_rotoData.Angle);
            }
        });
    }

    public void SetRotoData(RotoDataModel data)
    {
        Console.WriteLine($"Angle: {data.Angle}");
        m_rotoData = data;
    }

    public void Stop()
    {
        m_isRunning = false;
        MotionCompensationNative.Stop();
    }
}