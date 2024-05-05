using RotoVR.Common.Model;

namespace RotoVR.MotionCompensation;

public class CompensationBridge : ICompensationBridge
{
    private CompensationModel m_compensationModel = new();
    private RotoDataModel m_rotoData = new();

    public void Init()
    {
        /// Load OpenVR API
        ///
        /// 
    }

    public CompensationModel GetCompensationModel()
    {
        return m_compensationModel;
    }

    public void SetCompensationValue(CompensationModel model)
    {
        m_compensationModel = model;
    }


    public void Start()
    {
    }

    public void Stop()
    {
    }

    public void SetRotoData(RotoDataModel data)
    {
        Console.WriteLine($"Angle: {data.Angle}");
        m_rotoData = data;
    }
}