using System;

namespace RotoVR.SDK.Model
{
    [Serializable]
    public class RumbleModel
    {
        public RumbleModel(float duration, int power)
        {
            Duration = duration;
            Power = power;
        }

        public float Duration { get; }
        public int Power { get; }
    }
}