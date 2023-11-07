using System;

namespace RotoVR.SDK.Model
{
    [Serializable]
    public class RumbleModel
    {
        public RumbleModel(int duration, int power)
        {
            Duration = duration;
            Power = power;
        }

        public int Duration { get; }
        public int Power { get; }
    }
}