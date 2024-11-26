using RotoVR.Common.Model;

namespace RotoVR.MotionCompensation;

public interface ICompensationBridge
{
    void Init();
    void SetCompensationValue(CompensationModel model);
    void Start();
    void Stop();
    void SetRotoData(RotoDataModel data);
}