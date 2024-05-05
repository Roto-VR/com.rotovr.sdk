
using RotoVR.MotionCompensation;

namespace RotoVR.Communication;

public interface ICommunicationLayer
{
    void Inject(ICompensationBridge compensationBridge);
    void Start();
    void Stop();
}