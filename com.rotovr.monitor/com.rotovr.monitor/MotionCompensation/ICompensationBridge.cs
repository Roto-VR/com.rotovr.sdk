using RotoVR.Common.Model;

namespace RotoVR.MotionCompensation;

public interface ICompensationBridge
{
    void Init();

    CompensationModel GetCompensationModel();
    void SetCompensationValue(CompensationModel model);
    void Start();
    void Stop();
}