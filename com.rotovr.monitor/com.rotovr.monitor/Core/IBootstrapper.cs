using System;

namespace RotoVR.Core
{
    public interface IBootstrapper
    {
        void Bootstrap(Action complete);
    }
}